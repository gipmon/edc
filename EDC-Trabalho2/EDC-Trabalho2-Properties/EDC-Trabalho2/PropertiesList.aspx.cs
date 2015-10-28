using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace EDC_Trabalho2
{
    public partial class PropertiesList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed1_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDataSource1.XPath = "properties/property[@city='" + Cidades.SelectedValue + "']";
        }

        protected void propertyItemUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

            XmlDocument xdoc = XmlDataSource3.GetXmlDocument();

            HyperLink hyper = (HyperLink) row.Cells[0].Controls[0];
            XmlElement property = xdoc.SelectSingleNode("properties/property[@land_register='" + hyper.Text + "']") as XmlElement;
            XmlNode address = property.SelectSingleNode("address");
            address.SelectSingleNode("city").InnerText = e.NewValues["city"].ToString();
            address.SelectSingleNode("street").InnerText = e.NewValues["street"].ToString();
            address.SelectSingleNode("port_number").InnerText = e.NewValues["port_number"].ToString();
            property.SelectSingleNode("value").InnerText = e.NewValues["value"].ToString();

            XmlDataSource3.Save();
            XmlDataSource1.DataBind();
            XmlDataSource2.DataBind();

            e.Cancel = true;
            GridView1.EditIndex = -1;      
        }

    }
}