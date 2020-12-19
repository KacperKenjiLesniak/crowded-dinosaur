using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero,
                Quaternion.identity);
        }
    }
}
