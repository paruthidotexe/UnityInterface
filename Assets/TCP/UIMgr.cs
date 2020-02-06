using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIMgr : MonoBehaviour
{
    public static UIMgr instance;
    public GameObject startMenu;
    public TMP_InputField userNameField;

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
    }


    void Update()
    {
    }


    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        TCPClientMono.instance.ConnectToServer();
    }

}
