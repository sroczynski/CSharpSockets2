using System.Net.Sockets;
using System.Text;

string serverIp = "127.0.0.1";
int port = 8080;

try
{
    // Create a TCP/IP socket
    TcpClient client = new TcpClient();

    // Connect to the server
    client.Connect(serverIp, port);
    Console.WriteLine("Connected to server.");

    // Get the network stream from the server
    NetworkStream stream = client.GetStream();

    // Start a continuous chat loop
    while (true)
    {
        // Prompt for a client message
        Console.Write("Client: ");
        string clientMessage = Console.ReadLine();

        // Send the client message to the server
        byte[] clientMessageBytes = Encoding.ASCII.GetBytes(clientMessage);
        stream.Write(clientMessageBytes, 0, clientMessageBytes.Length);
        Console.WriteLine("Sent message: " + clientMessage);

        // Receive the response from the server
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Server: " + response);

        // Check if the server wants to end the chat
        if (response.Equals("Goodbye!", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }
    }

    // Close the connection
    client.Close();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

Console.WriteLine("Press any key to exit.");
Console.ReadKey();
