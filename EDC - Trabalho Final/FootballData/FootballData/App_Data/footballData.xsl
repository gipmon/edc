<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="rss">
    <rss>
      <xsl:for-each select="channel">
        <channel>
          <xsl:attribute name="title">
            <xsl:value-of select="title"/>
          </xsl:attribute>
          <xsl:attribute name="link">
            <xsl:value-of select="link"/>
          </xsl:attribute>
          <xsl:for-each select="item">
            <xsl:sort data-type="text" order="descending" select="pubDate"/>
            <item>
              <xsl:attribute name="title">
                <xsl:value-of select="title"/>
              </xsl:attribute>
              <xsl:attribute name="link">
                <xsl:value-of select="link"/>
              </xsl:attribute>
              <xsl:attribute name="team">
                <xsl:value-of select="team"/>
              </xsl:attribute>
              <xsl:attribute name="teamId">
                <xsl:value-of select="teamId"/>
              </xsl:attribute>
            </item>
          </xsl:for-each>
        </channel>
      </xsl:for-each>
    </rss>
  </xsl:template>
</xsl:stylesheet>
