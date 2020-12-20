using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LoadGameScene : MonoBehaviour
{
    void Start()
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
