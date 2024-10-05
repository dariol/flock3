using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManagerMirror : NetworkManager
{
    public Text clientsInfoText;
    public ClientHUD clientHudScript;
    public ServerHUDMirror serverHudScript;

    private int connectedClients = 0;

    [HideInInspector]
    public string serverPassword;

    public bool reconnectAutomatically;

    // Server Side
    public override void OnStartServer()
    {
        base.OnStartServer();
        
        serverPassword = serverHudScript.passwordText.text;
        connectedClients = 0;
        clientsInfoText.text = "Connected Clients : " + connectedClients;
    }

    public override void OnStartHost()
    {
        Debug.Log("starting host");
        base.OnStartHost();
    }

    // Keeping track of Clients connecting
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        connectedClients += 1;
        clientsInfoText.text = "Connected Clients : " + connectedClients;

        // Sending password information to client
        conn.Send(new PasswordMessage { password = serverPassword });
    }

    // Keeping track of Clients disconnecting
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        connectedClients -= 1;
        clientsInfoText.text = "Connected Clients : " + connectedClients;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        GameObject.Find("ResetPlayer")?.GetComponent<F_ResetPlayer>()?.Reset();
        SceneManager.LoadScene(0);
    }

    // Client Side
    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<PasswordMessage>(OnReceivePassword);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        clientHudScript.ConnectSuccses();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("adding player");
        var player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    // When client receives password information from the server
    public void OnReceivePassword(PasswordMessage msg)
    {
        if (msg.password != clientHudScript.passwordText.text)
            clientHudScript.DisConnect(true);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        clientHudScript.DisConnect(false);
        if (reconnectAutomatically)
            StartClient();
    }
}

// Custom message for password
public struct PasswordMessage : NetworkMessage
{
    public string password;
}
// using UnityEngine;
// using Mirror;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// public class NetworkManagerMirror : NetworkManager
// {
//     public Text clientsInfoText;
//     public ClientHUD clientHudScript;
//     public ServerHUDMirror serverHudScript;

//     private int connectedClients = 0;

//     [HideInInspector]
//     public string serverPassword;

//     public bool reconnectAutomatically;

//     // Custom message types
//     public const short MsgPassword = 1000;

//     // Server Side
//     public override void OnStartServer()
//     {
//         base.OnStartServer();
//         NetworkServer.RegisterHandler(MsgPassword, OnReceivePassword);

//         serverPassword = serverHudScript.passwordText.text;
//         connectedClients = 0;
//         UpdateClientInfoText();
//     }

//     public override void OnStartHost()
//     {
//         Debug.Log("starting host");
//         base.OnStartHost();
//     }

//     // Keeping track of Clients connecting
//     public override void OnServerConnect(NetworkConnection conn)
//     {
//         base.OnServerConnect(conn);
//         connectedClients++;
//         UpdateClientInfoText();

//         // Sending password information to client
//         PasswordMessage passwordMsg = new PasswordMessage(serverPassword);
//         conn.Send(MsgPassword, passwordMsg);
//     }

//     // Keeping track of Clients disconnecting
//     public override void OnServerDisconnect(NetworkConnection conn)
//     {
//         base.OnServerDisconnect(conn);
//         connectedClients--;
//         UpdateClientInfoText();
//     }

//     private void UpdateClientInfoText()
//     {
//         if (clientsInfoText != null)
//         {
//             clientsInfoText.text = "Connected Clients : " + connectedClients;
//         }
//     }

//     public override void OnStopServer()
//     {
//         base.OnStopServer();
//         GameObject resetPlayerObj = GameObject.Find("ResetPlayer");
//         if (resetPlayerObj != null)
//         {
//             F_ResetPlayer resetPlayer = resetPlayerObj.GetComponent<F_ResetPlayer>();
//             if (resetPlayer != null)
//             {
//                 resetPlayer.Reset();
//             }
//         }
//         SceneManager.LoadScene(0);
//     }

//     // Client Side
//     public override void OnStartClient()
//     {
//         base.OnStartClient();
//         NetworkClient.RegisterHandler(MsgPassword, OnClientReceivePassword);
//     }

//     public override void OnClientConnect(NetworkConnection conn)
//     {
//         base.OnClientConnect(conn);
//         clientHudScript.ConnectSuccses();
//     }

//     public override void OnServerAddPlayer(NetworkConnection conn)
//     {
//         Debug.Log("adding player");
//         GameObject player = Instantiate(playerPrefab);
//         NetworkServer.AddPlayerForConnection(conn, player);
//     }

