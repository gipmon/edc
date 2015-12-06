<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyFeed.aspx.cs" Inherits="FootballData.UserArea.MyFeed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:XmlDataSource ID="XmlDataSource_feed" TransformFile="~/App_Data/footballData.xsl" runat="server" EnableCaching="false"></asp:XmlDataSource>
    <h2><i class="fa fa-newspaper-o fa-2"></i> My Feed</h2>
    <div class="container">
        <div class="row">
            <%# newsFeed_html %>
            <div class="col-md-3">
                <div class="panel-group" id="accordion1" role="tablist" aria-multiselectable="true">
                  <asp:Repeater runat="server" ID="rssHtmlTab">
                      <ItemTemplate>
                          <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading<%# DataBinder.Eval(Container.DataItem, "id") %>">
                              <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion3" href="#collapse<%# DataBinder.Eval(Container.DataItem, "id") %>" aria-expanded="false" aria-controls="collapse<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                  <%# TeamNewRss.truncate(DataBinder.Eval(Container.DataItem, "title").ToString(), 26) %>...
                                </a>
                              </h4>
                            </div>
                            <div id="collapse<%# DataBinder.Eval(Container.DataItem, "id") %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading<%# DataBinder.Eval(Container.DataItem, "id") %>">
                              <div class="panel-body">
                                  <a href="<%# DataBinder.Eval(Container.DataItem, "link") %>"><%# DataBinder.Eval(Container.DataItem, "title") %></a>
                                  <br />
                                  <i class="fa fa-futbol-o"></i> <a href="/Team.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "teamId") %>"><%# DataBinder.Eval(Container.DataItem, "team") %></a>
                              </div>
                            </div>
                          </div>
                      </ItemTemplate>
                  </asp:Repeater>
                </div>
            </div>
            <div class="col-md-3">
                <div class="panel-group" id="accordion2" role="tablist" aria-multiselectable="true">
                  <asp:Repeater runat="server" ID="rssHtmlTab1">
                      <ItemTemplate>
                          <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading<%# DataBinder.Eval(Container.DataItem, "id") %>">
                              <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion3" href="#collapse<%# DataBinder.Eval(Container.DataItem, "id") %>" aria-expanded="false" aria-controls="collapse<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                  <%# TeamNewRss.truncate(DataBinder.Eval(Container.DataItem, "title").ToString(), 26) %>...
                                </a>
                              </h4>
                            </div>
                            <div id="collapse<%# DataBinder.Eval(Container.DataItem, "id") %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading<%# DataBinder.Eval(Container.DataItem, "id") %>">
                              <div class="panel-body">
                                  <a href="<%# DataBinder.Eval(Container.DataItem, "link") %>"><%# DataBinder.Eval(Container.DataItem, "title") %></a>
                                  <br />
                                  <i class="fa fa-futbol-o"></i> <a href="/Team.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "teamId") %>"><%# DataBinder.Eval(Container.DataItem, "team") %></a>
                              </div>
                            </div>
                          </div>
                      </ItemTemplate>
                  </asp:Repeater>
                </div>
            </div>
            <div class="col-md-3">
                <div class="panel-group" id="accordion3" role="tablist" aria-multiselectable="true">
                  <asp:Repeater runat="server" ID="rssHtmlTab2">
                      <ItemTemplate>
                          <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading<%# DataBinder.Eval(Container.DataItem, "id") %>">
                              <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion3" href="#collapse<%# DataBinder.Eval(Container.DataItem, "id") %>" aria-expanded="false" aria-controls="collapse<%# DataBinder.Eval(Container.DataItem, "id") %>">
                                  <%# TeamNewRss.truncate(DataBinder.Eval(Container.DataItem, "title").ToString(), 26) %>...
                                </a>
                              </h4>
                            </div>
                            <div id="collapse<%# DataBinder.Eval(Container.DataItem, "id") %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading<%# DataBinder.Eval(Container.DataItem, "id") %>">
                              <div class="panel-body">
                                  <a href="<%# DataBinder.Eval(Container.DataItem, "link") %>"><%# DataBinder.Eval(Container.DataItem, "title") %></a>
                                  <br />
                                  <i class="fa fa-futbol-o"></i> <a href="/Team.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "teamId") %>"><%# DataBinder.Eval(Container.DataItem, "team") %></a>
                              </div>
                            </div>
                          </div>
                      </ItemTemplate>
                  </asp:Repeater>
                </div>
            </div>
            <div class="col-md-3">
                <h4>Filter by team</h4>
                <div class="checkbox">
                    <label>
                        <asp:CheckBoxList ID="teams" runat="server" OnSelectedIndexChanged="teams_SelectedIndexChanged" AutoPostBack="True"></asp:CheckBoxList>
                    </label>
                </div>
                <div class="form-group">
                    <label for="rssFeedLink">Your RSS feed URL:</label>
                    <div class="input-group">
                        <div class="input-group-btn">
                            <asp:LinkButton runat="server" ID="urlBtn" CssClass="btn btn-default">  <i class="fa fa-hand-o-right"></i></asp:LinkButton>
                        </div>
                        <asp:TextBox runat="server" class="form-control"  onClick="this.setSelectionRange(0, this.value.length)" ID="rssFeedLink"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
