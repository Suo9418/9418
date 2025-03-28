using AddQuotationMarks.BLL;
using AddQuotationMarks.Utils;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddQuotationMarks.View
{
    public partial class ImportFrm: Form 
    {
        public ImportFrm()
        {
            InitializeComponent();
        }
        Controller _ctrl = new Controller();
        public delegate void SetStatusBarInNewThread(string text);
        string[] ColumnNames = new string[] { "USN", "KEYNAME", "KEYVALUE", "ENABLED" };

        internal void btnImportIm_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Filter = "(*.xls,*.xlsx)|*.xls;*.xlsx";
                fd.Title = "打开文件";
                string path = "";
                fd.InitialDirectory = "C:\\";
                fd.FilterIndex = 1;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    path = fd.FileName;//选择的文件赋值给一个字符串
                }

                // string FileExType = System.IO.Path.GetExtension(fd.FileName);
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                DataTable dt = imExcel.ExcelToTable(path);

                //  DataTable dt = Imes.Utility.Tools.ExcelHelp.GetSheetName(path);   此方法需要安装office 12


                if (!checkTable(dt))
                {
                    return;
                }
                gvDataIm.DataSource = dt;
                //  MessageBox.Show(dt.Columns[0].ColumnName + "--" + dt.Columns[1].ColumnName + "--" + dt.Columns[2].ColumnName);
            }
            catch (Exception ee)
            {
                showMsg("连接Excel文件错误:" + ee.Message);

                return;
            }
        }
        private bool checkTable(DataTable dt)
        {

            for (int i = 0; i < ColumnNames.Count(); i++)
            {
                if (dt.Columns[i].ColumnName.ToUpper() != ColumnNames[i])
                {
                    showMsg("第" + (i + 1) + "列不是'" + ColumnNames[i] + "'");
                    return false;
                }
            }
            return true;
        }
        public void showMsg(string text)
        {
            SetStatusBarInNewThread sss = new SetStatusBarInNewThread(this.setStatusBarInNewThread);
            textBox1.Invoke(sss, new object[] { text });
        }
        public void setStatusBarInNewThread(string text)
        {
            this.textBox1.AppendText(DateTime.Now + "--" + text + Environment.NewLine);
        }

        private void btnSaveIm_Click(object sender, EventArgs e)
        {
            if (gvDataIm.Rows.Count == 0)
                return;
            bindingNavigator1.Enabled = false;
            for (int i = 0; i < gvDataIm.RowCount; i++)
            {
                string sn = gvDataIm.Rows[i].Cells[0].Value.ToString().Trim();
                string keyname = gvDataIm.Rows[i].Cells[1].Value.ToString().Trim();
                string keyvalue = gvDataIm.Rows[i].Cells[2].Value.ToString().Trim();
                string enabled = gvDataIm.Rows[i].Cells[3].Value.ToString().Trim();


                if (!CheckLineItem(sn, keyname, keyvalue, i))
                {
                    showMsg("第" + (i + 1) + "列异常，导入中止。");
                    break;
                }

                try
                {
                    _ctrl.SaveGoldUnitInfor(sn, keyname, keyvalue, enabled);
                    showMsg("第" + (i + 1) + "列导入成功。");
                }
                catch (Exception ee)
                {
                    showMsg(ee.Message);
                    break;
                }


            }

            MessageBox.Show("导入结束！");
        }

        private bool CheckLineItem(string sn, string keyname, string keyvalue, int lineNum)
        {
            if (string.IsNullOrEmpty(sn))
            {
                showMsg("第" + (lineNum + 1) + "列的sn为空。");
                return false;
            }
            if (string.IsNullOrEmpty(keyname))
            {
                showMsg("第" + (lineNum + 1) + "列的keyname为空。");
                return false;
            }
            if (string.IsNullOrEmpty(keyvalue))
            {
                showMsg("第" + (lineNum + 1) + "列的keyvalue为空。");
                return false;
            }

            if (string.IsNullOrEmpty(keyvalue))
            {
                showMsg("第" + (lineNum + 1) + "列的ENABLED为空。");
                return false;
            }

            return true;
        }
    }
}
