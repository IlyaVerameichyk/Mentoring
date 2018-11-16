<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:cat="http://library.by/catalog" xml:space="default">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <rss version="2.0">
        <xsl:apply-templates />
    </rss>
  </xsl:template>
  <xsl:template match="cat:catalog">
    <channel>
      <xsl:apply-templates />
    </channel>
  </xsl:template> 
  <xsl:template match="cat:book">
    <item>
      <title>
        <xsl:apply-templates select="cat:title" />
      </title>
      <description>
        <xsl:apply-templates select="cat:description"></xsl:apply-templates>
      </description>
      <xsl:if test="cat:genre = 'Computer' and cat:isbn != '' ">
        <link>
          <xsl:copy-of select="'http://my.safaribooksonline.com/'" />
          <xsl:apply-templates select="cat:isbn"/>
        </link>
      </xsl:if>
    </item>
  </xsl:template>
</xsl:stylesheet>
