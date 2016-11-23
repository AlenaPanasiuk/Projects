using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string buildtype;
        private string buildNumber;
        public WebClient webClient;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
           // var build = await FindBuild("AppAssure_Windows_Develop_FullBuild", "develop", "10.35.37.5");
          //  MessageBox.Show(build.ToString());

            var server = "10.35.37.5";
            var buildType = "AppAssure_Windows_Develop_FullBuild";
             var usr = new System.Uri("https://" + server + "/httpAuth/app/rest/builds?locator=status:SUCCESS,buildType:" + buildType + ",count:1");
             using (var client = new HttpClient())
             {
                 var byteArray = Encoding.ASCII.GetBytes("qa-softheme:123asdQ");
                 var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                 client.DefaultRequestHeaders.Authorization = header;
                 var str = await client.GetStringAsync(usr);

                 var stream = await client.GetStreamAsync(usr);

                 XmlDocument doc = new XmlDocument();
                 using(stream)
                 {
                     using (XmlReader reader = XmlReader.Create(str))
                     {
                         doc.Load(str);
                     }
                 }

                 //XDocument buildXML = XDocument.Load();
                 //var builds = from build in buildXML.Descendants("build")
                 //             select new
                 //                 {
                 //                     build = build.Element("number").Value
                 //                 };

//                 MessageBox.Show(builds.FirstOrDefault().build.ToString());


             }
                 
              //  MessageBox.Show(this.buildNumber = await FindBuild("AppAssure_Windows_Develop_FullBuild", "develop", "10.35.37.5"));
              // this.buildNumber = await FindBuild("AppAssure_Windows_Develop_FullBuild", "develop", "10.35.37.5");
              //  return buildNumber;
            //   return getBetween(str, branch + "-", "\"");



        }



        private async Task<string> FindBuild(string buildType, string branch, string server)
        {
            var usr = new System.Uri("https://" + server + "/httpAuth/app/rest/builds?locator=status:SUCCESS,buildType:" + buildType + ",count:1");
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes("qa-softheme:123asdQ");
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                client.DefaultRequestHeaders.Authorization = header;
                var str = await client.GetStringAsync(usr);

                var xml = await client.GetStreamAsync(usr);

                XDocument buildXML = XDocument.Load(str);
                var builds = from build in buildXML.Descendants("builds")
                             select new
                                 {
                                     build = build.Element("number").Value
                                 };


                 
                 
              //  MessageBox.Show(this.buildNumber = await FindBuild("AppAssure_Windows_Develop_FullBuild", "develop", "10.35.37.5"));
              // this.buildNumber = await FindBuild("AppAssure_Windows_Develop_FullBuild", "develop", "10.35.37.5");
              //  return buildNumber;
               return getBetween(str, branch + "-", "\"");
            }
        }



        


        [Serializable()]
        public class Build
        {
                [System.Xml.Serialization.XmlElement("id")]
                public string Id { get; set; }

                [System.Xml.Serialization.XmlElement("buildTypeId")]
                public string buildTypeId { get; set; }

                [System.Xml.Serialization.XmlElement("number")]
                public string number { get; set; }

                [System.Xml.Serialization.XmlElement("status")]
                public string status { get; set; }

                [System.Xml.Serialization.XmlElement("state")]
                public string state { get; set; }

                [System.Xml.Serialization.XmlElement("branchNumber")]
                public string branchNumber { get; set; }

                [System.Xml.Serialization.XmlElement("defaultBranch")]
                public string defaultBranch { get; set; }

                [System.Xml.Serialization.XmlElement("href")]
                public string href { get; set; }

                [System.Xml.Serialization.XmlElement("webUrl")]
                public string webUrl { get; set; }
        }

         
   
        [Serializable()]
        [System.Xml.Serialization.XmlRoot("BuildCollection")]
        public class BuildCollection
        {
            [XmlArray("Builds")]
            [XmlArrayItem("Build", typeof(Build))]
            public Build[] Build { get; set; }
        }


        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(delegate { return true; });

             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
