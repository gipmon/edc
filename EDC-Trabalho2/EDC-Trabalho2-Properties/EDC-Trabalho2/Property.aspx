<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Property.aspx.cs" Inherits="EDC_Trabalho2.Property" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="MainContent">
    <br />
    <h2>Informação do Curso</h2>
    <hr />
    <style type="text/css">
        #MainContent_cursodetail > tbody > tr > td:first-child{
            background-color: #dff0d8;
            font-weight: bold;
        }
    </style>
    <asp:DetailsView ID="cursodetail" runat="server" GridLines="Horizontal" CssClass="table table-striped table-hover" AutoGenerateRows="False" DataSourceID="XmlDataSource1">
        <Fields>
            <asp:BoundField HtmlEncode="false" DataField="guid" HeaderText="Guid" SortExpression="guid"  />
            <asp:BoundField HtmlEncode="false" DataField="nome" HeaderText="Nome" SortExpression="nome" />
            <asp:BoundField HtmlEncode="false" DataField="codigo" HeaderText="Código" SortExpression="codigo" />
            <asp:BoundField HtmlEncode="false" DataField="grau" HeaderText="Grau" SortExpression="grau" />
            <asp:BoundField HtmlEncode="false" DataField="vagas" HeaderText="Vagas" SortExpression="vagas" />
            <asp:BoundField HtmlEncode="false" DataField="saidas_profissionais" HeaderText="Saídas profissionais" SortExpression="saidas_profissionais" />
            <asp:BoundField HtmlEncode="false" DataField="fase1" HeaderText="Média [1º Fase]" SortExpression="fase1" />
            <asp:BoundField HtmlEncode="false" DataField="fase2" HeaderText="Média [2º Fase]" SortExpression="fase2" />
            <asp:BoundField HtmlEncode="false" DataField="duracao" HeaderText="Duração" SortExpression="duracao" />
            <asp:BoundField HtmlEncode="false" DataField="provas" HeaderText="Provas" SortExpression="provas" />
        </Fields>
    </asp:DetailsView>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="http://acesso.ua.pt/xml/curso.asp?i=31" TransformFile="~/xml/EngenhariaQuimica.xsl" XPath="curso"></asp:XmlDataSource>
    </asp:Content>

