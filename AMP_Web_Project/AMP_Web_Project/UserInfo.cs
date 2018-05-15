using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace AMP_Web_Project
{

        public class UserInfo
        {
            private const string SessionUser = "user";

            public static WebUser currentUser
            {
                get
                {
                    if (HttpContext.Current == null) return null;
                    return HttpContext.Current.Session[SessionUser] as WebUser;
                }
                set
                {
                    HttpContext.Current.Session[SessionUser] = value;
                }
            }

            public static List<WebUser> getDepartments(string strUserName, bool isSuperUser)
            {
                List<WebUser> lstuser = new List<WebUser>();

                DataTable dt = new DataTable();

                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["strEBMS_Snapshot_Conn"].ConnectionString;

                SqlConnection conn = new SqlConnection(strConn);


                try
                {
                    conn.Open();

                    string strSQL = "";

                    if (isSuperUser)
                    {
                        strSQL = "select Noti_Dep_Code,Noti_Dep_Desc,Dep_User_Id,Noti_Method,EmailAddress, WindowsLoginManager from Noti_Dept where status='1' ";
                    }
                    else
                    {
                        strSQL = "select Noti_Dep_Code,Noti_Dep_Desc,Dep_User_Id,Noti_Method,EmailAddress, WindowsLoginManager from Noti_Dept where status='1' and WindowsLoginManager like '%" + strUserName + "%'";
                    }

                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    SqlDataAdapter da = new SqlDataAdapter(comm);
                    da.Fill(dt);
                    da.FillSchema(dt, SchemaType.Source);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Noti_Dep_Code"] != DBNull.Value && dr["Noti_Dep_Desc"] != DBNull.Value)
                        {
                            WebUser user = new WebUser();
                            user.username = strUserName;
                            user.departmentid = dr["Noti_Dep_Code"].ToString();
                            user.departmentname = dr["Noti_Dep_Desc"].ToString();
                            user.notify_method = dr["Noti_Method"].ToString();
                            user.dep_userid = dr["Dep_User_Id"].ToString();
                            user.emailaddress = dr["EmailAddress"].ToString();
                            user.windows_login_userid = dr["WindowsLoginManager"].ToString();
                            lstuser.Add(user);
                        }
                    }
                }

                return lstuser;
            }

            public static bool CheckDepartmentExist(string strDeptCode)
            {
                bool isExist = false;
                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["strEBMS_Snapshot_Conn"].ConnectionString;

                SqlConnection conn = new SqlConnection(strConn);

                try
                {
                    conn.Open();

                    string strSQL = "select count(Noti_Dep_Code) as countdeptcode from  Noti_Dept where Noti_Dep_Code=@deptcode";

                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;

                    SqlDataReader dr = comm.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr.HasRows)
                        {
                            int ncount;
                            if (int.TryParse(dr["countdeptcode"].ToString(), out ncount))
                            {
                                ncount = int.Parse(dr["countdeptcode"].ToString());
                                if (ncount > 0)
                                {
                                    isExist = true;
                                }
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
                return isExist;

            }
            public static bool createdepartment(string strDeptCode, string strDeptDesc, string strUserId, string strNotiMethod, string strWindowsUser, string strEmailAddress)
            {
                bool isSuccess = false;
                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["strEBMS_Snapshot_Conn"].ConnectionString;

                SqlConnection conn = new SqlConnection(strConn);

                try
                {
                    conn.Open();

                    bool isExist = false;

                    string strSQL0 = "select count(Noti_Dep_Code) as nCount from NOti_Dept where Noti_Dep_Code=@deptcode";
                    SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                    comm0.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                    SqlDataReader dr = comm0.ExecuteReader();
                    if (dr.Read())
                    {
                        if (!string.IsNullOrEmpty(dr["nCount"].ToString()))
                        {
                            if (int.Parse(dr["nCount"].ToString()) > 0)
                            {
                                isExist = true;
                            }
                        }
                    }
                    dr.Close();

                    if (!isExist)
                    {
                        string strSQL = "insert into Noti_Dept(Noti_Dep_Code,Noti_Dep_Desc,Dep_User_Id,Noti_Method,WindowsLoginManager,EmailAddress,Status ) values (@deptcode,@deptdesc,@userid,@notimethod,@windowsuser,@emailaddress,'1')";

                        SqlCommand comm = new SqlCommand(strSQL, conn);
                        comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                        comm.Parameters.Add("@deptdesc", SqlDbType.VarChar, 255).Value = strDeptDesc;
                        comm.Parameters.Add("@userid", SqlDbType.VarChar, 20).Value = strUserId;
                        comm.Parameters.Add("@notimethod", SqlDbType.VarChar, 20).Value = strNotiMethod;
                        comm.Parameters.Add("@windowsuser", SqlDbType.VarChar, 50).Value = strWindowsUser;
                        comm.Parameters.Add("@emailaddress", SqlDbType.VarChar, 50).Value = strEmailAddress;

                        comm.ExecuteNonQuery();
                        isSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
                return isSuccess;
            }

            public static WebUser getCurrentUser(string strDeptCode)
            {
                WebUser user = new WebUser();
                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["strEBMS_Snapshot_Conn"].ConnectionString;

                SqlConnection conn = new SqlConnection(strConn);

                try
                {
                    conn.Open();

                    string strSQL = "select Noti_Dep_Desc, Dep_User_Id, Noti_Method, EmailAddress, WindowsLoginManager from Noti_Dept where Noti_Dep_Code=@deptcode";

                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                    SqlDataReader dr = comm.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr.HasRows)
                        {
                            user.departmentname = dr["Noti_Dep_Desc"].ToString();
                            user.notify_method = dr["Noti_Method"].ToString();
                            user.dep_userid = dr["Dep_User_Id"].ToString();
                            user.emailaddress = dr["EmailAddress"].ToString();
                            user.windows_login_userid = dr["WindowsLoginManager"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
                return user;
            }
        }
    
}