using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BreathData
{
    public int id;
    public int breath;
}

public class TotalBreath
{
    public int totalBreathCount;
}


public class RESTInterface : MonoBehaviour
{
    public GameObject graphObj;
    public GameObject graphBarPrefab;

    float nextUpdateTime = 4.0f;
    float curTime = 0;
    int totalBreath = 0;
    int lastBarIndex = 0;

    string hostName = "localhost";

    public TMPro.TMP_InputField ipAddress;
    public TMPro.TMP_Text logText;
    string logString = "";

    public Image serverStatusImg;
    bool serverStatus = false;

    void Start()
    {
        //OnGetTotalBreath();
    }


    void Update()
    {
        logText.text = logString;
        curTime += Time.deltaTime;
        if(curTime > nextUpdateTime && lastBarIndex < totalBreath)
        {
            Debug.Log(lastBarIndex + " - " + totalBreath);
            curTime = 0;
            OnGetServerStatus();
            OnGetBarData();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //OnOpenPage();
        }
    }

    public void OnDataReceived()
    {
        int barVal = Random.Range(0, 3);
        CreateNewBar(barVal);
    }
    
    void CreateNewBar(int barVal)
    {
        //Debug.Log("NewBar : " + barVal);
        GameObject curObj = Instantiate(graphBarPrefab);
        curObj.transform.parent = graphObj.transform;
        curObj.name = "Bar_" + lastBarIndex;
        BarGraphData barGraphData = curObj.GetComponentInChildren<BarGraphData>();
        barGraphData.Init(barVal);
        curObj.transform.position = Vector3.right * lastBarIndex * 1.25f;
    }


    void OnGetBarData()
    {
        if (ipAddress.text != null && ipAddress.text.Length > 0)
            hostName = ipAddress.text;
        StartCoroutine(GetBarData("http://" + hostName + ":3333/nextBreath"));
    }


    IEnumerator GetBarData(string uri)
    {
        Debug.Log(uri);
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();

        // Show results as text
        // Debug.Log(request.downloadHandler.text);

        if (request.isNetworkError)
        {
            logString += "isNetworkError";
            Debug.Log("isNetworkError");
        }
        else if (request.isHttpError)
        {
            logString += "isHttpError";
            Debug.Log("isHttpError");
        }
        else
        {
            logString += "----" + request.downloadHandler.text + "----";

            BreathData breathData = JsonUtility.FromJson<BreathData>(request.downloadHandler.text);

            CreateNewBar(breathData.breath);
            lastBarIndex++;
        }
    }


    public void OnGetTotalBreath()
    {
        if (ipAddress.text != null && ipAddress.text.Length > 0)
            hostName = ipAddress.text;
        StartCoroutine(GetTotalBreath("http://" + hostName + ":3333/totalBreath"));
    }


    IEnumerator GetTotalBreath(string uri)
    {
        Debug.Log(uri);
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();

        if (request.isDone)
        {
            logString += "isDone";
            Debug.Log("isDone");
        }

        if (request.isNetworkError)
        {
            logString += "isNetworkError";
            Debug.Log("isNetworkError");
        }

        if (request.isHttpError)
        {
            logString += "isHttpError";
            Debug.Log("isHttpError");
        }

        // Show results as text
        Debug.Log(request.downloadHandler.text);

        TotalBreath totalBreathObj = JsonUtility.FromJson<TotalBreath>(request.downloadHandler.text);
        if(totalBreathObj != null)
            totalBreath = totalBreathObj.totalBreathCount;
    }


    public void OnGetServerStatus()
    {
        if (ipAddress.text != null && ipAddress.text.Length > 0)
            hostName = ipAddress.text;
        StartCoroutine(GetServerStatus("http://" + hostName + ":3333/"));
    }


    IEnumerator GetServerStatus(string serverUrl)
    {
        Debug.Log(serverUrl);
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError)
        {
            OnUpdateServerStatus(false);
        }
        else
        {
            OnUpdateServerStatus(true);
        }
    }


    void OnUpdateServerStatus(bool newServerStatus)
    {
        serverStatus = newServerStatus;
        if(serverStatus)
            serverStatusImg.color = Color.green;
        else
            serverStatusImg.color = Color.red;
    }

}

