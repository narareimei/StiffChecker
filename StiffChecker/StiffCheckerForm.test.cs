using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Stiff
{
    [TestFixture]
    public partial class StiffCheckerForm
    {
        [Test]
        [ExpectedException(typeof(System.Data.ConstraintException))]
        public void ブック情報登録_重複()
        {
            this.StiffForm_Load(null, null);
            int reccnt = this.excelFiles.Rows.Count;

            var info = new BookInfo
            {
                FileName = "TestBook.xlxs",
                //
                Author = "小林礼明",
                Title = "タイトル",
                Subject = "テスト用ブック",
                LastSaveTime = "2013/09/23 7:35:35",
                Company = "個人",
                Manager = "わし"
            };

            this.addBookInfo(info);
            Assert.True(true, "レコード１");
            this.addBookInfo(info);
        }

        [Test]
        public void ブック情報取得_一括()
        {
            this.StiffForm_Load(null, null);
            var cd = System.IO.Directory.GetCurrentDirectory();

            var filenames = new List<string>();
            {
                filenames.Add( cd + @"\TestBook.xlsx");
                filenames.Add( cd + @"\TestBook2.xlsx");
            }

            var list = this.getBookInformations(filenames.ToArray()).ToArray();
            Assert.True(list.Length == 2, "要素数");
            Assert.True(list[0].Author == "小林礼明" , "要素１");
            Assert.True(list[1].Author == "小林礼明2", "要素２");
        }
    }
}
