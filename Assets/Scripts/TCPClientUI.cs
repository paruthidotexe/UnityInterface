using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class TCPClientUI : MonoBehaviour
{
    public TMP_Text logText;
    public TMP_Text log2Text;
    TCPInterface tcpInterface = new TCPInterface();
    string logStr = "";

    string receivedStr = "";
    bool isCubeDataReceived = false;
    public GameObject cubeObj;
    int scaleVal = 2;

    void Start()
    {

    }


    void Update()
    {
        logText.text = tcpInterface.GetTcpLog();
        log2Text.text = receivedStr;

        if (isCubeDataReceived)
        {
            cubeObj.transform.localScale = Vector3.one * scaleVal;
            isCubeDataReceived = false;
        }
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
        Debug.Log("OnReceiveData:" + val);// + Convert.ToInt32(val)
        receivedStr = val;
        ParseReceivedData(val);
        //ApplyToCube(val);       
    }

    void ApplyToCube(string val)
    {
        try
        {
            scaleVal = Convert.ToInt32(val) * 2;
            if (scaleVal <= 0)
                scaleVal = 1;
            Debug.Log("----[" + scaleVal + "]------" );
            isCubeDataReceived = true;
        }
        catch (Exception e)
        {
            Debug.Log("\nExp: " + e.StackTrace);
        }
    }


    public void ParseReceivedData(string val)
    {
        string xmlStr = "<D01 NAME = \"Paruthi paruthidotexe@outlook.com\" LVL = \"1\" SSTAT = \"2\" STIME = \"17000\" S = \"0\" AS = \"0\" EP = \"1\" IBI = \"737\" ART = \"FALSE\" HR = \"0\" />";
        int startPos = val.IndexOf("S=");
        if (startPos >= 0)
        {
            string scaleValStr = val.Substring(startPos + 3, 1);
            //Debug.Log(scaleValStr);
            ApplyToCube(scaleValStr);
        }
        //parseXmlFile(val);
    }

    void parseXmlFile(string xmlData)
    {
        Debug.Log(xmlData);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);

        XmlElement xmlElement = xmlDoc.SelectSingleNode("D01") as XmlElement;

        if (xmlElement != null)
        {
            Debug.Log(xmlElement.GetAttribute("NAME"));
            Debug.Log(xmlElement.GetAttribute("LVL"));
            Debug.Log(xmlElement.GetAttribute("SSTAT"));
            Debug.Log(xmlElement.GetAttribute("STIME"));
            Debug.Log(xmlElement.GetAttribute("S"));
            Debug.Log(xmlElement.GetAttribute("AS"));
            Debug.Log(xmlElement.GetAttribute("EP"));
            Debug.Log(xmlElement.GetAttribute("IBI"));
            Debug.Log(xmlElement.GetAttribute("ART"));
            Debug.Log(xmlElement.GetAttribute("HR"));

            ApplyToCube(xmlElement.GetAttribute("S"));
        }
    }

}

public class TCPData_Breath
{
    public string NAME;
    public string LVL;
    public string SSTAT;
    public string STIME;
    public string S;
    public string AS;
    public string EP;
    public string IBI;
    public string ART;
    public string HR;

    public TCPData_Breath()
    {

    }

}
