
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FootballData.UserArea
{
    public partial class SubscribeTeam : System.Web.UI.Page
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    con = ConnectionDB.getConnection();

                    int id = 1;
                    try
                    {
                        id = int.Parse(Request["TeamID"]);
                    }
                    catch (Exception) { }

                    string CmdString = "football.sp_toggleSubscription";
                    SqlCommand cmd_subscribe = new SqlCommand(CmdString, con);
                    cmd_subscribe.CommandType = CommandType.StoredProcedure;
                    cmd_subscribe.Parameters.AddWithValue("@user_id", System.Web.HttpContext.Current.User.Identity.GetUserId());
                    cmd_subscribe.Parameters.AddWithValue("@team_id", id);
                    
                    try
                    {
                        con.Open();
                        cmd_subscribe.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception exc)
                    {
                        con.Close();
                    }

                    Response.Redirect(Request.UrlReferrer.ToString());
                }
            }
        }
    }
}