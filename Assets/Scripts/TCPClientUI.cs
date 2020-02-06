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
        tcpInterface.OnInit();
    }

    void Update()
    {
        logText.text = tcpInterface.GetTcpLog();
        Debug.Log(tcpInterface.GetTcpLog());
    }

    void OnDisable()
    {
        tcpInterface.OnDisable();
    }
}
