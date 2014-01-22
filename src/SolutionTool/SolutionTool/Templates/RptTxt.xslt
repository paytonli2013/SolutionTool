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
                  <xsl:choose>
                    <xsl:when test="../../summary = 'InspectCode' and ../../@status = 'Pass'">
                      <xsl:for-each select="msxsl:node-set(document(text()))">
                        <xsl:for-each select="Report">
                          <xsl:text>----------------------------------------------------------------------</xsl:text>
                          <br/>
                          <xsl:text>Issue Types: </xsl:text>
                          <br/>                          
                          <xsl:text>----------------------------------------------------------------------</xsl:text>
                          <br/>

                          <xsl:for-each select="IssueTypes/IssueType">
                            <xsl:value-of select="@Category"/>
                            <span>,</span>
                            <xsl:value-of select="@Description"/>
                            <br/>
                          </xsl:for-each>

                          <xsl:for-each select="Issues/Project">
                            <xsl:text>----------------------------------------------------------------------</xsl:text>
                            <br/>
                            <xsl:value-of select="@Name"/>
                            <br/>                            
                            <xsl:text>----------------------------------------------------------------------</xsl:text>
                            <br/>

                            <xsl:for-each select="Issue">
                              <xsl:value-of select="@Message"/>
                              <span>,</span>
                              <xsl:value-of select="@File"/>
                              <span>@</span>
                              <xsl:value-of select="@Line"/>
                              <span>:</span>
                              <xsl:value-of select="@Offset"/>
                              <br/>
                            </xsl:for-each>
                          </xsl:for-each>
                        </xsl:for-each>
                      </xsl:for-each>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="." />
                      <br/>
                    </xsl:otherwise>
                  </xsl:choose>
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
