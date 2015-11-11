using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Web.UI.HtmlControls;

namespace EDC_Trabalho2
{
    public partial class PropertiesList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            total_value();
        }

        private void total_value()
        {
            int total = 0;

            XmlDocument xdoc = XmlDataSource1.GetXmlDocument();
            XmlElement root = xdoc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes(XmlDataSource1.XPath); // You can also use XPath here
            String sd = XmlDataSource1.XPath;

            foreach (XmlNode node in nodes)
            {
                int value = Int32.Parse(node.Attributes[4].Value);
                total += value;
            }

            totalLabel.Text = "Total: €" + total.ToString();
        }

        protected void Unnamed1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Cidades.SelectedValue == "Todos")
            {
                XmlDataSource1.XPath = "properties/property";
            }
            else
            {
                XmlDataSource1.XPath = "/properties/property[@city='" + Cidades.SelectedValue + "']";
            }
            total_value();
        }

        protected void propertyItemUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            HyperLink hyper = (HyperLink)row.FindControl("HyperLink1");
            XmlDocument xdoc = XmlDataSource3.GetXmlDocument();

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

        protected void Search_Owner(object sender, EventArgs e)
        {
            if (tax_number.Text == "" )
            {
                if (Cidades.SelectedValue == "Todos")
                {
                    XmlDataSource1.XPath = "properties/property";
                }
                else
                {
                    XmlDataSource1.XPath = "/properties/property[@city='" + Cidades.SelectedValue + "']";
                }
            }
            else
            {
                if (Cidades.SelectedValue == "Todos")
                {
                    XmlDataSource1.XPath = "/properties/property[owners/owner[@tax_number='" + tax_number.Text + "']]";
                }
                else
                {
                    XmlDataSource1.XPath = "/properties/property[@city='" + Cidades.SelectedValue + "'][owners/owner[@tax_number='" + tax_number.Text + "'][@]";
                }
                
            }
            total_value();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GridView1.ShowFooter = true;
 
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            XmlDocument xdoc = XmlDataSource3.GetXmlDocument();

            XmlElement root = xdoc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/properties/property"); // You can also use XPath here
            int land_register_number = 0;
            foreach (XmlNode node in nodes)
            {
                int ld_register = Int32.Parse(node.Attributes[0].Value);
                if (land_register_number < ld_register)
                {
                    land_register_number = ld_register;
                }
            }

            land_register_number++;

            XmlElement properties = xdoc.SelectSingleNode("properties") as XmlElement;
            XmlElement property = xdoc.CreateElement("property");
            XmlElement land_register = xdoc.CreateElement("land_register");
            XmlElement address = xdoc.CreateElement("address");
            XmlElement city = xdoc.CreateElement("city");
            XmlElement street = xdoc.CreateElement("street");
            XmlElement port_number = xdoc.CreateElement("port_number");
            XmlElement value = xdoc.CreateElement("value");
            XmlElement owners = xdoc.CreateElement("owners");
            XmlAttribute aland_register = xdoc.CreateAttribute("land_register");
            aland_register.InnerText = (land_register_number).ToString();
            land_register.InnerText = (land_register_number).ToString();
            city.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txtcity")).Text;
            street.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txtstreet")).Text;
            port_number.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txtport")).Text;
            value.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txtvalue")).Text;


            property.Attributes.Append(aland_register);
            property.AppendChild(land_register);
            property.AppendChild(address);
            address.AppendChild(city);
            address.AppendChild(street);
            address.AppendChild(port_number);
            property.AppendChild(value);
            property.AppendChild(owners);
            properties.AppendChild(property);

            XmlDataSource3.Save();
            XmlDataSource1.DataBind();
            XmlDataSource2.DataBind();

            GridView1.ShowFooter = false;
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            GridView1.ShowFooter = false;
            // similarly you can find other controls and save

        }


    }
}