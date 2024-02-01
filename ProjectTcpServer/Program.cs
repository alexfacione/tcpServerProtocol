using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// <title> TCP SERVER</title>
/// Author: Alexandre de Oliveira Facione
/// Description: TCP Server developed in .NET 6
/// </summary>

const int port = 3124;

TcpListener server = new TcpListener(IPAddress.Any, port);

server.Start();
Console.WriteLine($"SERVER: TCP Server started on Port {port}\n");

try
{
    while (true)
    {
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Client Conected!");

        Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
        clientThread.Start(client);
    }
}
finally
{
    server.Stop();
}

void HandleClient(object obj)
{
    TcpClient tcpClient = (TcpClient)obj;
    NetworkStream clientStream = tcpClient.GetStream();

    byte[] messageBuffer = new byte[4096];
    int bytesRead;

    try
    {
        while ((bytesRead = clientStream.Read(messageBuffer, 0, messageBuffer.Length)) > 0)
        {
            var message = Encoding.UTF8.GetString(messageBuffer, 0, bytesRead);
            Console.WriteLine($"\nMessage received from Client: { message }");

            string responseMessage = $"Message received \"{ message }\"";

            // Send message to client
            byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
            clientStream.Write(responseBuffer, 0, responseBuffer.Length);
            Console.WriteLine($"Sent to Client: {responseMessage}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    finally
    {
        tcpClient.Close();
        Console.WriteLine("\nClient Desconected.\n");
    }
}