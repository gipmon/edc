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
    public partial class TeamSubscribed : System.Web.UI.Page
    {
        protected string teamsSubscribed;
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            con = ConnectionDB.getConnection();

            teamsSubscribed = "";

            String CmdString = "SELECT * FROM football.udf_get_teams_subscribed(@userId)";
            SqlCommand cmd = new SqlCommand(CmdString, con);
            cmd.Parameters.AddWithValue("@userId", System.Web.HttpContext.Current.User.Identity.GetUserId());
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("teams");
            sda.Fill(dt);
            
            foreach (DataRow team in dt.Rows)
            {
                var teamId = team.ItemArray[0];
                var teamName = team.ItemArray[1];
                var teamNews = team.ItemArray[2];

                teamsSubscribed += "<tr><td>"+ teamId + "</td>";
                teamsSubscribed += "<td><a href=\"/Team?ID="+ teamId + "\">" + teamName + "</a></td>";
                teamsSubscribed += "<td><a href=\"/Team?ID="+ teamId + "\">" + teamNews + "</a></td>";
                teamsSubscribed += "<td class=\"text-center\"><a href=\"/UserArea/SubscribeTeam.aspx?TeamID=" + teamId + "\" class=\"btn btn-danger btn-xs\"><span class=\"glyphicon glyphicon-remove\"></span> Unsubscribe</a></td>";
                teamsSubscribed += "</tr>";
            }

            Page.DataBind();

        }
    }
}