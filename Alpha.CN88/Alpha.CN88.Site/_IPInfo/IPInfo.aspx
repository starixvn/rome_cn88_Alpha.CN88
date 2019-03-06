<%@ Page Language="C#" AutoEventWireup="false" %>
<%@ Import Namespace="Sunplus.Core.Common.IPs" %>
<%@ Import Namespace="Sunplus.Agent.Model.Common" %>
<%@ Import Namespace="Alpha.CN88.AppResources" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link href="../App_Themes/Apo/IpInfo/IpInfo.min.css" rel="stylesheet" type="text/css" />
    <%
        var ip = Request.QueryString["ip"];
        if (string.IsNullOrEmpty(ip)) ip = AgentIPFormat.GetClientIP();
        ip = string.Equals(ip, "::1", StringComparison.OrdinalIgnoreCase) ? "127.0.0.1" : ip;
        var ipInfo = IpUtils.LocationFromIP(ip);
    %>
</head>
<body>
    <table class="tbl">
        <tr>
            <td><%=Resources.ip %></td>
            <td class="tdvalue"><%=ip %></td>
        </tr>
        <tr>
            <td><%=Resources.country %></td>
            <td class="tdvalue"><%=ipInfo.CountryLong %> <img src='<%="../Flags/" + ipInfo.CountryShort + ".gif" %>' /></td>
        </tr>
        <tr>
            <td><%=Resources.city %></td>
            <td class="tdvalue"><%=ipInfo.City %></td>
        </tr>
    </table>
    <div class="note">
        <%=string.Format(Resources.ipnote, string.Format("http://redirect.me/?https://www.infobyip.com/ip-{0}.html", ip)) %>
    </div>
</body>
</html>