//     // When client receives password information from the server
//     private void OnClientReceivePassword(NetworkMessage netMsg)
//     {
//         PasswordMessage msg = netMsg.ReadMessage<PasswordMessage>();
//         if (msg.password != clientHudScript.passwordText.text)
//         {
//             clientHudScript.DisConnect(true);
//         }
//     }

//     public override void OnClientDisconnect(NetworkConnection conn)
//     {
//         base.OnClientDisconnect(conn);
//         clientHudScript.DisConnect(false);
//         if (reconnectAutomatically)
//         {
//             StartClient();
//         }
//     }

//     // Server-side password check
//     private void OnReceivePassword(NetworkMessage netMsg)
//     {
//         PasswordMessage msg = netMsg.ReadMessage<PasswordMessage>();
//         // Implement server-side password check if needed
//     }
// }

// // Custom message class for password
// public class PasswordMessage : MessageBase
// {
//     public string password;

//     public PasswordMessage() {}

//     public PasswordMessage(string password)
//     {
//         this.password = password;
//     }

//     public override void Serialize(NetworkWriter writer)
//     {
//         writer.Write(password);
//     }

//     public override void Deserialize(NetworkReader reader)
//     {
//         password = reader.ReadString();
//     }
// }
// using UnityEngine;
// // using Mirror;
// using UnityEngine.UI;
// // using System;
// using UnityEngine.SceneManagement;

// namespace Mirror{
// public class NetworkManagerMirror : NetworkManager
// {
//     public Text clientsInfoText;
//     public ClientHUD clientHudScript;
//     public ServerHUDMirror serverHudScript;

//     private int connectedClients = 0;

//     [HideInInspector]
//     public string serverPassword;

//     public bool reconnectAutomatically;

//     // Server Side
//     public override void OnStartServer()
//     {
//         base.OnStartServer();
//         RegisterServerHandles();

//         serverPassword = serverHudScript.passwordText.text;
//         connectedClients = 0;
//         clientsInfoText.text = "Connected Clients : " + connectedClients;
//     }

//     public override void OnStartHost()
//     {
//         Debug.Log("starting host");
//         base.OnStartHost();
//         RegisterHostHandles();
//     }

//     // Keeping track of Clients connecting
//     public override void OnServerConnect(NetworkConnectionToClient conn)
//     {
//         base.OnServerConnect(conn);
//         connectedClients += 1;
//         clientsInfoText.text = "Connected Clients : " + connectedClients;

//         // Sending password information to client
//         // StringMessage msg = new StringMessage(serverPassword);
//         // conn.Send((int)MsgType.Highest + 1, msg);
//     }

//     // Keeping track of Clients disconnecting
//     public override void OnServerDisconnect(NetworkConnectionToClient conn)
//     {
//         base.OnServerDisconnect(conn);
//         connectedClients -= 1;
//         clientsInfoText.text = "Connected Clients : " + connectedClients;
//     }

//     public override void OnStopServer()
//     {
//         base.OnStopServer();
//         GameObject.Find("ResetPlayer").GetComponent<F_ResetPlayer>().Reset();
//         SceneManager.LoadScene(0);
//     }

//     // Client Side
//     public override void OnStartClient()
//     {
//         base.OnStartClient();
//         RegisterClientHandles();
//     }

//     public override void OnClientConnect()
//     {
//         base.OnClientConnect();
//         clientHudScript.ConnectSuccses();
//     }

//     public override void OnServerAddPlayer(NetworkConnectionToClient conn)
//     {
//         Debug.Log("adding player");
//         var player = Instantiate(playerPrefab);
//         NetworkServer.AddPlayerForConnection(conn, player);
//     }

//     // When client receives password information from the server
//     public void OnReceivePassword(NetworkMessage netMsg)
//     {
//         var msg = netMsg.ReadMessage<StringMessage>().value;
//         if (msg != clientHudScript.passwordText.text)
//             clientHudScript.DisConnect(true);
//     }

//     public override void OnClientDisconnect(NetworkConnection conn)
//     {
//         base.OnClientDisconnect(conn);
//         clientHudScript.DisConnect(false);
//         if (reconnectAutomatically)
//             StartClient();
//     }

//     // Messages that need to be Registered on Server and Client Startup
//     void RegisterServerHandles()
//     {
//         NetworkServer.RegisterHandler(MsgType.Highest + 1, OnReceivePassword);
//     }

//     void RegisterClientHandles()
//     {
//         NetworkClient.RegisterHandler(MsgType.Highest + 1, OnReceivePassword);
//     }

//     void RegisterHostHandles()
//     {
//         NetworkServer.RegisterHandler(MsgType.Highest + 1, OnReceivePassword);
//     }
// }
// }