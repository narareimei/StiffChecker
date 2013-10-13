using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using NUnit.Framework;

namespace Stiff
{
    [TestFixture]
    public partial class Stiffer
    {
        [Test]
        public void シングルトン()
        {
            var st1 = Stiffer.GetInstance();
            Assert.True(st1 != null);

            var st2 = Stiffer.GetInstance();
            Assert.True(st1.Equals(st2));

            st1.Dispose();
            st2.Dispose();
        }


        [Test]
        public void アプリケーション起動()
        {
            //Assert.True(1 == 1);
            var st = Stiffer.GetInstance();

            st.CreateApplication();
            {
                Assert.True(st._app != null, "１回目");
                Assert.True(st._app.DisplayAlerts == false, "１回目 画面表示設定");

                var ap = st._app;
                st.CreateApplication();
                Assert.True(ap.Equals(st._app), "２回目");

                Marshal.ReleaseComObject(ap);
                Marshal.ReleaseComObject(st._app);
                ap = null;
                st._app = null;
            }
            st.Dispose();
            return;
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ワークブックオープン_アプリ未起動()
        {
            var st = Stiffer.GetInstance();
            {
                Assert.True(st.OpenBook(@"c:\hoge.xls") == null);
            }
            st.Dispose();
        }

        [Test]
        [ExpectedException(typeof(System.Runtime.InteropServices.COMException))]
        public void ワークブックオープン_該当なし()
        {
            var st = Stiffer.GetInstance();
            st.CreateApplication();
            {
                Assert.True(st.OpenBook(@"c:\hoge.xls") == null);
            }
            st.Dispose();
        }

        [Test]
        public void ワークブックオープン_該当あり()
        {
            var st = Stiffer.GetInstance();
            st.CreateApplication();
            {
                var cd = System.IO.Directory.GetCurrentDirectory();
                Excel.Workbook oBook = null;

                try
                {
                    oBook = st.OpenBook(cd + @"\TestBook.xlsx");
                    Assert.True(oBook != null, "ファイルオープン");

                    var filename = oBook.FullName.ToString().ToUpper();
                    Assert.True(filename == (cd + @"\TestBook.xlsx").ToUpper(), "ファイルパス");
                }
                finally
                {
                    if (oBook != null)
                        Marshal.ReleaseComObject(oBook);
                    oBook = null;
                }
            }
            st.Dispose();
        }

        [Test]
        public void プロパティ_なし()
        {
            var st = Stiffer.GetInstance();
            st.CreateApplication();
            {
                var cd = System.IO.Directory.GetCurrentDirectory();
                Excel.Workbook oBook = null;

                try
                {
                    oBook = st.OpenBook(cd + @"\TestBook.xlsx");
                    string value = st.GetBuiltinProperty(oBook, "hoge");
                    Assert.True( value == "");
                }
                finally
                {
                    if (oBook != null)
                        Marshal.ReleaseComObject(oBook);
                    oBook = null;
                }
            }
            st.Dispose();
        }

        [Test]
        public void プロパティ_有り()
        {
            var st = Stiffer.GetInstance();
            st.CreateApplication();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();
                Excel.Workbook oBook = null;

                try
                {
                    oBook = st.OpenBook(cd + @"\TestBook.xlsx");
                    string value = st.GetBuiltinProperty(oBook, "Author");
                    Assert.True(value == "小林礼明", "Author");
                    Assert.True(st.GetBuiltinProperty(oBook, "Title") == "テスト用ブック", "タイトル");
                    Assert.True(st.GetBuiltinProperty(oBook, "Subject") == "Stiff", "サブタイトル");
                    Assert.True(st.GetBuiltinProperty(oBook, "Company") == "個人", "会社");
                    Assert.True(st.GetBuiltinProperty(oBook, "Manager") == "わし", "管理者" + st.GetBuiltinProperty(oBook, "Manager"));
                    Assert.True(st.GetBuiltinProperty(oBook, "Last Save Time") == "2013/10/13 5:49:18", "前回保存日時");
                }
                finally
                {
                    if (oBook != null)
                        Marshal.ReleaseComObject(oBook);
                    oBook = null;
                }
            }
            finally
            {
                st.Dispose();
            }
        }


        [Test]
        [ExpectedException(typeof(System.Runtime.InteropServices.COMException))]
        public void ブック情報取得_該当なし()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var info = st.GetBookInformations(cd + @"\Hoge.xlsx");
            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        public void ブック情報取得_該当あり()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var info = st.GetBookInformations(cd + @"\TestBook.xlsx");

                Assert.True(info != null, "ヌルチェック");
                Assert.True(info.Author         == "小林礼明", "Author");
                Assert.True(info.Title          == "テスト用ブック", "タイトル");
                Assert.True(info.Subject        == "Stiff", "サブタイトル");
                Assert.True(info.Company        == "個人", "会社");
                Assert.True(info.Manager        == "わし", "管理者");
                Assert.True(info.LastSaveTime   == "2013/10/13 5:49:18", "前回保存日時");

