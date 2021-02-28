using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTopPanel : MonoBehaviour
{
    [Header("UI References")] public Text ConnectionStatusText;
    private readonly string connectionStatusMessage = "    Connection Status: ";

    #region UNITY

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }

    #endregion
}