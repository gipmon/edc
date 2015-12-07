using FootballData.Controllers;
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
            var feed_language = Languages.userLanguage(Request);

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
                var teamNews = team.ItemArray[2];

                String CmdString4 = "SELECT * FROM football.udf_get_team(@teamID)";
                SqlCommand cmd4 = new SqlCommand(CmdString4, con);
                cmd4.Parameters.AddWithValue("@teamID", teamId);
                SqlDataAdapter sda4 = new SqlDataAdapter(cmd4);
                DataTable dt4 = new DataTable("team");
                sda4.Fill(dt4);

                var columnName = "name" + feed_language.ToUpper();
                var index = dt4.Columns.IndexOf(columnName);
                var teamName = dt4.Rows[0].ItemArray[index].ToString();

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