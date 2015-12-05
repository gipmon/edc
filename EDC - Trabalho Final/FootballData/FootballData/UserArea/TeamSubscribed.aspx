<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TeamSubscribed.aspx.cs" Inherits="FootballData.UserArea.TeamSubscribed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Teams subscribed</h2>
    <div class="container">
        <div class="row col-md-6 custyle">
            <table class="table table-striped custab">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Team</th>
                        <th>News</th>
                        <th class="text-center">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <%# teamsSubscribed %>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
