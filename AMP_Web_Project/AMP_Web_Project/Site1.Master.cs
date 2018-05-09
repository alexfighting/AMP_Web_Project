using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMP_Web_Project
{
    public partial class Site1 : System.Web.UI.MasterPage
    {        
        private static WebUser user0;
        protected void Page_Load(object sender, EventArgs e)
        {
            user0 = UserInfo.currentUser;
            WebUser user1 = UserInfo.currentUser;

            if (user0 != null)
            {
                user0 = UserInfo.getCurrentUser(user0.departmentid);
                user0.departmentid = user1.departmentid;
                user0.isSuperUser = user1.isSuperUser;
                user0.username = user1.username;
                username.Value = user0.username;
                departmentid.Value = user0.departmentid;
                departmentname.Value = user0.departmentname;
                dep_userid.Value = user0.dep_userid;
                txtdeptid.Value = user0.departmentid;
                notify_method.Value = user0.notify_method;
                emailaddress.Value = user0.emailaddress;
                isSuperUser.Value = user0.isSuperUser ? "true" : "false";
                txtwindowsuser.Value = user0.windows_login_userid;
                if (isSuperUser.Value == "true")
                {
                    liupdate.Visible = true;
                }
                else
                {
                    liupdate.Visible = false;
                }

                lbDepartmentName.Text = departmentname.Value;
                txtdeptname.Value = departmentname.Value;
                if (!string.IsNullOrEmpty(notify_method.Value))
                {
                    lbNotifyMethod.Text = notify_method.Value;
                    selmethod.Value = notify_method.Value;
                }

                if (notify_method.Value == "Activity")
                {
                    lbNotifyUser.Text = dep_userid.Value;
                    selmethod.SelectedIndex = 0;
                    lbNotifyUserId.InnerText = "Notify User Id: ";
                    txtUserId.Value = dep_userid.Value;
                }
                else if (notify_method.Value == "Email")
                {
                    lbNotifyUser.Text = emailaddress.Value;
                    selmethod.SelectedIndex = 1;
                    lbNotifyUserId.InnerText = "Email Address: ";
                    txtUserId.Value = emailaddress.Value;
                }
                else if (notify_method.Value == "Both")
                {
                    lbNotifyUser.Text = dep_userid.Value + " & " + emailaddress.Value;
                    txtUserId.Value = dep_userid.Value;
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }


    }
}