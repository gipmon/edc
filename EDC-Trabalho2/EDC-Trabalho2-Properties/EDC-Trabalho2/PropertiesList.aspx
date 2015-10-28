<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PropertiesList.aspx.cs" Inherits="EDC_Trabalho2.PropertiesList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h2>Lista de Propriedades</h2>
    <hr />

    <div class="row">
      <div class="col-md-12 text-center">Cidades: <asp:DropDownList runat="server"  AutoPostBack="True" ID="Cidades" DataSourceID="XmlDataSource2" DataTextField="city" DataValueField="city" OnSelectedIndexChanged="Unnamed1_SelectedIndexChanged" ></asp:DropDownList>
          <asp:XmlDataSource ID="XmlDataSource2" runat="server" DataFile="~/App_Data/properties.xml" TransformFile="~/App_Data/properties.xsl" XPath="properties/property[not(@city=preceding::property/@city)]"></asp:XmlDataSource>
        </div>
    </div>

    <hr />
    <asp:GridView ID="GridView1" onrowupdating="propertyItemUpdating" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" AllowPaging="True" DataSourceID="XmlDataSource1" GridLines="None">
        <Columns>
            <asp:HyperLinkField DataTextField="land_register" DataNavigateUrlFields="land_register" DataNavigateUrlFormatString="Property.aspx?ID={0}" HeaderText="Land Register" />
            <asp:BoundField DataField="city" HeaderText="City" SortExpression="city" />
            <asp:BoundField DataField="street" HeaderText="Street" SortExpression="street" />
            <asp:BoundField DataField="port_number" HeaderText="Port Number" SortExpression="port_number" />
            <asp:BoundField DataField="value" HeaderText="Value €" SortExpression="value" />                
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <div class="panel panel-warning">
              <div class="panel-heading">
                <h3 class="panel-title">Atenção!!</h3>
              </div>
              <div class="panel-body">
                Não há propriedades com essa seleção!
              </div>
            </div>
        </EmptyDataTemplate>
        <PagerStyle CssClass="pagination-ys" />
    </asp:GridView>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/properties.xml" EnableCaching="False" TransformFile="~/App_Data/properties.xsl"></asp:XmlDataSource>
    <asp:XmlDataSource ID="XmlDataSource3" runat="server" DataFile="~/App_Data/properties.xml" EnableCaching="False"></asp:XmlDataSource>
</asp:Content>
