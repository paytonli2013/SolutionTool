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
          .failed, .error { color: red; }
          .suggestion { color: darkgray; }
          td { border-left:1px solid black; border-top:1px solid black; padding: 4px, 8px; }
          table { width: 100%; border-right:1px solid black; border-bottom:1px solid black; }
          tr:firstChild { font-size: .3em; }
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
                              <xsl:when test="../../summary = 'InspectCode' and ../../@status = 'Pass'">
                                <!--
                                <a target="_blank">
                                  <xsl:attribute name="href">
                                    <xsl:value-of select="text()" />
                                  </xsl:attribute>
                                  <xsl:value-of select="text()" />
                                </a>
                                <xsl:variable name="inspectCodeReport">
                                  <xsl:value-of select="document(text())"/>
                                </xsl:variable>
                                -->
                                <xsl:for-each select="msxsl:node-set(document(text()))">
                                  <xsl:for-each select="Report">
                                    <h3>Issue Types: </h3>
                                    <table>
                                      <xsl:for-each select="IssueTypes/IssueType">
                                        <tr class="{@Severity}">
                                          <td>
                                            <xsl:value-of select="@Severity"/>
                                          </td>
                                          <td>
                                            <xsl:if test="string-length(@WikiUrl) != 0">
                                              <a target="_blank">
                                                <xsl:attribute name="href">
                                                  <xsl:value-of select="@WikiUrl" />
                                                </xsl:attribute>
                                                <xsl:value-of select="@Id" />
                                              </a>
                                            </xsl:if>
                                            <xsl:if test="string-length(@WikiUrl) = 0">
                                              <xsl:value-of select="@Id" />
                                            </xsl:if>
                                          </td>
                                          <td>
                                            <xsl:value-of select="@Category"/>
                                            <span>: </span>
                                            <xsl:value-of select="@Description"/>
                                          </td>
                                        </tr>
                                      </xsl:for-each>
                                    </table>

                                    <h3>Project Issues: </h3>
                                    <ul>
                                      <xsl:for-each select="Issues/Project">
                                        <li>
                                          <h3>
                                            <xsl:value-of select="@Name"/>
                                          </h3>
                                          <table>
                                            <xsl:for-each select="Issue">
                                              <tr class="error">
                                                <td>
                                                  <xsl:value-of select="@Message"/>
                                                </td>
                                                <td>
                                                  <xsl:value-of select="@File"/>
                                                  <span>@</span>
                                                  <xsl:value-of select="@Line"/>
                                                  <span>:</span>
                                                  <xsl:value-of select="@Offset"/>
                                                </td>
                                              </tr>
                                            </xsl:for-each>
                                          </table>
                                          <p/>
                                        </li>
                                      </xsl:for-each>
                                    </ul>
                                  </xsl:for-each>
                                </xsl:for-each>
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
