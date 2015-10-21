using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC_Trabalho2
{
    public partial class Property : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string guid = Request.QueryString["ID"];
            if (guid == null)
            {
                guid = "31";
            }
            string url = "http://acesso.ua.pt/xml/curso.asp?i=" + guid;

            XmlDataSource1.DataFile = url;
        }
        
    }
}