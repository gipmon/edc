<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Team.aspx.cs" Inherits="FootballData.Team" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:XmlDataSource ID="XmlDataSourceGoogle_feed" TransformFile="~/App_Data/googleNews.xsl" runat="server" EnableCaching="false"></asp:XmlDataSource>
    <div class="container">
        <script>
            (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id))
                    return;
                js = d.createElement(s);
                js.id = id;
                js.src = "//connect.facebook.net/en_EN/all.js";
                fjs.parentNode.insertBefore(js, fjs);
            }(document, 'script', 'facebook-jssdk'));

            window.fbAsyncInit = function () {
                FB.init({
                    appId: '1121300931213333',
                    status: true,
                    xfbml: true,
                    cookie: true
                });
            };
        </script>
        <h2><%# teamName %></h2>
        <div class="pull-right">
            <%# subscribe_html %>
        </div>

        <div class="row">    
            <div class="col-md-4">
                <div class="card">
                    <div class="card-image" style="text-align: center">
                        <img class="img-responsive" style="max-height: 350px; max-width: 300px" src="<%# teamCrestURL %>">
                    
                    </div><!-- card image -->
                
                    <div class="card-content">
                        <span class="card-title">Squad value: <%# teamSquadValue %></span>   
                    </div>
                    <!-- card content -->
                    <div class="card-action">
                        <a href="#" data-toggle="modal" data-target="#fixturesModal" target="new_blank">Fixtures</a>
                        <a href="#" data-toggle="modal" data-target="#squadModal" target="new_blank">Squad</a>
                        <a href="#" data-toggle="modal" data-target="#leaguesModal" target="new_blank">Leagues History</a>
                    </div><!-- card actions -->
                </div>
                
            </div>
            <div class="col-md-6">
                <h3>News</h3>
                <% if (db_news == 0 && subscribers == 0){ %>
                    <small>We don't have any subscription for that team, so we don't have stored feeds for that team. </small>
                    <hr />
                <% }else if(db_news == 0) { %>
                    <script type="text/javascript">
                        $(document).ready(function () {
                            downloadNews();
                        });
                    </script>
                    <div class="alert alert-info" id="showAlert" style="display: none;" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        News for this team are ready, please reload the page!
                    </div>
                    <small id="smallAlert">News for that team are currently being downloaded! </small>
                    <hr />
                <% } %>
                
                <div class="row">
                    <%# news_html %>
                </div>

                <div class="row">
                    <div class="col-md-12 text-center">
                        <nav>
                          <ul class="pagination" data-toggle="<%# paginationNews %>">
                            <li>
                              <a href="#" aria-label="Previous">
                                <span aria-hidden="true">&laquo;</span>
                              </a>
                            </li>
                            <li id="li_page_1" class="active"><a data-toggle="pagination" href="#1">1</a></li>
                            <li>
                              <a href="#" aria-label="Next">
                                <span aria-hidden="true">&raquo;</span>
                              </a>
                            </li>
                          </ul>
                        </nav>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade scroll-modal" id="fixturesModal" tabindex="-1" role="dialog" aria-labelledby="fixturesLabel">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="fixturesLabel">Fixtures</h4>
          </div>
          <div class="modal-body">
            <table class="table table-condensed">
                <thead>
                    <tr>
                        <td><strong>Date</strong></td>
                        <td><strong>Home</strong></td>
                        <td><strong>Away</strong></td>
                        <td><strong>Result</strong></td>
                    </tr>
                </thead>
                <tbody>
                    <%# fixturesTable_html %>
                </tbody>
            </table>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>
    <!-- Modal -->
    <div class="modal fade scroll-modal" id="squadModal" tabindex="-1" role="dialog" aria-labelledby="squadLabel">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="squadLabel">Squad</h4>
          </div>
          <div class="modal-body">
              <% if (players_list_html != "")
                  { %>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <td><strong>Name</strong></td>
                        <td><strong>Jersey Number</strong></td>
                        <td><strong>Position</strong></td>
                        <td><strong>Nationality</strong></td>
                        <td><strong>Date Of Birth</strong></td>
                        <td><strong>Market Value</strong></td>
                        <td><strong>Contract Until</strong></td>
                    </tr>
                </thead>
                <tbody>
                    <%# players_list_html %>
                </tbody>
            </table>
              <% }
    else
    {
                      %>
        <div class="alert alert-warning" role="alert">
                <strong>Sorry :(</strong>
                <br />
                We don't have any information about the players of this team.
            </div>
              <br />

              <%} %>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>
     <!-- Modal -->
    <div class="modal fade scroll-modal" id="leaguesModal" tabindex="-1" role="dialog" aria-labelledby="leaguesLabel">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="leaguesLabel">Leagues History</h4>
          </div>
          <div class="modal-body">
            <table class="table table-condensed">
                <thead>
                    <tr>
                        <td><strong>Name</strong></td>
                        <td><strong>Year</strong></td>
                    </tr>
                </thead>
                <tbody>
                    <%# leaguesHistory_html %>
                </tbody>
            </table>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>
</asp:Content>
