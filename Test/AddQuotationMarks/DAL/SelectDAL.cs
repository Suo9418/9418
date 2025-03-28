using AddQuotationMarks.Utils;
using imes.client;
using Imes.Utility.Model;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NPOI.HSSF.Util.HSSFColor;

namespace AddQuotationMarks.DAL
{
    public class SelectDAL
    {
        //Process[] pro=Process.GetProcesses();进程类,数组接收

        ExecuteResult exeRes = new ExecuteResult();

        public ExecuteResult GetSNStatus(string serial_number)
        {
            try
            {
                //exeRes = new ExecuteResult();
                string sqlStr = @"select * from imes.p_sn_status where serial_number =:serial_number AND CURRENT_STATUS='9'";
                object[] para = new object[] { serial_number };

                exeRes.Anything = utility.Query(sqlStr, para);
                exeRes.Status = true;

            }
            catch (Exception ex)
            {

                exeRes.Message = ex.Message;
                exeRes.Status = false;

            }
            return exeRes;
        }

        /// <summary>
        /// 使用查看图片需要有权限才可以使用
        /// </summary>
        /// <returns></returns>
        public ExecuteResult GetPictureRole()
        {
            try
            {
                string sqlStr = @" SELECT CONFIG_VALUE 
                                    FROM IMES.M_BLOCK_CONFIG_VALUE A,
                                    IMES.M_BLOCK_CONFIG_TYPE B 
                                         WHERE A.CONFIG_NAME='OFFLINE_EMPNO' 
                                         AND B.CONFIG_TYPE_NAME='Packing Offline'
                                         AND A.Config_Type_Id= B.Config_Type_Id
                                         AND A.ENABLED=B.ENABLED
                                         AND A.ENABLED='Y' ";
                DataTable dt = utility.Query(sqlStr);
                if (dt.Rows.Count > 0)
                {
                    exeRes.Anything = dt.Rows[0][0].ToString();
                    exeRes.Status = true;
                }
                else
                {
                    exeRes.Message = "卡关未维护";
                    exeRes.Status = false;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }




        public ExecuteResult GetScrapCheck()
        {
            exeRes = new ExecuteResult();
            try
            {
                string CONFIG_VALUESQL = @"SELECT A.CONFIG_VALUE
  FROM IMES.M_BLOCK_CONFIG_VALUE A, IMES.M_BLOCK_CONFIG_TYPE B
 WHERE A.CONFIG_NAME = 'Check_Wherher_Scrap'
       AND A.CONFIG_TYPE_ID = B.CONFIG_TYPE_ID
       AND A.ENABLED = B.ENABLED
       AND A.ENABLED = 'Y'";

                DataTable dt = utility.Query(CONFIG_VALUESQL);
                if (dt.Rows.Count > 0)
                {
                    exeRes.Anything = dt.Rows[0][0].ToString();
                    exeRes.Status = true;
                }
                else
                {
                    //exeRes.Message = "卡关未维护，请联系修护确认";
                    exeRes.Status = false;
                }


            }
            catch (Exception ex)
            {

                exeRes.Message = ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }


        public ExecuteResult GetDefectCode(string serial_number)
        {
            try
            {
                string sql = @"select STATION_TYPE from  IMES.P_SN_DEFECT where SERIAL_NUMBER=:serial_number";
                object[] obj = new object[] { serial_number };
                DataTable dt = utility.Query(sql, obj);
                if (dt.Rows.Count > 0)
                {
                    exeRes.Anything = dt.Rows[0][0].ToString();
                    exeRes.Status = true;
                }
                else
                {
                    exeRes.Message = "请确认SN是否为不良产品";
                    exeRes.Status = false;
                }
            }
            catch (Exception ex)
            {

                exeRes.Message = ex.Message;
                exeRes.Status = false;
            }
            return exeRes;
        }

        public static string[] GenUserName()
        {



            string sql = @"SELECT CONFIG_VALUE 
                                    FROM IMES.M_BLOCK_CONFIG_VALUE A,
                                    IMES.M_BLOCK_CONFIG_TYPE B 
                                         WHERE A.CONFIG_NAME='OFFLINE_EMPNO' 
                                         AND B.CONFIG_TYPE_NAME='Packing Offline'
                                         AND A.Config_Type_Id= B.Config_Type_Id
                                         AND A.ENABLED=B.ENABLED
                                         AND A.ENABLED='Y' ";
            DataTable dt = utility.Query(sql);
            string str = dt.Rows[0]["CONFIG_VALUE"].ToString();
            string[] strr = str.Split(new char[] { ',' });
            return strr;
        }

        public ExecuteResult GetUserName()
        {
            try
            {
                string sql = @"SELECT CONFIG_VALUE 
                                    FROM IMES.M_BLOCK_CONFIG_VALUE A,
                                    IMES.M_BLOCK_CONFIG_TYPE B 
                                         WHERE A.CONFIG_NAME='OFFLINE_EMPNO' 
                                         AND B.CONFIG_TYPE_NAME='Packing Offline'
                                         AND A.Config_Type_Id= B.Config_Type_Id
                                         AND A.ENABLED=B.ENABLED
                                         AND A.ENABLED='Y' ";
                DataTable dt = utility.Query(sql);

                if (dt.Rows.Count > 0)
                {
                    exeRes.Anything = dt.Rows[0]["CONFIG_VALUE"].ToString();////string[] strr = str.Split(new char[] { ',' });
                    exeRes.Status = true;
                }
                else
                {
                    exeRes.Message = "请确认卡关内容是否维护";
                    exeRes.Status = false;
                }
            }
            catch (Exception ex)
            {
                exeRes.Message = ex.Message;
            }
            return exeRes;

        }


        public DataTable GetShowcount(string Filter, string value)
        {
            string sql = @" select count(1)  from IMES.P_BOBCAT_GOLDVALUE  where 1=1";

            if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(value))
            {
                sql += $"  and {Filter}='{value}' ";
            }
            return utility.Query(sql);

        }

        public DataTable GetShowInfor(string Filter, string value, PageBean pageB)
        {
            string sql = @" SELECT USN, KEYNAME, KEYVALUE, enabled, userid, trndate, rownum rn
  FROM (SELECT USN, KEYNAME, KEYVALUE, enabled, userid, trndate, rownum rn
          FROM (SELECT * FROM IMES.P_BOBCAT_GOLDVALUE ORDER BY TRNDATE DESC)
         WHERE 1 = 1  and rownum<=" + (pageB.CurrentPageNum1) * pageB.EveryPageNum1 + " ";
            if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(value))
            {
                sql += $"  and {Filter}='{value}' ";
            }

            sql = sql + " ) result where rn>" + (pageB.CurrentPageNum1 - 1) * pageB.EveryPageNum1 + " ";
            return utility.Query(sql);

        }



        
    }
}
