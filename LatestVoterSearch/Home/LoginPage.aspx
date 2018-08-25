<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="LatestVoterSearch.Home.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Gentelella Alela! | </title>

    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- NProgress -->
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet" />
    <!-- Animate.css -->
    <link href="../vendors/animate.css/animate.min.css" rel="stylesheet" />

    <!-- Custom Theme Style -->
    <link href="../build/css/custom.min.css" rel="stylesheet" />
</head>
<body class="login">
    <form id="form1" runat="server">

        <div class="login_wrapper">
            <div class="animate form login_form">
                <section class="login_content">
                    <form>
                        <h1>Login Form</h1>
                        <div>
                            <%--  <input type="text" class="form-control" placeholder="Username" required="" />--%>
                            <asp:TextBox ID="txtLogin" runat="server" class="form-control" placeholder="Username" required=""></asp:TextBox>
                        </div>
                        <br />
                        <div>
                            <%-- <input type="password" class="form-control" placeholder="Password" required="" />--%>
                            <asp:TextBox ID="txtpassword" runat="server" class="form-control" placeholder="Password" required=""></asp:TextBox>
                        </div>
                        <br />
                        <div>
                            <a class="btn btn-default submit" href="../Home/AdminHome.aspx">Log in</a>
                            <%--<a class="reset_pass" href="#">Lost your password?</a>--%>
                        </div>

                        <div class="clearfix"></div>

                        <div class="separator">
                            <%-- <p class="change_link">New to site?
                  <a href="#signup" class="to_register"> Create Account </a>
                </p>--%>

                            <div class="clearfix"></div>
                            <br />

                            <div>
                                <h1><i class="fa fa-paw"></i>True Voter</h1>
                                <p>© 2017 - Abhinav IT Solutions Pvt. Ltd, Pune</p>
                            </div>
                        </div>
                    </form>
                </section>
            </div>

            <%--<div id="register" class="animate form registration_form">
          <section class="login_content">
            <form>
              <h1>Create Account</h1>
              <div>
                <input type="text" class="form-control" placeholder="Username" required="" />
              </div>
              <div>
                <input type="email" class="form-control" placeholder="Email" required="" />
              </div>
              <div>
                <input type="password" class="form-control" placeholder="Password" required="" />
              </div>
              <div>
                <a class="btn btn-default submit" href="index.html">Submit</a>
              </div>

              <div class="clearfix"></div>

              <div class="separator">
                <p class="change_link">Already a member ?
                  <a href="#signin" class="to_register"> Log in </a>
                </p>

                <div class="clearfix"></div>
                <br />

                <div>
                  <h1><i class="fa fa-paw"></i> True Voter!</h1>
                  <p class="text-success">© 2017 - Abhinav IT Solutions Pvt. Ltd, Pune</p>
                </div>
              </div>
            </form>
          </section>
        </div>--%>
        </div>

    </form>
</body>
</html>
