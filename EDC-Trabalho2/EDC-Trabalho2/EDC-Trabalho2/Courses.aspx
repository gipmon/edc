<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="EDC_Trabalho2.Courses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h2>Lista de cursos</h2>
    <asp:GridView runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false" AllowPaging="True" DataSourceID="XmlDataSource1" GridLines="None">
        <Columns>
            <asp:HyperLinkField DataTextField="Guid" DataNavigateUrlFields="Guid" DataNavigateUrlFormatString="Course.aspx?ID={0}" HeaderText="Guid" />
            <asp:BoundField DataField="Nome" HeaderText="Nome" />
            <asp:BoundField DataField="Grau" HeaderText="Grau" />
            <asp:BoundField DataField="Local" HeaderText="Local" />
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
