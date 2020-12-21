using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text lostGameText;
        [SerializeField] private string youLostText = "You lost!";

        private void Start()
        {
            PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0.01f;
            Time.timeScale = 0f;
        }

        public void LoseGame()
        {
            lostGameText.text = youLostText;
        }

        public void Restart()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("EmptyScene");
            }
        }

        public void StartGameMaster()
        {
            photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);
        }

        [PunRPC]
        void StartGame()
        {
            Time.timeScale = 1f;
        }
    }
}