using System.Net;
using System.Net.Sockets;
using System.Text;        

// Define the broadcast address and port
var broadcastAddress = "255.255.255.255";
var broadcastPort = 9293;

// Create a UDP client
using (var udpClient = new UdpClient())
{
    udpClient.EnableBroadcast = true;

    try
    {
        // The message to send
        var lastOctet = GetLastOctetOfIPAddress();
        var message = $"n.n.n.{lastOctet}:Hello!";
        var sendBytes = Encoding.ASCII.GetBytes(message);

        // Send the message
        udpClient.Send(sendBytes, sendBytes.Length, new IPEndPoint(IPAddress.Parse(broadcastAddress), broadcastPort));
        Console.WriteLine("Message sent to the broadcast address");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

string GetLastOctetOfIPAddress()
{
    string hostName = Dns.GetHostName();

    IPAddress[] addresses = Dns.GetHostAddresses(hostName);

    foreach (IPAddress address in addresses)
    {
        if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            string ipAddress = address.ToString();
            string[] octets = ipAddress.Split('.');
            return octets[octets.Length - 1];
        }
    }

    throw new InvalidOperationException("No IPv4 address found.");
}