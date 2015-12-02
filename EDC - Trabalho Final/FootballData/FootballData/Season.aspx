﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Season.aspx.cs" Inherits="FootballData.Season" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="col-md-6">
            <h2><%# seasonCaption %></h2>
            <table class="table table-condensed">
              <tr>
                  <td>Number of teams</td>
                  <td><%# numberOfTeams %></td>
              </tr>
              <tr>
                  <td>Number of games</td>
                  <td><%# numberOfGames %></td>
              </tr>
              <tr>
                  <td>Year</td>
                  <td><%# year %></td>
              </tr>
              <tr>
                  <td>Last updated</td>
                  <td><%# lastUpdated %></td>
              </tr>
            </table>
            <% if (int.Parse(matchday) == min_MatchDay) { %>
                <h4>Matchday <%# matchday %> <a href="?ID=<%# id_str %>&matchday=<%# (int.Parse(matchday)+1).ToString() %>"><i class="fa fa-caret-right"></i></a></h4>
            <% }else if(int.Parse(matchday) == max_MatchDay) { %>
                <h4><a href="?ID=<%# id_str %>&matchday=<%# (int.Parse(matchday)-1).ToString() %>"><i class="fa fa-caret-left"></i></a> Matchday <%# matchday %> </h4>
            <% }else{ %>
                <h4><a href="?ID=<%# id_str %>&matchday=<%# (int.Parse(matchday)-1).ToString() %>"><i class="fa fa-caret-left"></i></a> Matchday <%# matchday %> <a href="?ID=<%# id_str %>&matchday=<%# (int.Parse(matchday)+1).ToString() %>"><i class="fa fa-caret-right"></i></a></h4>
            <% } %>
            <table class="table table-condensed">
                <thead>
                    <tr>
                        <td>Date</td>
                        <td>Home</td>
                        <td>Away</td>
                        <td>Result</td>
                    </tr>
                </thead>
                <tbody>
                    <%# matchdayTable_html %>
                </tbody>
            </table>
        </div>
        <div class="col-md-6">
            <h3>League table</h3>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Points">
                            PTS
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Played">
                            P
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Won">
                            W
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Drawn">
                            D
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Lost">
                            L
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Goals For">
                            GF
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Goals Against">
                            GA
                        </th>
                        <th data-toggle="tooltip" data-placement="top" title="Goals Difference">
                            GD
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <%# leagueTable_html %>
                </tbody>
            </table>
        </div>
    </div>
    <br />
    <script type="text/javascript">
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    </script>
</asp:Content>