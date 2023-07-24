using System.Net.Sockets;
using System.Net;
using System.Text;

string ipAddress = "127.0.0.1";
int port = 8080;

TcpListener listener = new TcpListener(IPAddress.Parse(ipAddress), port);

try
{
    listener.Start();
    Console.WriteLine("Server started. Waiting for clients...");

    TcpClient client = listener.AcceptTcpClient();
    Console.WriteLine("Client connected.");

    // Get the network stream from the client
    NetworkStream stream = client.GetStream();

    // Start a continuous chat loop
    while (true)
    {
        // Receive the message from the client
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Client: " + message);

        // Check if the client wants to end the chat
        if (message.Equals("bye", StringComparison.OrdinalIgnoreCase))
        {
            // Send a goodbye message and close the connection
            string response = "Goodbye!";
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);
            Console.WriteLine("Sent response: " + response);
            break;
        }

        // Prompt for a server message
        Console.Write("Server: ");
        string serverMessage = Console.ReadLine();

        // Send the server message to the client
        byte[] serverMessageBytes = Encoding.ASCII.GetBytes(serverMessage);
        stream.Write(serverMessageBytes, 0, serverMessageBytes.Length);
        Console.WriteLine("Sent message: " + serverMessage);
    }

    client.Close();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
finally
{
    listener.Stop();
}

Console.WriteLine("Server stopped. Press any key to exit.");
Console.ReadKey();