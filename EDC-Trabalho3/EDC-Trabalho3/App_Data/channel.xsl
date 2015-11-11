<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="rss">
    <rss>
      <xsl:for-each select="property">
        <xsl:sort select="land_register" data-type="number"/>
        <property>
          <xsl:attribute name="land_register">
            <xsl:value-of select="land_register" />
          </xsl:attribute>
          <xsl:for-each select="address">
            <xsl:attribute name="city">
              <xsl:value-of select="city"/>
            </xsl:attribute>
            <xsl:attribute name="street">
              <xsl:value-of select="street"/>
            </xsl:attribute>
            <xsl:attribute name="port_number">
              <xsl:value-of select="port_number"/>
            </xsl:attribute>
          </xsl:for-each>
          <xsl:attribute name="value">
            <xsl:value-of select="value"/>
          </xsl:attribute>
          <xsl:for-each select="owners">
            <owners>
              <xsl:for-each select="owner">
              <owner>
                <xsl:attribute name="tax_number">
                  <xsl:value-of select="@tax_number"/>
                </xsl:attribute>
              </owner>
            </xsl:for-each>
            </owners>
          </xsl:for-each>
        </property>
      </xsl:for-each>
    </properties>
  </xsl:template>
</xsl:stylesheet>
