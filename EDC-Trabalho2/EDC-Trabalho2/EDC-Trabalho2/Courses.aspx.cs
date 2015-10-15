using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC_Trabalho2
{
    public partial class Courses : System.Web.UI.Page
    {
        bool locais_filtro_ativo = false;
        String locais_filtro = "";

        bool grau_filtro_ativo = false;
        String grau_filtro = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Unnamed1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grau_filtro = "@grau='" + Tipos.SelectedValue + "'";
            this.locais_filtro = "@local='" + Locais.SelectedValue + "'";

            XmlDataSource1.XPath = "cursos/curso[" + this.grau_filtro + " and " + this.locais_filtro + "]";
        }

        protected void Locais_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grau_filtro = "@grau='" + Tipos.SelectedValue + "'";
            this.locais_filtro = "@local='" + Locais.SelectedValue + "'";

            XmlDataSource1.XPath = "cursos/curso[" + this.grau_filtro + " and " + this.locais_filtro + "]";
        }
    }
}