                Assert.True(info.Sheets[0].Name           == "First", "Name");
                Assert.True(info.Sheets[0].CellPosition.X == 2,        "Cell.Left");
                Assert.True(info.Sheets[0].CellPosition.Y == 2,        "Cell.Top");
                Assert.True(info.Sheets[0].Zoom           == 175,      "Zoom");
                Assert.True(info.Sheets[0].Gridlines      == true,     "Gridlines");
                Assert.True(info.Sheets[0].View           == Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView,
                                                             "View");
                Assert.True(info.Sheets[1].Name           == "Second", "Name");
                Assert.True(info.Sheets[1].CellPosition.X == 6,        "Cell.Left");
                Assert.True(info.Sheets[1].CellPosition.Y == 7,        "Cell.Top");
                Assert.True(info.Sheets[1].Zoom           == 90,       "Zoom");
                Assert.True(info.Sheets[1].Gridlines      == false,    "Gridlines");
                Assert.True(info.Sheets[1].View           == Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView,
                                                             "View");
                Assert.True(info.Sheets[2].Name           == "Third", "Name");
                Assert.True(info.Sheets[2].CellPosition.X == 3,        "Cell.Left");
                Assert.True(info.Sheets[2].CellPosition.Y == 10,       "Cell.Top");
                Assert.True(info.Sheets[2].Zoom           == 60,       "Zoom");
                Assert.True(info.Sheets[2].Gridlines      == false,    "Gridlines");
                Assert.True(info.Sheets[2].View           == Microsoft.Office.Interop.Excel.XlWindowView.xlPageBreakPreview,
                                                             "View");

            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        //[ExpectedException(typeof(System.Runtime.InteropServices.COMException))]
        public void ブック情報取得_Excel外ファイル()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var info = st.GetBookInformations(cd + @"\StiffChecker.exe");
                Assert.True( info.LastSaveTime == "" );
            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        public void プロパティ_変更()
        {
            var st = Stiffer.GetInstance();
            st.CreateApplication();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();
                Excel.Workbook oBook = null;

                try
                {
                    BookInfo info = this.GetBookInformations(cd + @"\TestBook.xlsx");

                    info.Author  = "kobayashi";
                    info.Title   = "Modified";
                    info.Subject = "Tool";
                    info.Company = "The Man";
                    info.Manager = "Myself";

                    // 書き換え
                    oBook = st.OpenBook(cd + @"\TestBook.xlsx");
                    this.SetInformations(oBook, info);

                    // 確認
                    Assert.True(st.GetBuiltinProperty(oBook, "Author"        ) == "kobayashi", "Author");
                    Assert.True(st.GetBuiltinProperty(oBook, "Title"         ) == "Modified", "タイトル");
                    Assert.True(st.GetBuiltinProperty(oBook, "Subject"       ) == "Tool", "サブタイトル");
                    Assert.True(st.GetBuiltinProperty(oBook, "Company"       ) == "The Man", "会社");
                    Assert.True(st.GetBuiltinProperty(oBook, "Manager"       ) == "Myself", "管理者" + st.GetBuiltinProperty(oBook, "Manager"));
                    Assert.True(st.GetBuiltinProperty(oBook, "Last Save Time") == "2013/10/13 5:49:18", "前回保存日時");
                }
                finally
                {
                    if (oBook != null)
                        Marshal.ReleaseComObject(oBook);
                    oBook = null;
                }
            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        public void シート情報_該当あり_シート１()
        {
            Excel.Workbook   oBook          = null;
            Excel.Sheets     oSheets        = null;
            Excel.Worksheet  oSheet         = null;

            var st = Stiffer.GetInstance();
            try
            {
                st.CreateApplication();

                var cd = System.IO.Directory.GetCurrentDirectory();
                oBook   = st.OpenBook(cd + @"\TestBook.xlsx");
                oSheets = oBook.Worksheets;
                oSheet  = (Excel.Worksheet)oSheets[1];

                var info = st.GetSheetInformation(oSheet);


                Assert.True(info                != null,    "ヌルチェック");
                Assert.True(info.Name           == "First", "Name");
                Assert.True(info.CellPosition.X == 2,        "Cell.Left");
                Assert.True(info.CellPosition.Y == 2,        "Cell.Top");
                Assert.True(info.Zoom           == 175,      "Zoom");
                Assert.True(info.Gridlines      == true,     "Gridlines");
                Assert.True(info.View           == Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView,
                                                             "View");
            }
            finally
            {
                if (oSheet != null)
                    Marshal.ReleaseComObject(oSheet);
                oSheet = null;

                if (oSheets != null)
                    Marshal.ReleaseComObject(oSheets);
                oSheets = null;

                if (oBook != null)
                    Marshal.ReleaseComObject(oBook);
                oBook = null;

                st.Dispose();
            }
            return;
        }


        [Test]
        public void ブックのシート情報取得()
        {
            Excel.Workbook oBook = null;
            Excel.Sheets oSheets = null;
            Excel.Worksheet oSheet = null;

            var st = Stiffer.GetInstance();
            try
            {
                st.CreateApplication();

                var cd = System.IO.Directory.GetCurrentDirectory();
                oBook = st.OpenBook(cd + @"\TestBook.xlsx");

                var infos = st.GetSheetInformations(oBook);

                Assert.True(infos                   != null, "ヌルチェック");
                Assert.True(infos[0].Name           == "First", "Name");
                Assert.True(infos[0].CellPosition.X == 2,        "Cell.Left");
                Assert.True(infos[0].CellPosition.Y == 2,        "Cell.Top");
                Assert.True(infos[0].Zoom           == 175,      "Zoom");
                Assert.True(infos[0].Gridlines      == true,     "Gridlines");
                Assert.True(infos[0].View           == Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView,
                                                             "View");
                Assert.True(infos[1].Name           == "Second", "Name");
                Assert.True(infos[1].CellPosition.X == 6,        "Cell.Left");
                Assert.True(infos[1].CellPosition.Y == 7,        "Cell.Top");
                Assert.True(infos[1].Zoom           == 90,       "Zoom");
                Assert.True(infos[1].Gridlines      == false,    "Gridlines");
                Assert.True(infos[1].View           == Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView,
                                                             "View");
                Assert.True(infos[2].Name           == "Third", "Name");
                Assert.True(infos[2].CellPosition.X == 3,        "Cell.Left");
                Assert.True(infos[2].CellPosition.Y == 10,       "Cell.Top");
                Assert.True(infos[2].Zoom           == 60,       "Zoom");
                Assert.True(infos[2].Gridlines      == false,    "Gridlines");
                Assert.True(infos[2].View           == Microsoft.Office.Interop.Excel.XlWindowView.xlPageBreakPreview,
                                                             "View");
            }
            finally
            {
                if (oSheet != null)
                    Marshal.ReleaseComObject(oSheet);
                oSheet = null;

                if (oSheets != null)
                    Marshal.ReleaseComObject(oSheets);
                oSheets = null;

                if (oBook != null)
                    Marshal.ReleaseComObject(oBook);
                oBook = null;

                st.Dispose();
            }
            return;
        }

        [Test]
        public void シートチェック_同じ()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var criteria = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };

