#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:14

#endregion

namespace JPB.DynamicInputBox.InfoWindow
{
    public enum EingabeModus
    {
        ///// <summary>
        /////     Select a Path
        /////     Returns the Path as String
        ///// </summary>
        //Pfad,

        /// <summary>
        ///     Select a Intinger
        ///     Returrns the Intinger
        /// </summary>
        Zahl,

        /// <summary>
        ///     A normal Text reprersented by a TextBox
        ///     Returns The text as String
        /// </summary>
        Text,

        /// <summary>
        ///     A normal RichText reprersented by a TextBox with Textwrapping
        ///     Returns The text as String
        /// </summary>
        RichText,

        /// <summary>
        ///     A Collection represented by Radioboxes.
        ///     Syntax:
        ///     Hallo this is the question text #qFirst question#qSec Question#q more questions ...
        ///     Returns The index of the Selected item as int
        /// </summary>
        RadioBox,

        /// <summary>
        ///     A Collection represented by Checkboxes to allow Multible selections.
        ///     Syntax:
        ///     Hallo this is the question text #qFirst question#qSec Question#q more questions ...
        ///     Returns a collection of PB.Wrapper.ListBoxItemWrapper
        /// </summary>
        CheckBox,

        /// <summary>
        ///     A action<!--object--> that will be invoked Asyc
        ///     Returns the result of that action<!--object--> as Object
        /// </summary>
        ShowProgress,

        /// <summary>
        ///     Let the user Choose a Date
        ///     Returns the selected DateTime
        /// </summary>
        Date,

        /// <summary>
        ///     Select a collection of values
        ///     Returns the collection of all objects
        /// </summary>
        MultiInput,

        /// <summary>
        ///     Shows a Simple list of objects,
        ///     Use the IInputDescriptor interface to implement the Items
        /// </summary>
        ListView
    }
}