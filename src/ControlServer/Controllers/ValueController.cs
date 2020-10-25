using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Linq;
using System.Net.Http;
using System.Text;
using System.IO;

namespace ControlServer.Controllers
{
    [ApiController]
    [Route("api/groups/{group}/offsets/{offset}")]
    public class ValueController : ControllerBase
    {
        private readonly ILogger<ValueController> _logger;

        public ValueController(ILogger<ValueController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<int> Get(CancellationToken cancel, string group, string offset)
        {
            XmlDocument document = new XmlDocument();

            XmlElement envelope = document.CreateElement("soap:Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            envelope.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            envelope.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            XmlElement body = document.CreateElement("soap:Body", "http://schemas.xmlsoap.org/soap/envelope/");
            AddAttribute(body, "soap:encodingStyle", "http://schemas.xmlsoap.org/soap/envelope/", "http://schemas.xmlsoap.org/soap/encoding/");

            XmlElement read = document.CreateElement("q1:Read", "http://beckhoff.org/message/");
            XmlElement netId = document.CreateElement("netId");
            netId.AppendChild(document.CreateTextNode("192.168.168.11.1.1"));

            XmlElement nPort = document.CreateElement("nPort");
            nPort.AppendChild(document.CreateTextNode("801"));

            XmlElement indexGroup = document.CreateElement("indexGroup");
            indexGroup.AppendChild(document.CreateTextNode(group));

            XmlElement indexOffset = document.CreateElement("indexOffset");
            indexOffset.AppendChild(document.CreateTextNode(offset));

            XmlElement cbLen = document.CreateElement("indexOffset");
            cbLen.AppendChild(document.CreateTextNode("1"));

            document.AppendChild(envelope);
            envelope.AppendChild(body);
            body.AppendChild(read);
            read.AppendChild(netId);
            read.AppendChild(nPort);
            read.AppendChild(indexGroup);
            read.AppendChild(indexOffset);
            read.AppendChild(cbLen);

            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                document.Save(writer);
            }

            string content = builder.ToString();
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri("http://192.168.168.11/TcAdsWebService/TcAdsWebService.dll");
                request.Content = new StringContent(content);
                request.Headers.Add("SOAPAction", "http://beckhoff.org/action/TcAdsSync.Read");
                
                HttpResponseMessage response = await client.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }

            XDocument result = XDocument.Load(new StringReader(content));
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ns1 = "http://beckhoff.org/message/";
            string value = result.Root
                .Element(soap + "Body")
                .Element(ns1 + "ReadResponse")
                .Element("ppData")
                .Value;

            byte[] buffer = System.Convert.FromBase64String(value);
            return buffer[0];
        }

        [HttpPut]
        public async Task<ActionResult> Put(CancellationToken cancel, string id, string group, string offset)
        {
            // await _client.WriteValueAsync<string>(id, value, cancel);
            return this.Ok();
        }

        private static XmlElement AddAttribute(XmlElement element, string qualifiedName, string namespaceUri, string value)
        {
            XmlAttribute attribute = element.OwnerDocument.CreateAttribute(qualifiedName, namespaceUri);
            attribute.Value = value;

            element.SetAttributeNode(attribute);

            return element;
        }
    }
}
