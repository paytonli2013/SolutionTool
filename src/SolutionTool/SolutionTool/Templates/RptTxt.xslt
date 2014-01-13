<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/">
    <html>
      <head>
        <title>
          <xsl:text>Report -</xsl:text>
          <xsl:value-of select="report/@project" />
          <xsl:text> - </xsl:text>
          <xsl:value-of select="report/@createTime" />
        </title>        
      </head>
      <body>
        <code>
          <xsl:text>Report -</xsl:text>
          <xsl:value-of select="report/@project" />
          <xsl:text> - </xsl:text>
          <xsl:value-of select="report/@createTime" />
          <br/>
          <xsl:text>----------------------------------------------------------------------</xsl:text>
          <br/>
          <xsl:for-each select="report/items/item">
            <xsl:if test="violations">
              <xsl:for-each select="violations">
                <xsl:value-of select="@rulename" />
                <xsl:text>, </xsl:text>
                <xsl:value-of select="@description" />
                <br/>
              </xsl:for-each>
            </xsl:if>
            <xsl:if test="outputs">
              <xsl:for-each select="outputs/output">
                <xsl:value-of select="summary" />
                <br/>
                <xsl:for-each select="details/detail">
                  <xsl:value-of select="." />
                  <br/>
                </xsl:for-each>
                <xsl:text>----------------------------------------------------------------------</xsl:text>
                <br/>
              </xsl:for-each>
            </xsl:if>
          </xsl:for-each>
        </code>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
