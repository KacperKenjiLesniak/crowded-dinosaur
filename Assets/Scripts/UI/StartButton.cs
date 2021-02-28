using Photon.Pun;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }
    }
}