<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:x="http://library.by/catalog"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:cs="urn:cs"
                exclude-result-prefixes="x">

  <xsl:output method="html" indent="yes" />
  <msxsl:script language="C#" implements-prefix="cs">
    <![CDATA[
      public string datenow()
     {
        return(DateTime.Now.ToString());
     }
     ]]>
  </msxsl:script>
  <xsl:param name ="Title" select="''"/>

  <xsl:key name="book-by-genre" match="x:book" use="x:genre" />

  <xsl:template match="@* | node()">
    <xsl:element name="body">
      <i>
        <xsl:value-of select="cs:datenow()"/>
      </i>
      <h2>
        <xsl:value-of select="$Title"/>
      </h2>
      <br/>
      <xsl:variable name="books-total" select="count(x:book)"/>
      <xsl:variable name="books-genre" select="x:book[generate-id() = generate-id(key('book-by-genre', x:genre)[1])]"/>
      <xsl:for-each select="$books-genre">
        <h3>
          <xsl:value-of select="x:genre" />
        </h3>
        <table border="1" cellspacing="0">
          <tr>
            <th>Author</th>
            <th>Title</th>
            <th>Publication date</th>
            <th>Description</th>
          </tr>
          <xsl:variable name="books-genre-items" select="key('book-by-genre', x:genre)"/>
          <xsl:for-each select="$books-genre-items">
            <tr>
              <td>
                <xsl:value-of select="x:author" />
              </td>
              <td>
                <xsl:value-of select="x:title" />
              </td>
              <td>
                <xsl:value-of select="x:publish_date" />
              </td>
              <td>
                <xsl:value-of select="x:description" />
              </td>
            </tr>
          </xsl:for-each>
          <p>
            Total: <xsl:value-of select="count($books-genre-items)"/>
          </p>
        </table>
      </xsl:for-each>
      <p>
        <b>
          Total books: <xsl:value-of select="$books-total"/>
        </b>
      </p>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>