using inRiver.Remoting;
using inRiver.Remoting.Extension;
using inRiver.Remoting.Objects;
using ResourceImport;
using ServerExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionTester
{
    class ExtensionTester
    {
        static void Main(string[] args)
        {
            //TestCustomCompleteness();
            
            //TestBasicWorkFlow();

            //TestResourceImporter();
            //UploadToResourceImporter();

            TestChangeSender();

            Console.ReadKey();
        }


        static void TestCustomCompleteness()
        {
            //966 - EntityId
            //8d02e36a0204461594b3d47d11148cb0 - SpecificationId


        }

        static void TestBasicWorkFlow()
        {

        }


        static void TestChangeSender()
        {
            RemoteManager manager = RemoteManager.CreateInstance(
    "https://demo.remoting.productmarketingcloud.com",
    "academy62@inriver.com",
    "inRiverBest4Ever!"
    );
            ChangeSender.ChangeSenderEntityListener changeSenderEntityListener = new ChangeSender.ChangeSenderEntityListener();
            changeSenderEntityListener.Context = new inRiverContext(manager, new ConsoleLogger());

            changeSenderEntityListener.EntityCreated(102);
        }


        static void TestResourceImporter()
        {

        }

        static async void UploadToResourceImporter()
        {
            string filename = "B005002_front_portrait.jpg";
            byte[] fileContent = File.ReadAllBytes(@"C:\temp\" + filename);

            string extensionApiKey = "nokey";

            string customerSafename = "academyXXX";
            string environmentSafename = "test";
            string controllerName = "inboundfile";
            string extensionId = "{YourExtensionId}";

            string inboundEndpoint = $"https://demo.inbound.productmarketingcloud.com/api/{controllerName}/{customerSafename}/{environmentSafename}/{extensionId}";

            ByteArrayContent byteContent = new ByteArrayContent(fileContent);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Console.WriteLine("Sending " + fileContent.Length);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(inboundEndpoint);

                var byteArray = Encoding.ASCII.GetBytes("apikey:" + extensionApiKey);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(byteContent, filename, filename);

                HttpResponseMessage response = await httpClient.PostAsync(inboundEndpoint, multiContent);

                ////If the response contains content we want to read it!
                if (response.Content != null)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine("Response: " + responseContent);
                }
                else
                {
                    Console.WriteLine("No content");
                }
            }
        }

    }
}
