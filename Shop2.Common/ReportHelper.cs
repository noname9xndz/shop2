using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Common
{
    // add nuget : Epplus , HtmlRenderer.PdfSharp
    public static class ReportHelper
    {
        public static async Task GeneratePdf(string html, string filePath)
        {
            await Task.Run(() =>
            {
                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
                    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                    pdf.Save(ms);
                }
            });
        }


        //c1 : lấy trực tiếp bằng cách lấy dữ liệu trên form
        public static Task GenerateXls<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    ws.Cells["A1"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }

        // c2 : vẽ trực tiếp các cell
        public static Task GenerateXls2<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath),new FileInfo("đường dẫn file template")))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    //vẽ các cell
                    ws.Cells["A1"].Value = "tên";
                    ws.Cells["A2"].Value = "tên";
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }
    }
}
