using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ControlServer.Services.Ads
{
    public sealed class AdsService : IAdsService, IDisposable
    {
        private readonly Uri uri;

        private readonly string adsNetId;

        private readonly int adsPort;

        private readonly HttpClient client;

        public AdsService(Uri uri, string adsNetId, int adsPort)
        {
            this.uri = uri;
            this.adsNetId = adsNetId;
            this.adsPort = adsPort;

            this.client = new HttpClient();
        }

        public async Task<bool> ReadBoolAsync(int indexGroup, int indexOffset)
        {
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace message = "http://beckhoff.org/message/";

            XDocument requestDocument = new XDocument(
                new XElement(soap + "Envelope",
                    new XElement(soap + "Body",
                        new XAttribute(soap + "encodingStyle", "http://schemas.xmlsoap.org/soap/encoding/"),
                        new XElement(message + "Read",
                            new XElement("netId", this.adsNetId),
                            new XElement("nPort", $"{this.adsPort}"),
                            new XElement("indexGroup", indexGroup),
                            new XElement("indexOffset", indexOffset),
                            new XElement("cbLen", "1")
                        )
                    )
                )
            );

            XDocument responseDocument;
            using (Stream requestStream = new MemoryStream())
            {
                requestDocument.Save(requestStream);
                requestStream.Seek(0, SeekOrigin.Begin);

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = this.uri;
                request.Content = new StreamContent(requestStream);
                request.Headers.Add("SOAPAction", "http://beckhoff.org/action/TcAdsSync.Read");
                
                HttpResponseMessage response = await client.SendAsync(request);
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    responseDocument = XDocument.Load(responseStream);
                }
            }

            string result = responseDocument.Root
                .Element(soap + "Body")
                .Element(message + "ReadResponse")
                .Element("ppData")
                .Value;

            byte[] buffer = System.Convert.FromBase64String(result);
            return buffer[0] != 0;
        }

        public async Task WriteBoolAsync(int indexGroup, int indexOffset, bool value)
        {
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace message = "http://beckhoff.org/message/";

            XDocument requestDocument = new XDocument(
                new XElement(soap + "Envelope",
                    new XElement(soap + "Body",
                        new XAttribute(soap + "encodingStyle", "http://schemas.xmlsoap.org/soap/encoding/"),
                        new XElement(message + "Write",
                            new XElement("netId", this.adsNetId),
                            new XElement("nPort", $"{this.adsPort}"),
                            new XElement("indexGroup", indexGroup),
                            new XElement("indexOffset", indexOffset),
                            new XElement("pData", Convert.ToBase64String(new byte[] { Convert.ToByte(value) }))
                        )
                    )
                )
            );

            using (Stream requestStream = new MemoryStream())
            {
                requestDocument.Save(requestStream);
                requestStream.Seek(0, SeekOrigin.Begin);

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = this.uri;
                request.Content = new StreamContent(requestStream);
                request.Headers.Add("SOAPAction", "http://beckhoff.org/action/TcAdsSync.ReadWrite");
                
                await client.SendAsync(request);
            }
        }

        public async Task<bool> ReadWriteBoolAsync(int indexGroup, int indexOffset, bool value)
        {
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace message = "http://beckhoff.org/message/";

            XDocument requestDocument = new XDocument(
                new XElement(soap + "Envelope",
                    new XElement(soap + "Body",
                        new XAttribute(soap + "encodingStyle", "http://schemas.xmlsoap.org/soap/encoding/"),
                        new XElement(message + "ReadWrite",
                            new XElement("netId", this.adsNetId),
                            new XElement("nPort", $"{this.adsPort}"),
                            new XElement("indexGroup", indexGroup),
                            new XElement("indexOffset", indexOffset),
                            new XElement("cbRdLen", "1"),
                            new XElement("pwrData", Convert.ToBase64String(new byte[] { Convert.ToByte(value) }))
                        )
                    )
                )
            );

            XDocument responseDocument;
            using (Stream requestStream = new MemoryStream())
            {
                requestDocument.Save(requestStream);
                requestStream.Seek(0, SeekOrigin.Begin);

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = this.uri;
                request.Content = new StreamContent(requestStream);
                request.Headers.Add("SOAPAction", "http://beckhoff.org/action/TcAdsSync.ReadWrite");
                
                HttpResponseMessage response = await client.SendAsync(request);
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    responseDocument = XDocument.Load(responseStream);
                }
            }

            string result = responseDocument.Root
                .Element(soap + "Body")
                .Element(message + "ReadResponse")
                .Element("ppRdData")
                .Value;

            byte[] buffer = System.Convert.FromBase64String(result);
            return buffer[0] != 0;
        }

        public void Dispose()
        {
            if (this.client != null)
            {
                this.client.Dispose();
            }
        }
    }
}
