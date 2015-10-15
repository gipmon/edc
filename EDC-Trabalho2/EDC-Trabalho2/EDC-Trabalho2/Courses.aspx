<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="EDC_Trabalho2.Courses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h2>Lista de cursos</h2>
    <hr />
    <div class="row">
      <div class="col-md-6 text-center">Tipos: <asp:DropDownList runat="server"  AutoPostBack="True" ID="Tipos" DataSourceID="XmlDataSource2" DataTextField="Grau" DataValueField="Grau" OnSelectedIndexChanged="Unnamed1_SelectedIndexChanged" ></asp:DropDownList>
          <asp:XmlDataSource ID="XmlDataSource2" runat="server" DataFile="~/xml/cursos.xml" TransformFile="~/xml/cursos.xsl" XPath="cursos/curso[not(@grau=preceding::curso/@grau)]"></asp:XmlDataSource>
        </div>
      <div class="col-md-6 text-center">Locais: <asp:DropDownList runat="server"  AutoPostBack="True" ID="Locais" DataSourceID="XmlDataSource3" DataTextField="local" DataValueField="local" OnSelectedIndexChanged="Locais_SelectedIndexChanged"></asp:DropDownList>
          <asp:XmlDataSource ID="XmlDataSource3" runat="server" DataFile="~/xml/cursos.xml" TransformFile="~/xml/cursos.xsl" XPath="cursos/curso[not(@local=preceding::curso/@local)]"></asp:XmlDataSource>
      </div> 
    </div>
    <hr />
    <asp:GridView runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false" AllowPaging="True" DataSourceID="XmlDataSource1" GridLines="None">
        <Columns>
            <asp:HyperLinkField DataTextField="guid" DataNavigateUrlFields="guid" DataNavigateUrlFormatString="Course.aspx?ID={0}" HeaderText="Guid" />
            <asp:BoundField DataField="nome" HeaderText="Nome" />
            <asp:BoundField DataField="grau" HeaderText="Grau" />
            <asp:BoundField DataField="local" HeaderText="Local" />
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
