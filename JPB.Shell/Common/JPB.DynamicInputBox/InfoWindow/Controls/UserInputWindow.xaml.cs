using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace JPB.DynamicInputBox.InfoWindow.Controls
{
    public partial class UserInputWindow : Window
    {
        internal UserInputWindow(List<object> inputQuestions, Func<List<object>> returnlist,
                                 IEnumerable<EingabeModus> eingabeModi)
        {
            DataContext = new UserInputViewModel(inputQuestions, returnlist, () =>
                {
                    DialogResult = true;
                    Close();
                }, eingabeModi);
            InitializeComponent();
        }

        private void UserInputWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var vm = (DataContext as UserInputViewModel);
            if (!vm.IsClosing)
            {
                vm.IsClosing = true;
            }
        }
    }
}