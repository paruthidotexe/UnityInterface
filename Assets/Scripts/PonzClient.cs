using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


public class PonzClient 
{
    public static int dataBufferSize = 4096;
    public int id;
    public TCP tcp;

    public PonzClient(int clientId)
    {
        id = clientId;
        tcp = new TCP(id);
    }

    public class TCP
    {
        public TcpClient tcpClientSocket;
        readonly int id;
        NetworkStream tcpStream;
        byte[] receiveBuffer;

        public TCP(int newId)
        {
            id = newId;
        }

        public void Connect(TcpClient newSocket)
        {
            tcpClientSocket = newSocket;
            tcpClientSocket.SendBufferSize = dataBufferSize;
            tcpClientSocket.ReceiveBufferSize = dataBufferSize;

            tcpStream = tcpClientSocket.GetStream();
            receiveBuffer = new byte[dataBufferSize];

            tcpStream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

        }

        void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                int byteLen = tcpStream.EndRead(asyncResult);
                if(byteLen <= 0)
                {
                    // todo : disconnect
                    return;
                }

                byte[] data = new byte[byteLen];
                Array.Copy(receiveBuffer, data, byteLen);

                // todo: handle data
                tcpStream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Exp : ReceiveCallback : {exp}");
            }
        }


    }

}
