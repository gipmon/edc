<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="rss">
    <rss>
      <xsl:for-each select="channel">
        <channel>
          <xsl:attribute name="link">
            <xsl:value-of select="link"/>
          </xsl:attribute>
          <xsl:for-each select="item">
            <item>
              <xsl:attribute name="title">
                <xsl:value-of select="title"/>
              </xsl:attribute>
              <xsl:attribute name="description">
                <xsl:value-of select="description"/>
              </xsl:attribute>
              <xsl:attribute name="link">
                <xsl:value-of select="link"/>
              </xsl:attribute>
              <xsl:attribute name="isPermalink">
                <xsl:value-of select="guid/@isPermaLink"/>
              </xsl:attribute>
              <xsl:attribute name="guid">
                <xsl:value-of select="guid"/>
              </xsl:attribute>
              <xsl:attribute name="pubDate">
                <xsl:value-of select="pubDate"/>
              </xsl:attribute>
            </item>
          </xsl:for-each>
        </channel>
      </xsl:for-each>
    </rss>
  </xsl:template>
</xsl:stylesheet>
