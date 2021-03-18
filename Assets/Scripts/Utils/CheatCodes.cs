using System;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.Utils
{
    public class CheatCodes : MonoBehaviour
    {
        private bool timeSlow;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) && PhotonNetwork.IsMasterClient)
            {
                timeSlow = !timeSlow;
                Time.timeScale = timeSlow ? 0.1f : 1f;
            }
        }
    }
}