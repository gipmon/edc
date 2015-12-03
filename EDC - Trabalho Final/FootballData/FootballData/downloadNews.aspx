<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="downloadNews.aspx.cs" Inherits="FootballData.downloadNews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:XmlDataSource ID="XmlDataSourceGoogle_feed" TransformFile="~/App_Data/googleNews.xsl" runat="server" EnableCaching="false"></asp:XmlDataSource>
    Downloaded.
</asp:Content>
