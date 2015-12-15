<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FootballData._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>The ultimate agregator!</h1>
        <p class="lead">Search, get agregated news and browse team, players and leagues data!</p>
        <p><a href="Account/Register" class="btn btn-primary btn-lg">Register &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>FC Porto</h2>
            <p>
                Get all information about FC Porto.</p>
            <p>
                <a class="btn btn-default" href="Team.aspx?ID=503">See team &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Sporting CP</h2>
            <p> Get all information about Sporting CP.</p>
            <p>
                <a class="btn btn-default" href="Team.aspx?ID=498">See team &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>SL Benfica</h2>
            <p>Get all information about SL Benfica.</p>
            <p>
                <a class="btn btn-default" href="Team.aspx?ID=495">See team &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
