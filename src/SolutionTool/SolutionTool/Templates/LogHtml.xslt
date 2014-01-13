<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/">
    <html>
      <head>
        <style type="text/css">
          .bold { font-weight: bold; }
          .pass { color: green; }
          .failed { color: red; }
          td { border-left:1px solid black; border-top:1px solid black; padding: 4px, 8px; }
          table { border-right:1px solid black; border-bottom:1px solid black; }
          iframe { width: 100%; border: 1px solid red; }
        </style>
        <title>
          <xsl:text>Recent Run</xsl:text>
        </title>
      </head>
      <body>
        <div>
          <h1>List of Recent Runs</h1>
          <div>
            <table cellpadding="0" cellspacing="0">
              <tr>
                <td>Project</td>
                <td>Start</td>
                <td>End</td>
                <td>Summary</td>
                <td>Status</td>
                <td>Report</td>
              </tr>
              <xsl:for-each select="runlog/run">
                <tr class="{@status}">
                  <td>
                    <xsl:value-of select="@project" />
                  </td>
                  <td>
                    <xsl:value-of select="@start" />
                  </td>
                  <td>
                    <xsl:value-of select="@end" />
                  </td>
                  <td>
                    <xsl:value-of select="@summary" />
                  </td>
                  <td>
                    <xsl:value-of select="@status" />
                  </td>
                  <td>
                    <a href="{@report}" targe="_blank">
                      <xsl:value-of select="@report" />
                    </a>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
