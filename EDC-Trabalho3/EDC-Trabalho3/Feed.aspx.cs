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
           // XmlDataSource_feed.XPath = "rss/channel";

        }
    }
}