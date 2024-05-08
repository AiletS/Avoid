using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class UDPClient : MonoBehaviour
{
    // broadcast address (受信側サーバーのアドレス)
    public string host = "157.7.201.122";
    public int port = 28372;
    private UdpClient client;

    void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);

        byte[] dgram = Encoding.UTF8.GetBytes("hello!");
        client.Send(dgram, dgram.Length);
    }

    void Update()
    {
    }

    void OnApplicationQuit()
    {
        client.Close();
    }
}
