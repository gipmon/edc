<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="EDC_Trabalho2.Course" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="MainContent">
    <br />
    <h2>Informação do Curso</h2>
    <hr />
    <asp:DetailsView runat="server" GridLines="Horizontal" CssClass="table table-stripped" AutoGenerateRows="False" DataSourceID="XmlDataSource1">
        <Fields>
            <asp:BoundField HtmlEncode="false" DataField="guid" HeaderText="guid" SortExpression="guid"  />
            <asp:BoundField HtmlEncode="false" DataField="nome" HeaderText="nome" SortExpression="nome" />
            <asp:BoundField HtmlEncode="false" DataField="codigo" HeaderText="codigo" SortExpression="codigo" />
            <asp:BoundField HtmlEncode="false" DataField="grau" HeaderText="grau" SortExpression="grau" />
            <asp:BoundField HtmlEncode="false" DataField="vagas" HeaderText="vagas" SortExpression="vagas" />
            <asp:BoundField HtmlEncode="false" DataField="saidas_profissionais" HeaderText="saidas_profissionais" SortExpression="saidas_profissionais" />
            <asp:BoundField HtmlEncode="false" DataField="fase1" HeaderText="fase1" SortExpression="fase1" />
            <asp:BoundField HtmlEncode="false" DataField="fase2" HeaderText="fase2" SortExpression="fase2" />
            <asp:BoundField HtmlEncode="false" DataField="duracao" HeaderText="duracao" SortExpression="duracao" />
            <asp:BoundField HtmlEncode="false" DataField="provas" HeaderText="provas" SortExpression="provas" />
        </Fields>
    </asp:DetailsView>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/xml/EngenhariaQuimica.xml" TransformFile="~/xml/EngenhariaQuimica.xsl" XPath="curso"></asp:XmlDataSource>
    </asp:Content>

