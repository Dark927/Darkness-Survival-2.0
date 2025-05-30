﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Runtime.CompilerServices;

namespace Dark.Utils
{
    public static class Telemetry
    {
        private static string serverIp = "127.0.0.1";
        private static int serverPort = 5005;
        private static UdpClient udpClient = new UdpClient();

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(params object[] data)
        {
            try
            {
                string message = string.Join(",", data);
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                udpClient.Send(bytes, bytes.Length, serverIp, serverPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Telemetry Error: {ex.Message}");
            }
        }
    }
}
