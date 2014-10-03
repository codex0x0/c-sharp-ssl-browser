﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using GZipIO;


//additional namespaces for SSL
using System.Security.Authentication;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace lib_http
{
    class HTTP
    {
        private String m_LastPage;
        private String m_Cookies;
        private String m_Buffer;
        private SortedList colCookies;
        private bool UseGZ;
        private TcpClient client;

        /// <summary>
        /// The constructor for the HTTP class.
        /// </summary>
        /// <param name="GZipEnabled">Specifies whether GZIP encoded data should be requested.</param>
        public HTTP(bool GZipEnabled)
            : base()
        {
            UseGZ = GZipEnabled;
            colCookies = new SortedList();
            client = new TcpClient();
            m_LastPage = "";
            m_Buffer = "";
            m_Cookies = "";
        }

        /// <summary>
        /// LastPage variable. Initializes as blank
        /// </summary>
        public String LastPage
        {
            get { return this.m_LastPage; }
            set { this.m_LastPage = value; }
        }

        /// <summary>
        /// Current cookies being used. Initializes as blank.
        /// </summary>
        public String Cookies
        {
            get { return this.m_Cookies; }
            set { this.m_Cookies = value; }
        }

        /// <summary>
        /// The current text in the response buffer.
        /// </summary>
        public String Buffer
        {
            get { return this.m_Buffer; }
        }

        /// <summary>
        /// Indicates whether GZip encoded content should be requested.
        /// </summary>
        public bool IsGZIPEnabled
        {
            get { return UseGZ; }
            set { UseGZ = value; }
        }

        /// <summary>
        /// Parses out the cookie values of the new header and updates them with the current cookies.
        /// </summary>
        /// <param name="NewHeader">The headers to parse the new cookie values from</param>
        /// <param name="OldCookies">The current cookies being sent to the host</param>
        /// <returns></returns>
        public static String ParseCookies(String NewHeader, String OldCookies)
        {
            String strFinal = String.Empty;
            Regex rx = new Regex("Set-Cookie:\\s*(.+?)=(.+?);", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (rx.IsMatch(NewHeader) == false) 
            {
                return "";
            }

            MatchCollection colMatches = rx.Matches(NewHeader);
            
            foreach (Match curMatch in colMatches)
            {
                String strTemp = curMatch.Groups[1].ToString() + "=" + curMatch.Groups[2].ToString();
                if (OldCookies.Contains(strTemp) == false)
                {
                    strFinal = strFinal + strTemp + ";";
                }
            }

            if (OldCookies.EndsWith(";") == false) { strFinal = OldCookies + ";" + strFinal; }
            else { strFinal = OldCookies + strFinal; }

            if (strFinal.StartsWith(";") == true) { strFinal = strFinal.Substring(1, strFinal.Length - 1); }
            return strFinal;
        }

        /// <summary>
        /// Saves the current cookie into an array of cookies.
        /// </summary>
        /// <param name="Key">The key to identify the cookie in the array.</param>
        public void SaveCookie(String Key)
        {
            if (colCookies.ContainsKey(Key) == false)
            {
                colCookies.Add(Key, m_Cookies);
            }
        }

        /// <summary>
        /// Sets the value of the current cookie to that of the cookie loaded from the array.
        /// </summary>
        /// <param name="Key">The key to identify the wanted cookie.</param>
        public void LoadCookies(String Key)
        {
            if (colCookies.ContainsKey(Key) == false)
            {
                int i = colCookies.IndexOfKey(Key);
                m_Cookies = colCookies.GetByIndex(i).ToString();
            }
        }

        /// <summary>
        /// Clears the array of all the cookies.
        /// </summary>
        public void ClearCookies()
        {
            colCookies.Clear();
        }

        /// <summary>
        /// Removes the wanted cookie from the array.
        /// </summary>
        /// <param name="Key">The key of cookie you want to remove.</param>
        public void ClearCookies(String Key)
        {
            int i = colCookies.IndexOfKey(Key);
            colCookies.RemoveAt(i);
        }

        private String GetHeaderValue(String Header, String Headers)
        {
            Regex rx = new Regex(Header + ":\\s*(.*?)\\s\\s", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (rx.IsMatch(Headers) == false) { return ""; }
            MatchCollection colMatches = rx.Matches(Headers);
            return colMatches[0].Groups[1].ToString();
        }

        /// <summary>
        /// Decompresses a string that is encoded in GZip format.
        /// </summary>
        /// <param name="Digest">The string to be decompressed.</param>
        /// <returns></returns>
        public String DecompressString(String Digest)
        {
            //Save the text into a .gz file
            FileStream fOutStream = new FileStream("Compressed.gz", FileMode.Create, FileAccess.ReadWrite);
            Byte[] strData = System.Text.Encoding.ASCII.GetBytes(Digest);
            fOutStream.Write(strData, 0, strData.Length);
            fOutStream.Close();

            //Decompress the file we created
            GZIP gz = new GZIP();
            gz.Decompress("Compressed.gz");

            //Open the decompressed file and return the content
            FileStream fInStream = new FileStream("Compressed.gz", FileMode.Open, FileAccess.Read);
            StreamReader s = new StreamReader(fInStream);
            return s.ReadToEnd();
        }


        public string DecryptSSL(SslStream sslStream)
        { 
            int bytes = -1;
            byte [] buffer = new byte[8192];
            StringBuilder sslData = new StringBuilder();
             

                  bytes = sslStream.Read(buffer, 0, buffer.Length); 
                  Decoder decoder = Encoding.UTF8.GetDecoder();
                    char[] chars = new char[decoder.GetCharCount(buffer,0,bytes)];
                      decoder.GetChars(buffer, 0, bytes, chars,0);
                    sslData.Append(chars);
                    MessageBox.Show(sslData.ToString());


                  return sslData.ToString();


       }

             

                   
                  


        /// <summary>
        /// Uses the HTTP GET method to request a remote file on a host.
        /// </summary>
        /// <param name="Host">The host to send the request to.</param>
        /// <param name="File">The file to request.</param>
        /// <param name="Referer">The referer to the request.</param>
        /// <returns>The source of the request HTML file.</returns>
        public String Get(String Host, String File, String Referer, String UserAgent)
        {

            String strRequest = "GET " + File + " HTTP/1.1\r\n"
            + "Host: " + Host + "\r\n"
            + "User-Agent: " + UserAgent
            + "Accept: text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/ *;q=0.5\r\n"
            + "Accept-Language: en-us,en;q=0.5\r\n"
            + "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\r\n"
            + (UseGZ == true ? "Accept-Encoding: gzip, deflate\r\n" : "")
            + "Referer: " + Referer + "\r\n"
            + "Cookie: " + m_Cookies + "\r\n"
            + "Connection: close" + "\r\n\r\n";
            if (UseGZ == false) { strRequest.Replace("Accept-Encoding: gzip, deflate\r\n", ""); }

            client = new TcpClient(Host, 80);

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(strRequest);
            NetworkStream nStream = client.GetStream();

            nStream.Write(data, 0, data.Length);
            StreamReader lStreamReader = new StreamReader(nStream);

            String strHTML = lStreamReader.ReadToEnd();

            if (File.Contains("http://") == false) { m_LastPage = "http://" + Host + File; }
            else { m_LastPage = File; }

            m_Buffer = strHTML;
            String[] strParts = Regex.Split(strHTML, ".*?\\s\\s\\s\\s.*?", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            if (GetHeaderValue("Content-Encoding", strParts[0]) == "gzip")
            {
                strHTML = strParts[0] + "\r\n\r\n" + DecompressString(strParts[0]);
            }

            m_Cookies = ParseCookies(strParts[0], m_Cookies);



            return strHTML;
        }



        public String sslGet(String Host, String File, String Referer, String UserAgent)
        {







            String strRequest = "GET " + File + " HTTP/1.1\r\n"
           + "Host: " + Host + "\r\n"
           + "User-Agent: " + UserAgent
           + "Accept: text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/ *;q=0.5\r\n"
           + "Accept-Language: en-us,en;q=0.5\r\n"
           + "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\r\n"
           + (UseGZ == true ? "Accept-Encoding: gzip, deflate\r\n" : "")
           + "Referer: " + Referer + "\r\n"
           + "Cookie: " + m_Cookies + "\r\n"
           + "Connection: close" + "\r\n\r\n";
            if (UseGZ == false) { strRequest.Replace("Accept-Encoding: gzip, deflate\r\n", ""); }



            Byte[] data = System.Text.Encoding.ASCII.GetBytes(strRequest.ToString());
            //NetworkStream nStream = client.GetStream();
            client = new TcpClient(Host, 443);
            //Wrap it up in SSL got this from: http://stackoverflow.com/questions/10948317/construct-get-request-for-sslstream
            SslStream sslStream = new SslStream(client.GetStream(), false, null, null);
            sslStream.AuthenticateAsClient(Host);

            //Before sending the sslStream Write we need to convert strRequest to a byte stream
            sslStream.WriteTimeout = 1000;
            sslStream.Write(data, 0, data.Length);

            //Now we need to convert it back to a normal stream so we can write it out as a normal HTML Page
            sslStream.Flush();
            sslStream.ReadByte();

            String strHTML = DecryptSSL(sslStream);

            if (File.Contains("http://") == false) { m_LastPage = "http://" + Host + File; }
            else { m_LastPage = File; }

            m_Buffer = strHTML;
            String[] strParts = Regex.Split(strHTML, ".*?\\s\\s\\s\\s.*?", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            if (GetHeaderValue("Content-Encoding", strParts[0]) == "gzip")
            {
                strHTML = strParts[0] + "\r\n\r\n" + DecompressString(strParts[0]);
            }

            m_Cookies = ParseCookies(strParts[0], m_Cookies);
            //debug
            //MessageBox.Show(Convert.ToString(strHTML.Length));
            return strHTML;




        }


  public String sslPost(String Host, String File, String PostData,String Referer, String UserAgent)
  {





      String strRequest = "POST " + File + " HTTP/1.1\r\n"
      + "Host: " + Host + "\r\n"
      + "User-Agent: " + UserAgent
      + "Accept: text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/ *;q=0.5\r\n"
      + "Accept-Language: en-us,en;q=0.5\r\n"
      + "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\r\n"
      + (UseGZ == true ? "Accept-Encoding: gzip, deflate\r\n" : "")
      + "Referer: " + Referer + "\r\n"
      + "Cookie: " + m_Cookies + "\r\n"
      + "Content-Type: application/x-www-form-urlencoded\r\n"
      + "Content-Length: " + PostData.Length.ToString() + "\r\n"
      + "Connection: close\r\n\r\n"
      + PostData + "\r\n";


      client = new TcpClient(Host, 443);
      Byte[] data = System.Text.Encoding.ASCII.GetBytes(strRequest);
      NetworkStream nStream = client.GetStream();

      //Wrap it up in SSL got this from: http://stackoverflow.com/questions/10948317/construct-get-request-for-sslstream
      SslStream sslStream = new SslStream(nStream);
      sslStream.AuthenticateAsClient(Host);
      //Before sending the sslStream Write we need to convert strRequest to a byte stream
      sslStream.Write(data, 0, data.Length);
      //Now we need to convert it back to a normal stream so we can write it out as a normal HTML Page
      sslStream.Flush();
       
     String strHTML = DecryptSSL(sslStream);

      if (File.Contains("https://") == false) { m_LastPage = "https://" + Host + File; }
      else { m_LastPage = File; }

      m_Buffer = strHTML;
      String[] strParts = Regex.Split(strHTML, ".*?\\s\\s\\s\\s.*?", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
      m_Cookies = ParseCookies(strParts[0], m_Cookies);

      return strHTML;
 }

        


        /// <summary>
        /// Uses the HTTP POST method to request a remote file on a host.
        /// </summary>
        /// <param name="Host">The host that the request is being made to</param>
        /// <param name="File">The file on the server that is being requested</param>
        /// <param name="PostData">The data to send to the host along with the request</param>
        /// <param name="Referer">The referer to the request</param>
        /// <returns>The response from the server in the form of a String.</returns>
        public String Post(String Host, String File, String PostData, String Referer, String UserAgent)
        {


          
            String strRequest = "POST " + File + " HTTP/1.1\r\n"
            + "Host: " + Host + "\r\n"
            + "User-Agent: " + UserAgent
            + "Accept: text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/ *;q=0.5\r\n"
            + "Accept-Language: en-us,en;q=0.5\r\n"
            + "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\r\n"
            + (UseGZ == true ? "Accept-Encoding: gzip, deflate\r\n" : "")
            + "Referer: " + Referer + "\r\n"
            + "Cookie: " + m_Cookies + "\r\n"
            + "Content-Type: application/x-www-form-urlencoded\r\n"
            + "Content-Length: " + PostData.Length.ToString() + "\r\n"
            + "Connection: close\r\n\r\n"
            + PostData + "\r\n";


            client = new TcpClient(Host, 80);
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(strRequest);
            NetworkStream nStream = client.GetStream();

            nStream.Write(data, 0, data.Length);
            StreamReader lStreamReader = new StreamReader(nStream);

            String strHTML = lStreamReader.ReadToEnd();
           
            if (File.Contains("http://") == false) { m_LastPage = "http://" + Host + File; }
            else { m_LastPage = File; }

            m_Buffer = strHTML;
            String[] strParts = Regex.Split(strHTML, ".*?\\s\\s\\s\\s.*?", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            m_Cookies = ParseCookies(strParts[0], m_Cookies);

            return strHTML;
        }

        /// <summary>
        /// Returns the current data in the response buffer.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_Buffer;
        }
    }
}