using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TCPClientUI : MonoBehaviour
{
    public TMP_Text logText;
    TCPInterface tcpInterface = new TCPInterface();

    void Start()
    {
        
    }

    void Update()
    {
        logText.text = tcpInterface.GetTcpLog();
        Debug.Log(tcpInterface.GetTcpLog());
    }

    public void OnInit()
    {
        tcpInterface.OnInit();
    }

    public void OnDisconnect()
    {
        tcpInterface.OnDisconnect();
    }

    void OnDisable()
    {
        tcpInterface.OnDisable();
    }
}