                var sheet = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };
                var result = new[] { true, true, true, true };

                st.CompareSheetInfo(criteria, sheet, result);
                Assert.True(result[0] == true);
                Assert.True(result[1] == true);
                Assert.True(result[2] == true);
                Assert.True(result[3] == true);
            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        public void シートチェック_セル位置違い()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var criteria = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };

                var sheet = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(1, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };
                var result = new[] { true, true, true, true };

                st.CompareSheetInfo(criteria, sheet, result);
                Assert.True(result[0] == false);
                Assert.True(result[1] == true);
                Assert.True(result[2] == true);
                Assert.True(result[3] == true);
            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        public void シートチェック_倍率違い()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var criteria = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };

                var sheet = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 120,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };
                var result = new[] { true, true, true, true };

                st.CompareSheetInfo(criteria, sheet, result);
                Assert.True(result[0] == true);
                Assert.True(result[1] == false);
                Assert.True(result[2] == true);
                Assert.True(result[3] == true);
            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        public void シートチェック_枠線違い()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var criteria = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };

                var sheet = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = true,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };
                var result = new[] { true, true, true, true };

                st.CompareSheetInfo(criteria, sheet, result);
                Assert.True(result[0] == true);
                Assert.True(result[1] == true);
                Assert.True(result[2] == false);
                Assert.True(result[3] == true);
            }
            finally
            {
                st.Dispose();
            }
        }

        [Test]
        public void シートチェック_表示モード違い()
        {
            var st = Stiffer.GetInstance();
            try
            {
                var cd = System.IO.Directory.GetCurrentDirectory();

                var criteria = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                };

                var sheet = new SheetInfo
                {
                    Name = "",
                    CellPosition = new System.Drawing.Point(0, 0),
                    Zoom = 100,
                    Gridlines = false,
                    View = Microsoft.Office.Interop.Excel.XlWindowView.xlPageBreakPreview
                };
                var result = new[] { true, true, true, true };

                st.CompareSheetInfo(criteria, sheet, result);
                Assert.True(result[0] == true);
                Assert.True(result[1] == true);
                Assert.True(result[2] == true);
                Assert.True(result[3] == false);
            }
            finally
            {
                st.Dispose();
            }
        }


    
    }
}
