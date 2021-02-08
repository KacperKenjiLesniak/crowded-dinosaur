using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.Visualization
{
    public class ArrowSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject arrow;
        [SerializeField] private GameObject playerManager;

        private List<Color> playerColors;

        private void Start()
        {
            playerColors = playerManager.GetComponent<PlayerManager>().playerColors;
        }

        public void SpawnArrow(PlayerInput playerInput)
        {
            if (photonView.IsMine)
            {
                switch (playerInput.inputId)
                {
                    case Constants.INPUT_JUMP_ID:
                        photonView.RPC(nameof(SpawnArrowInClients), RpcTarget.All, playerInput.playerId);
                        break;
                }
            }
        }


        [PunRPC]
        public void SpawnArrowInClients(int playerId)
        {
            var arrowObject = Instantiate(arrow, transform.position, Quaternion.identity);
            arrowObject.GetComponent<SpriteRenderer>().color = playerColors[playerId];
        }

    }
}