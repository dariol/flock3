using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.XR;
using Mirror;

public class F_Player : NetworkBehaviour
{
    public InputField inputField;
    public Renderer cubeMat;

    public GameObject vrCameraRig;
    public GameObject fpsCtrl;
    GameObject vrCameraRigInstance;
    GameObject fpsCtrlInstance;

    public GameObject[] AvatarHide;
    public GameObject[] AvatarShow;
    public GameObject[] AvatarSpectate;

    [SyncVar(hook = nameof(OnSpectatorChanged))]
    public bool spectator;

    [Command]
    public void CmdSyncSpectator(bool val)
    {
        spectator = val;
    }

    public void SetSpectator()
    {
        for (int i = 0; i < AvatarSpectate.Length; i++)
        {
            AvatarSpectate[i].SetActive(!spectator);
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.J))
        {
            spectator = !spectator;
            CmdSyncSpectator(spectator);
            if (this.GetComponent<F_IsLocalPlayer>() != null)
                SetSpectator();
        }
    }

    public override void OnStartLocalPlayer()
    {
        if (GameObject.Find("TangoParent"))
        {
            CmdSyncSpectator(true);
            SetSpectator();
            return;
        }

        if (spectator)
            SetSpectator();

        // delete main camera
        if (Camera.main != null)
            Camera.main.gameObject.SetActive(false);

        this.gameObject.AddComponent<F_IsLocalPlayer>();

        if (XRSettings.isDeviceActive)
        {
            vrCameraRigInstance = Instantiate(vrCameraRig, transform.position, transform.rotation);

            Debug.Log(XRSettings.isDeviceActive);

            Transform bodyOfVrPlayer = transform.Find("VRPlayerBody");
            if (bodyOfVrPlayer != null)
                bodyOfVrPlayer.parent = null;

            GameObject head = vrCameraRigInstance.gameObject;
            transform.SetParent(head.transform);

            GameObject.Find("ResetPlayer").GetComponent<F_ResetPlayer>().PlayerController = vrCameraRigInstance;
        }
        else
        {
            fpsCtrlInstance = Instantiate(fpsCtrl, transform.position, transform.rotation);

            Debug.Log(XRSettings.isDeviceActive);

            GameObject head = fpsCtrlInstance.transform.GetChild(0).gameObject;
            transform.SetParent(head.transform);

            GameObject.Find("ResetPlayer").GetComponent<F_ResetPlayer>().PlayerController = fpsCtrlInstance;
        }

        F_Players.thisPlayer = this.gameObject;
        F_Players.thisPlayerID = F_Players.players;

        for (int i = 0; i < AvatarHide.Length; i++)
        {
            AvatarHide[i].SetActive(false);
        }
        for (int i = 0; i < AvatarShow.Length; i++)
        {
            AvatarShow[i].SetActive(true);
        }
    }

    void OnSpectatorChanged(bool oldValue, bool newValue)
    {
        SetSpectator();
    }
}