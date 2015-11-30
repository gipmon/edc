using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace EDC_Trabalho3
{
    public partial class Feed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = feedChooser.SelectedValue;
            if(url.Length == 0)
            {
                url = "http://feeds.feedburner.com/PublicoRSS?format=xml";
            }
            XmlReader reader = XmlReader.Create(url);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();

            XmlDataSource_feed.Data = doc.OuterXml;
            XmlDataSource_feed.DataBind();
            XmlDataSource_feed.XPath = "/rss/channel";


            XmlDocument xdoc = XmlDataSource_feed.GetXmlDocument();
            XmlElement root = xdoc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes(XmlDataSource_feed.XPath); 

            foreach (XmlNode node in nodes)
            {
                titleLabel.Text = node.Attributes[0].Value;
                linkLabel.Text = node.Attributes[1].Value;
                descriptionLabel.Text = node.Attributes[3].Value;
                languageLabel.Text = node.Attributes[2].Value;
                ManagingEditorLabel.Text = node.Attributes[4].Value;
                WebMasterLabel.Text = node.Attributes[5].Value;
                LastBuildDateLabel.Text = node.Attributes[6].Value;
                CategoryLabel.Text = node.Attributes[7].Value;

                if (node.Attributes[8].Value.Length != 0)
                {
                    channelImage.Attributes["src"] = node.Attributes[8].Value;
                }
                else
                {
                    channelImage.Attributes["src"] = "http://placehold.it/160x160";
                }
                
            }
            
            XmlNodeList nodes_items = root.SelectNodes("/rss/channel/item");

            String innerHtml = "";

            foreach (XmlNode node in nodes_items)
            {
                String node_html = "<div class=\"col-xs-12 col-md-6 col-lg-4\"><div class=\"well\" style=\"min-height: 300px\"> <div class=\"media\"> <div class=\"media-body\"> <h4 class=\"media-heading\"><a target=\"_blank\" href=\"" + node.Attributes[2].Value + "\">" + node.Attributes[0].Value + "</a></h4> <div class=\"row\"><div class=\"col-md-6\"><small><i class=\"fa fa-tag\"></i> " + node.Attributes[3].Value + "</small></div><div class=\"col-md-6\" style=\"text-align: right\"><small><i class=\"fa fa-calendar - check - o\"></i> " + node.Attributes[4].Value + "</small></div></div><p>" + node.Attributes[1].Value + "</p></div></div></div></div>";
                innerHtml += node_html;
            }

            news.InnerHtml = innerHtml;
        }

    }
}