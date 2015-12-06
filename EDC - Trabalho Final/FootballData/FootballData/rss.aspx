<?xml version="1.0" encoding="UTF-8"?>
<%@ Page Language="C#" ContentType="text/xml" AutoEventWireup="true" CodeBehind="rss.aspx.cs" Inherits="FootballData.rss" %>
<asp:Repeater ID="RepeaterRSS" runat="server">
    <HeaderTemplate>
    <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
    targetNamespace="http://www.w3schools.com/schema">
      <xs:include schemaLocation="<%# url %>App_Data/rss.xsd"/>
        <rss version="2.0">
            <channel>
                <title><%# RemoveIllegalCharacters(rssChannel.title) %></title>
                <link><%# RemoveIllegalCharacters(rssChannel.link) %></link>
                <language><%# RemoveIllegalCharacters(rssChannel.language) %></language>
    </HeaderTemplate>
    <ItemTemplate>
        <% if(rssChannel.format=="full") { %>
            <item>
                <title><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "title")) %></title>
                <link><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "link")) %></link>
                <pubDate><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "pubDate")) %></pubDate>
                <description><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "description")) %></description>
                <team><%# RemoveIllegalCharacters((object)teamsNames[(int)DataBinder.Eval(Container.DataItem, "team_id")]) %></team>
                <teamId><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "team_id")) %></teamId>
                <asp:Repeater ID="related" datasource='<%# ((TeamNew)Container.DataItem).related %>' runat="server">
                <ItemTemplate>
                <related>
                    <title><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "title")) %></title>
                    <link><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "link")) %></link>
                </related>
                </ItemTemplate>
                </asp:Repeater>
            </item>
        <% }else { %>
            <item>
                <title><%#  RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "title")) %></title>
                <link><%#  RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "link")) %></link>
                <team><%# RemoveIllegalCharacters((object)teamsNames[(int)DataBinder.Eval(Container.DataItem, "team_id")]) %></team>
                <teamId><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "team_id")) %></teamId>
                <pubDate><%# RemoveIllegalCharacters(((DateTime)DataBinder.Eval(Container.DataItem, "pubDate")).ToString("r")) %></pubDate>
            </item>
            <asp:Repeater ID="Repeater1" datasource='<%# ((TeamNew)Container.DataItem).related %>' runat="server">
            <ItemTemplate>
            <item>
                <title><%#  RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "title")) %></title>
                <link><%#  RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "link")) %></link>
                <team><%# RemoveIllegalCharacters((object)teamsNames[(int)DataBinder.Eval(Container.DataItem, "team_id")]) %></team>
                <teamId><%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "team_id")) %></teamId>
                <pubDate><%# RemoveIllegalCharacters(((DateTime)DataBinder.Eval(Container.DataItem, "pubDate")).ToString("r")) %></pubDate>
            </item>
            </ItemTemplate>
            </asp:Repeater>
        <% } %>
    </ItemTemplate>
    <FooterTemplate>
            </channel>
        </rss>
</xs:schema>
    </FooterTemplate>
</asp:Repeater>