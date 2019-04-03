<%@ Page Language="C#" AutoEventWireup="false" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="Sunplus.Agent.Model.Entities" %>

<html>
    <head>
        <title>Error</title>
        <%=Alpha.Web.Helpers.StaticResources.AssetStyles.Render("/bundles/site-cn88/error-page")%>
        <%=Alpha.Web.Helpers.StaticResources.RenderStyles("~/assets/bundles/error-page")%>
    </head>
    <body>
        <%
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(UserSession.Current.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        %>
        <div class="page-content">
            <span class="icon-error"></span>
            <div class="txt"><% Response.Write(Alpha.CN88.AppResources.Resources.Error); %></div>
        </div>
    </body>
</html>