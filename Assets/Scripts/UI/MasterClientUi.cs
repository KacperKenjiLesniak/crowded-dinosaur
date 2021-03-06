using Photon.Pun;
using UnityEngine;

public class MasterClientUi : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }
    }
}