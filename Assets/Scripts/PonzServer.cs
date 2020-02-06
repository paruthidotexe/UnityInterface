using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


public class PonzServer
{
    private static TcpListener tcpListener;
    public static string serverUrl = "127.0.0.1";
    public static int serverPort = 3333;
    int localPort = 3333;
    int HMPort = 20480;
    public static int maxPlayers = 5;
    public static Dictionary<int, PonzClient> clientDict = new Dictionary<int, PonzClient>();


    public PonzServer()
    {   
    }


    void OnStartServer()
    {
        serverPort = localPort;
    }


    public static void OnStartServer(int newMaxPlayers, int portNo)
    {
        maxPlayers = newMaxPlayers;
        serverPort = portNo;

        Console.WriteLine($"Starting Server");

        InitServerData();

        tcpListener = new TcpListener(IPAddress.Any, serverPort);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);

        Console.WriteLine($"Server started at {serverPort}");
    }


    public static void TcpConnectionCallback(IAsyncResult asyncResult)
    {
        TcpClient tcpClient = tcpListener.EndAcceptTcpClient(asyncResult);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);

        Console.WriteLine($"INcoming Connection from client : {tcpClient.Client.RemoteEndPoint}"); // RemoteEndPoint = meeans?
        for (int i = 1; i <= maxPlayers; i++)
        {
            if(clientDict[i].tcp.tcpClientSocket  == null)
            {
                return;
            }
        }
        Console.WriteLine($"Server is FULL");
    }


    private static void InitServerData()
    {
        for(int i = 1; i <= maxPlayers; i++)
        {
            clientDict.Add(i, new PonzClient(i));
        }
    }


}

