<%@ Page Language="C#" AutoEventWireup="false" %>

<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="Sunplus.Agent.Model.Entities" %>
<%
    var um = new ONEbook.UM.Services.Impl.MaintenanceService();
    if (!um.ByPassUM && um.IsUnderMaintenance())
    {
        Response.Redirect(Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl, true);
    }
%>
<html>
<head>
    <title>Error</title>
    <%=CN88.Web.Helpers.StaticResources.AssetStyles.Render("/bundles/site-cn88/error-page")%>
    <%=CN88.Web.Helpers.StaticResources.RenderStyles("~/assets/bundles/error-page")%>
</head>
<body>
    <%
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(UserSession.Current.Language);
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
    %>
    <div class="page-content">
        <span class="icon-cancel-circular"></span>
        <div class="txt"><% Response.Write(Alpha.CN88.AppResources.Resources.Error); %></div>
    </div>
</body>
</html>
