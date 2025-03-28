using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddQuotationMarks.DAL;
using AddQuotationMarks.Utils;
using Imes.Utility.Model;

namespace AddQuotationMarks.BLL
{
    public class Controller
    {
        //ExecuteResult ExecuteResult=new ExecuteResult();
        SelectDAL selectDAL = new SelectDAL();
        UpdateDAL updateDAL = new UpdateDAL();
        InsertDAL insertDAL = new InsertDAL();

        public ExecuteResult Getsnstatus(string serial_number)
        {
            return selectDAL.GetSNStatus(serial_number);
        }

        public ExecuteResult Updateststaus(string serial_number)
        {
            return updateDAL.UpdateStatus(serial_number);
        }

        public ExecuteResult InsertOfflineStatus(string serial_number, string station_type, string station_name)
        {
            return insertDAL.InsertOfflineStatus(serial_number, station_type, station_name);
        }

        public ExecuteResult Getpicturerole()
        {
            return selectDAL.GetPictureRole();
        }


        public ExecuteResult Getusername()
        {
            return selectDAL.GetUserName();
        }


        public long SaveGoldUnitInfor(string usn, string keyname, string keyvalue, string enabled)
        {
            return updateDAL.SaveGoldUnitInfor(usn, keyname, keyvalue, enabled);
        }

        public DataTable GetShowcount(string filter, string value)
        {
            return selectDAL.GetShowcount(filter, value);
        }

        public DataTable GetShowInfor(string filter, string value, PageBean pageB)
        {
            return selectDAL.GetShowInfor(filter, value, pageB);
        }

        public ExecuteResult GetScrapCheck()
        {
            return selectDAL.GetScrapCheck();
        }

        public ExecuteResult GetDefectCode(string serial_number)
        {
            return selectDAL.GetDefectCode(serial_number);
        }



    }

}
