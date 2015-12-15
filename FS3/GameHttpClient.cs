using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace FS3
{
    public static class GameHttpClient
    {
        public static string Get(string partialUrl)
        {
            var url = "http://tictactoe.homedir.eu/game/" + partialUrl + "/player/2";

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/bencode+map";
            
            var responseStream = request.GetResponse().GetResponseStream();

            var reader = new StreamReader(responseStream);
            var message = reader.ReadToEnd();

            responseStream.Close();
            reader.Close();

            return message;
        }

        public static void Post(string partialUrl, string messageToSend)
        {
            var url = "http://tictactoe.homedir.eu/game/" + partialUrl + "/player/2";
            var encodedMessage = Encoding.UTF8.GetBytes(messageToSend);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/bencode+map";

            var stream = request.GetRequestStream();

            stream.Write(encodedMessage, 0, encodedMessage.Length);
            stream.Close();

            var response = request.GetResponse();
            response.Close();
        }
    }
}