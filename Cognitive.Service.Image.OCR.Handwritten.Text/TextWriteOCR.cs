using Cognitive.Service.Image.OCR.Handwritten.Text.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace Cognitive.Service.Image.OCR.Handwritten.Text
{
    public class TextWriteOCR
    {
        public async Task<string> ReadHandwrittenText(string key, string imagePath, string endPoint = "https://westus.api.cognitive.microsoft.com/vision/v1.0/recognizeText")
        {
            var errors = new List<string>();
            ImageInfo responeData = new ImageInfo();
            string resultString = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                string requestParameters = "mode=Handwritten";
                string uri = endPoint + "?" + requestParameters;
                HttpResponseMessage response;
                string operationLocation;
                byte[] data = ImaByte(imagePath);
                using (ByteArrayContent byteArrayContent = new ByteArrayContent(data))
                {
                    byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await httpClient.PostAsync(uri, byteArrayContent);
                }
                if (response.IsSuccessStatusCode) operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
                else
                {
                    string errorList = await response.Content.ReadAsStringAsync();
                    return errorList;
                }
                string responseResult;
                int compter = 0;
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    response = await httpClient.GetAsync(operationLocation);
                    responseResult = await response.Content.ReadAsStringAsync();
                    ++compter;
                }
                while (compter < 10 && responseResult.IndexOf("\"status\":\"Succeeded\"") == -1);
                if (compter == 10 && responseResult.IndexOf("\"status\":\"Succeeded\"") == -1)
                {
                    Console.WriteLine("\nTimeout error.\n");
                    return "Error";
                }
                if (response.IsSuccessStatusCode)
                {
                    responeData = JsonConvert.DeserializeObject<ImageInfo>(responseResult, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs earg)
                        {
                            errors.Add(earg.ErrorContext.Member.ToString());
                            earg.ErrorContext.Handled = true;
                        }
                    });
                    var linesCount = responeData.recognitionResult.lines.Count;
                    for (int j = 0; j < linesCount; j++)
                    {
                        var imageText = responeData.recognitionResult.lines[j].text;
                        resultString += imageText + Environment.NewLine;
                    }
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
            return resultString;
        }
        public byte[] ImaByte(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
