using System;
using System.Net.Http;

namespace ET.Client
{
    [EntitySystemOf(typeof(UploadFileAddressComponent))]
    [FriendOf(typeof(UploadFileAddressComponent))]
    public static partial class UploadFileAddressComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UploadFileAddressComponent self, string address, int port)
        {
            self.UploadFileHost = address;
            self.UploadFilePort = port;
        }
        
        private static async ETTask UploadFile(this UploadFileAddressComponent self, byte[] bytes, string folderName, string fileName)
        {
            string url = $"http://{self.UploadFileHost}:{self.UploadFilePort}/upload?v={RandomGenerator.RandUInt32()}";
            Log.Debug($"upload file start: {url}");
            string s = await UploadFileAsync(bytes, folderName, fileName, url);
            Log.Debug($"upload file finish: {s}");
        }

        private static async ETTask<string> UploadFileAsync(byte[] bytes, string folderName, string fileName, string link)
        {
            try
            {
                using HttpClient httpClient = new();
                using MultipartFormDataContent formData = new();

                ByteArrayContent fileContent = new(bytes);
                formData.Add(fileContent, "file", fileName);
                formData.Add(new StringContent("folderName"), folderName);

                HttpResponseMessage response = await httpClient.PostAsync(link, formData);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                return $"http request fail: {link.Substring(0,link.IndexOf('?'))}\n{e}";
            }
        }
    }
}