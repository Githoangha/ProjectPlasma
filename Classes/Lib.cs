using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.IO;
using System.Net;
using System.Data.OleDb;
using System.Diagnostics;
using System.Data.SQLite;
using System.Threading;
using GemBox.Spreadsheet;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace LineGolden_PLasma
{
    public static class Lib
    {
        #region Variables
        public static bool IsExecuteFunction = true;
        #region FORMAT
        public const string CurrencyFormat = "###,###,###.00";
        public const string CurrencyFormatVND = "###,###,###,###";
        public const string FomatShortDate = "yyyy//MM/dd";
        public const string FormatLongDate = "yyyy//MM/dd HH:mm:ss";
        #endregion

        #region MESSAGE
        public static string Caption = "Thông báo";

        public static string CaptionError = "Thông báo lỗi";
        #endregion

        #region DATETIME
        public static DateTime MIN_DATE = new DateTime(1950, 1, 1);
        #endregion

        private static string[] Number_Patterns = new string[] { "{0:#,##0}", "{0:#,##0.0}", "{0:#,##0.00}", "{0:#,##0}.000", "{0:#,##0.0000}",
            "{0:#,##0.00000;#,##0.00000; }" };

        private static string[] Currency_Patterns = new string[] { "{0:$#,##0;($#,##0); }", "{0:$#,##0.0;($#,##0.0); }", "{0:$#,##0.00;($#,##0.00); }",
            "{0:$#,##0.000;($#,##0.000); }", "{0:$#,##0.0000;($#,##0.0000); }", "{0:$#,##0.00000;($#,##0.00000); }" };

        #endregion Variables

        #region Contructors

        #endregion Contructors

        #region Methods
        public static int ExecuteQuery(string sqlQuery)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=VisionDB.db;Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                cmd.ExecuteNonQuery();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static int ExecuteDeleteQuery(string table, string field, object value)
        {
            try
            {
                string sqlQuery = string.Format(@"delete from {0} where {1} = {2}", table, field, value);
                SQLiteConnection con = new SQLiteConnection("Data Source=VisionDB.db;Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                cmd.ExecuteNonQuery();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static object ExecuteScalar(string sqlQuery)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=VisionDB.db;Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                object value = cmd.ExecuteScalar();
                con.Close();
                return value;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataTable GetTableData(string sqlQuery)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection con = new SQLiteConnection("Data Source= VisionDB.db;Version=3;");
                SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con);

                con.Open();
                da.Fill(ds);
                dt = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return dt;
        }
        #region Chuyển kiểu, ép kiểu
        public static string ToString(object x)
        {
            if (x != null)
            {
                return x.ToString().Trim();
            }
            return "";
        }

        /// <summary>
        /// Chuyển giá trị sang kiểu integer
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static int ToInt(object x)
        {
            try
            {
                return Convert.ToInt32(x);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Chuyển giá trị sang kiểu long
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static Int64 ToInt64(object x)
        {
            try
            {
                return Convert.ToInt64(x);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Chuyển giá trị sang kiểu decimal
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static Decimal ToDecimal(object x)
        {
            try
            {
                return Convert.ToDecimal(x);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Chuyển giá trị sang kiểu double
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static Double ToDouble(object x)
        {
            try
            {
                return Convert.ToDouble(x);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Chuyển giá trị sang kiểu bool
        /// </summary>
        /// <param name="x">giá trị cần chuyển</param>
        /// <returns></returns>
        public static bool ToBoolean(object x)
        {
            try
            {
                return Convert.ToBoolean(x);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Chuyển giá trị chuỗi sang kiểu datetime
        /// </summary>
        /// <param name="date">chuỗi cần chuyển</param>
        /// <returns></returns>
        public static DateTime ToDate(string date)
        {
            try
            {
                try
                {
                    return DateTime.Parse(date, new CultureInfo("en-US", true));
                }
                catch
                {
                    return DateTime.Parse(date, new CultureInfo("fr-FR", true));
                }
            }
            catch
            {
                return Lib.MIN_DATE;
            }
        }

        public static DateTime ToDate1(string date)
        {
            try
            {
                try
                {
                    return DateTime.Parse(date, new CultureInfo("vi-VN", true));
                }
                catch
                {
                    return DateTime.Parse(date, new CultureInfo("fr-FR", true));
                }
            }
            catch
            {
                return Lib.MIN_DATE;
            }
        }

        public static DateTime? ToDate2(object x)
        {
            string date = "";
            if (x != null)
            {
                date = x.ToString();
            }
            try
            {
                try
                {
                    return DateTime.Parse(date, new CultureInfo("en-US", true));
                }
                catch
                {
                    return DateTime.Parse(date, new CultureInfo("fr-FR", true));
                }
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ToDate3(object x)
        {
            string date = "";
            if (x != null)
            {
                date = x.ToString();
            }
            try
            {
                try
                {
                    return DateTime.Parse(date, new CultureInfo("en-US", true));
                }
                catch
                {
                    return DateTime.Parse(date, new CultureInfo("fr-FR", true));
                }
            }
            catch
            {
                return Lib.MIN_DATE;
            }
        }

        /// <summary>
        /// Chuyển giá trị datetime sang kiểu chuỗi ngày tháng định dạng Việt Nam
        /// </summary>
        /// <param name="date">Ngày cần chuyển</param>
        /// <returns></returns>
        public static string DateToStringVN(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Chuyển dạng số thành dạng %
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string FormatPercentNumber(Decimal x)
        {
            return String.Format("{0:#0.00}%", x);
        }

        #endregion Các hàm chuyển kiểu
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Xử lý Chuỗi

        /// <summary>
        /// Viết hoa chữ cái đầu của một chuỗi
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUpperFC(string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// Thao
        /// Đổi từ số sang chữ tiếng việt
        /// </summary>
        /// <param name="Num">Giá trị số cần chuyển</param>
        /// <returns></returns>
        public static string NumericToString(decimal Num)
        {
            string[] Dvi = { "", "ngàn", "triệu", "tỷ", "ngàn" };
            string Result = "";
            long IntNum = (long)Num;
            for (int i = 0; i < 5; i++)
            {
                long Doc = (long)IntNum % 1000;
                if (Doc > 0) Result = NumericToString(Doc, IntNum < 1000) + " " + Dvi[i] + "" + Result;
                IntNum = (long)IntNum / 1000;
            }
            Result += " đồng" + ((((long)Num) % 1000 == 0) ? " chẵn" : "");
            if (Result.ToString().Trim().Substring(0, 1) == "m" && Result.ToString().Trim().Substring(3, 1) == "i")
                Result = "Mười " + Result.Remove(0, 5);
            return Result.Length == 4 ? "..." : Result;
        }

        /// <summary>
        /// Đổi từ số sang chữ Tiếng Anh
        /// </summary>
        /// <param name="number">Giá trị số cần chuyển</param>
        /// <returns></returns>
        public static string NumberToWordsENG(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWordsENG(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWordsENG(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWordsENG(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWordsENG(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        /// <summary>
        /// Dùng dể Xuất Excel
        /// </summary>
        /// <param path="path">Chọn nơi lưu file </param>
        /// <param moduleCode="moduleCode"> Tên file </param>
        /// <returns></returns>

        //public static void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grvData, string path, string moduleCode)
        //{
        //    grvData.OptionsPrint.AutoWidth = false;
        //    grvData.OptionsPrint.ExpandAllDetails = true;
        //    grvData.OptionsPrint.PrintDetails = true;

        //    grvData.OptionsPrint.UsePrintStyles = true;

        //    string filepath = path + "//" + moduleCode + ".xls";

        //    if (filepath != "")
        //    {
        //        try
        //        { grvData.ExportToXls(filepath); }
        //        catch
        //        {
        //            //grvData.ExportToExcelOld(filepath); 
        //        }
        //        Process.Start(filepath);
        //    }
        //}

        /// 
        /// <summary>
        /// Thao
        /// Dùng để đổi dấu phân biệt hàng nghìn, hàng đơn vị trong các câu lệnh UPDATE
        /// </summary>
        /// <param name="strNumber">Giá trị số cần chuyển theo dạng chuỗi</param>
        /// <returns></returns>
        public static string DoiDau(string strNumber)
        {
            int length = 0; int pos = 0; string st = ""; string DoiDau1 = "";
            st = strNumber;
            pos = st.IndexOf(".", 0);
            while (pos > 0)
            {
                st = st.Substring(0, pos) + st.Remove(0, pos);
                pos = st.IndexOf(".", 0);
            }
            length = st.Length;
            pos = st.IndexOf(",", 0);
            if (pos > 0)
                DoiDau1 = st.Substring(0, pos) + "." + st.Remove(0, pos + 1);
            else
                DoiDau1 = st;

            return DoiDau1;
        }

        public static string ArrayToString(string separator, string[] arr)
        {
            if (arr.Length > 0)
            {
                return string.Join(separator, arr);
            }
            else
            {
                return "";
            }

        }
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Xử lý thời gian     

        /// <summary>
        /// Lấy giá trị chênh lệch giữa 2 mốc thời gian
        /// </summary>
        /// <param name="Interval">dạng giá trị cần lấy h: Số giờ, day: số ngày, month: số tháng, year: số năm</param>
        /// <param name="Date1">Ngày bắt đầu</param>
        /// <param name="Date2">Ngày kết thúc</param>
        /// <returns></returns>
        public static int DateDiff(string Interval, DateTime Date1, DateTime Date2)
        {
            int intDateDiff = 0;
            TimeSpan time = Date1 - Date2;
            int timeHours = Math.Abs(time.Hours);
            int timeDays = Math.Abs(time.Days);

            switch (Interval.ToLower())
            {
                case "h": // hours
                    intDateDiff = timeHours;
                    break;
                case "d": // days
                    time = Date1.Date - Date2.Date;
                    intDateDiff = Math.Abs(time.Days);
                    //intDateDiff = timeDays;
                    break;
                case "w": // weeks
                    intDateDiff = timeDays / 7;
                    break;
                case "bw": // bi-weekly
                    intDateDiff = (timeDays / 7) / 2;
                    break;
                case "m": // monthly
                    timeDays = timeDays - ((timeDays / 365) * 5);
                    intDateDiff = timeDays / 30;
                    break;
                case "bm": // bi-monthly
                    timeDays = timeDays - ((timeDays / 365) * 5);
                    intDateDiff = (timeDays / 30) / 2;
                    break;
                case "q": // quarterly
                    timeDays = timeDays - ((timeDays / 365) * 5);
                    intDateDiff = (timeDays / 90);
                    break;
                case "y": // yearly
                    intDateDiff = (timeDays / 365);
                    break;
            }

            return intDateDiff;
        }

        /// <summary>
        /// Thao
        /// Lấy số tuần của 1 năm
        /// </summary>
        /// <param name="Year">Năm cần lấy</param>
        /// <returns></returns>
        public static List<string> LoadAllWeekOfYear(int Year)
        {
            List<DateTime[]> weeks = new List<DateTime[]>();
            List<string> str = new List<string>();

            DateTime beginDate = new DateTime(Year, 01, 01);
            DateTime endDate = new DateTime(Year, 12, 31);

            DateTime monday = DateTime.Today;
            DateTime satday = DateTime.Today;

            while (beginDate < endDate)
            {
                beginDate = beginDate.AddDays(1);

                if (beginDate.DayOfWeek == DayOfWeek.Monday)
                {
                    monday = beginDate;
                }
                else if (beginDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    satday = beginDate;
                }
                else if (beginDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weeks.Add(new DateTime[] { monday, satday });
                }
            }
            int count = 0;
            for (int x = 1; x < weeks.Count; x++)
            {
                if (x == 1)
                {
                    int startDay = weeks[x][0].Date.Day;
                    if (startDay >= 4)
                    {
                        str.Add("Tuần 1: " + "02/01/" + Year + " - 0" + (startDay - 2) + "/01/" + Year);
                        count = 1;
                    }
                }

                str.Add("Tuần " + (x + count) + ": " + (weeks[x][0]).ToString("dd/MM/yyyy") + " - " + (weeks[x][1]).ToString("dd/MM/yyyy"));

                if (x == weeks.Count - 1)
                {
                    int endDay = weeks[x][1].Date.Day;
                    if (endDay <= 29)
                    {
                        str.Add("Tuần " + (weeks.Count + count) + ": " + (endDay + 2) + "/01/" + Year + " - 31/01/" + Year);
                    }
                }
            }
            return str;
        }

        #endregion Cac ham xu li thoi gian
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        #region Xứ lý với File, Folder

        /// <summary>
        /// Lưu trũ một file dựa trên giao diện lưu trữ của window
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter">Kiểu file (.doc,xls,...)</param>
        /// <param name="FileName">Tên file</param>
        /// <returns></returns>
        public static string ShowSaveFileDialog(string title, string filter, string FileName)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = title;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.FileName = FileName;//name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        /// <summary>
        /// Mở một file
        /// </summary>
        /// <param name="fileName">đường dẫn file</param>
        public static void OpenFile(string fileName)
        {
            if (MessageBox.Show("Do you want to open file?", Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    MessageBox.Show("File not found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Kiểm tra xem đường dẫn có phải là 1 file không
        /// </summary>
        /// <param name="filePath">Đường dẫn</param>
        /// <returns></returns>
        public static bool IsFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra xem đường dẫn có phải là 1 thư mục không
        /// </summary>
        /// <param name="filePath">Đường dẫn</param>
        /// <returns></returns>
        public static bool IsFolder(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copy file nhưng không ghi đề file trùng tên mà tạo ra một phiên bản mới. VD: a(1).txt
        /// </summary>
        /// <param name="sourceFile">Đường dẫn file nguồn</param>
        /// <param name="fileName">Tên file</param>
        /// <param name="destinationPath">Thư mục chứa file</param>
        /// <returns></returns>
        public static bool FileCopyWithoutOverwriting(string sourceFile, string fileName, string destinationPath)
        {
            // if destinationPath doesn't add with a slash, add one
            if ((destinationPath.EndsWith("\\") || destinationPath.EndsWith("/")) == false)
                destinationPath += "\\";

            try
            {
                // if file already exists in destination
                if (File.Exists(destinationPath + fileName))
                {
                    // counter
                    int count = 1;

                    // extract extension
                    FileInfo info = new FileInfo(sourceFile);
                    string ext = info.Extension;
                    string prefix;

                    // if it has an extension, append it to a .
                    if (ext.Length > 0)
                    {
                        // get filename without extension
                        prefix = fileName.Substring(0, fileName.Length - ext.Length);
                        //ext = "." + ext;
                    }
                    else
                        prefix = fileName;

                    // while not found an valid destination file name, increase counter
                    while (File.Exists(destinationPath + fileName))
                    {
                        fileName = prefix + "(" + count.ToString() + ")" + ext;
                        count++;
                    }
                    // copy file
                    File.Copy(sourceFile, destinationPath + fileName);
                }
                else
                {
                    File.Copy(sourceFile, destinationPath + fileName);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// CreateFolder(Server.MapPath("Album/Folder1″)); sẽ tạo thư mục Folder1 trong thư mục Album của webroot
        /// </summary>
        /// <param name="strPath">Đường dẫn</param>
        public static void CreateFolder(string strPath)
        {

            try
            {

                if (Directory.Exists(strPath) == false)
                {

                    Directory.CreateDirectory(strPath);

                }

            }

            catch { }

        }
        /// <summary>
        /// RenameFolder(Server.MapPath("Album/Folder1″), Server.MapPath("Album/Folder2″)); 
        /// Sẻ đổi tên thư mục có tên Folder1 thành Folder2 trong thư mục Album của webroot
        /// </summary>
        /// <param name="strOldFolderName"></param>
        /// <param name="strNewFolderName"></param>
        public static void RenameFolder(string strOldFolderName, string strNewFolderName)
        {
            try
            {
                Directory.Move(strOldFolderName, strNewFolderName);
            }
            catch { }
        }
        /// <summary>
        /// Hàm xóa hết các thư mục và file bên trong một thư mục: 
        /// </summary>
        /// <param name="directoryInfo"></param>
        public static void EmptyFolder(DirectoryInfo directoryInfo)
        {

            try
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
                {
                    //EmptyFolder(subfolder);
                    subfolder.Delete(true);
                }
            }
            catch { }

        }
        /// <summary>
        /// Hàm Copy thư mục này đến thư mục khác
        /// </summary>
        /// <param name="ThuMucNguon"></param>
        /// <param name="ThucMucDich"></param>
        public static void CopyDirectory(DirectoryInfo ThuMucNguon, DirectoryInfo ThucMucDich)
        {
            try
            {
                if (!ThucMucDich.Exists)
                {
                    ThucMucDich.Create();
                }

                FileInfo[] files = ThuMucNguon.GetFiles(); foreach (FileInfo file in files)
                {
                    if ((File.Exists(System.IO.Path.Combine(ThucMucDich.FullName, file.Name))) == false)
                    {
                        file.CopyTo(Path.Combine(ThucMucDich.FullName, file.Name));
                    }
                }

                //Xử lý thư mục con
                DirectoryInfo[] dirs = ThuMucNguon.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    string ThucMucDichDir = Path.Combine(ThucMucDich.FullName, dir.Name); CopyDirectory(dir, new DirectoryInfo(ThucMucDichDir));
                }
            }

            catch { }

        }
        /// <summary>
        /// Hàm này sẽ xóa thư mục mục và nội dung bên trong của thư mục được chọn
        /// </summary>
        /// <param name="strFolderName"></param>
        public static void DeleteFolder(string strFolderName)
        {
            DirectoryInfo ThuMucNguonDir = new DirectoryInfo(strFolderName);

            if (Directory.Exists(strFolderName))
            {
                try
                {
                    //EmptyFolder(ThuMucNguonDir);
                    EmptyFolder(ThuMucNguonDir);
                    Directory.Delete(strFolderName);
                }

                catch { }
            }
        }


        #endregion Xứ lý với File
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Control
        #region TextBox
        /// <summary>
        /// Thao
        /// Kiểm tra dữ liệu kiểu số khi nhập vào các ô textbox
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool CheckNumber(KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',' && (e.KeyChar != '-'))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Thao
        /// Kiểm tra dữ liệu kiểu số khi nhập vào các ô textbox
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool CheckInterger(KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                return true;
            else
                return false;
        }

        #endregion TextBox

        #region Combobox

        /// <summary>
        /// Nhập dữ liệu dạng ArrayList vào Combobox
        /// </summary>
        /// <param name="item">tên combobox</param>
        /// <param name="list">dữ liệu</param>
        /// <param name="display">Trường hiển thị</param>
        /// <param name="value">Trường giá trị</param>
        public static void PopulateCombo(ComboBox item, ArrayList list, string display, string value)
        {
            if (list.Count > 0)
            {
                item.DataSource = list;
                item.DisplayMember = display;
                item.ValueMember = value;
            }
            else
            {
                item.DataSource = null;
                item.Items.Clear();
            }
            item.Focus();
        }

        /// <summary>
        /// Nhập dữ liệu dạng DataTable vào Combobox
        /// </summary>
        /// <param name="comboBox">tên combobox</param>
        /// <param name="data">dữ liệu</param>
        /// <param name="DisplayField">trường hiển thị</param>
        /// <param name="ValueField">Trường giá trị</param>
        /// <param name="NotSetItem">Giá trị khởi tạo</param>
        public static void PopulateCombo(ComboBox comboBox, DataTable data, string DisplayField, string ValueField, string NotSetItem)
        {
            if (NotSetItem != "")
            {
                DataRow dr = data.NewRow();
                dr[DisplayField] = NotSetItem;
                dr[ValueField] = 0;
                data.Rows.InsertAt(dr, 0);
            }
            comboBox.DataSource = data;
            comboBox.DisplayMember = DisplayField;
            comboBox.ValueMember = ValueField;
            comboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Đưa dữ liệu vào LookUpEdit trong Devexpress
        /// </summary>
        /// <param name="comboBox">LookUpEdit</param>
        /// <param name="data">dữ liệu muốn hiển thị trên lookupedit</param>
        /// <param name="DisplayField">Trường cần hiển thị</param>
        /// <param name="ValueField">Trường giá trị</param>
        /// <param name="NotSetItem">Giá trị khởi tạo</param>
        //public static void PopulateCombo(DevExpress.XtraEditors.LookUpEdit comboBox, DataTable data, string DisplayField, string ValueField, string NotSetItem)
        //{
        //    if (NotSetItem != null && NotSetItem != "")
        //    {
        //        DataRow dr = data.NewRow();
        //        dr[DisplayField] = NotSetItem;
        //        dr[ValueField] = 0;
        //        data.Rows.InsertAt(dr, 0);
        //    }
        //    comboBox.Properties.Columns.Clear();
        //    comboBox.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[]
        //    {
        //        new DevExpress.XtraEditors.Controls.LookUpColumnInfo(ValueField),
        //        new DevExpress.XtraEditors.Controls.LookUpColumnInfo(DisplayField),
        //    });
        //    //comboBox.Properties.Columns[ValueField].Visible = false;
        //    comboBox.Properties.DataSource = data;
        //    comboBox.Properties.DisplayMember = DisplayField;
        //    comboBox.Properties.ValueMember = ValueField;
        //    if (data.Rows.Count > 0)
        //        comboBox.EditValue = ((DataTable)comboBox.Properties.DataSource).Rows[0][ValueField];
        //}

        #endregion Combobox

        /// <summary>
        /// Reset tất cả các textbox trong form
        /// </summary>
        /// <param name="frm"></param>
        public static void ClearTexbox(Form frm)
        {
            foreach (Control ctrl in frm.Controls)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    ((TextBox)ctrl).Clear();
                }
            }
        }

        /// <summary>
        /// Reset tất cả các textbox trong các container
        /// </summary>
        /// <param name="ctrCollection"></param>
        static public void ClearTextBox(System.Windows.Forms.Control.ControlCollection ctrCollection)
        {
            foreach (object myObject in ctrCollection)
            {
                if (myObject is TextBox)
                {
                    ((TextBox)myObject).Clear();
                }
            }
        }

        /// <summary>
        /// Reset tất cả các textbox trong các container, chọn textbox sẽ được focus 
        /// </summary>
        /// <param name="ctrCollection">list container control</param>
        /// <param name="txtFocus">textbox sẽ được focus </param>
        static public void ClearTextBox(System.Windows.Forms.Control.ControlCollection ctrCollection, TextBox txtFocus)
        {
            ClearTextBox(ctrCollection);
            txtFocus.Focus();
        }
        #endregion Control
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Process Excel

        /// <summary>
        /// Lấy danh sách các tên sheet trong file excel
        /// </summary>
        /// <param name="filePath">Đường dẫn file Excel</param>
        /// <returns></returns>
        public static List<string> ListSheetInExcel(string filePath)
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = filePath;
            if (Path.GetExtension(filePath).Equals(".xls"))//for 97-03 Excel file
            {
                sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
                strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
            }
            else if (Path.GetExtension(filePath).Equals(".xlsx") || Path.GetExtension(filePath).Equals(".xlsm"))  //for 2007 Excel file
            {
                sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
            }
            sbConnection.Add("Extended Properties", strExtendedProperties);

            List<string> listSheet = new List<string>();
            OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
            try
            {
                conn.Open();
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    if (drSheet["TABLE_NAME"].ToString().EndsWith("$") || drSheet["TABLE_NAME"].ToString().EndsWith("$'"))
                    {
                        listSheet.Add(drSheet["TABLE_NAME"].ToString().Replace("$", "").Replace("#", ".").Replace("'", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                conn.Close();
            }

            return listSheet;
        }

        /// <summary>
        /// Lấy dữ liệu file excel dưới dạng Datatable
        /// </summary>
        /// <param name="filename">Đường dẫn file Excel</param>
        /// <param name="sheetName">Tên sheet cần lấy dữ liệu</param>
        /// <returns></returns>
        public static DataTable ExcelToDatatable(string filename)
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = filename;
            if (Path.GetExtension(filename).Equals(".xls"))//for 97-03 Excel file
            {
                sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
                strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
            }
            else if (Path.GetExtension(filename).Equals(".xlsx") || Path.GetExtension(filename).Equals(".xlsm"))  //for 2007 Excel file
            {
                sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
            }
            sbConnection.Add("Extended Properties", strExtendedProperties);
            OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + ListSheetInExcel(filename)[0] + "$]", conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static DataTable ExcelToDatatable(string filename, string sheetName)
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = filename;
            //if (Path.GetExtension(filename).Equals(".xls"))//for 97-03 Excel file
            //{
            //    sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
            //    strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
            //}
            //else //if (Path.GetExtension(filename).Equals(".xlsx"))  //for 2007 Excel file
            //{
            sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
            strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
            //}
            sbConnection.Add("Extended Properties", strExtendedProperties);
            OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
            DataTable dt = new DataTable();
            try
            {
                conn.Open();

                //OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + ListSheetInExcel(filename)[0] + "$]", conn);
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + sheetName.Replace("'", "") + "$]", conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static DataTable ExcelToDatatableNoHeader(string filename, string sheetName)
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = filename;
            //if (Path.GetExtension(filename).Equals(".xls"))//for 97-03 Excel file
            //{
            //    sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
            //    strExtendedProperties = "Excel 8.0;HDR=No;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
            //}
            //else //if (Path.GetExtension(filename).Equals(".xlsx"))  //for 2007 Excel file
            //{
            sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
            strExtendedProperties = "Excel 12.0;HDR=No;IMEX=1";
            //}
            sbConnection.Add("Extended Properties", strExtendedProperties);
            OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
            DataTable dt = new DataTable();
            try
            {
                conn.Open();

                //OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + ListSheetInExcel(filename)[0] + "$]", conn);
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + sheetName.Replace("'", "") + "$]", conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static void GridToCsv(DataGridView dGV, string filePathCSV)
        {
            string stOutput = "";
            string sHeaders = "";
            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";
            // Export data.
            for (int i = 0; i < dGV.RowCount - 1; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + dGV.Rows[i].Cells[j].Value.ToString() + "\t";
                stOutput += stLine + "\r\n";
            }
            FileStream fs = new FileStream(filePathCSV, FileMode.Create);
            StreamWriter bw = new StreamWriter(fs, Encoding.Unicode);
            bw.Write(stOutput); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        public static void DatatableToCSV(DataTable dataTable, string filePathCSV)
        {
            string stOutput = "";
            string sHeaders = "";
            for (int j = 0; j < dataTable.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dataTable.Columns[j].Caption.Replace("\n", "")) + "\t";
            stOutput += sHeaders + "\r\n";
            // Export data.
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string stLine = "";
                for (int j = 0; j < dataTable.Rows[i].ItemArray.Count(); j++)
                    stLine = stLine.ToString() + dataTable.Rows[i].ItemArray[j].ToString().Replace("\n", "") + "\t";
                stOutput += stLine + "\r\n";
            }
            FileStream fs = new FileStream(filePathCSV, FileMode.Create);
            StreamWriter bw = new StreamWriter(fs, Encoding.Unicode);
            bw.Write(stOutput); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }
        #endregion Process Excel       

        #region Cac ham xu li khac


        /// <summary>
        /// Hàm trả địa chỉ IP hoặc tên máy hiện tại
        /// </summary>
        /// <param name="IP"></param>
        /// <returns>IP=true: Trả lại địa chỉ IP. IP=false: Trả lại tên máy</returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// Kiểm tra một chuối có phải là định dạng của email không
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(string str)
        {
            bool State = true;
            if (!Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                State = false;
            }

            return State;
        }

        private static string NumberToString(long Num)
        {
            string[] Number = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            return Number[Num];
        }

        public static string NumberToStringH(long Num)
        {
            string[] Number = { "không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín" };
            return Number[Num];
        }

        /// <summary>
        /// Chuyển kiểu số sang kiểu chữ tiếna việt
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="Dau"></param>
        /// <returns></returns>
        public static string NumericToString(long Num, bool Dau)
        {
            long Tram = (long)Num / 100;
            long Chuc = (long)(Num % 100) / 10;
            long Dvi = Num % 10;

            string Doc = (((Tram == 0) && (Dau)) ? "" : (" " + NumberToString(Tram) + " trăm")) + ((Chuc == 0) ? (((Tram == 0) && Dau) ? "" : ((Dvi == 0) ? "" : " lẻ")) : ((Chuc == 1) ? " mười" : (" " + NumberToString(Chuc) + " mươi"))) + (((Dvi == 5) && (Chuc > 0)) ? " năm" : ((Dvi == 0) ? "" : " " + NumberToString(Dvi)));
            return Doc;
        }

        /// <summary>
        /// Định dạng số dưới dạng tiền Việt Nam
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string FormatVND(decimal amount)
        {
            if (amount == 0) { return "0"; }
            else
            {
                return amount.ToString(Lib.CurrencyFormatVND);
            }
        }

        /// <summary>
        /// Hiển thị Thông báo lỗi
        /// </summary>
        /// <param name="ex"></param>
        public static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, Lib.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        public static void ShowError(string content)
        {
            MessageBox.Show(content, Lib.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        public static void ShowError(string content, Exception ex)
        {
            MessageBox.Show(content + Environment.NewLine + ex.Message, Lib.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        public static void ReleaseComObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
        }

        public static void EndProcess(string processName)
        {
            Process[] ps = Process.GetProcesses().Where(o => o.ProcessName.ToUpper().Contains(processName.ToUpper())).ToArray();
            foreach (Process p in ps)
            {
                p.Kill();
            }
        }
        #endregion

        public static void ShowInfor(string content)
        {
            MessageBox.Show(content, Lib.Caption, MessageBoxButtons.OK, MessageBoxIcon.Information,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly);
        }
        public static void ShowWarning(string content)
        {
            MessageBox.Show(content, Lib.Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private static void ViewTime(Label label, ToolStripLabel show)
        {
            while (true)
            {
                try
                {
                    if (label.InvokeRequired)
                        label.Invoke((MethodInvoker)delegate
                        {
                            label.Text = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
                            show.Text = label.Text;
                        });
                    else
                    {
                        label.Text = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
                        show.Text = label.Text;
                    }
                    Thread.Sleep(100);
                }
                catch (Exception)
                {

                }
            }
        }
        public static void ViewTimeToLabel(Label label, ToolStripLabel Show)
        {
            Thread thread = new Thread(() => ViewTime(label, Show))
            {
                IsBackground = true
            };
            thread.Start();
        }

        private static void ChangeLang_Execute(DataTable dtLang, Control.ControlCollection control, string langeName)
        {
            foreach (Control item in control)
            {

                if (item.GetType() == typeof(DataGridView))
                {
                    DataGridView gridView = ((DataGridView)item);
                    foreach (DataGridViewColumn column in gridView.Columns)
                    {
                        DataRow row = dtLang.Rows.Find(column.Name);
                        if (row != null)
                            column.HeaderText = Lib.ToString(row[langeName]);
                    }
                }

                if (item.Tag != null)
                {
                    DataRow row = dtLang.Rows.Find(item.Tag.ToString());
                    if (row != null)
                        item.Text = Lib.ToString(row[langeName]);
                }

                if (item.Controls != null)
                    ChangeLang_Execute(dtLang, item.Controls, langeName);
            }
        }
        public static void ChangeLang(Control.ControlCollection control, string langeName)
        {
            string sql = "Select * from Languages";
            DataTable dt = Lib.GetTableData(sql);
            var keys = new DataColumn[1];
            keys[0] = dt.Columns[ColName.NameTag];
            dt.PrimaryKey = keys;
            ChangeLang_Execute(dt, control, langeName);
            GlobVar.LangChoose = langeName;
        }

        public static void SaveToLog(string error)
        {
            try
            {
                string path = Application.StartupPath + @"\Error\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string content = DateTime.Now.ToString("HH:mm:ss") + "----------->" + error + "\n";
                string filename = path + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (File.Exists(filename))
                {
                    File.AppendAllText(filename, content);
                }
                else
                {
                    FileStream fs = new FileStream(filename, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        public static void SaveToLog(string folder, string jig, string error)
        {
            try
            {
                string path = Application.StartupPath + @"\" + folder + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string content = DateTime.Now.ToString("HH:mm:ss") + "----------->" + $"{jig}\r\n{error}" + "\r\n";
                string filename = path + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (File.Exists(filename))
                {
                    File.AppendAllText(filename, content);
                }
                else
                {
                    FileStream fs = new FileStream(filename, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
            catch (Exception)
            {

            }
        }
        public static void SaveToLog(string folder, string jig, string charSpec, string error)
        {
            try
            {
                string path = Application.StartupPath + @"\" + folder + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string content = DateTime.Now.ToString("HH:mm:ss") + "-->" + $"{jig}{charSpec}{error}" + "\r\n";
                string filename = path + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (File.Exists(filename))
                {
                    File.AppendAllText(filename, content);
                }
                else
                {
                    FileStream fs = new FileStream(filename, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
            catch (Exception)
            {

            }
        }
        public static void SaveToLog(string folder,DateTime start, DateTime stop, string Send, string Receive,string Jig,double TimeSpan)
        {
            try
            {
                string path = Application.StartupPath + @"\"+ folder + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string content = /*DateTime.Now.ToString("HH:mm:ss")*/  $"\r\nJig:{Jig}\r\n{start.ToString("HH:mm:ss")} {Send}\r\n{stop.ToString("HH:mm:ss")} {Receive}" + $"\r\nTimeSpan:{TimeSpan}\r\n";
                string filename = path + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (File.Exists(filename))
                {
                    File.AppendAllText(filename, content);
                }
                else
                {
                    FileStream fs = new FileStream(filename, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
            catch (Exception)
            {

            }
        }
        public static string CreateString(int length, string character)
        {
            StringBuilder stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(character);
            }
            string result = stringBuilder.ToString();
            return result;
        }
        //public static bool CheckPermission(Guid userID, string tenPhanHe)
        //{

        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="fileSample"></param>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="dtAddCol"></param>
        /// <param name="dgvAddRow"></param>
        public static void SaveToExcel_GemBox(DataGridView dgv, string fileSample, string fileName, string sheetName, DataTable[] dtAddCol, bool dgvAddRow)
        {
            try
            {
                //ExcelFile workbook = new ExcelFile();
                //workbook.Worksheets.Add("12345");
                //workbook.Save(@"11111.xlsx");

                int totalRowDgv;
                #region nếu dgv đang để ở chế độ  AllowUserToAddRows
                if (dgvAddRow)
                {
                    totalRowDgv = dgv.RowCount - 1;
                }
                #endregion

                #region nếu dgv đã tắt chế độ AllowUserToAddRows
                else
                {
                    totalRowDgv = dgv.RowCount;
                }
                #endregion

                SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
                //tạo mới một Workbooks bằng phương thức add()
                ExcelFile workbook = ExcelFile.Load(fileSample);
                ExcelWorksheet worksheet = workbook.Worksheets[0];

                //đặt tên cho sheet
                if (workbook.Worksheets.FirstOrDefault(x => x.Name.ToLower() == sheetName.ToLower()) == null)
                {
                    worksheet.Name = sheetName;
                }

                worksheet.Charts[0].Series[0].ValuesReference = "'" + sheetName + "'!$K$3:$M$3";
                worksheet.Charts[0].CategoryLabelsReference = "'" + sheetName + "'!$K$2:$M$2";
                // export header trong DataGridView
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dgv.Columns[i].HeaderText;
                }
                // export nội dung trong DataGridView
                for (int i = 0; i < totalRowDgv; i++)
                {
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dgv.Rows[i].Cells[j].Value.ToString();
                    }
                }

                int numberColbgein = dgv.ColumnCount + 3;
                //////////////
                //export header 
                //for (int i = 0; i < dtAddCol.Length; i++)
                //{
                //    worksheet.Cells[1, i + numberColbgein].Value = dtAddCol[i].Columns[0].Caption;
                //}

                ///
                //export value
                for (int i = 0; i < dtAddCol.Length; i++)
                {
                    for (int j = 0; j < dtAddCol[i].Rows.Count; j++)
                    {
                        worksheet.Cells[j + 2, i + numberColbgein].Value = dtAddCol[i].Rows[j][0];
                    }
                }

                workbook.Save(fileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public static void SaveToExcel_GemBox(DataTable dtTable, string fileSample, string fileName, string sheetName, DataTable[] dtAddCol)
        {

            try
            {
                SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
                //tạo mới một Workbooks bằng phương thức add()
                ExcelFile workbook = ExcelFile.Load(fileSample);
                ExcelWorksheet worksheet = workbook.Worksheets[0];

                //đặt tên cho sheet
                if (workbook.Worksheets.FirstOrDefault(x => x.Name.ToLower() == sheetName.ToLower()) == null)
                {
                    worksheet.Name = sheetName;
                }

                worksheet.Charts[0].Series[0].ValuesReference = "'" + sheetName + "'!$K$3:$M$3";
                worksheet.Charts[0].CategoryLabelsReference = "'" + sheetName + "'!$K$2:$M$2";
                // export header trong DataGridView
                for (int i = 0; i < dtTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dtTable.Columns[i].Caption;
                }
                // export nội dung trong DataGridView
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dtTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dtTable.Rows[i][j].ToString();
                    }
                }

                int numberColbgein = dtTable.Columns.Count + 3;
                //////////////
                //export header 
                //for (int i = 0; i < dtAddCol.Length; i++)
                //{
                //    worksheet.Cells[1, i + numberColbgein].Value = dtAddCol[i].Columns[0].Caption;
                //}

                ///
                //export value
                for (int i = 0; i < dtAddCol.Length; i++)
                {
                    for (int j = 0; j < dtAddCol[i].Rows.Count; j++)
                    {
                        worksheet.Cells[j + 2, i + numberColbgein].Value = dtAddCol[i].Rows[j][0];
                    }
                }

                workbook.Save(fileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }





        public static DataTable ReadFileExcelModel(string fileSource, DataGridView dgv, DataTable dt, int colBegin, int rowBegin, int numberCol, bool overwrite)
        {
            SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
            ExcelFile workbook = ExcelFile.Load(fileSource);
            ExcelWorksheet worksheet = workbook.Worksheets[0];
            DataTable dt_add = new DataTable();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                // dt_add.Columns.Add(dt.Columns[i], typeof(string));
                dt_add.Columns.Add(dgv.Columns[i].DataPropertyName, typeof(string));
            }

            bool isRun = true;
            if (overwrite)
            {
                while (isRun)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + 1].Value?.ToString()) ||
                        string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + 2].Value?.ToString()))
                    {
                        break;
                    }

                    DataRow row = dt_add.NewRow();
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        row[dgv.Columns[i].DataPropertyName] = "";
                    }
                    for (int i = 0; i < numberCol; i++)
                    {
                        if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + i].Value?.ToString()))
                            continue;
                        row[i] = worksheet.Cells[rowBegin, colBegin + i].Value.ToString();
                    }
                    dt_add.Rows.InsertAt(row, dt_add.Rows.Count);
                    rowBegin += 1;
                }
            }
            else
            {
                while (isRun)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + 1].Value?.ToString()) ||
                        string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + 2].Value?.ToString()))
                    {
                        break;
                    }

                    var keys = new DataColumn[1];
                    keys[0] = dt.Columns[1];
                    dt.PrimaryKey = keys;
                    DataRow rowcheck = dt.Rows.Find(worksheet.Cells[rowBegin, colBegin + 1].Value);
                    if (rowcheck != null)
                    {
                        rowBegin += 1;
                        continue;
                    }
                    DataRow row = dt_add.NewRow();
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        row[dgv.Columns[i].DataPropertyName] = "";
                    }
                    for (int i = 0; i < numberCol; i++)
                    {
                        if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + i].Value?.ToString()))
                            continue;
                        row[i] = worksheet.Cells[rowBegin, colBegin + i].Value.ToString();
                    }
                    dt_add.Rows.InsertAt(row, dt_add.Rows.Count);
                    rowBegin += 1;
                }
            }
            return dt_add;
        }

        public static DataTable ReadFileExcelUser(string fileSource, DataGridView dgv, DataTable dt, int colBegin, int rowBegin, int numberCol, bool overwrite)
        {
            SpreadsheetInfo.SetLicense("ELAP-G41W-CZA2-XNNC");
            ExcelFile workbook = ExcelFile.Load(fileSource);
            ExcelWorksheet worksheet = workbook.Worksheets[0];
            DataTable dt_add = new DataTable();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i].DataPropertyName == "ID" || dgv.Columns[i].DataPropertyName == "Password")
                    continue;
                dt_add.Columns.Add(dgv.Columns[i].DataPropertyName, typeof(string));
            }

            bool isRun = true;
            if (overwrite)
            {
                while (isRun)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin].Value?.ToString()) ||
                        string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + 1].Value?.ToString()))
                    {
                        break;
                    }

                    DataRow row = dt_add.NewRow();
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        if (dgv.Columns[i].DataPropertyName == "ID" || dgv.Columns[i].DataPropertyName == "Password")
                            continue;
                        row[dgv.Columns[i].DataPropertyName] = "";
                    }
                    for (int i = 0; i < numberCol; i++)
                    {
                        if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + i].Value?.ToString()))
                            continue;
                        row[i] = worksheet.Cells[rowBegin, colBegin + i].Value.ToString();
                    }
                    dt_add.Rows.InsertAt(row, dt_add.Rows.Count);
                    rowBegin += 1;
                }
            }
            else
            {
                while (isRun)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + 1].Value?.ToString()) ||
                        string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + 2].Value?.ToString()))
                    {
                        break;
                    }

                    var keys = new DataColumn[1];
                    keys[0] = dt.Columns[1];
                    dt.PrimaryKey = keys;
                    DataRow rowcheck1 = dt.Rows.Find(worksheet.Cells[rowBegin, colBegin].Value);
                    DataRow rowcheck2 = dt.Rows.Find(worksheet.Cells[rowBegin, colBegin + 1].Value);
                    if (rowcheck1 != null || rowcheck2 != null)
                    {
                        rowBegin += 1;
                        continue;
                    }
                    DataRow row = dt_add.NewRow();
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        if (dgv.Columns[i].DataPropertyName == "ID" || dgv.Columns[i].DataPropertyName == "Password")
                            continue;
                        row[dgv.Columns[i].DataPropertyName] = "";
                    }
                    for (int i = 0; i < numberCol; i++)
                    {
                        if (string.IsNullOrEmpty(worksheet.Cells[rowBegin, colBegin + i].Value?.ToString()))
                            continue;
                        row[i] = worksheet.Cells[rowBegin, colBegin + i].Value.ToString();
                    }
                    dt_add.Rows.InsertAt(row, dt_add.Rows.Count);
                    rowBegin += 1;
                }
            }
            return dt_add;
        }

        public static void CopyFileTo(string sourceFile, string desiFileName)
        {
            if (!File.Exists(sourceFile))
            {
                MessageBox.Show("file sample ko tồn tại");
                return;
            }
            else
            {
                File.Copy(sourceFile, desiFileName, false);
            }
        }

        public static void CopyFileTo(string sourceFile, string destFileName, string name, string fileExt)
        {
            bool isFileUse;
            if (!Directory.Exists(destFileName))
            {
                MessageBox.Show("đường dẫn file ko tồn tại");
                return;
            }
            isFileUse = FileIsUsed(sourceFile);
            if (isFileUse)
            {
                MessageBox.Show("LOG ERROR");
            }
            else
            {
                try
                {
                    if (!File.Exists(destFileName + @"\" + fileExt))
                    {
                        Directory.CreateDirectory(destFileName + @"\" + fileExt);
                        File.Copy(sourceFile, destFileName + @"\" + fileExt + @"\" + name, true);
                    }
                    else
                        File.Copy(sourceFile, destFileName + @"\" + fileExt + @"\" + name, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    SaveToLog(ex.ToString());
                }
            }
        }

        public static void CopyFileTo_Daily(string sourceFile, string destFileName, string name)
        {
            bool isFileUse;
            if (!Directory.Exists(destFileName))
            {
                MessageBox.Show("đường dẫn file ko tồn tại");
                return;
            }
            isFileUse = FileIsUsed(sourceFile);
            if (isFileUse)
            {
                MessageBox.Show("LOG ERROR");
            }
            else
            {
                try
                {
                    if (!File.Exists(destFileName + @"\Save_Log_Daily"))
                    {
                        Directory.CreateDirectory(destFileName + @"\Save_Log_Daily");
                        File.Copy(sourceFile, destFileName + @"\Save_Log_Daily\" + name, true);
                    }
                    else
                        File.Copy(sourceFile, destFileName + @"\Save_Log_Daily\" + name, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    SaveToLog(ex.ToString());
                }
            }
        }

        private static bool FileIsUsed(string fileName)
        {
            bool check = false;
            try
            {
                FileStream fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Close();
            }
            catch (Exception ex)
            {
                check = true;
                SaveToLog(ex.ToString());
            }
            return check;
        }

        public static bool CheckEmpty(string ObjectCheck, string stringShow)
        {
            if (ObjectCheck == "")
            {
                MessageBox.Show($"không được để trống {stringShow}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
                return false;
        }

        #endregion

        #region thao tác dữ liệu với File Server
        public static int ExecuteQuery(string sqlQuery, string FilePathServer)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection($"Data Source={FilePathServer};Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                cmd.ExecuteNonQuery();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static object ExecuteScalar(string sqlQuery, string FilePathServer)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection($"Data Source={FilePathServer};Version=3;");
                SQLiteCommand cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = sqlQuery;//"delete from Student where ID = 0";
                object value = cmd.ExecuteScalar();
                con.Close();
                return value;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataTable GetTableData(string sqlQuery, string FilePathServer)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection con = new SQLiteConnection($"Data Source={FilePathServer};Version=3;");
                SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, con);

                con.Open();
                da.Fill(ds);
                dt = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return dt;
        }
        #endregion

        #region BUTTON
        public static void ShowButton(string value, Button btn, int result)
        {
            if (btn.InvokeRequired) btn.Invoke(new Action(() => btn.Text = value));
            else btn.Text = value;
            switch (result)
            {
                case 1:
                    {
                        if (btn.InvokeRequired) btn.Invoke(new Action(() => btn.BackColor = Color.FromName("ActiveCaption")));
                        else btn.BackColor = Color.FromName("ActiveCaption");
                        break;
                    }
                case 2:
                    {
                        if (btn.InvokeRequired) btn.Invoke(new Action(() => btn.BackColor = Color.DarkOrange));
                        else btn.BackColor = Color.DarkOrange;
                        break;
                    }
                case 3:
                    {
                        if (btn.InvokeRequired) btn.Invoke(new Action(() => btn.BackColor = Color.Red));
                        else btn.BackColor = Color.Red;
                        break;
                    }
            }

        }
        #endregion

        #region LABEL
        public static void ShowLabelResult(Color color, Label lbl, string text)
        {
            if (lbl == null) return;

            if (lbl.InvokeRequired)
            {
                lbl.Invoke(new Action(() => lbl.Text = text));
                lbl.Invoke(new Action(() => lbl.BackColor = color));
            }
            else
            {
                lbl.Text = text;
                lbl.BackColor = color;
            }

        }
        public static void EnableLabel(Label lbl, bool status)
        {
            try
            {
                if (lbl == null) return;

                if (lbl.InvokeRequired)
                {
                    lbl.Invoke(new Action(() => lbl.Visible = status));
                }
                else
                {
                    lbl.Visible = status;
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region KIỂM TRA KẾT NỐI ĐẾN MÁY KHÁC
        public static bool PingCheck(string AFM_IP)
        {
            try
            {
                bool chk = false;
                //copy_csvfile_to_PC(nameFile);
                Ping ping = new Ping();
                string IP = AFM_IP.Split('\\')[2].Trim();

                PingReply pingresult = ping.Send(IP);
                if (pingresult.Status.ToString() == "Success")
                {
                    chk = true;
                }
                else
                {
                    chk = false;
                    //MessageBox.Show("Ping Timeout");
                }
                return chk;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SaveToLog(ex.ToString());
                return false;
            }
        }
        #endregion

        #region TASK THỰC THI TRONG KHOẢNG THỜI GIAN
        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            //bool IsExecuteFunction = true;
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan - TimeSpan.FromMilliseconds(500));
                IsExecuteFunction = false;
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
                return false;
            }
        }
        #endregion

        #region File.Exe Is Running
        public static bool ProgramIsRunning(string FullPath)
        {
            string FilePath = Path.GetDirectoryName(FullPath);
            string FileName = Path.GetFileNameWithoutExtension(FullPath).ToLower();
            bool isRunning = false;

            Process[] pList = Process.GetProcessesByName(FileName);
            if (pList.Length > 0)
            {
                isRunning = true;
            }
            //foreach (Process p in pList)
            //{
            //    if (p.MainModule.FileName.StartsWith(FilePath, StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        isRunning = true;
            //        break;
            //    }
            //}

            return isRunning;
        }
        #endregion
    }
}