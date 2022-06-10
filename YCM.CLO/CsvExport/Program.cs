using ExportCsv.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using static ExportCsv.ConfigurationConstants;
using static ExportCsv.ExtensionMethods.DateTimeExtensionMethods;
using static ExportCsv.ExtensionMethods.IDataReaderExtensionMethods;
using static ExportCsv.ExtensionMethods.IDataRecordExtensionMethods;
using static ExportCsv.Utilities;
using System.IO.Compression;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Net.Mime;
using System.Web;
using System.Net.Http;
using NPOI.HSSF.Util;

namespace ExportCsv
{
    class Program
    {
        private readonly static NLog.ILogger _logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

        private static ImportArgument _argument;

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Export Id parameter expected.");
                return Environment.ExitCode = (int)ExitCode.MissingParameter;
            }
            _argument = new ImportArgument(args);

            _logger.Info(_argument);
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Yoda"].ConnectionString;

            Process(_argument, connectionString);
            return (int)ExitCode.Success;
        }

        private static void Process(ImportArgument argument, string connectionString)
        {

            ExportData(_argument, connectionString);
        }

        private static void ExportData(ImportArgument argument, string connectionString)
        {

            // var date = argument.ImportDate;
            //

            var import = GetFileExport(connectionString, argument.ImportId);
            _logger.Info($"File Export properties: {import}");

            var ds = GetExportDatafromSP(connectionString, import.TableName);

            if (ds.Rows.Count == 0)
            {
                _logger.Info("No data to Export. Exiting.");
                return;
            }

            WriteExcelWithNPOI(ds, "xlsx", Path.Combine(import.FileLocation, import.FileNameMask), "Sheet 1", import.TableName);

            _logger.Info("Exported succesfully File path" + Path.Combine(import.FileLocation, import.FileNameMask));

        }

        private static void ExportTOCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);

            }

            sw.Close();

        }
        //private static void ExportTOExcle(DataTable dtDataTable, string strFilePath)
        //{
        //    XLWorkbook wb = new XLWorkbook();
        //    DataTable dt = GetDataTableOrWhatever();
        //    wb.Worksheets.Add(dt, "WorksheetName");


        //}
        private static string GetDateFilterString(DateTime date, FileImport import)
            => import.UseDateIdOnDeleteClauseMask ? GetDateId(date).ToString() : $"'{date}'";

        private static void PopulateMetaData(ImportArgument argument, string connectionString)
        {
            try
            {
                var import = GetFileExport(connectionString, argument.ImportId);
                //  var fileName = GetFileName(import, argument);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }


        private static DataTable GetExportDatafromSP(string connectionString, string SPName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(SPName, connection);
                // cmd.Parameters.Add("@TABLE_NAME", SqlDbType.VarChar, 100).Value = TextBox1.Text;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                return dt;


            }
        }

        private static FileImport GetFileExport(string connectionString, int importId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("SELECT FileNameMask, TableName, FileLocation, DeleteWhereClause"
                + $", HasHeader, UseDateIdOnFileMask, UseDateIdOnDeleteClauseMask FROM CLO.FileExport WHERE Id = {importId}"
                    , connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        var imp = new FileImport
                        {
                            FileLocation = reader[nameof(FileImport.FileLocation)].ToString()
                            ,
                            FileNameMask = reader[nameof(FileImport.FileNameMask)].ToString()
                            ,
                            TableName = reader[nameof(FileImport.TableName)].ToString()
                            ,
                            DeleteWhereClause = reader[nameof(FileImport.DeleteWhereClause)].ToString()
                            ,
                            HasHeader = reader.GetBoolValueOrDefault(nameof(FileImport.HasHeader))
                            ,
                            UseDateIdOnFileMask = reader.GetBoolValueOrDefault(nameof(FileImport.UseDateIdOnFileMask))
                            ,
                            UseDateIdOnDeleteClauseMask = reader.GetBoolValueOrDefault(nameof(FileImport.UseDateIdOnDeleteClauseMask))
                        };
                        return imp;
                    }
                }
            }
        }

        public static void WriteExcelWithNPOI(DataTable dt, String extension, string Filepath, string SheetName, string SPName)
        {
            if (SPName == "[dbo].[CLO.GetPurchaseSalePrices_Monthly]")
            {
                using (FileStream stream = new FileStream(Filepath, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;

                    if (extension == "xlsx")
                    {
                        workbook = new XSSFWorkbook();
                    }
                    else if (extension == "xls")
                    {
                        workbook = new HSSFWorkbook();
                    }
                    else
                    {
                        throw new Exception("This format is not supported");
                    }

                    ISheet sheet1 = workbook.CreateSheet(SheetName);

                    sheet1.SetColumnWidth(0, 4500);
                    sheet1.SetColumnWidth(1, 5500);
                    sheet1.SetColumnWidth(2, 7500);
                    sheet1.SetColumnWidth(3, 3500);
                    sheet1.SetColumnWidth(4, 3500);
                    sheet1.SetColumnWidth(5, 3500);
                    sheet1.SetColumnWidth(6, 5500);
                    sheet1.SetColumnWidth(7, 6500);
                    sheet1.SetColumnWidth(8, 6000);

                    //make a header row
                    IRow row1 = sheet1.CreateRow(0);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        String columnName = dt.Columns[j].ToString();
                        cell.SetCellValue(columnName);
                    }


                    //loops through data
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row = sheet1.CreateRow(i + 1);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);
                            String columnName = dt.Columns[j].ToString();
                            if (Convert.ToDouble(dt.Rows[i][11].ToString()) >= 1 || Convert.ToDouble(dt.Rows[i][12].ToString()) >= 1)
                            {
                                ICellStyle fCellStyle = workbook.CreateCellStyle();
                                fCellStyle.FillForegroundColor = HSSFColor.Yellow.Index;
                                fCellStyle.FillPattern = FillPattern.SolidForeground;
                                cell.CellStyle = fCellStyle;
                            }
                            if (j == 3 || j == 4 || j == 9 || j == 10 || j == 11 || j == 12)
                            {
                                cell.SetCellValue(Convert.ToDouble(dt.Rows[i][columnName].ToString()));
                                cell.CellStyle.Alignment = HorizontalAlignment.Right;

                            }
                            else
                            {
                                cell.SetCellValue(dt.Rows[i][columnName].ToString());
                            }

                        }
                    }
                    workbook.Write(stream);
                }
            }
        }

        

    }
}
