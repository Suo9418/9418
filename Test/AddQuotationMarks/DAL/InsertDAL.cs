using imes.client;
using Imes.Utility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddQuotationMarks.DAL
{
    public class InsertDAL
    {
        public ExecuteResult InsertOfflineStatus(string serial_number, string station_type, string station_name)
        {
            ExecuteResult exeRes = new ExecuteResult();
            try
            {
                string sqlStr = @"Insert into imes.p_sn_status_offline (serial_number,station_type,station_name,emp_no)
                                       VALUES (:SERIAL_NUMBER,:station_type,:station_name,:emp_no)";
                object[] para = new object[] { serial_number, station_type, station_name, utility.GlobalUserNo };

                exeRes.Anything = utility.ExecuteSql(sqlStr, para);
                exeRes.Status = true;
            }
            catch (Exception ex)
            {

                exeRes.Message = ex.Message;
                exeRes.Status = false;

            }
            return exeRes;
        }
    }
}//Process
