﻿using HttpMultipartParser;
using System.Net;
using System.Text;

namespace MultiServer.HTTPService.Addons.PlayStationHome.UFC
{
    public class UFCClass
    {
        public static Task ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url == null)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return Task.CompletedTask;
            }

            switch (request.HttpMethod)
            {
                case "POST":
                    switch (request.Url.AbsolutePath)
                    {
                        case "/index.php":
                            string boundary = Extensions.ExtractBoundary(request.ContentType);

                            // Get the input stream from the context
                            Stream inputStream = request.InputStream;

                            // Create a memory stream to copy the content
                            using (MemoryStream copyStream = new MemoryStream())
                            {
                                // Copy the input stream to the memory stream
                                inputStream.CopyTo(copyStream);

                                // Reset the position of the copy stream to the beginning
                                copyStream.Position = 0;

                                var data = MultipartFormDataParser.Parse(copyStream, boundary);

                                string val2 = "";

                                string func = data.GetParameterValue("func");

                                string id = data.GetParameterValue("id");

                                // Reset the position of the copy stream to the beginning
                                copyStream.Position = 0;

                                byte[] ticketData = null;

                                foreach (var file in data.Files)
                                {
                                    Stream filedata = file.Data;

                                    filedata.Position = 0;

                                    // Find the number of bytes in the stream
                                    int contentLength = (int)filedata.Length;

                                    // Create a byte array
                                    byte[] buffer = new byte[contentLength];

                                    // Read the contents of the memory stream into the byte array
                                    filedata.Read(buffer, 0, contentLength);

                                    if (file.FileName == "ticket.bin")
                                        ticketData = buffer;

                                    filedata.Dispose();
                                }

                                if (ticketData == null)
                                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                                else
                                {
                                    try
                                    {
                                        val2 = data.GetParameterValue("val2");
                                    }
                                    catch (Exception)
                                    {
                                        // Sometimes this data is not here, so we catch.
                                    }

                                    // Extract the desired portion of the binary data
                                    byte[] extractedData = new byte[0x63 - 0x54 + 1];

                                    // Copy it
                                    Array.Copy(ticketData, 0x54, extractedData, 0, extractedData.Length);

                                    // Convert 0x00 bytes to 0x20 so we pad as space.
                                    for (int i = 0; i < extractedData.Length; i++)
                                    {
                                        if (extractedData[i] == 0x00)
                                            extractedData[i] = 0x20;
                                    }

                                    // Convert the modified data to a string
                                    string psnname = Encoding.ASCII.GetString(extractedData).Replace(" ", "");

                                    if (id == psnname)
                                    {
                                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"{ServerConfiguration.HTTPStaticFolder}HOME_THQ/{id}/");

                                        if (!File.Exists(Directory.GetCurrentDirectory() + $"/loginformNtemplates/HOME_THQ/{id}/data.xml"))
                                        {
                                            File.WriteAllText(Directory.GetCurrentDirectory() + $"{ServerConfiguration.HTTPStaticFolder}HOME_THQ/{id}/data.xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                                "<UFC>1</UFC><tokens>100000</tokens><books><book value=\"1\" /><set01 value=\"1\"><card001 value=\"1\" /><fb01 value=\"Card one picked up!\" /></set01><set02 value=\"1\"><card001 value=\"2\" /><fb01 value=\"Card two picked up!\" /></set02></books>");
                                        }
                                        else if (File.Exists(Directory.GetCurrentDirectory() + $"{ServerConfiguration.HTTPStaticFolder}HOME_THQ/{id}/data.xml") && func == "write" && val2 != null)
                                        {
                                            File.WriteAllText(Directory.GetCurrentDirectory() + $"{ServerConfiguration.HTTPStaticFolder}HOME_THQ/{id}/data.xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                                $"<UFC>1</UFC><tokens>{val2}</tokens><books><book value=\"1\" /><set01 value=\"1\"><card001 value=\"1\" /><fb01 value=\"Card one picked up!\" /></set01><set02 value=\"1\"><card001 value=\"2\" /><fb01 value=\"Card two picked up!\" /></set02></books>");
                                        }

                                        byte[] responsetooutput = File.ReadAllBytes(Directory.GetCurrentDirectory() + $"{ServerConfiguration.HTTPStaticFolder}HOME_THQ/{id}/data.xml");

                                        response.Headers.Set("Content-Type", "application/xml;charset=UTF-8");
                                        response.StatusCode = (int)HttpStatusCode.OK;
                                        response.ContentLength64 = responsetooutput.Length;

                                        if (response.OutputStream.CanWrite)
                                        {
                                            try
                                            {
                                                response.OutputStream.Write(responsetooutput, 0, responsetooutput.Length);
                                                response.OutputStream.Close();
                                            }
                                            catch (Exception)
                                            {
                                                // Not Important.
                                            }
                                        }
                                    }
                                    else
                                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                                }

                                copyStream.Dispose();
                            }
                            break;
                        default:
                            response.StatusCode = (int)HttpStatusCode.Forbidden;
                            break;
                    }
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
