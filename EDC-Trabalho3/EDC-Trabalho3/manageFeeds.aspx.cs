using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace EDC_Trabalho3
{
    public partial class manageFeeds : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void formFeeds_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            XmlDocument xdoc = XmlDataSource_feed.GetXmlDocument();
            
            XmlElement feed = xdoc.SelectSingleNode("feeds/feed[@name='" + e.OldValues[0].ToString() + "'][@url='" + e.OldValues[1].ToString() + "']") as XmlElement;
            feed.Attributes["url"].Value = e.NewValues["url"].ToString();

            XmlDataSource_feed.Save();
            XmlDataSource_feed.DataBind();

            e.Cancel = true;
            formFeeds.ChangeMode(FormViewMode.ReadOnly);
        }

        protected void formFeeds_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            XmlDocument xdoc = XmlDataSource_feed.GetXmlDocument();
            
            XmlElement feeds = xdoc.SelectSingleNode("feeds") as XmlElement;
            XmlElement feed = xdoc.CreateElement("feed");
            XmlAttribute name = xdoc.CreateAttribute("name");
            XmlAttribute url = xdoc.CreateAttribute("url");

            name.InnerText = ((TextBox)formFeeds.Row.Cells[0].FindControl("nameInsert")).Text;
            url.InnerText = ((TextBox)formFeeds.Row.Cells[0].FindControl("urlInsert")).Text;

            feeds.AppendChild(feed);
            feed.Attributes.Append(name);
            feed.Attributes.Append(url);

            XmlDataSource_feed.Save();
            XmlDataSource_feed.DataBind();

            formFeeds.DataBind();
            formFeeds.ChangeMode(FormViewMode.ReadOnly);
            e.Cancel = true;
        }

        protected void formFeeds_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            String name = ((Label)formFeeds.Row.Cells[0].FindControl("nameFeed")).Text;
            String url = ((Label)formFeeds.Row.Cells[0].FindControl("urlFeed")).Text;
            XmlDocument xdoc = XmlDataSource_feed.GetXmlDocument();

            XmlElement feeds = xdoc.SelectSingleNode("feeds") as XmlElement;
            XmlElement feed = xdoc.SelectSingleNode("feeds/feed[@name='" + name + "'][@url = '" + url + "']") as XmlElement;
            feeds.RemoveChild(feed);

            XmlDataSource_feed.Save();

            e.Cancel = true;

            formFeeds.DataBind();

        }
    }
}