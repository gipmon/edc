<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="manageFeeds.aspx.cs" Inherits="EDC_Trabalho3.manageFeeds" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1><i class="fa fa-rss fa-4"></i> Gestão de feeds</h1>
    <hr />
    <asp:XmlDataSource ID="XmlDataSource_feed" DataFile="~/App_Data/FeedsList.xml" XPath="/feeds/feed" runat="server" EnableCaching="false"></asp:XmlDataSource>
        
    <asp:FormView ID="formFeeds" OnItemUpdating="formFeeds_ItemUpdating" OnItemDeleting="formFeeds_ItemDeleting" OnItemInserting="formFeeds_ItemInserting" DataSourceID="XmlDataSource_feed" AllowPaging="True" runat="server" Width="100%">
        <ItemTemplate>
            <table class="table table-bordered">
                <tr>
                    <td class="col-xs-2">Name:</td>
                    <td class="col-xs-10">
                        <asp:Label runat="server" ID="nameFeed" Text='<%# Bind("name") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>URL:</td>
                    <td>
                        <asp:Label runat="server" ID="urlFeed" Text='<%# Bind("url") %>'></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:LinkButton runat="server" CssClass="btn btn-default" CommandName="Edit"><i class="fa fa-pencil-square-o"></i></asp:LinkButton>
            <asp:LinkButton runat="server" CssClass="btn btn-default" CommandName="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
            <asp:LinkButton runat="server" CssClass="btn btn-default" CommandName="New"><i class="fa fa-star-o"></i></asp:LinkButton>
        </ItemTemplate>
        <EditItemTemplate>
            <table class="table table-bordered" border="0">
                <tr>
                    <td>Name:</td>
                    <td>
                        <asp:Label runat="server" ID="nameFeed" Text='<%# Bind("name") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>URL:</td>
                    <td>
                        <asp:TextBox runat="server" ID="url" CssClass="form-control"  Text='<%# Bind("url") %>'></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:LinkButton runat="server" CssClass="btn btn-default" CommandName="Update"><i class="fa fa-floppy-o"></i></asp:LinkButton>
            <asp:LinkButton runat="server" CssClass="btn btn-default" CommandName="Cancel"><i class="fa fa-times"></i></asp:LinkButton>
        </EditItemTemplate>
        <InsertItemTemplate>
            <table class="table table-bordered" border="0">
                <tr>
                    <td>Name:</td>
                    <td>
                        <asp:TextBox runat="server" ID="nameInsert" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>URL:</td>
                    <td>
                        <asp:TextBox runat="server" ID="urlInsert" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:LinkButton runat="server" CssClass="btn btn-default" CommandName="Insert"><i class="fa fa-floppy-o"></i></asp:LinkButton>
            <asp:LinkButton runat="server" CssClass="btn btn-default" CommandName="Cancel"><i class="fa fa-times"></i></asp:LinkButton>
        </InsertItemTemplate>
       
        <FooterTemplate>
        </FooterTemplate>
        <PagerStyle CssClass="pagination-ys" />
        <PagerSettings Mode="Numeric" />
    </asp:FormView>
    <asp:LinkButton ID="Feeds" PostBackUrl="~/Feed.aspx" runat="server" CssClass="btn btn-primary"><i class="fa fa-rss"></i>&nbsp;Feeds</asp:LinkButton>
        
</asp:Content>
