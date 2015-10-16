<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="EDC_Trabalho2.Course" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:DetailsView runat="server" AutoGenerateRows="False" DataSourceID="XmlDataSource1">
    </asp:DetailsView>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/xml/EngenhariaQuimica.xml" TransformFile="~/xml/EngenhariaQuimica.xsl"></asp:XmlDataSource>
</asp:Content>
