<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="EDC_Trabalho2.Courses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br />
    <asp:GridView runat="server" CssClass="table table-stripped table-hover" AllowPaging="True" DataSourceID="XmlDataSource1" GridLines="None">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
        <PagerStyle CssClass="pagination-ys" />
    </asp:GridView>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" 
                        DataFile="~/xml/cursos.xml" 
                        TransformFile="~/xml/cursos.xsl" 
                        XPath=""
                        EnableCaching="false">
    </asp:XmlDataSource>
</asp:Content>
