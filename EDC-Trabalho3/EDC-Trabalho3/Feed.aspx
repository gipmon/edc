<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Feed.aspx.cs" Inherits="EDC_Trabalho3.Feed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1><i class="fa fa-rss fa-4"></i> My Feed Reader</h1>
    <hr />
    <div class="row">
        <asp:XmlDataSource ID="XmlDataSource_feed" TransformFile="~/App_Data/channel.xsl" runat="server" EnableCaching="false"></asp:XmlDataSource>
        <div class="col-md-6" style="text-align: center">
            <asp:DropDownList ID="feedChooser" runat="server" CssClass="form-control" AutoPostBack="True" DataSourceID="XmlDataSourceFeedReader" DataTextField="name" DataValueField="url"></asp:DropDownList>
             <asp:XmlDataSource ID="XmlDataSourceFeedReader" runat="server" DataFile="~/App_Data/FeedsList.xml"></asp:XmlDataSource>
        </div>
        <div class="col-md-6" style="text-align: right; margin-top: 0px;">
            <asp:LinkButton ID="ManageFeeds" runat="server" CssClass="btn btn-primary"><i class="fa fa-rss"></i>&nbsp;Manage feeds</asp:LinkButton>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <h3>Feed Info</h3>
            <table class="table table-striped">
                <tbody>
                <tr>
                    <th scope="row">Title</th>
                    <td><asp:Label ID="titleLabel" runat="server"  /></td>
                </tr>
                <tr>
                    <th scope="row">Link</th>
                    <td><asp:Label ID="linkLabel" runat="server"  /></td>
                </tr>
                <tr>
                    <th scope="row">Description</th>
                    <td><asp:Label ID="descriptionLabel" runat="server"  /></td>
                </tr>
                <tr>
                    <th scope="row">Language</th>
                    <td><asp:Label ID="languageLabel" runat="server"  /></td>
                </tr>
                <tr>
                    <th scope="row">ManagingEditor</th>
                    <td><asp:Label ID="ManagingEditorLabel" runat="server" /></td>
                </tr>
                <tr>
                    <th scope="row">WebMaster</th>
                    <td><asp:Label ID="WebMasterLabel" runat="server"  /></td>
                </tr>
                <tr>
                    <th scope="row">LastBuildDate</th>
                    <td><asp:Label ID="LastBuildDateLabel" runat="server"  /></td>
                </tr>
                <tr>
                    <th scope="row">Category</th>
                    <td><asp:Label ID="CategoryLabel" runat="server" Text='' /></td>
                </tr>
                </tbody>
            </table>
        </div>
        <div class="col-md-6 text-center">
            <h3>Feed Image</h3>
            <div class="row">
                <div class="col-xs-4"></div>
        		<div class="col-xs-4">
        			<img runat="server" ID="channelImage" src="http://placehold.it/160x160" style="height:160px; width:160px" class="img-responsive img-radio">
        		</div>
                <div class="col-xs-4"></div>
        	</div>
        </div>
    </div>
        
    <hr />
    <h3>Feed News <small><asp:Label runat="server" ID="counter_news" Text="10"></asp:Label></small></h3>

    <div class="row">
          <div class="col-md-4">
            <div class="well">
              <div class="media">
      	        <a class="pull-left" href="#">
    		        <img class="media-object" src="http://placekitten.com/150/150">
  		        </a>
  		        <div class="media-body">
    		        <h4 class="media-heading">Receta 1</h4>
                  <p class="text-right">By Francisco</p>
                  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis pharetra varius quam sit amet vulputate. 
        Quisque mauris augue, molestie tincidunt condimentum vitae, gravida a libero. Aenean sit amet felis 
        dolor, in sagittis nisi. Sed ac orci quis tortor imperdiet venenatis. Duis elementum auctor accumsan. 
        Aliquam in felis sit amet augue.</p>
                  <ul class="list-inline list-unstyled">
  			        <li><span><i class="glyphicon glyphicon-calendar"></i> 2 days, 8 hours </span></li>
                    <li>|</li>
                    <span><i class="glyphicon glyphicon-comment"></i> 2 comments</span>
                    <li>|</li>
                    <li>
                       <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star-empty"></span>
                    </li>
                    <li>|</li>
                    <li>
                    <!-- Use Font Awesome http://fortawesome.github.io/Font-Awesome/ -->
                      <span><i class="fa fa-facebook-square"></i></span>
                      <span><i class="fa fa-twitter-square"></i></span>
                      <span><i class="fa fa-google-plus-square"></i></span>
                    </li>
			        </ul>
               </div>
            </div>
          </div>
          </div>
          <div class="col-md-4">
            <div class="well">
              <div class="media">
      	        <a class="pull-left" href="#">
    		        <img class="media-object" src="http://placekitten.com/150/150">
  		        </a>
  		        <div class="media-body">
    		        <h4 class="media-heading">Receta 1</h4>
                  <p class="text-right">By Francisco</p>
                  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis pharetra varius quam sit amet vulputate. 
        Quisque mauris augue, molestie tincidunt condimentum vitae, gravida a libero. Aenean sit amet felis 
        dolor, in sagittis nisi. Sed ac orci quis tortor imperdiet venenatis. Duis elementum auctor accumsan. 
        Aliquam in felis sit amet augue.</p>
                  <ul class="list-inline list-unstyled">
  			        <li><span><i class="glyphicon glyphicon-calendar"></i> 2 days, 8 hours </span></li>
                    <li>|</li>
                    <span><i class="glyphicon glyphicon-comment"></i> 2 comments</span>
                    <li>|</li>
                    <li>
                       <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star-empty"></span>
                    </li>
                    <li>|</li>
                    <li>
                    <!-- Use Font Awesome http://fortawesome.github.io/Font-Awesome/ -->
                      <span><i class="fa fa-facebook-square"></i></span>
                      <span><i class="fa fa-twitter-square"></i></span>
                      <span><i class="fa fa-google-plus-square"></i></span>
                    </li>
			        </ul>
               </div>
            </div>
          </div>
          </div>
          <div class="col-md-4">
            <div class="well">
              <div class="media">
      	        <a class="pull-left" href="#">
    		        <img class="media-object" src="http://placekitten.com/150/150">
  		        </a>
  		        <div class="media-body">
    		        <h4 class="media-heading">Receta 1</h4>
                  <p class="text-right">By Francisco</p>
                  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis pharetra varius quam sit amet vulputate. 
        Quisque mauris augue, molestie tincidunt condimentum vitae, gravida a libero. Aenean sit amet felis 
        dolor, in sagittis nisi. Sed ac orci quis tortor imperdiet venenatis. Duis elementum auctor accumsan. 
        Aliquam in felis sit amet augue.</p>
                  <ul class="list-inline list-unstyled">
  			        <li><span><i class="glyphicon glyphicon-calendar"></i> 2 days, 8 hours </span></li>
                    <li>|</li>
                    <span><i class="glyphicon glyphicon-comment"></i> 2 comments</span>
                    <li>|</li>
                    <li>
                       <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star"></span>
                                <span class="glyphicon glyphicon-star-empty"></span>
                    </li>
                    <li>|</li>
                    <li>
                    <!-- Use Font Awesome http://fortawesome.github.io/Font-Awesome/ -->
                      <span><i class="fa fa-facebook-square"></i></span>
                      <span><i class="fa fa-twitter-square"></i></span>
                      <span><i class="fa fa-google-plus-square"></i></span>
                    </li>
			        </ul>
               </div>
            </div>
          </div>
          </div>
    </div>
</asp:Content>
