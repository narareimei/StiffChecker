using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stiff.Properties;

namespace Stiff
{
    public partial class StiffCheckerForm : Form
    {
        private Stiffer         stiffer;

        private DataTable       excelFiles;
        private List<BookInfo>  bookInfoList ;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StiffCheckerForm()
        {
            InitializeComponent();

            bookInfoList = new List<BookInfo>();
        }

        #region イベントハンドラ

        /// <summary>
        /// フォーム初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StiffForm_Load(object sender, EventArgs e)
        {
            // 画面コントロール初期化
            {
                this.zoom.Text          = Settings.Default.zoom.ToString();
                this.gridOff.Checked    = !Settings.Default.grid;
                this.gridOn.Checked     = Settings.Default.grid;
                this.view.Text          = Settings.Default.view;
            }

            // 
            stiffer = Stiffer.GetInstance();

            // グリッド初期化
            {
                bookGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                bookGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            {
                var dt = new DataTable();

                // カラム定義
                dt.Columns.Add(new DataColumn("Seq",        typeof(int)));
                dt.Columns.Add(new DataColumn("File",       typeof(string)));
                dt.Columns.Add(new DataColumn("Author",     typeof(string)));   // 作成者
                dt.Columns.Add(new DataColumn("Title",      typeof(string)));   // タイトル
                dt.Columns.Add(new DataColumn("Subject",    typeof(string)));   // サブジェクト
                dt.Columns.Add(new DataColumn("Update",     typeof(string)));   // 更新日時
                dt.Columns.Add(new DataColumn("Company",    typeof(string)));   // 会社
                dt.Columns.Add(new DataColumn("Manager",    typeof(string)));   // 管理者
                // 比較結果
                dt.Columns.Add(new DataColumn("セル位置",   typeof(string)));
                dt.Columns.Add(new DataColumn("倍率",       typeof(string)));
                dt.Columns.Add(new DataColumn("枠線",       typeof(string)));
                dt.Columns.Add(new DataColumn("表示",       typeof(string)));
                dt.Columns.Add(new DataColumn("シート",     typeof(string)));
                // プライマリキー設定
                var pk = new DataColumn[1];
                pk[0] = dt.Columns["File"];
                dt.PrimaryKey = pk;
                this.excelFiles = dt;
            }
            bookGrid.DataSource = this.excelFiles;
        }

        #region ドラッグアンドドロップ

        /// <summary>
        /// ドラッグアンドドロップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StiffForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                // ドラッグ中のファイルやディレクトリの取得
                string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string d in drags)
                {
                    if (!System.IO.File.Exists(d))
                    {
                        // ファイル以外であればイベント・ハンドラを抜ける
                        return;
                    }
                }
                e.Effect = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StiffForm_DragDrop(object sender, DragEventArgs e)
        {
            chekcExcelFiles((string[])e.Data.GetData(DataFormats.FileDrop));
           return;
        }

        #endregion //ドラッグアンドドロップ


        /// <summary>
        /// 確認実施
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRun_Click(object sender, EventArgs e)
        {
            if (bookGrid.Rows == null || bookGrid.Rows.Count <= 0)
            {
                return;
            }

            var files = new List<string>();
            foreach( DataRow row in ((DataTable)this.bookGrid.DataSource).Rows)
            {
                files.Add( (string)row["File"] );
            }
            chekcExcelFiles(files.ToArray());
            return ;
        }

        #endregion //イベントハンドラ

        #region ロジック

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        private void chekcExcelFiles(string[] files)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                excelFiles.Clear();

                this.bookInfoList = getBookInformations(files);
                foreach (var info in this.bookInfoList)
                {
                    addBookInfo(info);
                }
                this.GridRefresh();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return;        
        }

        /// <summary>
        /// ブック情報をデータテーブルへ追加する
        /// </summary>
        /// <param name="info"></param>
        private void addBookInfo(BookInfo info)
        {
            var row = this.excelFiles.NewRow();

            row["Seq"       ] = excelFiles.Rows.Count + 1;
            row["File"      ] = info.FileName;
            row["Author"    ] = info.Author;
            row["Title"     ] = info.Title;
            row["Subject"   ] = info.Subject;
            row["Update"    ] = info.LastSaveTime;
            row["Company"   ] = info.Company;
            row["Manager"   ] = info.Manager;
            row["セル位置"  ] = (info.CheckResult[0] == true ? "OK" : "NG");
            row["倍率"      ] = (info.CheckResult[1] == true ? "OK" : "NG");
            row["枠線"      ] = (info.CheckResult[2] == true ? "OK" : "NG");
            row["表示"      ] = (info.CheckResult[3] == true ? "OK" : "NG");
            row["シート"    ] = (info.CheckResult[4] == true ? "OK" : "NG");
            excelFiles.Rows.Add(row);
            return;
        }

        /// <summary>
        /// ブック情報の取得
        /// </summary>
        /// <param name="files">ファイル名配列</param>
        private List<BookInfo>  getBookInformations(string[] files)
        {
            var list = new List<BookInfo>();

            // この値が基準だ
            var criteria = new SheetInfo
            {
                CellPosition    = new System.Drawing.Point(1, 1),
                Zoom            = Double.Parse(this.zoom.Text),
                Gridlines       = this.gridOn.Checked,
                View            = this.view.Text == "標準モード" ?
                                      Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView
                                    : Microsoft.Office.Interop.Excel.XlWindowView.xlPageBreakPreview
            };
            // 設定を保存しておく
            {
                Settings.Default.zoom   = criteria.Zoom ;
                Settings.Default.grid   = this.gridOn.Checked;
                Settings.Default.view   = this.view.Text;
                Settings.Default.Save();
            }

            // 情報取得
            foreach( var file in files ) 
            {
                var info = this.stiffer.GetBookInformations(file);
                this.stiffer.CheckSheetInformations(criteria, info);
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 行ごとの色付け
        /// </summary>
        private void GridRefresh()
        {
            foreach (DataGridViewRow row in bookGrid.Rows)
            {
                if( (string)row.Cells["セル位置"].Value == "NG" ||
                    (string)row.Cells["倍率"].Value     == "NG" ||
                    (string)row.Cells["枠線"].Value     == "NG" ||
                    (string)row.Cells["表示"].Value     == "NG" ||
                    (string)row.Cells["シート"].Value   == "NG")
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            return;
        }

        #endregion //ロジック




    }
}
