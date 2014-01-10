<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <html>
      <head>
        <title>Report</title>
        <style type="text/css">
          table { width: 100%; }
          td { border: 1px solid WhiteSmoke; }
          .head { background-color: WhiteSmoke; }
          .head td { font-weight: bold; }
        </style>
      </head>
      <body>
        <div class="titleBar">
          <h3>
            <xsl:value-of select="report/@project" />
          </h3>
          <p>
            <xsl:for-each select="report/items/output">
              <xsl:value-of select="." />
              <br/>
            </xsl:for-each>
          </p>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
