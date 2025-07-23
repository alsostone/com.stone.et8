using System;
using System.IO;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.UploadFile, "/upload_file")]
    public class HttpUploadFileHandler : IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            try
            {
                if (context.Request.HttpMethod != "POST")
                {
                    SendResponse(context.Response, 405, "Method Not Allowed");
                    return;
                }
                
                if (!context.Request.ContentType.StartsWith("multipart/form-data"))
                {
                    SendResponse(context.Response, 400, "Invalid Content-Type");
                    return;
                }

                string boundary = GetBoundary(context.Request.ContentType);
                if (string.IsNullOrEmpty(boundary))
                {
                    SendResponse(context.Response, 400, "Missing boundary");
                    return;
                }
                
                await SaveFilesFromMultipart(context.Request.InputStream, boundary);
                SendResponse(context.Response, 200, "File(s) uploaded successfully");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await using StreamWriter writer = new(context.Response.OutputStream);
                await writer.WriteAsync($"Error: {ex.Message}");
            }
            finally
            {
                context.Response.Close();
            }
        }
        
        private string GetBoundary(string contentType)
        {
            const string boundaryPrefix = "boundary=";
            int index = contentType.IndexOf(boundaryPrefix, StringComparison.Ordinal);
            return index == -1 ? null : contentType.Substring(index + boundaryPrefix.Length).Trim('"');
        }

        private async ETTask SaveFilesFromMultipart(Stream stream, string boundary)
        {
            // 准备边界字节数组
            byte[] boundaryBytes = Encoding.UTF8.GetBytes($"--{boundary}");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes($"--{boundary}--");
            byte[] newLineBytes = Encoding.UTF8.GetBytes("\r\n\r\n");
            
            using (MemoryStream ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                byte[] requestBytes = ms.ToArray();
                
                int currentPosition = 0;
                
                // 1. 查找第一个边界
                int boundaryStart = IndexOf(requestBytes, boundaryBytes, currentPosition);
                if (boundaryStart == -1) return;
                
                // 2. 遍历所有部分
                while (true)
                {
                    // 移动到边界之后
                    currentPosition = boundaryStart + boundaryBytes.Length;
                    
                    // 3. 查找头部结束位置 (空行)
                    int headersEnd = IndexOf(requestBytes, newLineBytes, currentPosition);
                    if (headersEnd == -1) break;
                    
                    // 4. 提取头部
                    int headersLength = headersEnd - currentPosition;
                    string headers = Encoding.UTF8.GetString(requestBytes, currentPosition, headersLength);
                    
                    // 5. 移动到内容开始位置
                    currentPosition = headersEnd + newLineBytes.Length;
                    
                    // 6. 查找下一个边界
                    int nextBoundary = IndexOf(requestBytes, boundaryBytes, currentPosition);
                    if (nextBoundary == -1)
                    {
                        // 尝试查找结束边界
                        nextBoundary = IndexOf(requestBytes, endBoundaryBytes, currentPosition);
                        if (nextBoundary == -1) break;
                    }
                    
                    // 7. 计算内容长度 (减去结尾的 \r\n)
                    int contentLength = nextBoundary - currentPosition - 2;
                    if (contentLength < 0) contentLength = 0;
                    
                    // 8. 提取内容
                    byte[] content = new byte[contentLength];
                    Buffer.BlockCopy(requestBytes, currentPosition, content, 0, contentLength);
                    
                    // 9. 解析头部信息
                    string fileName = ExtractValue(headers, "filename");
                    
                    // 10. 保存数据
                    string full = Path.Combine("../Logs", fileName);
                    string path = Path.GetDirectoryName(full);
                    if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    await File.WriteAllBytesAsync(full, content);
                    
                    // 移动到下一个边界
                    currentPosition = nextBoundary;
                    boundaryStart = nextBoundary;
                }
            }
        }
        
        // 在字节数组中查找子数组的位置
        private int IndexOf(byte[] array, byte[] pattern, int startIndex)
        {
            if (pattern.Length == 0 || array.Length == 0 || startIndex >= array.Length)
                return -1;
            
            for (int i = startIndex; i <= array.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (array[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return i;
            }
            return -1;
        }

        // 从头部字符串中提取值
        private string ExtractValue(string headers, string key)
        {
            string search = $"{key}=\"";
            int start = headers.IndexOf(search, StringComparison.Ordinal);
            if (start == -1) return null;
        
            start += search.Length;
            int end = headers.IndexOf('"', start);
            return end > start ? headers.Substring(start, end - start) : null;
        }
        
        private void SendResponse(HttpListenerResponse response, int statusCode, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            response.StatusCode = statusCode;
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}