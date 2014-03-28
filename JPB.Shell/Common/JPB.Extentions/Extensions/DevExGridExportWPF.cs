using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using DevExpress.Xpf.Grid;
using PB.Base;

namespace PB.Extensions
{
    public enum ExportType
    {
        Xls,
        Xlsx,
        Rtf,
        PDF,
        Html,
        Mht 
    }

    public static class DevExGridExportWpf
    {
        static DevExGridExportWpf()
        {
            SaveGridCommand = new DelegateCommand(SaveAsGrid, CanSaveGrid);
        }

        public static readonly DependencyProperty ExportTypeProperty =
          DependencyProperty.RegisterAttached(
              "ExportType",
              typeof(ExportType),
              typeof(GridControl),
              new FrameworkPropertyMetadata(ExportType.Xlsx, FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static void SetExportType(UIElement element, ExportType value)
        {
            element.SetValue(ExportTypeProperty, value);
        }

        public static ExportType GetExportType(UIElement element)
        {
            return (ExportType)element.GetValue(ExportTypeProperty);
        }

        public static readonly DependencyProperty IsExportAllowedProperty =
            DependencyProperty.RegisterAttached(
                "IsExportAllowed",
                typeof (Boolean),
                typeof (GridControl),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static void SetIsExportAllowed(UIElement element, Boolean value)
        {
            element.SetValue(IsExportAllowedProperty, value);
        }

        public static Boolean GetIsExportAllowed(UIElement element)
        {
            return (Boolean)element.GetValue(IsExportAllowedProperty);
        }

        public static readonly DependencyProperty CanSaveGridProperty =
            DependencyProperty.RegisterAttached(
                "CanSaveGrid",
                typeof (Boolean),
                typeof (GridControl),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static void SetCanSaveGrid(UIElement element, Boolean value)
        {
            element.SetValue(CanSaveGridProperty, value);
        }

        public static Boolean GetCanSaveGrid(UIElement element)
        {
            return (Boolean) element.GetValue(CanSaveGridProperty);
        }


        public static readonly DependencyProperty AutoRunAfterExportProperty =
            DependencyProperty.RegisterAttached(
                "AutoRunAfterExport",
                typeof (Boolean),
                typeof (GridControl),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static void SetAutoRunAfterExport(UIElement element, Boolean value)
        {
            element.SetValue(AutoRunAfterExportProperty, value);
        }

        public static Boolean GetAutoRunAfterExport(UIElement element)
        {
            return (Boolean) element.GetValue(AutoRunAfterExportProperty);
        }

        public static readonly DependencyProperty ExportPathProperty =
            DependencyProperty.RegisterAttached(
                "ExportPath",
                typeof (String),
                typeof (GridControl),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static void SetExportPath(UIElement element, String value)
        {
            element.SetValue(ExportPathProperty, value);
        }

        public static String GetExportPath(UIElement element)
        {
            return (String) element.GetValue(ExportPathProperty);
        }
        
        #region SaveGrid DelegateCommand

        public static DelegateCommand SaveGridCommand { get; private set; }

        /// <summary>
        /// Use this Method to Save all viewed data in the Grid
        /// Importent:
        /// The CommandParameter must be the Grid itself
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private static void SaveAsGrid(object sender)
        {
            var buff = sender as GridControl;

            var path = GetExportPath(buff);
            var autostart = GetAutoRunAfterExport(buff);
            var type = GetExportType(buff);

            if (String.IsNullOrEmpty(path))
            {
                path = Path.GetTempFileName();
            }

            path = Path.ChangeExtension(path, type.ToString());

            DevExGridExportBase.DoExport(buff, path, autostart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private static bool CanSaveGrid(object sender)
        {
            return sender is GridControl && GetCanSaveGrid(sender as UIElement);
        }

        #endregion
    }
}
