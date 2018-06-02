<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>


<html class="no-js">

<head>
    <meta charset="utf-8">

    <title>:::GVK EMRI:::</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">

    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <meta name="description" content="AppUI is a Web App Bootstrap Admin Template created by pixelcave and published on Themeforest">
    <meta name="author" content="pixelcave">
    <meta name="robots" content="noindex, nofollow">
    <meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1.0">

    <!-- Icons -->
    <!-- The following icons can be replaced with your own, they are used by desktop and mobile browsers -->
    <link rel="shortcut icon" href="img/favicon.png">
    <link rel="apple-touch-icon" href="img/icon57.png" sizes="57x57">
    <link rel="apple-touch-icon" href="img/icon72.png" sizes="72x72">
    <link rel="apple-touch-icon" href="img/icon76.png" sizes="76x76">
    <link rel="apple-touch-icon" href="img/icon114.png" sizes="114x114">
    <link rel="apple-touch-icon" href="img/icon120.png" sizes="120x120">
    <link rel="apple-touch-icon" href="img/icon144.png" sizes="144x144">
    <link rel="apple-touch-icon" href="img/icon152.png" sizes="152x152">
    <link rel="apple-touch-icon" href="img/icon180.png" sizes="180x180">
    <!-- END Icons -->

    <!-- Stylesheets -->
    <!-- Bootstrap is included in its original form, unaltered -->
    <link rel="stylesheet" href="css/bootstrap.min.css">

    <!-- Related styles of various icon packs and plugins -->
    <link rel="stylesheet" href="css/plugins.css">

    <!-- The main stylesheet of this template. All Bootstrap overwrites are defined in here -->
    <link rel="stylesheet" href="css/main.css">

    <!-- Include a specific file here from css/themes/ folder to alter the default theme of the template -->

    <!-- The themes stylesheet of this template (for using specific theme color in individual elements - must included last) -->
    <link rel="stylesheet" href="css/themes.css">
    <!-- END Stylesheets -->

    <!-- Modernizr (browser feature detection library) -->
    <script src="js/vendor/modernizr-2.8.3.min.js"></script>
</head>
<body>
    <!-- Login Container -->
    <div id="login-container">
        <!-- Login Header -->
        <h1 class="h2 text-light text-center push-top-bottom animation-slideDown">
            <i class="fa fa-ambulance"></i><strong>&nbsp Welcome to GVK EMRI</strong>
        </h1>
        <!-- END Login Header -->

        <!-- Login Block -->
        <div class="block animation-fadeInQuickInv">
            <!-- Login Title -->
            <div class="block-title">
                <h2>Please Login</h2>
            </div>
            <!-- END Login Title -->

            <!-- Login Form -->
            <form runat="server" class="form-horizontal">
                <div class="form-group">
                    <div class="col-xs-12">
                        
                        <asp:TextBox ID="txtFmsUsername" placeholder="Your LoginID.." runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFmsUsername"
                            Display="Dynamic" ErrorMessage="Enter The User Name"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-12">
                       
                        <asp:TextBox ID="txtFmsPassword" runat="server" placeholder="Your Password.." TextMode="Password" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFmsPassword"
                            Display="Dynamic" ErrorMessage="Enter The Password"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group form-actions">
                    <div class="col-xs-8">
                        <asp:Label ID="lblResult" runat="server" class="csscheckbox csscheckbox-primary"></asp:Label>
                    </div>
                    <div class="col-xs-4 text-right">
                       
           <asp:Button runat="server" CssClass="btn btn-effect-ripple btn-sm btn-primary" Text="Login" OnClick="btnFmsLoginSubmit_Click" />

                    </div>
                </div>
            </form>
            <!-- END Login Form -->
        </div>
        <!-- END Login Block -->

        <!-- Footer -->
        <footer class="text-muted text-center animation-pullUp">
            <small>&copy; <a href="#" target="_blank">GVK EMRI</a></small>
        </footer>
        <!-- END Footer -->
    </div>
    <!-- END Login Container -->

    <!-- jQuery, Bootstrap, jQuery plugins and Custom JS code -->
    <script src="js/vendor/jquery-2.1.4.min.js"></script>
    <script src="js/vendor/bootstrap.min.js"></script>
    <script src="js/plugins.js"></script>
    <script src="js/app.js"></script>

    <!-- Load and execute javascript code used only in this page -->
    <script src="js/pages/readyLogin.js"></script>
    <script>$(function () { ReadyLogin.init(); });</script>
</body>
</html>
