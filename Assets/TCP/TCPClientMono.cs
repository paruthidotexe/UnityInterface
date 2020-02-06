using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


public class TCPClientMono : MonoBehaviour
{
    public static TCPClientMono instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 3434;
    public int myId = 0;
    public TCP tcp;

    private delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        tcp = new TCP();
    }

    public void ConnectToServer()
    {
        InitClientData();
        tcp.Connect();
    }


    void Update()
    {

    }

    public class TCP
    {
        public TcpClient tcpClientSocket;
        NetworkStream tcpStream;
        Packet receivedPacket;
        byte[] receiveBuffer;

        public TCP()
        {
        }

        public void Connect()
        {
            try
            {
                tcpClientSocket = new TcpClient
                {
                    SendBufferSize = dataBufferSize,
                    ReceiveBufferSize = dataBufferSize
                };

                //tcpStream = tcpClientSocket.GetStream();
                receiveBuffer = new byte[dataBufferSize];

                tcpClientSocket.BeginConnect(instance.ip, instance.port, ConnectCallback, tcpClientSocket);
            }
            catch(Exception exp)
            {
                Debug.Log("exp : " + exp);
            }

        }

        void ConnectCallback(IAsyncResult asyncResult)
        {
            tcpClientSocket.EndConnect(asyncResult);

            if(!tcpClientSocket.Connected)
            {
                return;
            }
            tcpStream = tcpClientSocket.GetStream();

            receivedPacket = new Packet();

            tcpStream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

        }

        void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                int byteLen = tcpStream.EndRead(asyncResult);
                if (byteLen <= 0)
                {
                    // todo : disconnect
                    return;
                }

                byte[] data = new byte[byteLen];
                Array.Copy(receiveBuffer, data, byteLen);

                receivedPacket.Reset(HandleData(data));

                tcpStream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Exp : ReceiveCallback : {exp}");
            }
        }


        private bool HandleData(byte[] data)
        {
            int packetLength = 0;
            receivedPacket.SetBytes(data);

            if(receivedPacket.UnreadLength() >= 4)
            {
                packetLength = receivedPacket.ReadInt();
                if(packetLength <= 0)
                {
                    return true;
                }
            }

            while(packetLength > 0 && packetLength <= receivedPacket.UnreadLength())
            {
                byte[] packetBytes = receivedPacket.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        packetHandlers[packetId](packet);
                    }
                });

                packetLength = 0;
                if (receivedPacket.UnreadLength() >= 4)
                {
                    packetLength = receivedPacket.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (packetLength <= 1)
            {
                return true;
            }

            return false;
        }
    }


    private void InitClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ServerPackets.welcome, ClientHandle.Welcome }
        };
        Debug.Log("Initialized Packets");
    }
}
