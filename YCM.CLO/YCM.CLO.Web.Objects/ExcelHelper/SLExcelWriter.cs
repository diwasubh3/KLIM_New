using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace YCM.CLO.Web.Objects.ExcelHelper
{
    public class SlExcelWriter
    {
        private string ColumnLetter(int intCol)
        {
            var intFirstLetter = intCol / 676 + 64;
            var intSecondLetter = intCol % 676 / 26 + 64;
            var intThirdLetter = intCol % 26 + 65;

            var firstLetter = intFirstLetter > 64 ? (char)intFirstLetter : ' ';
            var secondLetter = intSecondLetter > 64 ? (char)intSecondLetter : ' ';
            var thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter, thirdLetter).Trim();
        }

        private Cell CreateTextCell(string header, UInt32 index, string text)
        {
            
            UInt32 styleIndex = 0;
            bool isNumber;
            if (text.IndexOf("Style:", System.StringComparison.InvariantCulture) >= 0)
            {
                var texts = text.Split(new string[] { "Style:" }, StringSplitOptions.None);
                styleIndex = UInt32.Parse(texts[texts.Length > 1 ? 1 : 0]);
                text = texts.Length > 1 ? texts[0] : "";
            }

            if (text.IndexOf("Type:", System.StringComparison.InvariantCulture) >= 0)
            {
                var texts = text.Split(new string[] { "Type:" }, StringSplitOptions.None);
                text = texts.Length > 1 ? texts[0] : "";
                isNumber = texts[texts.Length > 1 ? 1 : 0] == "number";
            }
            else
            {
                decimal dec;
                isNumber = decimal.TryParse(text, out dec);
            }


            var cell = new Cell
            {
                CellReference = header + index,
                DataType = isNumber ? CellValues.Number : CellValues.String,//CellValues.InlineString,
                StyleIndex = styleIndex,
                
                CellValue =  new CellValue(isNumber ? text.Replace(",",""):text),
            };

            if (text.IndexOf("=", StringComparison.Ordinal) == 0)
            {
                cell.CellFormula = new CellFormula(text.Substring(1));
                cell.CellValue = new CellValue();
            }
            
            return cell;
        }

        public byte[] GenerateExcel(IList<SlExcelData> datas)
        {
            var stream = new MemoryStream();
            var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);

            var workbookpart = document.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            var sheets = document.WorkbookPart.Workbook.
			AppendChild(new Sheets());

            uint sheetId = 1;
            datas.ToList().ForEach(data =>
            {
                var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();

                worksheetPart.Worksheet = new Worksheet(sheetData);

                var sheet = new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = sheetId++,
                    Name = data.SheetName ?? "Sheet 1"
                };

                sheets.Append(sheet);


                // Add header
                UInt32 rowIdex = 0;
                var row = new Row { RowIndex = ++rowIdex };
                sheetData.AppendChild(row);
                var cellIdex = 0;

                if (data.PreHeaders != null && data.PreHeaders.Count > 0)
                {
                    foreach (var header in data.PreHeaders)
                    {
                        row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++), rowIdex, header ?? string.Empty));
                    }

                    cellIdex = 0;
                    row = new Row { RowIndex = ++rowIdex };
                    sheetData.AppendChild(row);
                }

                foreach (var header in data.Headers)
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++), rowIdex, header ?? string.Empty));
                }

                if (data.Headers.Count > 0)
                {
                    // Add the column configuration if available
                    if (data.ColumnConfigurations != null)
                    {
                        var columns = (Columns)data.ColumnConfigurations.Clone();
                        worksheetPart.Worksheet
                            .InsertAfter(columns, worksheetPart.Worksheet.SheetFormatProperties);
                    }
                }

                // Add sheet data
                foreach (var rowData in data.DataRows)
                {
                    cellIdex = 0;
                    row = new Row { RowIndex = ++rowIdex };
                    sheetData.AppendChild(row);
                    foreach (var callData in rowData)
                    {
                        var cell = CreateTextCell(ColumnLetter(cellIdex++), rowIdex, callData ?? string.Empty);
                        row.AppendChild(cell);
                    }
                }
            });

            WorkbookStylesPart stylesPart = workbookpart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = GenerateStyleSheet();

            workbookpart.Workbook.Save();
            document.Close();

            return stream.ToArray();
        }

        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 - The default font.
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 1 - The bold font.
                        new Bold(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Italic font.
                        new Italic(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "FF0000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 3 - The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Times New Roman" }),
                        new Font(                                                               // Index 3 - The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "FF0000" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 - The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 - The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(                                                           // Index 2 - The yellow fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFDFAF" } }
                        ) { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 - The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new RightBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new TopBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new BottomBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 1 - Bold 
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 - Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 - Times Roman
                    new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Index 4 - Yellow Fill
                    new CellFormat(                                                                   // Index 5 - Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    ) { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 - Border
                )
            ); // return
        }
    }
}