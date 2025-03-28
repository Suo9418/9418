using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imes.client;
using Imes.Utility.Model;


namespace AddQuotationMarks.DAL
{

    public class UpdateDAL
    {

        public ExecuteResult UpdateStatus(string serial_number)
        {
            ExecuteResult exeRes = new ExecuteResult();

            try
            {
                string sqlStr = @"update imes.p_sn_status set next_station_type='99',emp_no=:V_EMP,out_stationtype_time=sysdate where serial_number=:1  ";
                object[] para = new object[] { utility.GlobalUserNo,serial_number };
                exeRes.Anything = utility.ExecuteSql(sqlStr, para);

                exeRes.Status = true;

            }
            catch (Exception ex)
            {

                exeRes.Message = ex.ToString();
                exeRes.Status = false;

            }

            return exeRes;

        }



        public long SaveGoldUnitInfor(string usn, string keyname, string keyvalue, string enabled)
        {
            string sql = $@" merge into  (select * from IMES.P_BOBCAT_GOLDVALUE WHERE keyname='{keyname}') t1
         using(select '{usn}' as usn,'{keyname}' as keyname,'{keyvalue}' as keyvalue,'{utility.GlobalUserNo}' as userid ,'{enabled}' as enabled FROM dual) t2 on (t1.usn=t2.usn)
         when matched then update set t1.keyname=t2.keyname,t1.keyvalue=t2.keyvalue,t1.enabled=t2.enabled,t1.trndate=sysdate,t1.userid=t2.userid
           when not matched then insert values(t2.usn,t2.keyname,t2.keyvalue,t2.userid,t2.enabled,sysdate)";
            utility.ExecuteSql(sql);
            sql = $"   insert into IMES.P_BOBCAT_GOLDVALUE_HT select * from IMES.P_BOBCAT_GOLDVALUE where usn='{usn}'";
            return utility.ExecuteSql(sql);

        }



    }
}
