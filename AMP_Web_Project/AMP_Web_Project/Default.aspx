<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AMP_Web_Project.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">
	    <div class="page-content">
			<div class="page-header">
				<h1>
					
					<small>
					    
                        Department: <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
					</small>
				</h1>
			</div><!-- /.page-header -->

			<div class="row">
				<div class="col-xs-12">
					<!-- PAGE CONTENT BEGINS -->

					<div class="row">
						<div class="col-xs-12">
							<div class="tabbable">
								<ul id="inbox-tabs" class="inbox-tabs nav nav-tabs padding-16 tab-size-bigger tab-space-1">									
									<li class="active" id="lirules" runat="server">
										<a data-toggle="tab" href="#rules" onclick="showrules(1);$('li').removeClass('active'); $(this).parents('li').addClass('active');">	
                                            <i class="fa fa-bell" aria-hidden="true"></i>										
											<span class="bigger-110">My Rules</span>
										</a>
									</li>

									<li>
										<a data-toggle="tab" href="#resource"  onclick="showres(1);$('li').removeClass('active'); $(this).parents('li').addClass('active');">
											<i class="fa fa-hand-pointer-o" aria-hidden="true"></i>
											<span class="bigger-110">Resources </span>
										</a>
									</li>

									<li>
										<a data-toggle="tab" href="#function"  onclick="showfunc(1);$('li').removeClass('active'); $(this).parents('li').addClass('active');">
											<i class="fa fa-tasks" aria-hidden="true"></i>
											<span class="bigger-110">Function Usage</span>
										</a>
									</li>

									<li>
										<a data-toggle="tab" href="#document"  onclick="showdoc(1);$('li').removeClass('active'); $(this).parents('li').addClass('active');">
											<i class="fa fa-folder" aria-hidden="true"></i>
											<span class="bigger-110">Document Heading</span>
										</a>
									</li>

                                    <li>
										<a data-toggle="tab" href="#notecls"  onclick="shownote(1);$('li').removeClass('active'); $(this).parents('li').addClass('active');">
											<i class="fa fa-commenting" aria-hidden="true"></i>
											<span class="bigger-110">Note Class</span>
										</a>
									</li>

                                    <li>
										<a data-toggle="tab" href="#History"  onclick="showhistory(1);$('li').removeClass('active'); $(this).parents('li').addClass('active');">
											<i class="fa fa-history" aria-hidden="true"></i>
											<span class="bigger-110">Run History</span>
										</a>
									</li>
                                    
								</ul>
                                <div class="tab-content no-border no-padding"> 
                                        <div class="tab-pane in active">
			                                <div class="message-container" id="myrules" >
                                               <!-- <asp:GridView ID="gvRules" runat="server">                                                   
                                                </asp:GridView>                                                -->
                                            </div>
                                            <div class="message-container" id="myresour">
                                                <!--<asp:GridView ID="gvResources" runat="server"></asp:GridView>-->
                                            </div>
                                            <div class="message-container" id="myfuncuse" >
                                                <!--<asp:GridView ID="gvFunction" runat="server"></asp:GridView>-->
                                            </div>
                                            <div class="message-container" id="mydochead" >
                                                <!--<asp:GridView ID="gvDocHeading" runat="server"></asp:GridView>-->
                                            </div>
                                            <div class="message-container" id="mynotecls" >
                                                <!--<asp:GridView ID="gvNoteClass" runat="server"></asp:GridView>-->
                                            </div>
                                            <div class="message-container" id="myhistory" >
                                                <!--<asp:GridView ID="gvHistory" runat="server"></asp:GridView>-->
                                            </div>
                                            <div class="col-sm-6">
                                                <div id="dialog-confirm" class="hide">
											        <div class="alert alert-info bigger-110">
												        These items will be permanently deleted and cannot be recovered.
											        </div>

											        <div class="space-6"></div>

											        <p class="bigger-110 bolder center grey">
												        <i class="icon-hand-right blue bigger-120"></i>
												        Are you sure?
											        </p>
										        </div><!-- #dialog-confirm -->
                                            </div>                                            
                                        </div>
                                    </div>
							</div><!-- /.tabbable -->
						</div><!-- /.col -->
					</div><!-- /.row -->
                    <div class="modal fade" id="activitymodal" tabindex="-2" role="dialog" aria-labelledby="exampleModalLabel">
                        <div class="modal-dialog" role="document">
                        <div class="modal-content" >
                            <div class="modal-header">
					            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
					            <h4 class="modal-title">Amendment Activities</h4>
                                <input type="button" onclick="printactivities()" value="print me!" />
				            </div>                            
                            <div id="dvActivityContent" style="padding:20px;"></div>
                            </div>
                        </div>
                    </div>


                    <div class="modal fade" id="rulemodal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
                        <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
					            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
					            <h4 class="modal-title">Rule</h4>
				            </div>

                            <div class="modal-body">                   
                            <div class="form-horizontal" role="form" id="frmRule">
                                <div class="row">
                                    <div class="row">
                                        &nbsp;
                                        <input type="hidden" id="txtruleid" />
                                        <input type="hidden" id="txtruleinfo" />
                                        <input type="hidden" id="txtrulepageindex" />
                                    </div>
								<div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="txtrulename"> Rule Name </label>

									<div class="col-sm-6">
										<input  type="text" id="txtrulename" placeholder="" class="col-xs-12 col-sm-7" />
									</div>
								</div>

								<div class="space-4"></div>

								<div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="selruletype"> Rule Type</label>

									<div class="col-sm-6">
										<select class="col-xs-12 col-sm-8" id="selruletype">
                                            <option value="EVENT">Event Change</option>

										<!--<option value="">&nbsp;</option>
										<option value="NEWEVT">New Event</option>
										<option value="EVTSTS">Event Status Change</option>
										
										<option value="FUNC">Function Change</option>
										<option value="ORD">Order Change</option>																
                                        <option value="DOC">Document Change</option>																
                                        <option value="CONTRACT">Contract Change</option>
                                        <option value="NOTES">Notes Change</option> -->
									</select>
									</div>
								</div>

								<div class="space-4"></div>

								<div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="spinner1"> Trigger Minutes </label>

									<div class="col-sm-3">											                    
                                        <input type="text" class="input-mini" id="spinner1" disabled="disabled" />                                                                 											                    
									</div>
								</div>

								<div class="space-4"></div>

								<div class="form-group" id="dvlastrun">
									<label class="col-sm-5 control-label no-padding-right" for="txtLastRun">Last Run</label>

									<div class="col-sm-6">
										<input readonly="" type="text" id="txtLastRun" placeholder="" class="col-xs-12 col-sm-8" />
									</div>
								</div>

                                <div class="space-4"></div>
                                                        
                                <div class="form-group" id="dvnextrun">
									<label class="col-sm-5 control-label no-padding-right" for="txtNextRun">Next Run</label>

									<div class="col-sm-6">
										<input readonly="" type="text" id="txtNextRun" placeholder="" class="col-xs-12 col-sm-8" />
                                        <button class="btn btn-warning btn-sm" type="button" onclick="resetNextRun()">
										<i class="fa fa-refresh"></i>
										Reset
									</button>
									</div>
								</div>

                                <div class="space-4"></div>

								<div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="spinner2">Event Notify Day From</label>
									<div class="col-sm-6">											                    
                                        <input type="text" class="input-mini" id="spinner2" disabled="disabled"/>                                                                 											                    
									</div>
								</div>

                                <div class="space-4"></div>

                                    <div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="spinner3">Event Notify Day To</label>
									<div class="col-sm-6">											                    
                                        <input type="text" class="input-mini" id="spinner3" disabled="disabled" />                                                                 											                    
									</div>
								</div>
                                                        
                                <div class="space-4"></div>

                                <div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="txtShortLeadList">Short Lead Event Status List</label>
									<div class="col-sm-6">											                    
                                        <input type="text" class="col-xs-12 col-sm-8" id="txtShortLeadList"  placeholder="" />                                                                 											                    
									</div>
								</div>

                                <div class="space-4"></div>
                                
                                <div class="form-group">
								<label class="col-sm-5 control-label no-padding-right" for="txtShortLeadList">Selected Event Status List</label>
								<div class="col-sm-6">											                    
                                    <input type="text" class="col-xs-12 col-sm-8" id="txtEventStatusList"  placeholder="" />                                                                 											                    
								</div>
								</div>

								<div class="space-4"></div>
                                
                                <div class="form-group">
								<label class="col-sm-5 control-label no-padding-right" for="showfuncid">Show Function Id</label>
								<div class="col-sm-6">											                    
                                    <label>
										<input name="switch-field-1" class="ace ace-switch ace-switch-5" value="1" type="checkbox" id="showfuncid" />
									<span class="lbl"></span>
									</label>
								</div>
								</div>


                                    <div class="space-4"></div>
                                
                                <div class="form-group">
								<label class="col-sm-5 control-label no-padding-right" for="showspacecode">Show Space Code</label>
								<div class="col-sm-6">											                    
                                    <label>
										<input name="switch-field-1" class="ace ace-switch ace-switch-5" value="1" type="checkbox" id="showspacecode" />
									<span class="lbl"></span>
									</label>
								</div>
								</div>

                                    <div class="space-4"></div>
                                
                                <div class="form-group">
								<label class="col-sm-5 control-label no-padding-right" for="showhierarchydesc">Show Function Hierarchy Description</label>
								<div class="col-sm-6">											                    
                                    <label>
										<input name="switch-field-1" class="ace ace-switch ace-switch-5" value="1" type="checkbox" id="showhierarchydesc" />
									<span class="lbl"></span>
									</label>
								</div>
								</div>


                                    <div class="space-4"></div>
                                
                                <div class="form-group">
								<label class="col-sm-5 control-label no-padding-right" for="showpackagetime">Show Date Time for Package Items</label>
								<div class="col-sm-6">
                                    <label>
										<input name="switch-field-1" class="ace ace-switch ace-switch-5" value="1" type="checkbox" id="showpackagetime" />
									<span class="lbl"></span>
									</label>
								</div>
								</div>

                                <div class="space-4"></div>

 
                                <div class="form-group">
								<label class="col-sm-5 control-label no-padding-right" for="showsignage">Show Function Signage Amendments</label>
								<div class="col-sm-6">
                                    <label>
										<input name="switch-field-1" class="ace ace-switch ace-switch-5" value="0" type="checkbox" id="showsignage" />
									<span class="lbl"></span>
									</label>
								</div>
								</div>

                                <div class="space-4"></div>
								<div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="nNotesLength">Show Notes Length</label>

									<div class="col-sm-6">
										<input type="text" id="nNotesLength" value="200" placeholder="" class="col-xs-12 col-sm-8"/>
									</div>
								</div>
                                   
								<div class="space-4"></div>


								<div class="form-group">
									<label class="col-sm-5 control-label no-padding-right" for="txtEmailSubject">Email Subject</label>

									<div class="col-sm-6">
										<input type="text" id="txtEmailSubject" value="" placeholder="" class="col-xs-12 col-sm-8"/>
									</div>
								</div>
                                <div class="form-group">
                                    <label class="col-sm-5"></label>
                                    <div class="col-sm-6">
									<button class="btn btn-info" type="button" onclick="checkrules()">
										<i class="icon-ok bigger-110"></i>
										Submit
									</button>
                                        &nbsp;&nbsp;&nbsp;
                                        <button class="btn" data-dismiss="rulemodal">
										<i class="icon-remove"></i>
										Cancel
									</button>	   	
                                    </div>
								</div>
                                </div>
							</div>
                            </div>
                        </div>
                        </div>
                    </div>

					<!-- PAGE CONTENT ENDS -->
				</div><!-- /.col -->
			</div><!-- /.row -->
		</div><!-- /.page-content -->
	</div>
		<script src="assets/js/fuelux/fuelux.spinner.min.js"></script>	
		<script src="assets/js/bootstrap-tag.min.js"></script>

    <script src="assets/js/ace-elements.min.js"></script>
    <script>
        var departmentid = $('#departmentid').val();

        $(document).ready(function () {
            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });

            $('#spinner1').ace_spinner({ value: 60, min: 60, max: 1440, step: 60, btn_up_class: 'btn-info', btn_down_class: 'btn-info' })
               .on('change', function () {
                   //alert(this.value)
               });
            $('#spinner2').ace_spinner({ value: 0, min: 0, max: 16, step: 1, btn_up_class: 'btn-info', btn_down_class: 'btn-info' })
              .on('change', function () {
                  //alert(this.value)
              });
            $('#spinner3').ace_spinner({ value: 15, min: 1, max: 16, step: 1, btn_up_class: 'btn-info', btn_down_class: 'btn-info' })
              .on('change', function () {
                  //alert(this.value)
              });
            
            if (document.getElementById("isSuperUser").value == "true") {
                showrules(1);
            }
            else
            {
                $("#myrules").hide();
                showres(1);
            }
        });

  
       
        function showrules(npageIndex,nPageRows) {
            if (nPageRows == undefined)
            {
                 nPageRows = 18;
            }
            
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/getMyRules",
                    data: "{'strDeptCode':'" + departmentid + "', 'nPageIndex':"+npageIndex+",'nPageRows':"+nPageRows+"}", 
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#myrules").html(response.d); $("#myrules").show(); $("#myresour").hide(); $("#myhistory").hide(); $("#myfuncuse").hide(); $("#mydochead").hide(); $("#mynotecls").hide(); },
                    failure: function (e) { alert(e); }
                })
                ;
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }           
        }
        
        function showres(npageIndex,nPageRows, searchdepartment, searchkeywords) {            
            
            if (nPageRows == undefined) {                
                if ($("#slshowrecordsshowres").val() == undefined) {                    
                    nPageRows = 18;
                }
                else {                    
                    if ($("#slshowrecordsshowres").val() != "") {
                        nPageRows = $("#slshowrecordsshowres").val();
                        $("#slshowrecordsshowres").val(nPageRows);
                    }
                    else {
                        nPageRows = 18;
                        $("#slshowrecordsshowres").val(nPageRows);
                    }                    
                }
            }
            if (searchdepartment == undefined)
            {
                if ($("#sldept").val() == undefined) {
                    searchdepartment = "";
                }
                else {
                    if ($("#sldept").val() != "") {
                        searchdepartment = $("#sldept").val();
                        $("#sldept").val(searchdepartment);
                    }
                    else {
                        searchdepartment = "";
                        $("#sldept").val(searchdepartment);
                    }
                }
            }
            if (searchkeywords == undefined) {
                if ($("#txtsearchresource").val() == undefined) {
                    searchkeywords = "";
                }
                else {
                    if ($("#txtsearchresource").val() != "") {
                        searchkeywords = $("#txtsearchresource").val();
                        $("#txtsearchresource").val(searchkeywords);
                    }
                    else {
                        searchkeywords = "";
                        $("#txtsearchresource").val(searchkeywords);
                    }                    
                }              
            }             
                       
            try {               
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/getMyResource",

                    data: "{'strDeptCode':'" + departmentid + "', 'nPageIndex':" + npageIndex + ",'nPageRows':" + nPageRows + ", 'strSearchDept':'" + searchdepartment + "', 'strSearchKeyWords':'" + searchkeywords + "'}",
                    contentType: "application/json; charset=utf-8",                    
                    success: function (response) { $("#myresour").html();$("#myresour").html(response.d); $("#myresour").show(); $("#myhistory").hide(); $("#myrules").hide(); $("#myfuncuse").hide(); $("#mydochead").hide(); $("#mynotecls").hide(); },
                    failure: function (e) {  alert(e); }
                })
                ;
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function showfunc(npageIndex, nPageRow) {

            if (nPageRow == undefined) {
                if ($("#slshowrecordsshowfunc").val() == undefined) {
                    nPageRow = 18;
                }
                else {
                    if ($("#slshowrecordsshowfunc").val() != "") {
                        nPageRow = $("#slshowrecordsshowfunc").val();
                        $("#slshowrecordsshowfunc").val(nPageRow);
                    }
                    else {
                        nPageRow = 18;
                        $("#slshowrecordsshowfunc").val(nPageRow);
                    }
                }
            }
           
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/getMyFunctionUsage",
                    data: "{'strDeptCode':'" + departmentid + "', 'nPageIndex':" + npageIndex + ",'nPageRows':"+nPageRow+"}", 
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#myfuncuse").html(response.d); $("#myresour").hide(); $("#myhistory").hide(); $("#myrules").hide(); $("#myfuncuse").show(); $("#mydochead").hide(); $("#mynotecls").hide(); },
                    failure: function (e) { alert(e); }
                })
                ;
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function showdoc(npageIndex, nPageRow) {

            if (nPageRow == undefined) {
                if ($("#slshowrecordsshowdoc").val() == undefined) {
                    nPageRow = 18;
                }
                else {
                    if ($("#slshowrecordsshowdoc").val() != "") {
                        nPageRow = $("#slshowrecordsshowdoc").val();
                        $("#slshowrecordsshowdoc").val(nPageRow);
                    }
                    else {
                        nPageRow = 18;
                        $("#slshowrecordsshowdoc").val(nPageRow);
                    }
                }
            }
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/getMyDocumentHeadings",
                    data: "{'strDeptCode':'" + departmentid + "', 'nPageIndex':" + npageIndex + ",'nPageRows':"+nPageRow+"}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#mydochead").html(response.d); $("#myresour").hide(); $("#myhistory").hide(); $("#myrules").hide(); $("#myfuncuse").hide(); $("#mydochead").show(); $("#mynotecls").hide(); },
                    failure: function (e) { alert(e); }
                })
                ;
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function shownote(npageIndex, nPageRow) {

            if (nPageRow == undefined) {
                if ($("#slshowrecordsshownote").val() == undefined) {
                    nPageRow = 18;
                }
                else {
                    if ($("#slshowrecordsshownote").val() != "") {
                        nPageRow = $("#slshowrecordsshownote").val();
                        $("#slshowrecordsshownote").val(nPageRow);
                    }
                    else {
                        nPageRow = 18;
                        $("#slshowrecordsshownote").val(nPageRow);
                    }
                }
            }
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/getMyNoteClass",
                    data: "{'strDeptCode':'" + departmentid + "', 'nPageIndex':" + npageIndex + ",'nPageRows':"+nPageRow+"}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#mynotecls").html(response.d); $("#myresour").hide(); $("#myhistory").hide(); $("#myrules").hide(); $("#myfuncuse").hide(); $("#mydochead").hide(); $("#mynotecls").show(); },
                    failure: function (e) { alert(e); }
                })
                ;
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function showhistory(npageIndex, nPageRow) {
            if (nPageRow == undefined) {
                if ($("#slshowrecordsshowhistory").val() == undefined) {
                    nPageRow = 18;
                }
                else {
                    if ($("#slshowrecordsshowhistory").val() != "") {
                        nPageRow = $("#slshowrecordsshowhistory").val();
                        $("#slshowrecordsshowhistory").val(nPageRow);
                    }
                    else {
                        nPageRow = 18;
                        $("#slshowrecordsshowhistory").val(nPageRow);
                    }
                }
            }
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/getMyDeptHistory",
                    data: "{'strDeptCode':'" + departmentid + "', 'nPageIndex':" + npageIndex + ",'nPageRows':"+nPageRow+"}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#myhistory").html(response.d); $("#myresour").hide(); $("#mynotecls").hide(); $("#myrules").hide(); $("#myfuncuse").hide(); $("#mydochead").hide(); $("#myhistory").show(); },
                    failure: function (e) { alert(e); }
                })
                ;
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function editrule(ruleid, ruleinfo, pageindex)
        {
            $('#rulemodal').modal('show');
            
            $('#txtruleid').val(ruleid);
            $('#txtruleinfo').val(ruleinfo);
            $('#txtrulepageindex').val(pageindex);

            var arrruleinfo = ruleinfo.split('][');
            $('#txtrulename').val(arrruleinfo[0]);
            $('#selruletype').val(arrruleinfo[1]);
            $('#spinner1').val(arrruleinfo[2]);
            $('#txtLastRun').val(arrruleinfo[3]);
            $('#txtNextRun').val(arrruleinfo[4]);
            $('#spinner2').val(arrruleinfo[5]);
            $('#spinner3').val(arrruleinfo[6]);
            $('#txtShortLeadList').val(arrruleinfo[7]);
            $('#txtEventStatusList').val(arrruleinfo[8]);            
            if (arrruleinfo[9] == "True") $('#showfuncid').prop('checked', true); else $('#showfuncid').prop('checked', false);
            if (arrruleinfo[10] == "True") $('#showspacecode').prop('checked', true); else $('#showspacecode').prop('checked', false);
            if (arrruleinfo[11] == "True") $('#showhierarchydesc').prop('checked', true); else $('#showhierarchydesc').prop('checked', false);            
            if (arrruleinfo[12] == "True") $('#showpackagetime').prop('checked', true); else $('#showpackagetime').prop('checked', false);
            if (arrruleinfo[13] == "True") $('#showsignage').prop('checked', true); else $('#showsignage').prop('checked', false);            
            $('#txtEmailSubject').val(arrruleinfo[14]);
            $('#nNotesLength').val(arrruleinfo[15]);
        }

        function viewactivity(uniqueno) {

            $('#activitymodal').modal('show');

            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/getActivity",
                    data: "{'uniqueno':'" + uniqueno + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#dvActivityContent").html(response.d); },
                    failure: function (e) { alert(e); }
                });
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function checkrules()
        {            
            var ruleid = $('#txtruleid').val();

            //updated rule info
            if (ruleid != "")
            {
                var ruleinfo = $('#txtruleinfo').val();
                var pageindex = $('#txtrulepageindex').val();
                var arrruleinfo = ruleinfo.split('][');

                if ($('#selruletype').val() != arrruleinfo[1] || $('#spinner1').val() != arrruleinfo[2] || $('#spinner2').val() != arrruleinfo[5] ||
                    $('#spinner3').val() != arrruleinfo[6] || $('#txtShortLeadList').val() != arrruleinfo[7] ||
                    $('#txtEventStatusList').val() != arrruleinfo[8] || $('#txtEmailSubject').val() != arrruleinfo[14] || $('#nNotesLength').val() != arrruleinfo[15]
                    || (document.getElementById("showfuncid").checked && (arrruleinfo[9] != "True") || !document.getElementById("showfuncid").checked && (arrruleinfo[9] == "True"))
                    || (document.getElementById("showspacecode").checked && (arrruleinfo[10] != "True") || !document.getElementById("showspacecode").checked && (arrruleinfo[10] == "True"))
                    || (document.getElementById("showhierarchydesc").checked && (arrruleinfo[11] != "True") || !document.getElementById("showhierarchydesc").checked && (arrruleinfo[11] == "True"))
                    || (document.getElementById("showpackagetime").checked && (arrruleinfo[12] != "True") || !document.getElementById("showpackagetime").checked && (arrruleinfo[12] == "True"))
                    || (document.getElementById("showsignage").checked && (arrruleinfo[13] != "True") || !document.getElementById("showsignage").checked && (arrruleinfo[13] == "True"))
                    ) {
                    saveRuleInfo();
                }
            }
            //new rule info
            else
            {
                saveRuleInfo();
            }
            
            showrules(pageindex)
            $('#rulemodal').modal('hide');
        }

        function addnewrule()
        {
            $('#rulemodal').modal('show');

            $('#txtruleid').val('');
            $('#txtruleinfo').val('');
            $('#txtrulepageindex').val(1);

            $('#txtrulename').val('');
            $('#selruletype').val('EVENT');
            $('#spinner1').val('60');
            $('#dvlastrun').hide();
            $('#dvnextrun').hide();         
            $('#spinner2').val('0');
            $('#spinner3').val('13');
            $('#txtShortLeadList').val('31,61,62,64');
            $('#txtEventStatusList').val('31,61,62,64,80,86');
            document.getElementById("showfuncid").checked = false;
            document.getElementById("showspacecode").checked = false;
            document.getElementById("showhierarchydesc").checked = true;
            document.getElementById("showpackagetime").checked = true;
            document.getElementById("showsignage").checked = false;
            $('#txtEmailSubject').val('');
            $('#nNotesLength').val('200');
            
        }

        function saveRuleInfo()
        {
            try {

                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/SaveRule",
                    data: "{'strRuleId':'" + $('#txtruleid').val() + "', 'strRuleName':'" + $('#txtrulename').val() + "','strRuleType':'" + $('#selruletype').val() + "','strShortLeadStatusList':'" + $('#txtShortLeadList').val() + "','nTriggerMinutes':'" + $('#spinner1').val()
                            + "','nEventDayFrom':'" + $('#spinner2').val() + "','nEventDayTo':'" + $('#spinner3').val() + "','strEventStatusList':'" + $('#txtEventStatusList').val()
                            + "','isShowFuncId':'" + (document.getElementById("showfuncid").checked ? 1 : 0) + "','isShowSpaceCode':'" + (document.getElementById("showspacecode").checked ? 1 : 0) + "','isShowFuncHierarchyDesc':'" + (document.getElementById("showhierarchydesc").checked ? 1 : 0) + "','isShowPackageItemDateTime':'" + (document.getElementById("showpackagetime").checked ? 1 : 0) + "','isShowFuncSignage':'" + (document.getElementById("showsignage").checked ? 1 : 0)
                            + "','strEmailSubject':'" + $('#txtEmailSubject').val() + "','nNotesLength':'" + $('#nNotesLength').val() + "','strDeptCode':'" + departmentid + "','nPageIndex':1,'nPageRows':18}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#myrules").html(response.d); $("#myrules").show(); $("#myresour").hide(); $("#myfuncuse").hide(); $("#mydochead").hide(); $("#mynotecls").hide(); },
                    failure: function (e) { alert(e); }
                });
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e );
            }
        }

        function resetNextRun() {
            var ruleid = $('#txtruleid').val();
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/resetnextrun",
                    data: "{'strDeptCode':'" + departmentid + "',  'RuleId':'" + ruleid + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $('#rulemodal').modal('hide'); },
                    failure: function (e) { alert(e); }
                });
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
            
        }

        function delrule(ruleid, pageindex)
        {
            $("#dialog-confirm").removeClass('hide').dialog({
                resizable: false,
                modal: true,
                title: "Delete this rule?",
                title_html: true,
                buttons: [
                    {
                        html: "<i class='icon-trash bigger-110'></i>&nbsp; Delete this items",
                        "class": "btn btn-danger btn-xs",
                        click: function () {
                            try {
                                $.ajax({
                                    type: "POST",
                                    url: "WebService1.asmx/DelRule",
                                    data: "{'strDeptCode':'" + departmentid + "',  'RuleId':'" + ruleid + "', 'nPageIndex':" + pageindex + ",'nPageRows':18}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) { $("#myrules").html(response.d); $("#myrules").show(); $("#myresour").hide(); $("#myfuncuse").hide(); $("#mydochead").hide(); $("#mynotecls").hide(); },
                                    failure: function (e) { alert(e); }
                                });
                            }
                            catch (e) {
                                alert('failed to call web service. Error: ' + e);
                            }
                            $(this).dialog("close");
                        }
                    }
                    ,
                    {
                        html: "<i class='icon-remove bigger-110'></i>&nbsp; Cancel",
                        "class": "btn btn-xs",
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ]
            });

        }

        function updres(restype, rescode, action, pageindex) {
            var nPageRows = 18;
            
            if ($("#slshowrecordsshowres").val() == undefined) {
                nPageRows = 18;
            }
            else {
                if ($("#slshowrecordsshowres").val() != "") {
                    nPageRows = $("#slshowrecordsshowres").val();
                    $("#slshowrecordsshowres").val(nPageRows);
                }
                else {
                    nPageRows = 18;
                    $("#slshowrecordsshowres").val(nPageRows);
                }
            }
            

            alert("{'strDeptCode':'" + departmentid + "',  'ResourceType':'" + restype + "', 'ResourceCode':'" + rescode + "', 'Action':'" + action + "', 'nPageIndex':" + pageindex + ",'nPageRows':" + nPageRows + ",'strSearchDept':'" + $("#sldept").val() + "', 'strSearchKeyWords':'" + $("#txtsearchresource").val() + "'}");
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/UpdateResource",
                    data: "{'strDeptCode':'" + departmentid + "',  'ResourceType':'" + restype + "', 'ResourceCode':'" + rescode + "', 'Action':'" + action + "', 'nPageIndex':" + pageindex + ",'nPageRows':" + nPageRows + ",'strSearchDept':'" + $("#sldept").val() + "', 'strSearchKeyWords':'" + $("#txtsearchresource").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { showres(pageindex); },
                    failure: function (e) { alert(e); }
                });
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function updfunc(functype, action, pageindex)
        {
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/UpdateFuncType",
                    data: "{'strDeptCode':'" + departmentid + "',  'FuncType':'" + functype + "', 'Action':'" + action + "', 'nPageIndex':" + pageindex + ",'nPageRows':18}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#myfuncuse").html(response.d); $("#myresour").hide(); $("#myrules").hide(); $("#myfuncuse").show(); $("#mydochead").hide(); $("#mynotecls").hide(); },
                    failure: function (e) { alert(e); }
                });
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }
        
        function upddoc(headseq1, headseq2, action, pageindex) {
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/UpdateDocHeading",
                    data: "{'strDeptCode':'" + departmentid + "',  'HeadSeq1':'" + headseq1 + "',  'HeadSeq2':'" + headseq2 + "', 'Action':'" + action + "', 'nPageIndex':" + pageindex + ",'nPageRows':18}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#mydochead").html(response.d); $("#myresour").hide(); $("#myrules").hide(); $("#myfuncuse").hide(); $("#mydochead").show(); $("#mynotecls").hide(); },
                    failure: function (e) { alert(e); }
                });
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }
        }

        function updnote(noteclass, action, pageindex) {
            try {
                $.ajax({
                    type: "POST",
                    url: "WebService1.asmx/UpdateNoteClass",
                    data: "{'strDeptCode':'" + departmentid + "',  'NoteClassCode':'" + noteclass + "', 'Action':'" + action + "', 'nPageIndex':" + pageindex + ",'nPageRows':18}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) { $("#mynotecls").html(response.d); $("#myresour").hide(); $("#myrules").hide(); $("#myfuncuse").hide(); $("#mydochead").hide(); $("#mynotecls").show(); },
                    failure: function (e) { alert(e); }
                });
            }
            catch (e) {
                alert('failed to call web service. Error: ' + e);
            }   
        }

        function isNumber(p,evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert("Only numbers for page number");
                p.val('');p.focus();
            }
            return true;
        }

       
        function searchresource()
        {
            var dept = $("#sldept").val();            
            var searchstring = $("#txtsearchresource").val();
            var pagerows = $("#slshowrecordsshowres").val();
            showres(1,pagerows, dept, searchstring);
                event.preventDefault();
                return false;

                //function showres(npageIndex,nPageRows) {
        }

        function printactivities() {
            var printContents = $("#dvActivityContent").html();
            myWindow = window.open('', '', 'width=800,height=700');
            myWindow.document.write(printContents);
            myWindow.document.close();                      
            myWindow.print(); 
            myWindow.close();

            $('#activitymodal').modal('hide');
        }

    </script>
</asp:Content>
