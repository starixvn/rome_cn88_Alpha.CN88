<%@ Page Language="C#" AutoEventWireup="false" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="Sunplus.Agent.Model.Entities" %>

<%
    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(UserSession.Current.Language);
    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

    Response.Write(new Fanex.Age.UI.Components.ErrorPage_Page(Alpha.CN88.AppResources.Resources.Error));
%>