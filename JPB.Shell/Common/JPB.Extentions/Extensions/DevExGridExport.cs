using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;

namespace PB.Extensions
{
    public static class DevExGridExportBase
    {
        public static void DoExport(GridControl sender, string path, bool autostart)
        {
            var control = PrepareControl(sender);
            control.CheckColumns();
            var stream = new MemoryStream();
            try
            {
                control.SaveLayout(stream);
                var view = ((TableView)control.View);
                view.BestFitColumns();
                var buff = new PrintableControlLink(view);
                buff.ExportData(path);
            }
            finally
            {
                control.RestoreLayout(stream);
                control.CleanControl();
                if (autostart)
                    Process.Start(path);
            }
        }


        public static void ExportData(this PrintableControlLink control, string path)
        {
            string fileExtenstion = new FileInfo(path).Extension;

            switch (fileExtenstion.ToLower())
            {
                case ".xls":
                    control.ExportToXls(path);
                    break;
                case ".xlsx":
                    control.ExportToXlsx(path, new XlsxExportOptions(TextExportMode.Text, true, true));
                    break;
                case ".rtf":
                    control.ExportToRtf(path);
                    break;
                case ".pdf":
                    control.ExportToPdf(path);
                    break;
                case ".html":
                    control.ExportToHtml(path);
                    break;
                case ".mht":
                    control.ExportToMht(path);
                    break;
                default:
                    break;
            }
        }

        private static void CheckColumns(this GridControl control)
        {
            foreach (var column in control.Columns.Where(column => string.IsNullOrEmpty(column.Name)))
            {
                column.Name = column.FieldName;
            }
        }

        private static void SaveLayout(this GridControl control, Stream stream)
        {
            control.SaveLayoutToStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
        }


        private static void RestoreLayout(this GridControl control, Stream stream)
        {
            control.RestoreLayoutFromStream(stream);
        }

        private static GridControl PrepareControl(object sender)
        {
            var control = sender as GridControl;
            Monitor.Enter(control);
            control.ShowLoadingPanel = true;
            control.IsEnabled = false;
            return control;
        }

        private static void CleanControl(this GridControl control)
        {
            control.ShowLoadingPanel = false;
            control.IsEnabled = true;
        }
    }
}
