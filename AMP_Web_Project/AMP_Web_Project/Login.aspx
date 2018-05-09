<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AMP_Web_Project.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - MCEC Amendments Configuration - EBMS Prod</title>
    <meta name="keywords" content="MCEC Amendments Configuration" />
		<meta name="description" content="MCEC Amendments Settings" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;" />
		<meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
	    
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    
		<link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans:400,300" />

    <!--[if IE]>
		  <script src="assets/js/json2.js"></script>
		<![endif]-->
		

		<!-- page specific plugin styles -->

		<!-- fonts -->

		
		<!-- ace styles -->

		<link rel="stylesheet" href="assets/css/ace.min.css" />
		<link rel="stylesheet" href="assets/css/ace-rtl.min.css" />

</head>
<body class="login-layout">
   
    <div class="main-container">
			<div class="main-content">
                <form id="frmLogin" runat="server">
				<div class="row">
					<div class="col-sm-11 col-sm-offset-1">
						<div class="login-container">
							<div class="center">                                
								<h2 id="Logo">									
                                    <i class="fa fa-cog blue"></i>
									<span class="blue">MCEC Amendment Configuration - EBMS Prod</span>									
								</h2>
								
							</div>
                             
							<div class="space-6"></div>

							<div class="position-relative">
								<div id="login-box" class="login-box visible widget-box no-border">
									<div class="widget-body">
										<div class="widget-main">
											<h4 class="header blue lighter bigger">
												<i class="icon-coffee green"></i>
												Please Select the Department/User
											</h4>

											<div class="space-6"></div>
											
												<fieldset>
													<label class="block clearfix">
														<span class="block input-icon input-icon-right">																														
                                                            <asp:DropDownList ID="dlUsers" runat="server"></asp:DropDownList>

                                                       <button class="btn btn-danger btn-sm" id="btndelete" runat="server">
												            <i class="fa fa-trash-o bigger-120"></i>
											            </button>
														</span>
													</label>
													

													<div class="space"></div>

													<div class="clearfix">
                                                        <asp:Button ID="Button1" runat="server" CssClass="width-35 pull-left btn btn-sm btn-primary" Text="Select" OnClick="Button1_Click" CausesValidation="False" />														
													</div>

													<div class="space-4"></div>
												</fieldset>		
                                            								
                                            <div class="space-24"></div>
										<div class="toolbar clearfix" id="dvnewuser" runat="server">
											<div class="">
												<a href="#" onclick="show_box('signup-box'); return false;" class="user-signup-link">													
													Create a new User/Department
												</a>
											</div>										
										</div>
									</div><!-- /widget-body -->


								</div><!-- /login-box -->
								
							</div><!-- /position-relative -->
							
							<div id="signup-box" class="signup-box widget-box no-border" >
									<div class="widget-body">
										<div class="widget-main">
											<h4 class="header green lighter bigger">
												<i class="icon-group blue"></i>
												New Department/User Registration
											</h4>

											<div class="space-6"></div>
											<p> Enter your details to begin: </p>
											<div class="space-6"></div>
                                            <fieldset>
                                                    <div class="form-horizontal" role="form" id="frmDept">
                                                        <div class="widget-main">                                                           
								                        <div class="form-group">
									                        <label class="col-sm-4 control-label no-padding-right" for="txtdeptid"> Department Id </label>
                                                            <input type="text" id="txtdeptid" placeholder="" class="col-xs-12 col-sm-8 col-md-10" runat="server" />									                        
								                        </div>
                                                        <div class="form-group">
									                        <label class="col-sm-4 control-label no-padding-right" for="txtdeptname"> Description </label>									                       
										                    <input  type="text" id="txtdeptname" placeholder="" class="col-xs-12 col-sm-12 col-md-12" runat="server"  />									                        
								                        </div>	
                                                        <div class="form-group">
									                        <label class="col-sm-4 control-label no-padding-right" for="selmethod">Notify Method</label>									                        
										                    <select class="form-control" id="selmethod" runat="server" >
                                                                <option value="Activity">Activity</option>
                                                                <option value="Email">Email</option>
					                                        </select>									                        
								                        </div>    							                       
                                                        <div class="form-group">
									                        <label class="col-sm-4 control-label no-padding-right" for="txtUserId">Notify User Id </label>									                        
										                    <input  type="text" id="txtUserId" placeholder="" class="col-xs-12 col-sm-8 col-md-10" runat="server"  />									                        
								                        </div>
                                                        <div class="form-group">
									                        <label class="col-sm-4 control-label no-padding-right" for="txtwindowsuser">Manager User Id </label>
										                    <input  type="text" id="txtwindowsuser" placeholder="" class="col-xs-12 col-sm-8 col-md-10" runat="server"  />									                        
								                        </div>
                                                        </div>
							                        </div>
                                                  <hr />
                                                    <div class="clearfix">														
														<asp:Button ID="btnRegister" runat="server" CssClass="width-45 pull-right btn btn-sm btn-primary" Text="Register" OnClick="btnRegister_Click" />	                                                       
													</div>
												</fieldset>	
                                          
																						
										</div>

										<div class="toolbar center">
											<a href="#" onclick="show_box('login-box'); return false;" class="back-to-login-link">
												<i class="icon-arrow-left"></i>
												Back to login
											</a>
										</div>
									</div><!-- /widget-body -->


								</div><!-- /signup-box -->
							
						   
						
						</div>
						
					</div><!-- /.col -->
				</div><!-- /.row -->
			</div>

         </form>	
		</div><!-- /.main-container -->
        </div>

    
    <script src="assets/js/jquery-2.0.3.min.js"></script>
    <script src="assets/js/bootstrap.min.js"></script>
    
    <script src="assets/js/bootstrap-tag.min.js"></script>
    <script src="assets/js/bootbox.min.js"></script>
    <script src="assets/js/ace-elements.min.js"></script>  
    <script src="assets/js/ace.min.js"></script>  
    
    <script src="assets/js/fuelux/fuelux.spinner.min.js"></script>
    <script>        
        $(document).ready(function () {

            $('#btndelete').click(function (e) {
                e.preventDefault();

                var curruser = $('#dlUsers').val();
                var currusertext = $("#dlUsers :selected").text();
                if (curruser != "") {
                    bootbox.confirm("Are you sure to delete " + currusertext + "?", function (result) {
                        if (result) {
                            try {
                                $.ajax({
                                    type: "POST",
                                    url: "WebService1.asmx/DelUser",
                                    data: "{'strUserCode':'" + curruser + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) { location.reload(); },
                                    failure: function (e) { alert(e); }
                                });
                            }
                            catch (e) {
                                alert('failed to call web service. Error: ' + e);
                            }
                            //
                        }
                    });
                }
            });
        });

        function delrule(ruleid, pageindex) {
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
                                    url: "WebService1.asmx/DelUser",
                                    data: "{'strUserCode':'" + departmentid + "',  'RuleId':'" + ruleid + "', 'nPageIndex':" + pageindex + ",'nPageRows':18}",
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


        </script>
		<!-- <![endif]-->

		<!--[if IE]>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<![endif]-->

		<!--[if !IE]> -->

		<script type="text/javascript">
			window.jQuery || document.write("<script src='assets/js/jquery-2.0.3.min.js'>"+"<"+"/script>");
		</script>

		<!-- <![endif]-->

		<!--[if IE]>
<script type="text/javascript">
 window.jQuery || document.write("<script src='assets/js/jquery-1.10.2.min.js'>"+"<"+"/script>");
</script>
<![endif]-->
        <script type="text/javascript">
			function show_box(id) {
			 jQuery('.widget-box.visible').removeClass('visible');
			 jQuery('#'+id).addClass('visible');
			}

            

		</script>
		<script type="text/javascript">
			if("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>"+"<"+"/script>");
		</script>

</body>
</html>
