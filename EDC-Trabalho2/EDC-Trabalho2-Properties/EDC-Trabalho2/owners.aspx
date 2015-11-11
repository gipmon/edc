<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="owners.aspx.cs" Inherits="EDC_Trabalho2.owners" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <br />
    <h2>Lista de Donos</h2>
    <hr />
    <asp:GridView ID="GridView1" runat="server" OnRowDeleting="ownersItemDeleting" onrowupdating="ownersItemUpdating" CssClass="table table-striped table-hover" AutoGenerateColumns="False" AllowPaging="True" DataSourceID="XmlDataSource1" GridLines="None">
        <Columns>
            <asp:TemplateField HeaderText="Name" SortExpression="name">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate> 
                    <asp:TextBox ID="txtname" runat="server"></asp:TextBox> 
                </FooterTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tax Number" SortExpression="tax_number">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("tax_number") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("tax_number") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate> 
                    <asp:TextBox ID="txttax" runat="server"></asp:TextBox> 
                </FooterTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Puchase Date" SortExpression="date_purchase">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("date_purchase") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("date_purchase") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate> 
                    <asp:TextBox ID="txtpurchase" runat="server"></asp:TextBox> 
                </FooterTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sale Date" SortExpression="data_sale">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("data_sale") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("data_sale") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate> 
                   <!-- <asp:TextBox ID="txtsale" runat="server"></asp:TextBox> -->
                </FooterTemplate> 
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"><i class="fa fa-pencil-square-o"></i></asp:LinkButton>
                     &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="True" CommandName="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                </ItemTemplate>
                <FooterTemplate> 
                    <asp:LinkButton ID="lnkSave" runat="server" CommandName="Save" OnClick="lnkSave_Click">Save</asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CommandName="Cancel" OnClick="lnkCancel_Click">Cancel</asp:LinkButton> 
                </FooterTemplate> 
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <div class="panel panel-warning">
              <div class="panel-heading">
                <h3 class="panel-title">Atenção!!</h3>
              </div>
              <div class="panel-body">
                Não há owners nesta propriedade!
              </div>
            </div>
        </EmptyDataTemplate>
        <PagerStyle CssClass="pagination-ys" />
    </asp:GridView>
    <asp:Button ID="button1" runat="server" CssClass="btn btn-default" OnClick="Button1_Click" Text="Add new owner" />
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/properties.xml" EnableCaching="False" XPath="/properties/property[land_register=1]/owners/owner"></asp:XmlDataSource>
</asp:Content>
