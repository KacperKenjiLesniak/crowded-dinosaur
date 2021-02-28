using Photon.Pun;
using UnityEngine;

public class LoadGameScene : MonoBehaviour
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke(nameof(LoadNextScene), 2f);
        }
    }

    private void LoadNextScene()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}