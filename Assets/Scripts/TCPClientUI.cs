﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TCPClientUI : MonoBehaviour
{
    public TMP_Text logText;
    public TMP_Text log2Text;
    TCPInterface tcpInterface = new TCPInterface();
    string logStr = "";

    public GameObject cubeObj;
    int scaleVal = 2;

    void Start()
    {

    }


    void Update()
    {
        log2Text.text = tcpInterface.GetReceiveData();
        logText.text = tcpInterface.GetTcpLog();

        cubeObj.transform.localScale = Vector3.one * scaleVal;
    }


    void OnEnable()
    {
        TCPInterface.receiveDataEvent += OnReceiveData;
    }


    void OnDisable()
    {
        tcpInterface.OnDisable();
        TCPInterface.receiveDataEvent -= OnReceiveData;
    }


    public void OnInit()
    {
        tcpInterface.OnInit();
    }


    public void OnDisconnect()
    {
        tcpInterface.OnDisconnect();
    }


    public void OnReceiveData(string val)
    {
        //logStr += val;
        //logText.text = logStr;
        Debug.Log("OnReceiveData : [" + val);//"]= " + Convert.ToInt32(val)
        scaleVal = Convert.ToInt32(val);
        if (scaleVal <= 2)
            scaleVal = 2;
        Debug.Log("]= " + scaleVal);
        
    }

}
