using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMP_Web_Project
{

    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        public string strEBMSConn = ConfigurationManager.ConnectionStrings["strEBMS_Snapshot_Conn"].ToString();
        public string strCompareDB = "usiprddb";

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]
        public string UpdateDepartment(string strDeptCode, string strDeptDesc, string strUserId, string strNotiMethod, string strWindowsUser, string strEmailAddress)
        {
            string strSuccess = "unsuccess";

            SqlConnection conn = new SqlConnection(strEBMSConn);
            try
            {
                conn.Open();

                string strSQL = "update Noti_Dept set Noti_Dep_Desc=@deptdesc, Dep_User_Id=@userid,Noti_Method=@notimethod,WindowsLoginManager=@windowsuser, EmailAddress=@emailaddress, Status='1' where Noti_Dep_Code=@deptcode";
                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@deptdesc", SqlDbType.VarChar, 255).Value = strDeptDesc;

                comm.Parameters.Add("@notimethod", SqlDbType.VarChar, 20).Value = strNotiMethod;
                comm.Parameters.Add("@windowsuser", SqlDbType.VarChar, 50).Value = strWindowsUser;
                if (strNotiMethod == "Email")
                {
                    comm.Parameters.Add("@userid", SqlDbType.VarChar, 20).Value = "";
                    comm.Parameters.Add("@emailaddress", SqlDbType.VarChar, 50).Value = strEmailAddress;

                }
                else
                {
                    comm.Parameters.Add("@userid", SqlDbType.VarChar, 20).Value = strUserId;
                    comm.Parameters.Add("@emailaddress", SqlDbType.VarChar, 50).Value = "";

                }

                comm.ExecuteNonQuery();
                strSuccess = "success";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
            return strSuccess;
        }


        [WebMethod]
        public string getMyRules(string strDeptCode = "OPSEP", int nPageIndex = 1, int nPageRows = 20)
        {
            int nRows = 0;

            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();
                //get all rows number
                string strSQL0 = "select count(RuleId) as nRowsCount from AMP_New_Rules where Noti_Dep_Code=@deptcode and Status='A'";
                SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                comm0.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;

                SqlDataReader dr0 = comm0.ExecuteReader();
                if (dr0.Read())
                    nRows = int.Parse(dr0["nRowsCount"].ToString());

                dr0.Close();


                //get the page index of rows
                string strSQL = "select RuleId, RuleName, RuleType, ShortLeadStatusList, TriggerMinutes, LastRun, NextRun, EventNotifyDaysFrom, EventNotifyDaysTo, EventStatusFrom, EventStatusTo, EventStatusList, EmailSubject, ShowFuncId, ShowSpaceCode,ShowHierarchyFuncDesc, ShowPackageItemDateTime,ShowFuncSignage,NotesLength from ";
                strSQL += " (select row_number() over (order by RuleId) as rowno, * from AMP_New_Rules where Noti_Dep_Code=@deptcode and Status='A') as tab0 ";
                strSQL += " where rowno between @startno and @endno ";
                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@startno", SqlDbType.Int).Value = (nPageIndex - 1) * nPageRows + 1;
                comm.Parameters.Add("@endno", SqlDbType.Int).Value = (nPageIndex) * nPageRows;

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);

                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }

            string strHTML = string.Empty;

            strHTML = "<div class='table-responsive'><table id='sample-table-2' class='table table-striped table-bordered table-hover'><thead><tr><th class='center'></th><th>Rule Name</th><th>Trigger Every Minutes</th><th>Event Day From</th><th>Event Day To</th></tr></thead><tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                string strRuleInfo = string.Empty;

                strRuleInfo += dr["RuleName"].ToString() + "][" + dr["RuleType"].ToString() + "][" + dr["TriggerMinutes"].ToString() + "][" + dr["LastRun"].ToString() + "][" + dr["NextRun"].ToString() + "][" + dr["EventNotifyDaysFrom"].ToString() + "][" + dr["EventNotifyDaysTo"].ToString() + "][" + dr["ShortLeadStatusList"].ToString() + "][" + dr["EventStatusList"].ToString() + "][" + dr["ShowFuncId"].ToString() + "][" + dr["ShowSpaceCode"].ToString() + "][" + dr["ShowHierarchyFuncDesc"].ToString() + "][" + dr["ShowPackageItemDateTime"].ToString() + "][" + dr["ShowFuncSignage"].ToString() + "][" + dr["EmailSubject"].ToString() + "][" + dr["NotesLength"].ToString();

                strHTML += "<tr><td><div class='action-buttons'><a class='green' href='#' onclick='editrule(\"" + dr["RuleId"].ToString() + "\",\"" + strRuleInfo + "\"," + nPageIndex + ")'><i class='icon-pencil bigger-130'></i></a><a class='red' href='#' onclick='delrule(\"" + dr["RuleId"].ToString() + "\"," + nPageIndex + ")'><i class='icon-trash bigger-130'></i></a></div></td><td>" + dr["RuleName"].ToString() + "</td><td>" + dr["TriggerMinutes"].ToString() + "</td><td>" + dr["EventNotifyDaysFrom"].ToString() + "</td><td>" + dr["EventNotifyDaysTo"].ToString() + "</td></tr>";
            }

            strHTML += "</tbody></table>";
            strHTML += "<div class='row clearfix'><a class='btn btn-primary' onclick='addnewrule()'><i class='fa fa-plus-circle align-top bigger-125'></i>Add A New Rule</a></div> ";
            strHTML += "</div>";

            strHTML += getPager(nRows, nPageIndex, nPageRows, "showrules");

            return strHTML;
        }

        [WebMethod]
        public string SaveRule(string strRuleId, string strRuleName, string strRuleType, string strShortLeadStatusList, int nTriggerMinutes, int nEventDayFrom, int nEventDayTo, string strEventStatusList,
            int isShowFuncId, int isShowSpaceCode, int isShowFuncHierarchyDesc, int isShowPackageItemDateTime, int isShowFuncSignage, string strEmailSubject, int nNotesLength, string strDeptCode = "OPSEP", int nPageIndex = 1, int nPageRows = 20)
        {
            int nRows = 0;

            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();

                string strSQL0 = string.Empty;
                //update rule
                if (!string.IsNullOrEmpty(strRuleId))
                {
                    int nRuleId;
                    if (int.TryParse(strRuleId, out nRuleId))
                    {
                        nRuleId = int.Parse(strRuleId);
                        strSQL0 = "update AMP_New_Rules set RuleName=@rulename, RuleType=@ruletype, ShortLeadStatusList=@shortleadstatuslist, TriggerMinutes=@triggerminutes,EventNotifyDaysFrom=@eventdayfrom, ";
                        strSQL0 += " EventNotifyDaysTo =@eventdayto,EventStatusList=@eventstatuslist, ShowFuncId=@showfuncid, ShowSpaceCode=@showspacecode, ";
                        strSQL0 += " ShowHierarchyFuncDesc=@showfunchierarchydesc, ShowPackageItemDateTime=@showpackageitemdatetime,ShowFuncSignage=@showfunctionsignage,NotesLength=@notelength, EmailSubject =@emailsubject ";
                        strSQL0 += " where RuleId=@ruleid and Noti_Dep_Code=@deptcode";
                    }
                }

                else
                {
                    strSQL0 = "insert into AMP_NEW_Rules (RuleName, RuleType, ShortLeadStatusList, CreatedBy, CreatedDate, Status, TriggerMinutes, LastRun, NextRun, Noti_Dep_Code, EventNotifyDaysFrom, EventNotifyDaysTo, ";
                    strSQL0 += " EventStatusFrom, EventStatusTo, EventStatusList,ShowFuncId,ShowSpaceCode, ShowHierarchyFuncDesc, ShowPackageItemDateTime,ShowFuncSignage, NotesLength, EmailSubject) Values ";
                    strSQL0 += "(@rulename, @ruletype, @shortleadstatuslist,  'azheng_v' + cast(@ruleid as varchar), getdate(), 'A', @triggerminutes, null,null,  @deptcode,  @eventdayfrom, @eventdayto, '30', '90', ";
                    strSQL0 += " @eventstatuslist, @showfuncid, @showspacecode,@showfunchierarchydesc,@showpackageitemdatetime,@showfunctionsignage, @notelength, @emailsubject ) ";
                }

                if (!string.IsNullOrEmpty(strSQL0))
                {
                    SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                    comm0.Parameters.Add("@rulename", SqlDbType.VarChar, 255).Value = strRuleName;
                    comm0.Parameters.Add("@ruletype", SqlDbType.VarChar, 50).Value = strRuleType;
                    comm0.Parameters.Add("@shortleadstatuslist", SqlDbType.VarChar, 50).Value = strShortLeadStatusList;
                    comm0.Parameters.Add("@triggerminutes", SqlDbType.Int).Value = nTriggerMinutes;
                    comm0.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                    comm0.Parameters.Add("@eventdayfrom", SqlDbType.VarChar, 3).Value = nEventDayFrom;
                    comm0.Parameters.Add("@eventdayto", SqlDbType.VarChar, 3).Value = nEventDayTo;
                    comm0.Parameters.Add("@eventstatuslist", SqlDbType.VarChar, 20).Value = strEventStatusList;
                    comm0.Parameters.Add("@showfuncid", SqlDbType.Bit).Value = isShowFuncId;
                    comm0.Parameters.Add("@showspacecode", SqlDbType.Bit).Value = isShowSpaceCode;
                    comm0.Parameters.Add("@showfunchierarchydesc", SqlDbType.Bit).Value = isShowFuncHierarchyDesc;
                    comm0.Parameters.Add("@showpackageitemdatetime", SqlDbType.Bit).Value = isShowPackageItemDateTime;
                    comm0.Parameters.Add("@showfunctionsignage", SqlDbType.Bit).Value = isShowFuncSignage;                    
                    comm0.Parameters.Add("@notelength", SqlDbType.Int).Value = nNotesLength;
                    comm0.Parameters.Add("@emailsubject", SqlDbType.VarChar, 255).Value = strEmailSubject;
                    if (!string.IsNullOrEmpty(strRuleId))
                    {
                        int nRuleId;
                        if (int.TryParse(strRuleId, out nRuleId))
                        {
                            nRuleId = int.Parse(strRuleId);
                            comm0.Parameters.Add("@ruleid", SqlDbType.Int).Value = strRuleId;
                        }
                    }
                    else
                    {
                        comm0.Parameters.Add("@ruleid", SqlDbType.Int).Value = 5;
                    }

                    comm0.ExecuteNonQuery();
                }

                //get all rows number
                string strSQL1 = "select count(RuleId) as nRowsCount from AMP_New_Rules where Noti_Dep_Code=@deptcode and Status='A'";
                SqlCommand comm1 = new SqlCommand(strSQL1, conn);
                comm1.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;

                SqlDataReader dr1 = comm1.ExecuteReader();
                if (dr1.Read())
                    nRows = int.Parse(dr1["nRowsCount"].ToString());

                dr1.Close();

                //get the page index of rows
                string strSQL = "select RuleId, RuleName, RuleType, ShortLeadStatusList, TriggerMinutes, LastRun, NextRun, EventNotifyDaysFrom, EventNotifyDaysTo, EventStatusFrom, EventStatusTo, EventStatusList, EmailSubject, ";
                strSQL += " ShowFuncId, ShowSpaceCode,ShowHierarchyFuncDesc, ShowPackageItemDateTime,ShowFuncSignage, NotesLength from ";
                strSQL += " (select row_number() over (order by RuleId) as rowno, * from AMP_New_Rules where Noti_Dep_Code=@deptcode and Status='A') as tab0 ";
                strSQL += " where rowno between @startno and @endno ";
                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@startno", SqlDbType.Int).Value = (nPageIndex - 1) * nPageRows + 1;
                comm.Parameters.Add("@endno", SqlDbType.Int).Value = (nPageIndex) * nPageRows;

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);

                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }

            string strHTML = string.Empty;

            strHTML = "<div class='table-responsive'><table id='sample-table-2' class='table table-striped table-bordered table-hover'><thead><tr><th class='center'></th><th>Rule Name</th><th>Trigger Every Minutes</th><th>Event Day From</th><th>Event Day To</th></tr></thead><tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                string strRuleInfo = string.Empty;

                strRuleInfo += dr["RuleName"].ToString() + "][" + dr["RuleType"].ToString() + "][" + dr["TriggerMinutes"].ToString() + "][" + dr["LastRun"].ToString() + "][" + dr["NextRun"].ToString() + "][" + dr["EventNotifyDaysFrom"].ToString() + "][" + dr["EventNotifyDaysTo"].ToString() + "][" + dr["ShortLeadStatusList"].ToString() + "][" + dr["EventStatusList"].ToString() + "][" + dr["ShowFuncId"].ToString() + "][" + dr["ShowSpaceCode"].ToString() + "][" + dr["ShowHierarchyFuncDesc"].ToString() + "][" + dr["ShowPackageItemDateTime"].ToString() + "][" + dr["ShowFuncSignage"].ToString() + "][" + dr["EmailSubject"].ToString() + "][" + dr["NotesLength"].ToString();

                strHTML += "<tr><td><div class='action-buttons'><a class='green' href='#' onclick='editrule(\"" + dr["RuleId"].ToString() + "\",\"" + strRuleInfo + "\"," + nPageIndex + ")'><i class='icon-pencil bigger-130'></i></a><a class='red' href='#' onclick='delrule(\"" + dr["RuleId"].ToString() + "\"," + nPageIndex + ")'><i class='icon-trash bigger-130'></i></a></div></td><td>" + dr["RuleName"].ToString() + "</td><td>" + dr["TriggerMinutes"].ToString() + "</td><td>" + dr["EventNotifyDaysFrom"].ToString() + "</td><td>" + dr["EventNotifyDaysTo"].ToString() + "</td></tr>";
            }

            strHTML += "</tbody></table>";
            strHTML += "<div class='row clearfix'><a class='btn btn-primary' onclick='addnewrule()'><i class='fa fa-plus-circle align-top bigger-125'></i>Add A New Rule</a></div> ";
            strHTML += "</div>";

            strHTML += getPager(nRows, nPageIndex, nPageRows, "showrules");

            return strHTML;
        }

        [WebMethod]
        public string getMyResource(string strDeptCode = "OPSEP", int nPageIndex = 1, int nPageRows = 20, string strSearchDept = "", string strSearchKeyWords = "")
        {
            DataTable dt = new DataTable();
            int nRows;

            dt = getSearchResourceData(strDeptCode, nPageIndex, nPageRows, strSearchDept, strSearchKeyWords, out nRows);

            string strHTML = string.Empty;

            DataTable dtDepts = getDepartments();

            strHTML = "<div class='message-navbar align-center clearfix'><div class='pull-left'><span>";
            strHTML += " <select class='input-small' id='sldept'> <option value=''>Search by Department</option> ";
            foreach (DataRow drdept in dtDepts.Rows)
            {
                if (strSearchDept.Equals(drdept["EV065_DEPT_CODE"].ToString()))
                {
                    strHTML += "<option value='" + drdept["EV065_DEPT_CODE"].ToString() + "' selected>" + drdept["EV065_DEPT_DESC"].ToString() + "</option> ";
                }
                else
                {
                    strHTML += "<option value='" + drdept["EV065_DEPT_CODE"].ToString() + "'>" + drdept["EV065_DEPT_DESC"].ToString() + "</option> ";
                }

            }

            if (!string.IsNullOrEmpty(strSearchKeyWords))
            {
                strHTML += " </select></span><span><input type='text' id='txtsearchresource' class='input-large' placeholder='resource name or code' value='" + strSearchKeyWords + "' /></span>";
            }
            else
            {
                strHTML += " </select></span><span><input type='text' id='txtsearchresource' class='input-large' placeholder='resource name or code' value='' /></span>";
            }

            strHTML += " <span> <a class='btn btn-xs btn-purple' onclick='searchresource()'> <i class='icon-search nav-search-icon'></i> Search</a></span></div>";

            strHTML += getHeaderPager(nRows, nPageIndex, nPageRows, "showres");
            strHTML += "</div>";

            //strHTML = "<div class='message-navbar align-center clearfix'><div class='messagebar-item-left'><div class='nav-search minimized'><form class='form-search'><span class='input-icon'><input type='text' autocomplete='off' class='input-small nav-search-input' placeholder='Search inbox ...' /><i class='icon-search nav-search-icon'></i></span></form></div></div></div>";

            strHTML += "<div class='table-responsive'><table id='sample-table-2' class='table table-striped table-bordered table-hover'><thead><tr><th class='center'><div class='action-buttons'><a class='red' title='Remove All' href='#' onclick='updres(\"all\",\"all\", \"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></th><th><div><a class='green' title='Add All' href='#' onclick='updres(\"all\",\"all\", \"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></th><th>Resource Description</th><th>Resource Type</th><th>Resource Code</th><th class='hidden-480 hidden-sm hidden-xs'>Resource Type Description</th><th class='hidden-480 hidden-sm hidden-xs'>Department</th><th class='hidden-480 hidden-sm hidden-xs'>Mgmt Rpt Code</th></tr></thead><tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                strHTML += "<tr> ";

                if (dr["Noti_Dep_Code"] == DBNull.Value)
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='green' href='#' onclick='updres(\"" + dr["EV370_NEW_RES_TYPE"].ToString() + "\",\"" + dr["EV370_RES_CODE"].ToString() + "\",\"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></td>";
                }
                else
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' checked onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='red' href='#' onclick='updres(\"" + dr["EV370_NEW_RES_TYPE"].ToString() + "\",\"" + dr["EV370_RES_CODE"].ToString() + "\",\"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></td>";
                }
                strHTML += "<td>" + dr["EV370_CODE_DESC"].ToString() + "</td><td>" + dr["EV370_NEW_RES_TYPE"].ToString() + "</td><td>" + dr["EV370_RES_CODE"].ToString() + "</td><td  class='hidden-480 hidden-sm hidden-xs'>" + dr["EV370_TYPE_DESC"].ToString() + "</td><td  class='hidden-480 hidden-sm hidden-xs'>" + dr["EV065_DEPT_DESC"].ToString() + "</td><td class='hidden-480 hidden-sm hidden-xs'>" + dr["EV370_MGMT_RPT_CODE"].ToString() + "</td></tr>";
            }
            strHTML += "</tbody></table></div>";

            strHTML += getPager(nRows, nPageIndex, nPageRows, "showres");

            return strHTML;
        }

        private DataTable getSearchResourceData(string strDeptCode, int nPageIndex, int nPageRows, string strSearchDept, string strSearchKeyWords, out int nRows)
        {
            DataTable dt = new DataTable();
            nRows = 0;

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();
                string strSQL0 = string.Empty;

                strSQL0 = "SELECT count(EV370_NEW_RES_TYPE+EV370_RES_CODE) as nRowsCount ";
                strSQL0 += " FROM " + strCompareDB + ".DBO.EV370_RES_MASTER LEFT JOIN " + strCompareDB + ".DBO.EV065_DEPT_MASTER on EV370_RES_DEPT = EV065_DEPT_CODE and EV370_ORG_CODE = EV065_ORG_CODE AND EV370_RES_CODE<>''";
                strSQL0 += " LEFT JOIN EBMS_Snapshot.dbo.AMP_Noti_ResDep resde on EV370_NEW_RES_TYPE = resde.New_Res_Type and EV370_RES_CODE = resde.Res_Code and resde.Noti_Dep_Code = @deptcode ";
                strSQL0 += " where EV370_STATUS = 'A' and EV065_STATUS = 'A' and EV370_ORG_CODE = '10' ";

                if (!string.IsNullOrEmpty(strSearchDept))
                {
                    strSQL0 += " and (EV370_RES_DEPT='" + strSearchDept + "') ";
                }

                if (!string.IsNullOrEmpty(strSearchKeyWords))
                {
                    if (strSearchKeyWords.IndexOf(' ') > -1)
                    {
                        string[] arrKeyWords = strSearchKeyWords.Split(' ');
                        strSQL0 += "  ";
                        foreach (string keyword in arrKeyWords)
                        {
                            strSQL0 += " and (EV370_CODE_DESC like '%" + keyword + "%' or  EV370_NEW_RES_TYPE+EV370_RES_CODE like '%" + keyword + "%') ";
                        }

                    }
                    else
                    {
                        strSQL0 += " and (EV370_CODE_DESC like '%" + strSearchKeyWords + "%' or  EV370_NEW_RES_TYPE+EV370_RES_CODE like '%" + strSearchKeyWords + "%') ";
                    }
                }

                SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                comm0.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                SqlDataReader dr0 = comm0.ExecuteReader();
                if (dr0.Read())
                    nRows = int.Parse(dr0["nRowsCount"].ToString());

                dr0.Close();

                if (nPageIndex * nPageRows > nRows)
                    nPageIndex = Math.Abs(nRows / nPageRows) + 1;

                string strSQL = string.Empty;

                strSQL = " SELECT EV370_CODE_DESC, EV370_NEW_RES_TYPE, EV370_TYPE_DESC, EV370_RES_CODE, EV065_DEPT_DESC, EV370_MGMT_RPT_CODE, Noti_Dep_Code FROM";
                strSQL += " (SELECT row_number() over (order by EV065_DEPT_DESC, EV370_NEW_RES_TYPE, EV370_RES_CODE) as rowno, EV370_CODE_DESC, EV370_NEW_RES_TYPE, EV370_TYPE_DESC, ";
                strSQL += " EV370_RES_CODE, EV065_DEPT_DESC, EV370_MGMT_RPT_CODE, resde.Noti_Dep_Code ";
                strSQL += " FROM " + strCompareDB + ".DBO.EV370_RES_MASTER LEFT JOIN " + strCompareDB + ".DBO.EV065_DEPT_MASTER on EV370_RES_DEPT=EV065_DEPT_CODE and EV370_ORG_CODE=EV065_ORG_CODE AND EV370_RES_CODE<>''";
                strSQL += " LEFT JOIN EBMS_Snapshot.dbo.AMP_Noti_ResDep resde on EV370_NEW_RES_TYPE=resde.New_Res_Type and EV370_RES_CODE=resde.Res_Code and resde.Noti_Dep_Code=@deptcode";
                strSQL += " where EV370_STATUS='A' and EV065_STATUS='A' and EV370_ORG_CODE='10' ";

                if (!string.IsNullOrEmpty(strSearchDept))
                {
                    strSQL += " and (EV370_RES_DEPT='" + strSearchDept + "') ";
                }

                if (!string.IsNullOrEmpty(strSearchKeyWords))
                {
                    if (strSearchKeyWords.IndexOf(' ') > -1)
                    {
                        string[] arrKeyWords = strSearchKeyWords.Split(' ');
                        strSQL += "  ";
                        foreach (string keyword in arrKeyWords)
                        {
                            strSQL += " and (EV370_CODE_DESC like '%" + keyword + "%' or  EV370_NEW_RES_TYPE+EV370_RES_CODE like '%" + keyword + "%') ";
                        }

                    }
                    else
                    {
                        strSQL += " and (EV370_CODE_DESC like '%" + strSearchKeyWords + "%' or  EV370_NEW_RES_TYPE+EV370_RES_CODE like '%" + strSearchKeyWords + "%') ";
                    }
                }

                strSQL += ") AS tab0";
                strSQL += " where rowno between @startno and @endno ";

                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@startno", SqlDbType.Int).Value = (nPageIndex - 1) * nPageRows + 1;
                comm.Parameters.Add("@endno", SqlDbType.Int).Value = (nPageIndex) * nPageRows;

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);
                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        private DataTable getDepartments()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();
                string
                    strSQL = "SELECT distinct EV065_DEPT_CODE, EV065_DEPT_DESC FROM  " + strCompareDB + ".dbo.EV065_DEPT_MASTER where EV065_ORG_CODE='10' and EV065_STATUS='A' and EV065_RES_DPT_STS='A' ";

                SqlCommand comm = new SqlCommand(strSQL, conn);
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);
                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        [WebMethod]
        public string getMyFunctionUsage(string strDeptCode = "OPSEP", int nPageIndex = 1, int nPageRows = 20)
        {
            int nRows = 0;

            DataTable dt = getfunctionUsageData(strDeptCode, nPageIndex, nPageRows, out nRows);

            string strHTML = string.Empty;

            strHTML = "<div class='table-responsive'><table id='sample-table-2' class='table table-striped table-bordered table-hover'><thead><tr><th class='center'><div class='action-buttons'><a class='red' href='#' title='Remove All' onclick='updfunc(\"all\",\"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></th><th><div class='action-buttons'><a class='green' title='Add All' href='#' onclick='updfunc(\"all\",\"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></th><th>Function Usage Code</th><th>Function Usage Description</th></tr></thead><tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                strHTML += "<tr> ";

                if (dr["Dept_Code"] == DBNull.Value)
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='green' href='#' onclick='updfunc(\"" + dr["EV585_FUNC_TYPE"].ToString() + "\",\"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></td>";
                }
                else
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' checked onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='red' href='#' onclick='updfunc(\"" + dr["EV585_FUNC_TYPE"].ToString() + "\",\"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></td>";
                }
                strHTML += "<td>" + dr["EV585_FUNC_TYPE"].ToString() + "</td><td>" + dr["EV585_FUNC_TYPE_DESC"].ToString() + "</td></tr>";
            }
            strHTML += "</tbody></table></div>";

            strHTML += getPager(nRows, nPageIndex, nPageRows, "showfunc");

            return strHTML;
        }

        private DataTable getfunctionUsageData(string strDeptCode, int nPageIndex, int nPageRows, out int nRows)
        {
            DataTable dt = new DataTable();
            nRows = 0;

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();

                //get all rows number
                string strSQL0 = "SELECT count(EV585_FUNC_TYPE) as nRowsCount FROM " + strCompareDB + ".dbo.EV585_FUNC_TYPES where EV585_ORG_CODE='10' ";
                SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                comm0.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;

                SqlDataReader dr0 = comm0.ExecuteReader();
                if (dr0.Read())
                    nRows = int.Parse(dr0["nRowsCount"].ToString());

                dr0.Close();

                string strSQL = " SELECT EV585_FUNC_TYPE, EV585_FUNC_TYPE_DESC, Dept_Code FROM";
                strSQL += " (SELECT  row_number() over (order by EV585_FUNC_TYPE_DESC) as rowno, EV585_FUNC_TYPE, EV585_FUNC_TYPE_DESC, funcdep.Dept_Code ";
                strSQL += " FROM " + strCompareDB + ".dbo.EV585_FUNC_TYPES left join AMP_Noti_FuncTypeDep funcdep on EV585_FUNC_TYPE = funcdep.FuncType and funcdep.Dept_Code = @deptcode ";
                strSQL += " where EV585_ORG_CODE = '10') AS tab0";
                strSQL += " where rowno between @startno and @endno ";
                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@startno", SqlDbType.Int).Value = (nPageIndex - 1) * nPageRows + 1;
                comm.Parameters.Add("@endno", SqlDbType.Int).Value = (nPageIndex) * nPageRows;

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);
                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        [WebMethod]
        public string getMyDocumentHeadings(string strDeptCode = "OPSEP", int nPageIndex = 1, int nPageRows = 20)
        {
            int nRows = 0;

            DataTable dt = getDocumentHeadingData(strDeptCode, nPageIndex, nPageRows, out nRows);

            string strHTML = string.Empty;

            strHTML = "<div class='table-responsive'><table id='sample-table-2' class='table table-striped table-bordered table-hover'><thead><tr><th class='center'><div class='action-buttons'><a class='red' title='Remove All' href='#' onclick='upddoc(-1,-1,\"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></th><th><div class='action-buttons'><a class='green' title='Add All' href='#' onclick='upddoc(-1,-1,\"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></th><th>Document Heading</th></tr></thead><tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                strHTML += "<tr> ";

                if (dr["Noti_Dept_Code"] == DBNull.Value)
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='green' href='#' onclick='upddoc(" + dr["DOC_HDG_SEQ1"].ToString() + "," + dr["DOC_HDG_SEQ2"].ToString() + ",\"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></td>";
                }
                else
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' checked onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='red' href='#' onclick='upddoc(" + dr["DOC_HDG_SEQ1"].ToString() + "," + dr["DOC_HDG_SEQ2"].ToString() + ",\"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></td>";
                }
                strHTML += "<td>" + dr["HDG_DESC"].ToString() + "</td></tr>";
            }
            strHTML += "</tbody></table></div>";

            strHTML += getPager(nRows, nPageIndex, nPageRows, "showdoc");

            return strHTML;
        }

        private DataTable getDocumentHeadingData(string strDeptCode, int nPageIndex, int nPageRows, out int nRows)
        {
            DataTable dt = new DataTable();
            nRows = 0;

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();

                //get all rows number
                string strSQL0 = "SELECT count(hdg_1.MM442_HDG_SEQ * 10000 + hdg_2.MM442_HDG_SEQ) as nRowsCount FROM ";
                strSQL0 += " (SELECT MM442_ORG_CODE, MM442_HDG_SEQ, MM442_HDG_DESC, MM442_HDG_TYPE, MM442_HDG_LEVEL, MM442_HDG_SEQ_1, MM442_DOC_SUBJECT   ";
                strSQL0 += " FROM " + strCompareDB + ".dbo.MM442_DOC_HEADINGS   where  MM442_HDG_TYPE='G' and MM442_DOC_SUBJECT in ('EVD','STE') and MM442_HDG_LEVEL='1') as hdg_1 ";
                strSQL0 += " INNER JOIN  ";
                strSQL0 += " (SELECT MM442_ORG_CODE, MM442_HDG_SEQ, MM442_HDG_DESC, MM442_HDG_TYPE, MM442_HDG_LEVEL, MM442_HDG_SEQ_1, MM442_DOC_SUBJECT   ";
                strSQL0 += " FROM " + strCompareDB + ".dbo.MM442_DOC_HEADINGS   where  MM442_HDG_TYPE='G' and MM442_DOC_SUBJECT in ('EVD','STE') and MM442_HDG_LEVEL='2') as hdg_2 ";
                strSQL0 += " ON hdg_1.MM442_HDG_SEQ=hdg_2.MM442_HDG_SEQ_1 and hdg_1.MM442_ORG_CODE=hdg_2.MM442_ORG_CODE ";

                SqlCommand comm0 = new SqlCommand(strSQL0, conn);

                SqlDataReader dr0 = comm0.ExecuteReader();
                if (dr0.Read())
                    nRows = int.Parse(dr0["nRowsCount"].ToString());

                dr0.Close();

                string strSQL = " SELECT DOC_HDG_SEQ1, DOC_HDG_SEQ2,HDG_DESC, Noti_Dept_Code FROM";
                strSQL += " (SELECT  row_number() over (ORDER BY hdg_1.MM442_HDG_DESC , hdg_2.MM442_HDG_Desc) as rowno, hdg_1.MM442_HDG_SEQ as DOC_HDG_SEQ1, ";
                strSQL += " hdg_1.MM442_HDG_DESC + ' : ' + hdg_2.MM442_HDG_DESC as HDG_DESC, hdg_2.MM442_HDG_SEQ as DOC_HDG_SEQ2, docdep.Noti_Dept_Code FROM";
                strSQL += " (SELECT MM442_ORG_CODE, MM442_HDG_SEQ, MM442_HDG_DESC, MM442_HDG_TYPE, MM442_HDG_LEVEL, MM442_HDG_SEQ_1, MM442_DOC_SUBJECT  ";
                strSQL += " FROM " + strCompareDB + ".dbo.MM442_DOC_HEADINGS   where  MM442_HDG_TYPE='G' and MM442_DOC_SUBJECT in ('EVD','STE') and MM442_HDG_LEVEL='1') as hdg_1";
                strSQL += " INNER JOIN ";
                strSQL += " (SELECT MM442_ORG_CODE, MM442_HDG_SEQ, MM442_HDG_DESC, MM442_HDG_TYPE, MM442_HDG_LEVEL, MM442_HDG_SEQ_1, MM442_DOC_SUBJECT  ";
                strSQL += " FROM " + strCompareDB + ".dbo.MM442_DOC_HEADINGS   where  MM442_HDG_TYPE='G' and MM442_DOC_SUBJECT in ('EVD','STE') and MM442_HDG_LEVEL='2') as hdg_2";
                strSQL += " ON hdg_1.MM442_HDG_SEQ=hdg_2.MM442_HDG_SEQ_1 and hdg_1.MM442_ORG_CODE=hdg_2.MM442_ORG_CODE";
                strSQL += " LEFT JOIN AMP_Noti_DocHDGDept docdep ON hdg_1.MM442_HDG_SEQ=docdep.HDG_SEQ_1 and hdg_2.MM442_HDG_SEQ=docdep.HDG_SEQ_2 and docdep.Noti_Dept_Code=@deptcode) AS tab0";
                strSQL += " where rowno between @startno and @endno ";
                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@startno", SqlDbType.Int).Value = (nPageIndex - 1) * nPageRows + 1;
                comm.Parameters.Add("@endno", SqlDbType.Int).Value = (nPageIndex) * nPageRows;

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);
                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        [WebMethod]
        public string getMyNoteClass(string strDeptCode = "OPSEP", int nPageIndex = 1, int nPageRows = 20)
        {
            int nRows = 0;

            DataTable dt = getNoteClassData(strDeptCode, nPageIndex, nPageRows, out nRows);

            string strHTML = string.Empty;

            strHTML = "<div class='table-responsive'><table id='sample-table-2' class='table table-striped table-bordered table-hover'><thead><tr><th class='center'><div class='action-buttons'><a class='red' href='#' onclick='updnote(\"all\",\"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></th><th><div class='action-buttons'><a class='green' href='#' onclick='updnote(\"all\",\"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></th><th>Note Class Code</th><th>Note Class Description</th></tr></thead><tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                strHTML += "<tr> ";

                if (dr["Noti_Dept_Code"] == DBNull.Value)
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='green' href='#' onclick='updnote(\"" + dr["CC015_NOTE_CLASS"].ToString() + "\",\"add\"," + nPageIndex + ")'><i class='fa fa-plus-square bigger-120'></i></a></div></td>";
                }
                else
                {
                    strHTML += "<td class='center'><label><input type ='checkbox' class='ace disabled' checked onclick='return false'/><span class='lbl'></span></label></td><td><div class='action-buttons'><a class='red' href='#' onclick='updnote(\"" + dr["CC015_NOTE_CLASS"].ToString() + "\",\"del\"," + nPageIndex + ")'><i class='icon-trash bigger-120'></i></a></div></td>";
                }
                strHTML += "<td>" + dr["CC015_NOTE_CLASS"].ToString() + "</td><td>" + dr["CC015_NOTE_CLS_DESC"].ToString() + "</td></tr>";
            }
            strHTML += "</tbody></table></div>";

            strHTML += getPager(nRows, nPageIndex, nPageRows, "shownote");

            return strHTML;
        }

        private DataTable getNoteClassData(string strDeptCode, int nPageIndex, int nPageRows, out int nRows)
        {
            DataTable dt = new DataTable();

            nRows = 0;

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();

                //get all rows number
                string strSQL0 = "SELECT COUNT(CC015_NOTE_CLASS) as nRowsCount FROM " + strCompareDB + ".dbo.CC015_NOTE_CLASS ";
                strSQL0 += " where CC015_RETIRE = 'N' and(CC015_USE_EVENTS = 'Y' or CC015_USE_ORDERS = 'Y') ";

                SqlCommand comm0 = new SqlCommand(strSQL0, conn);

                SqlDataReader dr0 = comm0.ExecuteReader();
                if (dr0.Read())
                    nRows = int.Parse(dr0["nRowsCount"].ToString());

                dr0.Close();

                string strSQL = " SELECT CC015_NOTE_CLASS, CC015_NOTE_CLS_DESC,Noti_Dept_Code FROM";
                strSQL += " (SELECT  row_number() over (order by CC015_NOTE_CLS_DESC) as rowno, CC015_NOTE_CLASS, CC015_NOTE_CLS_DESC, Noti_Dept_Code ";
                strSQL += " FROM " + strCompareDB + ".dbo.CC015_NOTE_CLASS LEFT JOIN AMP_Noti_NoteClassDep notedep ON CC015_NOTE_CLASS=notedep.Note_Class and notedep.Noti_Dept_Code=@deptcode ";
                strSQL += " where CC015_RETIRE = 'N' and(CC015_USE_EVENTS = 'Y' or CC015_USE_ORDERS = 'Y') ) AS tab0";
                strSQL += " where rowno between @startno and @endno ";
                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@startno", SqlDbType.Int).Value = (nPageIndex - 1) * nPageRows + 1;
                comm.Parameters.Add("@endno", SqlDbType.Int).Value = (nPageIndex) * nPageRows;

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);
                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        [WebMethod]
        public string getActivity(int uniqueno)
        {
            string strActivityHTML = string.Empty;

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();

                //get all rows number
                string strSQL = "SELECT DiaryHTML  FROM EBMS_Snapshot.dbo.AMP_Logs where UniqueNumber=@uniqueno ";

                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@uniqueno", SqlDbType.Int).Value = uniqueno;

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read() && dr.HasRows)
                {
                    strActivityHTML = dr["DiaryHTML"].ToString();
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
            return strActivityHTML;
        }

        [WebMethod]
        public string DelUser(string strUserCode)
        {
            string strHTML = string.Empty;

            SqlConnection conn = new SqlConnection(strEBMSConn);
            try
            {
                conn.Open();

                string strSQL = "Update EBMS_Snapshot.dbo.Noti_Dept set Status='0' where Noti_Dep_Code=@deptcode";

                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strUserCode;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }

            return strHTML;
        }


        [WebMethod]
        public string DelRule(string strDeptCode = "OPSEP", string RuleId = "0", int nPageIndex = 1, int nPageRows = 20)
        {
            string strHTML = string.Empty;

            SqlConnection conn = new SqlConnection(strEBMSConn);
            try
            {
                conn.Open();

                string strSQL = "Update EBMS_Snapshot.dbo.AMP_New_Rules set Status='R' where Noti_Dep_Code=@deptcode and RuleId=@ruleid";

                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@ruleid", SqlDbType.Int).Value = int.Parse(RuleId);
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }

            strHTML = getMyRules(strDeptCode, nPageIndex, nPageRows);
            return strHTML;
        }

        [WebMethod]
        public string UpdateResource(string strDeptCode, string ResourceType, string ResourceCode, string Action, int nPageIndex, int nPageRows = 20, string strSearchDept = "", string strSearchKeyWords = "")
        {
            string strHTML = string.Empty;

            SqlConnection conn = new SqlConnection(strEBMSConn);

            if (ResourceType == "all" && ResourceCode == "all")
            {

                int nRows = 0;

                DataTable dt = getSearchResourceData(strDeptCode, nPageIndex, nPageRows, strSearchDept, strSearchKeyWords, out nRows);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["EV370_NEW_RES_TYPE"].ToString()) && !string.IsNullOrEmpty(dr["EV370_RES_CODE"].ToString()))
                        {
                            UpdateResource(strDeptCode, dr["EV370_NEW_RES_TYPE"].ToString(), dr["EV370_RES_CODE"].ToString(), Action);
                        }
                    }

                }
            }
            else
            {
                UpdateResource(strDeptCode, ResourceType, ResourceCode, Action);
            }

            strHTML = getMyResource(strDeptCode, nPageIndex, nPageRows, strSearchDept, strSearchKeyWords);

            return strHTML;
        }

        private void UpdateResource(string strDeptCode, string ResourceType, string ResourceCode, string Action)
        {
            SqlConnection conn = new SqlConnection(strEBMSConn);

            if (Action == "add")
            {
                try
                {
                    conn.Open();

                    bool isExist = false;

                    string strSQL0 = "select Count(*) as nCount from EBMS_Snapshot.dbo.AMP_Noti_ResDep where New_Res_Type=@restype and Res_Code=@rescode and Noti_Dep_Code=@deptcode";
                    SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                    comm0.Parameters.Add("@restype", SqlDbType.VarChar, 6).Value = ResourceType;
                    comm0.Parameters.Add("@rescode", SqlDbType.VarChar, 12).Value = ResourceCode;
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
                        string strSQL = "INSERT INTO EBMS_Snapshot.dbo.AMP_Noti_ResDep(New_Res_Type,Res_Code,Noti_Dep_Code ) values (@restype, @rescode, @deptcode) ";

                        SqlCommand comm = new SqlCommand(strSQL, conn);
                        comm.Parameters.Add("@restype", SqlDbType.VarChar, 6).Value = ResourceType;
                        comm.Parameters.Add("@rescode", SqlDbType.VarChar, 12).Value = ResourceCode;
                        comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                        comm.ExecuteNonQuery();
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
            }
            else if (Action == "del")
            {
                try
                {
                    conn.Open();

                    string strSQL = "Delete From EBMS_Snapshot.dbo.AMP_Noti_ResDep where New_Res_Type=@restype and Res_Code=@rescode and Noti_Dep_Code=@deptcode ";

                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    comm.Parameters.Add("@restype", SqlDbType.VarChar, 6).Value = ResourceType;
                    comm.Parameters.Add("@rescode", SqlDbType.VarChar, 12).Value = ResourceCode;
                    comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                    comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        [WebMethod]
        public void resetnextrun(string strDeptCode, string RuleId)
        {
            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();

                string strSQL = "Update EBMS_Snapshot.dbo.AMP_New_Rules set nextrun =null where Noti_Dep_Code=@deptcode and RuleId=@ruleid";

                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@ruleid", SqlDbType.Int).Value = int.Parse(RuleId);
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
        }

        [WebMethod]
        public string UpdateFuncType(string strDeptCode, string FuncType, string Action, int nPageIndex, int nPageRows = 20)
        {
            string strHTML = string.Empty;

            if (FuncType == "all")
            {
                int nRows = 0;
                DataTable dt = getfunctionUsageData(strDeptCode, nPageIndex, nPageRows, out nRows);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["EV585_FUNC_TYPE"].ToString()))
                        {
                            UpdateFuncType(strDeptCode, dr["EV585_FUNC_TYPE"].ToString(), Action);
                        }
                    }
                }
            }
            else
            {
                UpdateFuncType(strDeptCode, FuncType, Action);
            }

            strHTML = getMyFunctionUsage(strDeptCode, nPageIndex, nPageRows);

            return strHTML;
        }

        public void UpdateFuncType(string strDeptCode, string FuncType, string Action)
        {
            SqlConnection conn = new SqlConnection(strEBMSConn);

            if (Action == "add")
            {
                try
                {
                    conn.Open();

                    bool isExist = false;

                    string strSQL0 = "select Count(*) as nCount from AMP_Noti_FuncTypeDep where FuncType=@functype and Dept_Code=@deptcode ";
                    SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                    comm0.Parameters.Add("@functype", SqlDbType.VarChar, 5).Value = FuncType;
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
                        string strSQL = "INSERT INTO AMP_Noti_FuncTypeDep(FuncType,Dept_Code) values (@functype, @deptcode) ";

                        SqlCommand comm = new SqlCommand(strSQL, conn);
                        comm.Parameters.Add("@functype", SqlDbType.VarChar, 5).Value = FuncType;
                        comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                        comm.ExecuteNonQuery();
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
            }
            else if (Action == "del")
            {
                try
                {
                    conn.Open();

                    string strSQL = "Delete From AMP_Noti_FuncTypeDep where FuncType=@functype and Dept_Code=@deptcode ";

                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    comm.Parameters.Add("@functype", SqlDbType.VarChar, 5).Value = FuncType;
                    comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                    comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        [WebMethod]
        public string UpdateDocHeading(string strDeptCode, int HeadSeq1, int HeadSeq2, string Action, int nPageIndex, int nPageRows = 20)
        {
            string strHTML = string.Empty;

            if (HeadSeq1 == -1 && HeadSeq2 == -1)
            {
                int nRows = 0;
                DataTable dt = getDocumentHeadingData(strDeptCode, nPageIndex, nPageRows, out nRows);

                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["DOC_HDG_SEQ1"].ToString()) && !string.IsNullOrEmpty(dr["DOC_HDG_SEQ2"].ToString()))
                    {
                        int nSeq1 = int.Parse(dr["DOC_HDG_SEQ1"].ToString());
                        int nSeq2 = int.Parse(dr["DOC_HDG_SEQ2"].ToString());
                        UpdateDocHeadingData(strDeptCode, nSeq1, nSeq2, Action);
                    }
                }
            }
            else
            {
                UpdateDocHeadingData(strDeptCode, HeadSeq1, HeadSeq2, Action);
            }

            strHTML = getMyDocumentHeadings(strDeptCode, nPageIndex, nPageRows);

            return strHTML;
        }

        private void UpdateDocHeadingData(string strDeptCode, int HeadSeq1, int HeadSeq2, string Action)
        {
            SqlConnection conn = new SqlConnection(strEBMSConn);

            if (Action == "add")
            {
                try
                {
                    conn.Open();

                    bool isExist = false;

                    string strSQL0 = "select Count(*) as nCount from AMP_Noti_DocHDGDept where HDG_SEQ_1=@headseq1 and HDG_SEQ_2=@headseq2 and Noti_Dept_Code=@deptcode ";
                    SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                    comm0.Parameters.Add("@headseq1", SqlDbType.Int).Value = HeadSeq1;
                    comm0.Parameters.Add("@headseq2", SqlDbType.Int).Value = HeadSeq2;
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
                        string strSQL = "INSERT INTO AMP_Noti_DocHDGDept(HDG_SEQ_1, HDG_SEQ_2,Noti_Dept_Code) values (@headseq1, @headseq2, @deptcode) ";

                        SqlCommand comm = new SqlCommand(strSQL, conn);
                        comm.Parameters.Add("@headseq1", SqlDbType.Int).Value = HeadSeq1;
                        comm.Parameters.Add("@headseq2", SqlDbType.Int).Value = HeadSeq2;
                        comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                        comm.ExecuteNonQuery();
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
            }
            else if (Action == "del")
            {
                try
                {
                    conn.Open();

                    string strSQL = "Delete From AMP_Noti_DocHDGDept where HDG_SEQ_1=@headseq1 and HDG_SEQ_2=@headseq2 and Noti_Dept_Code=@deptcode ";

                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    comm.Parameters.Add("@headseq1", SqlDbType.Int).Value = HeadSeq1;
                    comm.Parameters.Add("@headseq2", SqlDbType.Int).Value = HeadSeq2;
                    comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                    comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        [WebMethod]
        public string UpdateNoteClass(string strDeptCode, string NoteClassCode, string Action, int nPageIndex, int nPageRows = 20)
        {
            if (NoteClassCode == "all")
            {
                int nRows = 0;
                DataTable dt = getNoteClassData(strDeptCode, nPageIndex, nPageRows, out nRows);

                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["CC015_NOTE_CLASS"].ToString()))
                    {
                        UpdateNoteClassData(strDeptCode, dr["CC015_NOTE_CLASS"].ToString(), Action);
                    }
                }
            }
            else
            {
                UpdateNoteClassData(strDeptCode, NoteClassCode, Action);
            }

            string strHTML = string.Empty;

            strHTML = getMyNoteClass(strDeptCode, nPageIndex, nPageRows);

            return strHTML;
        }

        private void UpdateNoteClassData(string strDeptCode, string NoteClassCode, string Action)
        {

            SqlConnection conn = new SqlConnection(strEBMSConn);

            if (Action == "add")
            {
                try
                {
                    conn.Open();
                    bool isExist = false;

                    string strSQL0 = "select Count(*) as nCount from AMP_Noti_NoteClassDep where Note_Class=@noteclass and Noti_Dept_Code=@deptcode ";
                    SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                    comm0.Parameters.Add("@noteclass", SqlDbType.VarChar, 8).Value = NoteClassCode;
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

                        string strSQL = "INSERT INTO AMP_Noti_NoteClassDep (Note_Class,Noti_Dept_Code) values (@noteclass, @deptcode) ";

                        SqlCommand comm = new SqlCommand(strSQL, conn);
                        comm.Parameters.Add("@noteclass", SqlDbType.VarChar, 8).Value = NoteClassCode;
                        comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                        comm.ExecuteNonQuery();
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
            }
            else if (Action == "del")
            {
                try
                {
                    conn.Open();

                    string strSQL = "Delete From AMP_Noti_NoteClassDep where Note_Class=@noteclass and Noti_Dept_Code=@deptcode ";

                    SqlCommand comm = new SqlCommand(strSQL, conn);
                    comm.Parameters.Add("@noteclass", SqlDbType.VarChar, 8).Value = NoteClassCode;
                    comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                    comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
            }

        }


        [WebMethod]
        public string getMyDeptHistory(string strDeptCode, int nPageIndex, int nPageRows = 20)
        {
            int nRows = 0;

            DataTable dt = getDeptHistoryData(strDeptCode, nPageIndex, nPageRows, out nRows);

            string strHTML = string.Empty;

            strHTML = "<div class='table-responsive'><table id='sample-table-2' class='table table-striped table-bordered table-hover'><thead><tr><th class='center'></th><th>Run Date</th><th>Snapshot Start</th><th>Snapshot End</th><th>Event</th><th>Activity Text</th></tr></thead><tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                strHTML += "<tr> ";

                strHTML += "<td><div class='action-buttons'><a class='red' href='#' onclick='viewactivity(\"" + dr["UniqueNumber"].ToString() + "\")'><i class='icon-envelope-alt bigger-120'></i></a></div></td>";

                strHTML += "<td>" + dr["RunDate"].ToString() + "</td><td>" + dr["PrevSnapshotDateTime"].ToString() + "</td><td>" + dr["CurrSnapshotDateTime"].ToString() + "</td><td>" + dr["EventName"].ToString() + "</td><td>" + dr["DiaryText"].ToString() + "</td></tr>";
            }
            strHTML += "</tbody></table></div>";

            strHTML += getPager(nRows, nPageIndex, nPageRows, "showhistory");

            return strHTML;
        }

        private DataTable getDeptHistoryData(string strDeptCode, int nPageIndex, int nPageRows, out int nRows)
        {
            DataTable dt = new DataTable();

            nRows = 0;

            SqlConnection conn = new SqlConnection(strEBMSConn);

            try
            {
                conn.Open();

                //get all rows number
                string strSQL0 = "SELECT COUNT(DiarySeq) as nRowsCount FROM AMP_Logs ";
                strSQL0 += " where DepartmentCode=@deptcode ";

                SqlCommand comm0 = new SqlCommand(strSQL0, conn);
                comm0.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                SqlDataReader dr0 = comm0.ExecuteReader();
                if (dr0.Read())
                    nRows = int.Parse(dr0["nRowsCount"].ToString());

                dr0.Close();

                string strSQL = "select * from (SELECT row_number() over (order by Rundate desc) as rowno,UniqueNumber, RunDate, snapshotprev.SnapshotDateTime as PrevSnapshotDateTime, snapshotcurr.SnapshotDateTime as CurrSnapshotDateTime, ";
                strSQL += " DepartmentCode, UserId, EmailAddress, EV200_EVT_DESC + ' (' + cast(EV200_EVT_ID as varchar) + ')' as EventName, AccountCode, ";
                strSQL += " DiarySeq, DiaryText";
                strSQL += " FROM AMP_Logs left join SnapshotTimes snapshotprev on nSnapshotPrevious = snapshotprev.SnapshotID";
                strSQL += " left join SnapshotTimes snapshotcurr on nSnapshotCurrent = snapshotcurr.SnapshotID";
                strSQL += " left join " + strCompareDB + ".dbo.EV200_EVENT_MASTER on EventId = EV200_EVT_ID";
                strSQL += " where DepartmentCode=@deptcode ";

                strSQL += " ) AS tab0";
                strSQL += " where rowno between @startno and @endno ";
                SqlCommand comm = new SqlCommand(strSQL, conn);
                comm.Parameters.Add("@deptcode", SqlDbType.VarChar, 20).Value = strDeptCode;
                comm.Parameters.Add("@startno", SqlDbType.Int).Value = (nPageIndex - 1) * nPageRows + 1;
                comm.Parameters.Add("@endno", SqlDbType.Int).Value = (nPageIndex) * nPageRows;

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);
                da.FillSchema(dt, System.Data.SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        private string getHeaderPager(int nRows, int nPageIndex, int nPageRows, string strFuncName)
        {
            string strPagerHTML = string.Empty;
            if (nRows > nPageRows)
            {
                int pages = Math.Abs(nRows / nPageRows) + 1;

                strPagerHTML += "<div class='message-pager pull-right'><div class='inline middle'> page " + nPageIndex + " of " + pages + " </div>&nbsp; &nbsp;";

                strPagerHTML += "<ul class='pagination middle'>";

                if (nPageIndex == 1)
                {
                    strPagerHTML += "<li class='disabled'><span><i class='icon-step-backward middle'></i></span></li><li class='disabled'><span><i class='icon-caret-left bigger-140 middle'></i></span></li>";
                }
                else
                {
                    strPagerHTML += "<li><a href='#' onclick='" + strFuncName + "(1)'><i class='icon-step-backward middle'></i></a></li><li><a href='#' onclick='" + strFuncName + "(" + (nPageIndex - 1) + ")'><i class='icon-caret-left bigger-140 middle'></i></a></li>";
                }

                strPagerHTML += "<li><span><input value='" + nPageIndex + "' maxlength='5' type='text' onchange= '" + strFuncName + "($(this).val())' onkeypress='return isNumber(this,event)'/></span></li>";

                if (nPageIndex != pages)
                {
                    strPagerHTML += "<li><a href='#' onclick='" + strFuncName + "(" + (nPageIndex + 1) + ")'><i class='icon-caret-right bigger-140 middle'></i></a></li><li><a href='#' onclick='" + strFuncName + "(" + pages + ")'><i class='icon-step-forward middle'></i></a></li>";
                }
                else
                {
                    strPagerHTML += "<li class='disabled'><span><i class='icon-caret-right bigger-140 middle'></i></span></li><li class='disabled'><span><i class='icon-step-forward middle'></i></span></li>";
                }
                strPagerHTML += "</ul></div>";
            }

            return strPagerHTML;
        }//end of getHeaderPager

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nRows"> Total rows of the records</param>
        /// <param name="nPageIndex"> Current Page Number (for highlight the current page)</param>
        /// <param name="nPageRows"> Row numbers to show on each page</param>
        /// <param name="strFuncName"> Action function for page click</param>
        /// <returns></returns>
        private string getPager(int nRows, int nPageIndex, int nPageRows, string strFuncName)
        {
            string strPagerHTML = string.Empty;

            strPagerHTML = "<div class='message-footer clearfix'><div class='pull-left'> " + nRows + " records total </div>";

            if (nRows > nPageRows)
            {
                int pages = Math.Abs(nRows / nPageRows) + 1;


                strPagerHTML += "<div class='pull-right'><div><label>Display <select size='1' id='slshowrecords" + strFuncName + "' onchange='" + strFuncName + "(1,this.value)'>";

                if (nPageRows == 18)
                {
                    strPagerHTML += "<option value='18' selected='selected'>18</option><option value='50'>50</option><option value='200'>200</option><option value='500'>500</option>	";
                }

                else if (nPageRows == 50)
                {
                    strPagerHTML += "<option value='18' >18</option><option value='50' selected='selected'>50</option><option value='200'>200</option><option value='500'>500</option>	";
                }
                else if (nPageRows == 200)
                {
                    strPagerHTML += "<option value='18' >18</option><option value='50'>50</option><option value='200' selected='selected'>200</option><option value='500'>500</option>	";
                }
                else if (nPageRows == 500)
                {
                    strPagerHTML += "<option value='18' >18</option><option value='50'>50</option><option value='200'>200</option><option value='500' selected='selected'>500</option>	";
                }

                strPagerHTML += "</select> records each page	</label></div><div class='inline middle'> page " + nPageIndex + " of " + pages + " </div>&nbsp; &nbsp;";

                strPagerHTML += "<ul class='pagination middle'>";

                if (nPageIndex == 1)
                {
                    strPagerHTML += "<li class='disabled'><span><i class='icon-step-backward middle'></i></span></li><li class='disabled'><span><i class='icon-caret-left bigger-140 middle'></i></span></li>";
                }
                else
                {
                    strPagerHTML += "<li><a href='#' onclick='" + strFuncName + "(1)'><i class='icon-step-backward middle'></i></a></li><li><a href='#' onclick='" + strFuncName + "(" + (nPageIndex - 1) + ")'><i class='icon-caret-left bigger-140 middle'></i></a></li>";
                }

                strPagerHTML += "<li><span><input value='" + nPageIndex + "' maxlength='5' type='text' onchange= '" + strFuncName + "($(this).val())' onkeypress='return isNumber(this,event)'/></span></li>";

                if (nPageIndex != pages)
                {
                    strPagerHTML += "<li><a href='#' onclick='" + strFuncName + "(" + (nPageIndex + 1) + ")'><i class='icon-caret-right bigger-140 middle'></i></a></li><li><a href='#' onclick='" + strFuncName + "(" + pages + ")'><i class='icon-step-forward middle'></i></a></li>";
                }
                else
                {
                    strPagerHTML += "<li class='disabled'><span><i class='icon-caret-right bigger-140 middle'></i></span></li><li class='disabled'><span><i class='icon-step-forward middle'></i></span></li>";
                }
                strPagerHTML += "</ul></div></div>";
            }

            return strPagerHTML;
        }//end of getPager


    }

}
