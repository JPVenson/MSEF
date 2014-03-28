#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 14:54

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using JPB.DynamicInputBox.InfoWindow.Wrapper;

namespace JPB.DynamicInputBox.InfoWindow.IQuestionModelImp
{
    public class QuestionComplexList : QuestionMultipleChoiceAbstrViewModel
    {
        public QuestionComplexList(object question, EingabeModus eingabeModus)
            : base(question, eingabeModus)
        {
            Input = "EMPTY";
        }

        public override void Init()
        {
            //Question = ParsexQuestionText(Question);
            base.Init();
        }

        #region PropertyInfos property

        private List<PropertyInfo> _propertyInfos;

        public List<PropertyInfo> PropertyInfos
        {
            get { return _propertyInfos; }
            set
            {
                _propertyInfos = value;
                SendPropertyChanged(() => PropertyInfos);
            }
        }

        #endregion

        //public string ParsexQuestionText(object input)
        //{
        //    if (input is IDynamicInputDescriptor)
        //    {
        //        var relinput = input as IDynamicInputDescriptor;
        //        PropertyInfos = input.GetType().GetProperties(BindingFlags.CreateInstance | BindingFlags.Instance).ToList();
        //        PropertyInfos.AddRange((input as ICustomTypeDescriptor).GetProperties().Cast<PropertyInfo>());


        //        return relinput.Text;
        //    }
        //}
    }

    public class QuestionSimpleList : QuestionMultipleChoiceAbstrViewModel
    {
        public QuestionSimpleList(object question, EingabeModus eingabeModus)
            : base(question, eingabeModus)
        {
            Input = "EMPTY";
        }

        public override void Init()
        {
            Question = ParsexQuestionText(Question);
            base.Init();
        }

        public string ParsexQuestionText(object input)
        {
            if (!(input is string))
                throw new ArgumentException("Can not parse text!");
            var text = (input as string);
            string lowertext = text.ToLower();
            if (!lowertext.Contains("#q"))
                throw new ArgumentException("Can not parse text!");
            List<string> allquestions = text.Split(new[] {"#Q", "#q"}, StringSplitOptions.RemoveEmptyEntries).ToList();
            text = allquestions.ElementAt(0);
            allquestions.RemoveAt(0);
            PossibleInput =
                new ObservableCollection<IListBoxItemWrapper>(
                    allquestions.Select(
                        allquestion =>
                        new ListBoxItemWrapper {Text = allquestion, Index = allquestions.IndexOf(allquestion)}));
            return text;
        }
    }
}