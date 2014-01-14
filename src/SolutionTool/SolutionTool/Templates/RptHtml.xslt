<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <html>
      <head>
        <style type="text/css">
          .bold { font-weight: bold; }
          .pass { color: green; }
          .warning { color: darkorange ; }
          .failed { color: red; }
          td { border-left:1px solid black; border-top:1px solid black; padding: 4px, 8px; }
          table { border-right:1px solid black; border-bottom:1px solid black; }
        </style>
        <title>
          <xsl:text>Report -</xsl:text>
          <xsl:value-of select="report/@project" />
          <xsl:text> - </xsl:text>
          <xsl:value-of select="report/@createTime" />
        </title>
      </head>
      <body>
        <div>
          <h1>
            Report - <xsl:value-of select="report/@project" /> - <xsl:value-of select="report/@createTime" />
          </h1>
          <div>
            <xsl:for-each select="report/items/item">
              <xsl:if test="violations">
                <table>
                  <tr>
                    <th>Rule</th>
                    <th>Description</th>
                  </tr>
                  <xsl:for-each select="violations">
                    <tr>
                      <td>
                        <xsl:value-of select="@rulename" />
                      </td>
                      <td>
                        <xsl:value-of select="@description" />
                      </td>
                    </tr>
                  </xsl:for-each>
                </table>
              </xsl:if>
              <xsl:if test="outputs">
                <ul>
                  <xsl:for-each select="outputs/output">
                    <li class="{@status}">
                      <span class="bold">
                        <xsl:value-of select="summary" />
                      </span>
                      <ul>
                        <xsl:for-each select="details/detail">
                          <li>
                            <xsl:choose>
                              <xsl:when test="../../summary = 'InspectCode'">
                                <a target="_blank">
                                  <xsl:attribute name="href">
                                    <xsl:value-of select="text()" />
                                  </xsl:attribute>
                                  <xsl:value-of select="text()" />
                                </a>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="." />
                              </xsl:otherwise>
                            </xsl:choose>
                          </li>
                        </xsl:for-each>
                      </ul>
                    </li>
                  </xsl:for-each>
                </ul>
              </xsl:if>
            </xsl:for-each>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
