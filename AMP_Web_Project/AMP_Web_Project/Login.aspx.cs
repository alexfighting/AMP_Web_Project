using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMP_Web_Project
{
    public partial class Login : System.Web.UI.Page
    {
        private static string strLoginUser;
        private static bool isSuper;
        private static List<WebUser> lstUsers;
        protected void Page_Load(object sender, EventArgs e)
        {
            strLoginUser = Request.ServerVariables["LOGON_USER"].ToString(); 

            if (!string.IsNullOrEmpty(strLoginUser))
            {
                strLoginUser = strLoginUser.ToLower().Replace(@"mecc\", "");

                isSuper = CheckSuperUser(strLoginUser);
                if (!Page.IsPostBack)
                {
                    LoadDepartment(strLoginUser, isSuper);
                }

            }
            else
            {
                Response.Redirect("~/ErrorPage/Error.html");
            }
        }


        private bool CheckSuperUser(string strUserName)
        {
            bool isSuperUser = false;

            string[] admin = null;
            admin = Application["superuser"].ToString().Split(new char[] { ';' });

            foreach (string suser in admin)
            {
                if (strUserName.Equals(suser))
                {
                    isSuperUser = true;
                }
            }

            return isSuperUser;
        }

        private void LoadDepartment(string strUserName, bool isSuper)
        {
            dlUsers.Items.Clear();
            ListItem item0 = new ListItem { Value = "", Text = "Please select" };
            dlUsers.Items.Add(item0);

            if (isSuper)
            {
                lstUsers = UserInfo.getDepartments(strUserName,isSuper);
                foreach(WebUser user in lstUsers)
                {
                    ListItem item = new ListItem { Value = user.departmentid, Text = user.departmentname };
                    dlUsers.Items.Add(item);
                }

                dvnewuser.Visible = true;
            }
            else
            {
                lstUsers = UserInfo.getDepartments(strUserName, isSuper);
                foreach (WebUser user in lstUsers)
                {
                    ListItem item = new ListItem { Value = user.departmentid, Text = user.departmentname };
                    dlUsers.Items.Add(item);
                                        
                    //UserInfo.currentUser = user;

                    //Response.Redirect("default.aspx");
                }

                dvnewuser.Visible = false;
                btndelete.Visible = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (dlUsers.SelectedIndex != 0)
            {
                WebUser user = new WebUser();
                user.username = strLoginUser;
                user.departmentid = dlUsers.SelectedItem.Value;
                foreach(WebUser user0 in lstUsers)
                {
                    if (user0.departmentid == user.departmentid && user0.username == user.username)
                    {
                        user = user0;
                    }
                }
                user.isSuperUser = isSuper;                

                UserInfo.currentUser = user;

                Response.Redirect("default.aspx");
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string strDeptCode = txtdeptid.Value.ToString();
            string strDeptDesc = txtdeptname.Value.ToString();
            string strNotiMethod = selmethod.Value.ToString();
            string strUserId = txtUserId.Value.ToString();
            string strWindowsUser = txtwindowsuser.Value.ToString();

            bool isExist = UserInfo.CheckDepartmentExist(strDeptCode);
            if (!isExist)
            {
                bool isSuccess = UserInfo.createdepartment(strDeptCode, strDeptDesc, strUserId, strNotiMethod, strWindowsUser, "");

                if (isSuccess)
                {
                    WebUser user = new WebUser();
                    user.username = strLoginUser;
                    user.departmentid = strDeptCode;
                    user.departmentname = strDeptDesc;
                    user.dep_userid = strUserId;
                    user.notify_method = strNotiMethod;
                    user.emailaddress = "";
                    user.isSuperUser = true;
                    user.windows_login_userid = strWindowsUser;

                    UserInfo.currentUser = user;

                    Response.Redirect("default.aspx");
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('the department/user is already exist')", true);
            }
        }

        
    }
}