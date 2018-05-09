using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMP_Web_Project
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebUser user = new WebUser()
            {
                username = "",
                departmentid = "",
                departmentname = "",
                dep_userid = "",
                notify_method = "",
                emailaddress = "",
                isSuperUser = false
            };

            UserInfo.currentUser = user;

            Response.Redirect("Login.aspx");
        }
    }
}