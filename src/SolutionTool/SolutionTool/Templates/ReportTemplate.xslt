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
        <table>
          <tr class="head">
            <td>Index</td>
            <td>Project</td>
            <td>Rules</td>
            <td>Start</td>
            <td>End</td>
            <td>Result</td>
            <td>Summary</td>
          </tr>
          <xsl:for-each select="//run">
            <xsl:sort select="@project" order="ascending"/>
            <xsl:variable name="ix" select="position() - 1"/>
            <tr>
              <td>
                <xsl:value-of select="$ix" />
              </td>
              <td>
                <xsl:value-of select="@project" />
              </td>
              <td>
                <xsl:value-of select="@rules" />
              </td>
              <td>
                <xsl:value-of select="@start" />
              </td>
              <td>
                <xsl:value-of select="@end" />
              </td>
              <td>
                <xsl:value-of select="@result" />
              </td>
              <td>
                <xsl:value-of select="@summary" />
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
