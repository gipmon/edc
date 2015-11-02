using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace EDC_Trabalho2
{
    public partial class owners : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string land_register = Request.QueryString["ID"];
            if (land_register == null)
            {
                land_register = "1";
            }

            XmlDataSource1.DataFile = "~/App_Data/properties.xml";
            XmlDataSource1.XPath = "/properties/property[land_register="+ land_register + "]/owners/owner";
        }

        protected void ownersItemUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            TextBox tax = (TextBox)row.FindControl("TextBox2");
            string land_register = Request.QueryString["ID"];
            XmlDocument xdoc = XmlDataSource1.GetXmlDocument();

            XmlElement owner = xdoc.SelectSingleNode("properties/property[@land_register='" + land_register + "']/owners/owner[@tax_number='"+ tax.Text + "']" ) as XmlElement;
            owner.Attributes["name"].Value = e.NewValues["name"].ToString();
            owner.Attributes["tax_number"].Value = e.NewValues["tax_number"].ToString();
            owner.Attributes["date_purchase"].Value = e.NewValues["date_purchase"].ToString();
            if (e.NewValues["data_sale"] != null)
            {
                owner.Attributes["data_sale"].Value = e.NewValues["data_sale"].ToString();
            }
            else
            {
                owner.Attributes["data_sale"].Value = "";
            }

            XmlDataSource1.Save();
            XmlDataSource1.DataBind();

            e.Cancel = true;
            GridView1.EditIndex = -1;
        }

        protected void ownersItemDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            Label tax = (Label) row.Cells[1].Controls[1];
            string land_register = Request.QueryString["ID"];
            XmlDocument xdoc = XmlDataSource1.GetXmlDocument();

            XmlElement owners = xdoc.SelectSingleNode("properties/property[@land_register='" + land_register + "']/owners") as XmlElement;
            XmlElement owner = xdoc.SelectSingleNode("properties/property[@land_register='" + land_register + "']/owners/owner[@tax_number = '" + tax.Text + "']") as XmlElement;
            owners.RemoveChild(owner);

            XmlDataSource1.Save();

            e.Cancel = true;

            GridView1.DataBind();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GridView1.ShowFooter = true;
            GridView1.DataSource = null;
            GridView1.DataBind();

        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
          
            XmlDocument xdoc = XmlDataSource1.GetXmlDocument();
            string land_register = Request.QueryString["ID"];

            XmlElement owners = xdoc.SelectSingleNode("properties/property[@land_register='" + land_register + "']/owners") as XmlElement;
            XmlElement owner = xdoc.CreateElement("owner");
            XmlAttribute name = xdoc.CreateAttribute("name");
            XmlAttribute tax_number = xdoc.CreateAttribute("tax_number");
            XmlAttribute date_purchase = xdoc.CreateAttribute("date_purchase");
            XmlAttribute data_sale = xdoc.CreateAttribute("data_sale");

            name.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txtname")).Text;
            tax_number.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txttax")).Text;
            date_purchase.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txtpurchase")).Text;
            data_sale.InnerText = ((TextBox)GridView1.FooterRow.FindControl("txtsale")).Text;

            owners.AppendChild(owner);
            owner.Attributes.Append(name);
            owner.Attributes.Append(tax_number);
            owner.Attributes.Append(date_purchase);
            owner.Attributes.Append(data_sale);

            XmlDataSource1.Save();
            XmlDataSource1.DataBind();

            GridView1.ShowFooter = false;
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            GridView1.ShowFooter = false;
            // similarly you can find other controls and save

        }
    }
}