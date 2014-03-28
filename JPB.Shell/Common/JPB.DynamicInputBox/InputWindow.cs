using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JPB.DynamicInputBox.InfoWindow;
using JPB.DynamicInputBox.InfoWindow.Controls;
using JPB.DynamicInputBox.InfoWindow.Wrapper;

namespace JPB.DynamicInputBox
{
    public static class InputWindow
    {
        public static T ReparseList<T>(IEnumerable<T> input, IListBoxItemWrapper selected) where T : class
        {
            return input.ElementAt(selected.Index);
        }

        public static T ReparseList<T>(IEnumerable<T> input, IEnumerable<IListBoxItemWrapper> selected) where T : class
        {
            return input.ElementAt(selected.First().Index);
        }

        public static IEnumerable<string> ParseMultiItemsList(object elementAt)
        {
            return ParseMultiItemsList(elementAt as IEnumerable<IListBoxItemWrapper>);
        }

        public static IEnumerable<string> ParseMultiItemsList(IEnumerable<IListBoxItemWrapper> input)
        {
            return input.Select(s => s.Text);
        }

        public static T ReparseList<T>(IEnumerable<T> input, object selected)
        {
            if (selected is IListBoxItemWrapper)
            {
                return input.ElementAt((selected as IListBoxItemWrapper).Index);
            }
            if (selected is IEnumerable<IListBoxItemWrapper>)
            {
                return input.ElementAt((selected as IEnumerable<IListBoxItemWrapper>).First().Index);
            }
            return default(T);
        }

        public static object ShowInput(IWaiterWrapper inputQuestion)
        {
            var returns = new List<object>();
            if (WindowThread(new List<object> { inputQuestion }, () => returns,
                             new List<EingabeModus> { EingabeModus.ShowProgress }))
                return returns.First();
            return null;
        }

        public static object ShowInput(Func<object> inputQuestion)
        {
            var returns = new List<object>();
            if (WindowThread(new List<object> { inputQuestion }, () => returns,
                             new List<EingabeModus> { EingabeModus.ShowProgress }))
                return returns.First();
            return null;
        }

        public static string ShowInput(string inputQuestion)
        {
            var returns = new List<object>();
            if (WindowThread(new List<object> { inputQuestion }, () => returns, new List<EingabeModus> { EingabeModus.Text }))
                return returns.First() as string;
            return null;
        }

        public static object ShowInput(string inputQuestion, EingabeModus modus)
        {
            var returns = new List<object>();
            if (WindowThread(new List<object> { inputQuestion }, () => returns, new List<EingabeModus> { modus }))
                return returns.First();
            return null;
        }

        public static bool ShowInput(Func<List<object>> updateDelegate, List<object> inputQuestions,
                                     List<EingabeModus> eingabeModi)
        {
            return WindowThread(inputQuestions, updateDelegate, eingabeModi);
        }

        public static IEnumerable<object> ShowInput(List<object> inputQuestions, List<EingabeModus> eingabeModi)
        {
            var returns = new List<object>();
            WindowThread(inputQuestions, () => returns, eingabeModi);
            return returns;
        }

        public static IEnumerable<object> ShowInput(List<object> inputQuestions)
        {
            var returns = new List<object>();
            WindowThread(inputQuestions, () => returns, returns.Select(retursn => EingabeModus.Text));
            return returns;
        }

        private static bool WindowThread(List<object> inputQuestions, Func<List<object>> returnlist,
                                         IEnumerable<EingabeModus> eingabeModi)
        {
            bool? ret = false;
            var windowThread = new Thread(() =>
                {
                    var inputwindow = new UserInputWindow(inputQuestions, returnlist, eingabeModi);
                    ret = inputwindow.ShowDialog();
                });
            windowThread.Name = "InputWindowThread";
            windowThread.SetApartmentState(ApartmentState.STA);
            windowThread.Start();
            windowThread.Join();
            return ret.HasValue && ret.Value;
        }
    }
}