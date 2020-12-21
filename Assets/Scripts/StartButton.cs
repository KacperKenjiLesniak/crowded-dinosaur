using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }
    }
}
