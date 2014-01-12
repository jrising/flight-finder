/******************************************************************************\
 * WebUtilities - tools for working with web pages and forms
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  Virsona, Inc.
 * Written by James Rising
 * 
 * This file is part of FFlight, which is free software: you can redistribute
 * it and/or modify it under the terms of the GNU General Public License as 
 * published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 * 
 * FFlight is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
 * details (license.txt).
 * 
 * You should have received a copy of the GNU General Public License along with
 * FFlight.  If not, see <http://www.gnu.org/licenses/>.
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using MIL.Html;
using System.Web;
using System.Threading;

namespace ffly
{
    public class WebUtilities
    {
        public static string GetPage(string url, ref CookieContainer cookies)
        {
            string result = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
			request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
           	//request.Method = "GET";

            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }

            if (res.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream());
                    result = reader.ReadToEnd();

                    // grab the cookies
                    foreach (Cookie cookie in res.Cookies)
                        cookies.Add(cookie);
                }
                catch (Exception ex)
                {
                    return "ERROR: " + ex.Message;
                }
            }
            else
                result = "ERROR: " + res.StatusDescription;

            res.Close();

            return result;
        }

        public static string AddUrlParam(string url, string key, string value)
        {
            if (url.Contains("?"))
                return url + "&" + key + "=" + HttpUtility.UrlEncode(value);
            else
                return url + "?" + key + "=" + HttpUtility.UrlEncode(value);
        }

        public static string PostForm(string url, Dictionary<string, string> form, ref CookieContainer cookies)
        {
            string result = "";

            StringBuilder query = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in form)
            {
                query.Append(kvp.Key);
                query.Append("=");
                query.Append(HttpUtility.UrlEncode(kvp.Value));
                query.Append("&");
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(query.ToString());

            // Send query to website
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentLength = byteArray.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookies;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                HttpWebResponse res = (HttpWebResponse)request.GetResponse();

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream());
                    result = reader.ReadToEnd();

                    // grab the cookies
                    foreach (Cookie cookie in res.Cookies)
                        cookies.Add(cookie);
                }
                else
                    result = "ERROR: " + res.StatusDescription;

                res.Close();
            }
            catch (WebException ex)
            {
                return "ERROR: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }

            return result;
        }

        public static HtmlNodeCollection ParseHtml(string html)
        {
			if (html == null)
				return null;
			MIL.Html.HtmlDocument document = MIL.Html.HtmlDocument.Create(html, true);
            return document.Nodes;
        }
		
        // return all name, value pairs in a form
        public static Dictionary<string, string> FindForm(HtmlNodeCollection nodes, string name, out Dictionary<string, string> submits, out string action)
        {
            HtmlElement form = FindOne(nodes, "form", "name", name);
            if (form == null)
            {
				// try by class
				form = FindOne(nodes, "form", "class", name);
				if (form == null) {
	                submits = null;
    	            action = null;
        	        return null;    // failed!
				}
            }
            action = form.Attributes["action"].Value;
            
            List<HtmlElement> inputs = FindAll(form.Nodes, "input", null, null);

            // fill out dictionary
            Dictionary<string, string> values = new Dictionary<string, string>();
            submits = new Dictionary<string, string>();
            foreach (HtmlElement input in inputs)
            {
                HtmlAttribute attrname = input.Attributes.FindByName("name");
                if (attrname == null)
                    continue;
                HtmlAttribute attrvalue = input.Attributes.FindByName("value");
                if (attrvalue == null)
                    attrvalue = new HtmlAttribute("value", "");
                string attrtype = input.Attributes.FindByName("type").Value.ToLower();

                if (attrtype == "submit" || attrtype == "button" || attrtype == "image")
                    submits[attrname.Value] = attrvalue.Value;
                else
                    values[attrname.Value] = attrvalue.Value;
            }

            return values;
        }

        // Look for a given node with a given attribute
        public static HtmlElement FindOne(HtmlNodeCollection nodes, string name, string attrname, string attrvalue)
        {
            foreach (HtmlNode node in nodes)
            {
                if (node is HtmlElement)
                {
                    HtmlElement elt = (HtmlElement)node;
                    // is this a match?
                    if (elt.Name == name)
                    {
                        if (attrname == null)
                            return elt;

                        HtmlAttribute attr = elt.Attributes.FindByName(attrname);
                        if (attr != null && attr.Value == attrvalue)
                            return elt;
                    }

                    // look down the branch
                    HtmlElement result = FindOne(elt.Nodes, name, attrname, attrvalue);
                    if (result != null)
                        return result;
                }
            }

            // not found in this branch
            return null;
        }

        // Look for all nodes of a given type
        public static List<HtmlElement> FindAll(HtmlNodeCollection nodes, string name, string attrname, string attrvalue)
        {
            List<HtmlElement> elts = new List<HtmlElement>();

            foreach (HtmlNode node in nodes)
            {
                if (node is HtmlElement)
                {
                    HtmlElement elt = (HtmlElement)node;
                    // is this a match?
                    if (elt.Name == name)
                    {
                        if (attrname == null)
                            elts.Add(elt);
                        else
                        {
                            HtmlAttribute attr = elt.Attributes.FindByName(attrname);
                            if (attr != null && attr.Value == attrvalue)
                                elts.Add(elt);
                        }
                    }

                    // look down the branch
                    elts.AddRange(FindAll(elt.Nodes, name, attrname, attrvalue));
                }
            }

            // return all found
            return elts;
        }

        public static string ToText(HtmlNodeCollection nodes)
        {
            StringBuilder result = new StringBuilder();

            foreach (HtmlNode node in nodes)
            {
                if (node is HtmlElement)
                {
                    HtmlElement elt = (HtmlElement)node;
                    string text = ToText(elt.Nodes);
                    if (text.StartsWith(" ") && result.ToString().EndsWith(" "))
                        text = text.Substring(1);

                    result.Append(text);

                    string name = elt.Name.ToLower();
                    if (name == "br" || name == "div")
                        result.Append("\n");
                }
                else
                {
                    string text = node.ToString();
                    text = text.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
                    string next = text.Replace("  ", " ");
                    while (text != next)
                    {
                        text = next;
                        next = next.Replace("  ", " ");
                    }

                    if (text.StartsWith(" ") && result.ToString().EndsWith(" "))
                        text = text.Substring(1);

                    result.Append(text);
                }
            }

            return result.ToString();
        }
    }
}
