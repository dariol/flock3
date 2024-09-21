using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Mirror;
using System;

public class ClientHUD : MonoBehaviour
{
    public GameObject connectToServer, disConnect, addressPanel, connecting, menuCam, disConnectMessage;
    public InputField portText, ipText, passwordText;
    public Text connectingText;

    private float connectingTimer, connectionFaileTimer;
    private bool connected;

    public bool autoConnectToServer;
    public string autoConnectIP;
    public string autoConnectPort;
    public bool isConnected;

    private NetworkManager manager;

    void Start()
    {
        manager = NetworkManager.singleton;

        if (PlayerPrefs.HasKey("nwPortC"))
        {
            manager.GetComponent<TelepathyTransport>().port = Convert.ToUInt16(PlayerPrefs.GetString("nwPortC"));
            portText.text = PlayerPrefs.GetString("nwPortC");
        }
        if (PlayerPrefs.HasKey("IPAddressC"))
        {
            manager.networkAddress = PlayerPrefs.GetString("IPAddressC");
            ipText.text = PlayerPrefs.GetString("IPAddressC");
        }
        if(autoConnectToServer){
            StartCoroutine(Connect());
        }
    }

    void Update()
    {
        if (!connected)
        {
            if (connectingTimer > 0)
                connectingTimer -= Time.deltaTime;
            else
            {
                connectingText.text = "Failed To Connect !!";
                if (connectionFaileTimer > 0)
                    connectionFaileTimer -= Time.deltaTime;
                else connecting.SetActive(false);
            }
        }
    }

    public void ConnectToServer()
    {
        if (ipText.text != "" && portText.text != "")
        {
            connected = false;
            disConnectMessage.SetActive(false);
            connectingText.text = "Connecting !!";
            connecting.SetActive(true);
            connectingTimer = 8;
            connectionFaileTimer = 2;
            manager.networkAddress = ipText.text;
            manager.GetComponent<TelepathyTransport>().port = Convert.ToUInt16(portText.text);
            PlayerPrefs.SetString("IPAddressC", autoConnectIP);
            PlayerPrefs.SetString("nwPortC", autoConnectPort);

            manager.StartClient();
        }
    }

    IEnumerator Connect(){
        while(!connected){
            yield return new WaitForSeconds(1);
            Debug.Log("Connecting to server");
            ConnectToServer();
        }
    }

    public void AutoConnect()
    {
        if (ipText.text != "" && portText.text != "")
        {
            connected = false;
            disConnectMessage.SetActive(false);
            connectingText.text = "Connecting !!";
            connecting.SetActive(true);
            connectingTimer = 8;
            connectionFaileTimer = 2;
            manager.networkAddress = ipText.text;
            manager.GetComponent<TelepathyTransport>().port = Convert.ToUInt16(portText.text);
            PlayerPrefs.SetString("IPAddressC", ipText.text);
            PlayerPrefs.SetString("nwPortC", portText.text);

            manager.StartClient();
        }
    }

    public void ConnectSuccses()
    {
        connected = true;
        connecting.SetActive(false);
        disConnect.SetActive(true);
        connectToServer.SetActive(false);
        addressPanel.SetActive(false);
        //menuCam.SetActive(false);
    }

    public void ButtonDisConnect()
    {
        DisConnect(false);
    }

    public void DisConnect(bool showMessage)
    {
        GameObject.Find("ResetPlayer").GetComponent<F_ResetPlayer>().Reset();
        if (showMessage)
            disConnectMessage.SetActive(true);
        connectToServer.SetActive(true);
        disConnect.SetActive(false);
        addressPanel.SetActive(true);
        //menuCam.SetActive(true);
        manager.StopClient();
    }
}