<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="displayReports.aspx.cs" Inherits="EDC_Trabalho1.personal.displayReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>My Sales</h2>
    <p>
      
        <asp:GridView runat="server" CssClass="table table-striped table-hover" GridLines="None" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="title" HeaderText="title" SortExpression="title" />
                <asp:BoundField DataField="order num" HeaderText="order num" SortExpression="order num" />
                <asp:BoundField DataField="store id" HeaderText="store id" SortExpression="store id" />
                <asp:BoundField DataField="order date" HeaderText="order date" SortExpression="order date" />
                <asp:BoundField DataField="quantaty" HeaderText="quantaty" SortExpression="quantaty" />
                <asp:BoundField DataField="payterms" HeaderText="payterms" SortExpression="payterms" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:pubsConnectionString %>" SelectCommand="Procedure" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="UserName" />
            </SelectParameters>
        </asp:SqlDataSource>
    </p>
</asp:Content>
