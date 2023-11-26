using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Reporting.Service.Core.Exporting
{
    public class NpoiExport : IDisposable
    {
        const int MaximumNumberOfRowsPerSheet = 1048576; //65500;
        const int MaximumSheetNameLength = 25;
        protected IWorkbook Workbook { get; set; }

        public NpoiExport()
        {
            this.Workbook = new XSSFWorkbook();
        }

        protected string EscapeSheetName(string sheetName)
        {
            var escapedSheetName = sheetName
                   .Replace("/", "-")
                   .Replace("\\", " ")
                   .Replace("?", string.Empty)
                   .Replace("*", string.Empty)
                   .Replace("[", string.Empty)
                   .Replace("]", string.Empty)
                   .Replace(":", string.Empty);

            if (escapedSheetName.Length > MaximumSheetNameLength)
                escapedSheetName = escapedSheetName.Substring(0, MaximumSheetNameLength);

            return escapedSheetName;
        }

        protected ISheet CreateExportDataTableSheetAndHeaderRow(DataTable exportData, string sheetName, ICellStyle headerRowStyle)
        {
            var sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));

            // Create the header row
            var row = sheet.CreateRow(0);

            for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
            {
                var cell = row.CreateCell(colIndex);
                cell.SetCellValue(exportData.Columns[colIndex].ColumnName);

                if (headerRowStyle != null)
                    cell.CellStyle = headerRowStyle;
            }

            return sheet;
        }

        public void ExportDataTableToWorkbook(DataTable exportData, string sheetName)
        {
            var dateStyle = this.Workbook.CreateCellStyle();
            var builtinFormatId = HSSFDataFormat.GetBuiltinFormat("dd/MM/yyyy");
            if (builtinFormatId != -1)
            {
                dateStyle.DataFormat = builtinFormatId;
            }
            else
            {
                // not a built-in format, so create a new one
                var newDataFormat = this.Workbook.CreateDataFormat();
                dateStyle.DataFormat = newDataFormat.GetFormat("dd/MM/yyyy");
            }


            // Create the header row cell style
            var headerLabelCellStyle = this.Workbook.CreateCellStyle();
            headerLabelCellStyle.BorderBottom = BorderStyle.Thin;
            var headerLabelFont = this.Workbook.CreateFont();
            headerLabelFont.Boldweight = (short)FontBoldWeight.Bold;
            headerLabelCellStyle.SetFont(headerLabelFont);

            var sheet = CreateExportDataTableSheetAndHeaderRow(exportData, sheetName, headerLabelCellStyle);
            var currentNPOIRowIndex = 1;
            var sheetCount = 1;

            for (var rowIndex = 0; rowIndex < exportData.Rows.Count; rowIndex++)
            {
                if (currentNPOIRowIndex >= MaximumNumberOfRowsPerSheet)
                {
                    sheetCount++;
                    currentNPOIRowIndex = 1;

                    sheet = CreateExportDataTableSheetAndHeaderRow(exportData,
                                  sheetName + " - " + sheetCount,
                                  headerLabelCellStyle);
                }

                var row = sheet.CreateRow(currentNPOIRowIndex++);

                for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
                {
                    var cell = row.CreateCell(colIndex);
                    //cell.SetCellValue(exportData.Rows[rowIndex][colIndex].ToString());
                    object value = exportData.Rows[rowIndex][colIndex];

                    if (value is string)
                    {
                        cell.SetCellValue((string)value);
                    }
                    else if (value is decimal)
                    {
                        cell.SetCellValue((double)(decimal)value);
                    }
                    else if (value is double)
                    {
                        cell.SetCellValue((double)value);
                    }
                    else if (value is float)
                    {
                        cell.SetCellValue((double)(float)value);
                    }
                    else if (value is int)
                    {
                        cell.SetCellValue((int)value);
                    }
                    else if (value is DateTime)
                    {
                        cell.SetCellValue((DateTime)value);
                        cell.CellStyle = dateStyle;
                    }
                    else if (value is bool)
                    {
                        cell.SetCellValue((bool)value);
                    }
                    else if (value is DBNull)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    else
                    {
                        throw new InvalidOperationException("No se puede procesar el tipo de dato para la conversión, NPOI Export Esception");
                    }
                }
            }

            for (var colIndex = 0; colIndex < 16; colIndex++)
            {
                sheet.AutoSizeColumn(colIndex);
            }
        }

        public byte[] GetBytes()
        {
            using (var buffer = new MemoryStream())
            {
                this.Workbook.Write(buffer);
                return buffer.GetBuffer();
            }
        }

        public void Dispose()
        {
            //if (this.Workbook != null)
            // this.Workbook.Dispose();
        }

        public void ExportToFile(string filePath)
        {

            using (FileStream fs = File.Create(filePath))
            {
                this.Workbook.Write(fs);
            }
        }

        //public byte[] ExportToStream()
        //{
        // MemoryStream stream = new MemoryStream();
        // this.Workbook.Write(stream);
        // return stream.ToArray();
        //}

        public MemoryStream ExportToStream()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.Workbook.Write(stream);
                return new MemoryStream(stream.ToArray());
            }
        }
    }
}