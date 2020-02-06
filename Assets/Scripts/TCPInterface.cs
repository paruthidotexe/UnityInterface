using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;
using System.Text;
using System.Threading;


public class TCPInterface
{
    string serverUrl = "127.0.0.1";
    int serverPort = 3333;
    int localPort = 3333;
    int HMPort = 20480;

    TcpClient tcpClient;
    string message = "dummy string";

    string logStr = "LOG:\n";
    Thread tcpThread;
    bool isThreadRunning = false;
    string logFileName = "TCPLog.txt";
    StreamWriter logStreamWriter;


    public TCPInterface()
    {
    }


    public void OnInit()
    {
        logStreamWriter = new StreamWriter(Application.persistentDataPath + "/" + logFileName);
        serverPort = localPort;
        isThreadRunning = true;
        ThreadStart threadStart = new ThreadStart(OnConnectToServer);
        tcpThread = new Thread(threadStart);
        tcpThread.Start();
        tcpThread.IsBackground = true;
        logStr += "-- Thread done --";
        logStreamWriter.Write(logStr);
    }


    public void OnDisable()
    {
        logStreamWriter.Write("------------------------ x EOF x ------------------------");
        logStreamWriter.Close();
        tcpClient.Close();
        tcpThread.Abort();
    }


    //IEnumerator DelayedCall()
    //{
    //    try
    //    {
    //        tcpClient = new TcpClient();
    //        logText.text += "\n new TcpClient";
    //        tcpClient.Connect(serverUrl, serverPort);
    //        logText.text += "\nConnect";
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Error..... " + e.StackTrace);
    //        logText.text += "\n new TcpClient error";
    //    }
    //    yield return new WaitForSeconds(1.0f);
    //    //StartCoroutine(OnConnectToServer());
    //}


    void OnConnectToServer()
    {
        try
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(serverUrl, serverPort);
            logStr += "-- Connect -- ";
        }
        catch (Exception e)
        {
            logStr += "-- Exp : " + e.StackTrace;
        }

        NetworkStream networkStream = tcpClient.GetStream();
        for (int j = 0; j < 10; j++)
        {
            string fullStr = "";
            //yield return new WaitForSeconds(0.1f);
            fullStr += "\n[" + j + "] -> ";
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                //Byte[] data = Encoding.ASCII.GetBytes(message);
                // Send the message to the connected TcpServer. 
                //networkStream.Write(data, 0, data.Length);

                byte[] byteBuffer = new byte[2048];
                int bufferLength = networkStream.Read(byteBuffer, 0, 2048);
                //Debug.Log(byteBuffer);
                for (int i = 0; i < bufferLength; i++)
                    fullStr += Convert.ToChar(byteBuffer[i]);
                this.ReceivedData(fullStr);
            }
            catch (Exception e)
            {
                logStr += "-- Exp : " + e.StackTrace;
            }
            //Thread.Sleep(2000);
        }
        networkStream.Close();
        tcpClient.Close();
    }


    void ReceivedData(string receivedStr)
    {
        logStr += receivedStr;
        logStreamWriter.Write(receivedStr);
    }

    public string GetTcpLog()
    {
        return logStr;
    }
}


