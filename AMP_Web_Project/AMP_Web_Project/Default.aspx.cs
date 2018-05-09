using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMP_Web_Project
{
    public partial class Default : System.Web.UI.Page
    {
        public string strEBMSConn = ConfigurationManager.ConnectionStrings["strEBMS_Snapshot_Conn"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            WebUser user0 = UserInfo.currentUser;

            if (user0 != null)
            {
                HiddenField hidusername = (HiddenField)Master.FindControl("username");                
                HiddenField hiddepartmentid = (HiddenField)Master.FindControl("departmentid");
                HiddenField hiddepartmentname = (HiddenField)Master.FindControl("departmentname");
                HiddenField hiddep_userid = (HiddenField)Master.FindControl("dep_userid");
                HiddenField hidnotify_method = (HiddenField)Master.FindControl("notify_method");
                HiddenField hidemailaddress = (HiddenField)Master.FindControl("emailaddress");
                HiddenField hidisSuperUser = (HiddenField)Master.FindControl("isSuperUser");
                if (hidusername != null) hidusername.Value = user0.username;
                if (hiddepartmentid != null) hiddepartmentid.Value = user0.departmentid;
                if (hiddepartmentname != null) hiddepartmentname.Value = user0.departmentname;
                if (hiddep_userid != null) hiddep_userid.Value = user0.dep_userid;
                if (hidnotify_method != null) hidnotify_method.Value = user0.notify_method;
                if (hidemailaddress != null) hidemailaddress.Value = user0.emailaddress;
                if (hidisSuperUser != null) hidisSuperUser.Value = user0.isSuperUser ? "true" : "false";

                Label1.Text = hiddepartmentname.Value;

                if (!user0.isSuperUser)
                {
                    lirules.Visible = false;
                }

            }
            else
            {
                Response.Redirect("Login.aspx");
            }



           // LoadMyRules();
        }

    
    }
}