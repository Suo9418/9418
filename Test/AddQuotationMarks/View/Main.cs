using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using 添加引号111;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AddQuotationMarks.BLL;
using Imes.Utility.Model;
using System.Data;
using System.Data.Common;
using System.Drawing;
using imes.client;
using System.Threading.Tasks;
using System.Media;
using System.IO;
using System.Diagnostics;
using System.Security.Policy;
using System.Net;
using System.Net.Http;
using System.Linq;
using AddQuotationMarks.View;
using AddQuotationMarks.Utils;
using NPOI.OpenXmlFormats.Dml.Diagram;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;

namespace AddQuotationMarks
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

        }
        // 假设这是您的基本URL模板，其中包含一个占位符（例如{param}）来表示参数应插入的位置
        private const string UrlTemplate = "http://10.11.18.51:88/emp_photo/{param}.jpg";
        Controller con = new Controller();
        private string sound = utility.GlobalClientRootPath + "\\3rdDll\\AUDIO\\";
        string g_str_empno = utility.GlobalUserNo;
        string[] Filters = { "USN", "KEYNAME", "KEYVALUE", "USERID", "ENABLED" };
        public PageBean pageB;
        int totalRows = 0;
        int everyPageNum = 0;


        //进度条打印信息
        private void outputinfo(string info)
        {
            ProgressBarTxt.Text = DateTime.Now.ToString("HH:mm:ss") + info + "\r\n";
            //ProgressBarTxt.AppendText(DateTime.Now.ToString("HH:mm:ss")+info+"\r\n");
        }



        //报错提示+PASS提示
        public void ErrorMsg(string msg)
        {
            Label_Message_Text.Text = msg;
            Label_Message_Text.BackColor = Color.Red;
            Label_Message_Text.ForeColor = Color.White;
            PictureBox1.BackColor = Color.Red;
            PictureBox1.Image = Image.FromFile(utility.GlobalClientRootPath + "\\3rdDll\\Images\\FAIL.png");
        }
        private void Sussful(string msg)
        {
            Label_Message_Text.Text = msg;
            Label_Message_Text.BackColor = Color.Green;
            Label_Message_Text.ForeColor = Color.White;
            PictureBox1.BackColor = Color.Green;
            PictureBox1.Image = Image.FromFile(utility.GlobalClientRootPath + "\\3rdDll\\Images\\PASS.png");
        }
        //NG\OK\INPUT输入提示
        public void SoundNG()
        {
            PlayWav(sound + "audio_ng.wav");
        }
        private void SoundOK()
        {
            PlayWav(sound + "audio_ok.wav");
        }
        private void SoundINPUT()
        {
            PlayWav(sound + "audio_input.wav");
        }
        private void PlayWav(string fileName)
        {
            if (!File.Exists(fileName)) return;
            Task.Run(() =>
            {
                SoundPlayer soundPlayer = new SoundPlayer();
                soundPlayer.SoundLocation = fileName;
                soundPlayer.Play();
            });
        }

        private void ADD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputBox.Text))
            {
                ErrorMsg("请输入字符！");
                return;
            }
            InputBox.Focus();
            string[] lines = InputBox.Lines;//数组长度
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == lines.Length - 1) // 是最后一行,lines.Length是返回数组长度，可以看上面引用，而获取下标元素索引是从0开始，所以要减一
                {
                    string formattedLine = $"'{lines[i]}'";
                    sb.AppendLine(formattedLine);
                    LogHelper.WriteLog("判断SN是最后一行，不在添加逗号：", formattedLine);
                }
                else
                {
                    string formattedLine = $"'{lines[i]}',";
                    sb.AppendLine(formattedLine);
                    LogHelper.WriteLog("判断SN不是最后一行，继续添加逗号、单引号：", formattedLine);
                }
            }

            OutBox.Text = sb.ToString();
            string Upper = OutBox.Text.ToUpper();
            OutBox.Text = Upper;
            Sussful("转换成功");
            SoundOK();
            LogHelper.WriteLog("转换成功-->", OutBox.Text);
            OutBox.Focus();
        }

        private void CLEAN_Click(object sender, EventArgs e)
        {
            InputBox.Clear();
            OutBox.Clear();
            InputBox.Focus();
            Sussful("已全部CLEAR成功");
            SoundOK();
            LogHelper.WriteLog("已全部CLEAR成功", "");
        }

        private void COPY_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(OutBox.Text))
                {
                    MessageBox.Show("请注意文本框为空！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                else
                {
                    Sussful("已全部复制成功");
                    SoundOK();
                    Clipboard.SetText(OutBox.Text);
                    LogHelper.WriteLog("已全部复制成功", OutBox.Text);
                    OutBox.Focus();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void TO_UPEER_Click(object sender, EventArgs e)
        {
            string UPPER = InputBox.Text.ToUpper().Replace(":", "").Replace("：", "").Replace(" ", "").Replace("-", "").Replace("_", "").Replace("      ", "").Replace(" ", "");
            UPPER.TrimStart();
            OutBox.Text = UPPER;

            Sussful("转大写成功");
            SoundOK();
            LogHelper.WriteLog("转大写成功", UPPER);
            OutBox.Focus();
        }

        private void WAIT_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("暂无此需求，待开发哦 ！！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            #region 自己琢磨
            string[] linee = InputBox.Text.Split('\n');
            for (int i = 0; i < linee.Length; i++)
            {
                linee[i] = InputBox.Text.TrimStart();
            }
            string[] lineee = InputBox.Text.Split('\n');
            OutBox.Text = string.Join("\n", lineee);
            //InputBox.Focus();
            #endregion
            // 初始化一个空的字符串列表来存储处理后的行
            #region 移除每行开始的空格     
            List<string> lines = new List<string>();

            // 遍历 textBox1 的每一行
            foreach (string line in InputBox.Lines)
            {
                // 移除每行开始的空格，然后添加到列表
                lines.Add(line.TrimStart());
            }

            // 将处理后的行放到 textBox2 中
            OutBox.Lines = lines.ToArray();

            #endregion

            ExecuteResult exe = new ExecuteResult();//con.Getsnstatus("");

            exe = con.Getsnstatus(InputBox.Text);
            DataTable dt = (DataTable)exe.Anything;

            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("SELECT OK");
                #region 冗余
                //string SN1 = "";
                //string station_type = "";
                //string station_name = "";
                //string emp_no = "";
                //string serial_number = dt.Rows[0]["serial_number"].ToString();
                //station_type= dt.Rows[0]["station_type"].ToString();
                //station_name = dt.Rows[0]["station_name"].ToString();
                //emp_no = dt.Rows[0]["station_type"].ToString();
                //string SN = dt.Rows[0]["serial_number"].ToString();
                #endregion

                
                exe = con.GetScrapCheck();  //检查维护Code
                if (exe.Status)
                {
                    string[] str = exe.Anything.ToString().Split(new char[] { ',' });
                    ExecuteResult code = new ExecuteResult();
                    code = con.GetDefectCode(InputBox.Text);
                    if (!code.Status)
                    {
                        ErrorMsg(code.Message);
                        return;
                    }
                    
                    bool CodeInArray = str.Contains(code.Anything.ToString().Trim()); //如果抛修护代码表里，code在维护卡关里并未勾选报废框则进行报错
                    if (CodeInArray == true && !Checked_Discarded.Checked)
                    {
                        ErrorMsg("该机台未勾选是否报废框，请联系修护工程师确认");
                        return;
                    }
                }


                con.Updateststaus(dt.Rows[0]["serial_number"].ToString());
                con.InsertOfflineStatus(dt.Rows[0]["serial_number"].ToString(), dt.Rows[0]["station_type"].ToString(), dt.Rows[0]["station_name"].ToString());
                Sussful("已插入数据表STATUS||STATUS_OFFLINE");
                SoundOK();
            }
            else
            {
                ErrorMsg("查询失败");
                SoundNG();
                MessageBox.Show("SELECT NG");
            }
        }

        private void AD_ZERO_Click(object sender, EventArgs e)
        {
            string[] line = InputBox.Text.Split('\n');
            //获取inputTextBox所有行

            for (int i = 0; i < line.Length; i++)
            {
                line[i] = IntextBox.Text + line[i];
                LogHelper.WriteLog("添加字符串成功-->", line[i]);
            }
            //每行前加0
            string[] lines = InputBox.Text.Split();
            OutBox.Text = string.Join("\n", line);
            LogHelper.WriteLog("每行前添加字符串成功-->", OutBox.Text);
            // LogHelper.
            //更新inputTextBox全部内容
            //输出至OutBox1内容
            OutBox.Focus();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //wthis.WindowState = FormWindowState.Maximized;
            //UrTextBox.Enabled =false;
            Sussful("请刷入信息");
            combFilter.Items.AddRange(Filters);
            totalRows = GetShowcount();
            if (everyPageNumCKB.Text == "")
            {
                everyPageNumCKB.Text = "100";
            }
            if (totalRows == 0)
            {
                return;
            }
            everyPageNum = int.Parse(everyPageNumCKB.Text);
            pageB = new PageBean(totalRows, everyPageNum, 1);
            ShowInfor();
            Multiplication();
            timer1.Start();
        }

        public void Multiplication()///乘法口诀
        {
            try
            {
                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j < i; j++)
                    {
                        textBox1.AppendText(string.Format("{0}×{1}={2}\t", i, j, i * j));
                    }
                    textBox1.AppendText(Environment.NewLine);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("乘法口诀输出有误");
            }
        }

        private int GetShowcount()
        {

            DataTable dt = con.GetShowcount(combFilter.Text, editFilter.Text.Trim());
            //gvData.DataSource = dt;
            int x = Convert.ToInt32(dt.Rows[0][0].ToString());
            return x;

        }

        private void ShowInfor()
        {
            everyPageNum = int.Parse(everyPageNumCKB.Text);
            pageB = new PageBean(totalRows, everyPageNum, pageB.CurrentPageNum1);

            DataTable dt = con.GetShowInfor(combFilter.Text, editFilter.Text.Trim(), pageB);
            gvData.DataSource = dt;

            //共0条记录  共0页  当前第0页 每页0条
            pageNumLabel.Text = "共" + pageB.ItemsTotalNum1 + "条记录 共" + pageB.MaxPageNum + "页 当前第" + pageB.CurrentPageNum1 + "页";
        }

        /// <summary>
        /// 查看图片输入框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UrTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ExecuteResult exeRes = con.Getpicturerole();
            if (!exeRes.Status)
            {
                ErrorMsg(exeRes.Message);
                UrTextBox.Enabled = false;
            }

            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    string inputParameter = UrTextBox.Text;
                    //检查输入是否为空
                    if (string.IsNullOrWhiteSpace(inputParameter))
                    {
                        //MessageBox.Show("请输入一个参数。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMsg("不能为空，请输入！");
                        return;
                    }

                    #region
                    // 检查URL是否为空
                    //if (string.IsNullOrEmpty(inputParameter))
                    //{
                    //    MessageBox.Show("请输入字符", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    #endregion
                    // 获取TextBox中的输入参数
                    string parameterValue = UrTextBox.Text;
                    string url = UrlTemplate.Replace("{param}", parameterValue);
                    // 从TextBox中获取图片的URL，然后使用WebClient打开URL，获取图片流，然后将图片流转换为图片，最后将图片显示在PictureBox中
                    using (WebClient client = new WebClient())    
                    {
                        Stream stream = client.OpenRead(url);
                        Sussful("查询OK");
                        PictureBox1.Image = new Bitmap(stream);
                        UrTextBox.Clear();
                    }
                    //Process.Start(url);

                }
            }
            catch (Exception ex)
            {

                ErrorMsg(ex.Message);
                UrTextBox.Clear();
            }

        }
        /// <summary>
        /// 使用GDI绘制验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaptChas_Click(object sender, EventArgs e)
        {
            Random r = new Random();//思路：搞5个随机的字符串，随机5个字符串添加到字符串里，所以创建了
            string str = null;//字符串，下面用来做随机字符串,创建循环，因为字符串验证码为5个数字

            for (int i = 0; i < 5; i++)//随机5次
            {
                int n1 = r.Next(0, 10);
                str += n1;//右侧的值添加到左侧的变量中,在第一次尝试使用+=运算符时，会触发一个隐式的类型转换，将n1转换为字符串，然后尝试将null与这个字符串连接
            }//点击图片框随机5个数累加，也就是5个随机数，下面开始画图,画图就要产生GDI对象
            //数值类型，+= 操作符会执行加法运算并更新变量的值；字符串类型，+= 操作符会执行字符串连接操作并更新变量的值

            Bitmap bmp = new Bitmap(120, 28);//Bitmap位图类，因为所有图片默认都是位图，也有矢量图，Bitmap继承与Image，括号里是宽和高
            Graphics g = Graphics.FromImage(bmp);//创建GDI对象， Graphics.FromImage(bmp)画图片对象，括号里需要一个Image，所以这里可以填位图类，要求是从图片里来

            for (int i = 0; i < 5; i++)//需要将随机数画到PicBox框里
            {
                string[] fonts = { "宋体", "微软雅黑", "宋体", "微软雅黑", "宋体" };
                Color[] colors = { Color.Red, Color.BlueViolet, Color.Pink, Color.Black, Color.Cyan };
                Point p = new Point(i * 20, 0);//画数字的点，代表i和i之间像素是10个像素,第一个参数是横坐标之间的像素第一次循环i是0.0*10等于0，纵坐标是0，现在画的点就是0,0的坐标也就是左上角，第二次i是1了，1*3=3，所以间距就是3px开始坐标了，纵坐标还是0，也就是永远在一个y一个水平线上
                g.DrawString(str[i].ToString(), new Font(fonts[r.Next(0, 5)], 20, FontStyle.Bold), new SolidBrush(colors[r.Next(0, 5)]), p);
                //第一个参数这里下标拿到的类型是char类型，但是方法里需要string类型，所以.ToString()一下，参数为画的是什么
                //第二个参数需要字体，但是验证码字体也是随机的，所以创建了一个数组，从数组里取随机字体，Font类第一个参数是字体随机取，第二个是字体大小，第三个是是否加粗（Bold是加粗的意思）
                //第三个参数需要画颜色，但是验证码画的颜色也是随机的，所以创建了一个数组，从数组里取随机颜色，SolidBrush是定义了一个单色画笔他括号里构造函数可以填Color
                //第四个参数是点，不能随机给横纵坐标，因为会随机万一重叠了，所以每两个数字之间必须有间距,横坐标不能重叠
                //tip:在调试时候报错索引超出数组范围，因为r.Next(0, 5)是下标第一个数组参数是0开始的，0到5也就是四个，所以一定要注意数组和随机数范围要对应或者要小于，不能随机数大，数组里参数没有那么多，不然随机到大于数组里的下标时候找不到对应值会报错，索引超出数组范围
            }

            //画随机线体
            for (int i = 0; i < 10; i++)
            {
                Point p1 = new Point(r.Next(0,bmp.Width),r.Next(0,bmp.Height));//第一个点的xy取图片框的随机高宽
                Point p2 = new Point(r.Next(0, bmp.Width), r.Next(0, bmp.Height));//第一个点的xy取图片框的随机高宽

                g.DrawLine(new Pen(Brushes.Red), p1, p2);//第2个参数点不能超出图片的范围//DrawLine画线方法
            }

            //画点，指定像素的颜色
            for (int i = 0; i < 10; i++)
            {
                Point p = new Point(r.Next(0, bmp.Width), r.Next(0, bmp.Height));//第一个点的xy取图片框的随机高宽
                bmp.SetPixel(p.X,p.Y,Color.Black);//，指定像素的颜色,添加点.SetPixel指定像素的颜色
            }
            //将图片镶嵌到PicBox中，以上把画的数字随机显示出来，//下面开始画随机线体
            CaptChas.Image = bmp;

            //总结步骤：1、产生随机数，2、通过循环1个1个往图片里画随机数，字体颜色都在自己的数组里随机，3、画线，4、画像素，5、最后赋值镶嵌到PicBox上
        }
        Random r = new Random();

        /// <summary>
        /// 点击获取随机工号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicEmpBox_Click(object sender, EventArgs e)
        {
            //string[] strr = NewMethod();
            ExecuteResult exe = new ExecuteResult();
            Random r=new Random();

            exe =con.Getusername();
            if (!exe.Status)
            {
                ErrorMsg(exe.Message);
            }
            else
            {
                Sussful("OK");
                string[] strr = exe.Anything.ToString().Split(new char[] { ',' });
                UrTextBox.Text = strr[r.Next(0, strr.Length + 1)].ToString();
            }

            // 生成一个随机下标
            //int Index = r.Next(0, strr.Length);
            // 显示随机下标对应的字符串
            //UrTextBox.Text = strr[r.Next(0, strr.Length)];
            //MessageBox.Show(r.Next(0,strr.Length+1).ToString());
        }

        private void uploadBTN_Click(object sender, EventArgs e)
        {
            ImportFrm importFrm = new ImportFrm();  
            importFrm.ShowDialog();
            


        }

        private void everyPageNumCKB_SelectedValueChanged(object sender, EventArgs e)
        {
            if (pageB == null)
                return; 
            pageB.CurrentPageNum1 = 1;
            ShowInfor();
            

        }

        private void CheckYesNo_KeyPress(object sender, KeyPressEventArgs e)
        
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                ExecuteResult exe = new ExecuteResult();
                exe = con.GetScrapCheck();  //检查维护Code
                if (exe.Status)
                {
                    string[] str = exe.Anything.ToString().Split(new char[] { ',' });
                    ExecuteResult flag9 = new ExecuteResult();
                    flag9 = con.GetDefectCode(CheckYesNo.Text);
                    if (!flag9.Status)
                    {
                        ErrorMsg(flag9.Message);
                        return;
                    }

                    bool CodeInArray = str.Contains(flag9.Anything.ToString().Trim()); //如果抛修护代码表里，code在维护卡关里并未勾选报废框则进行报错
                    if (CodeInArray == true && !Checked_Discarded.Checked)
                    {
                        ErrorMsg("该机台未勾选是否报废框，请联系修护工程师确认");
                        return;
                    }
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProgressBar.Maximum = 100;
            if (ProgressBar.Value< ProgressBar.Maximum)
            {
                ProgressBar.Value++;
                outputinfo("进度运行中("+ ProgressBar.Value.ToString()+"/"+ ProgressBar.Maximum+")...");
            }
            else
            {
                outputinfo("进度完成");
                timer1.Stop();
            }
        }
    }
}
