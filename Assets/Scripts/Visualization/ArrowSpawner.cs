﻿using System.Collections.Generic;
using DefaultNamespace.Events;
using MutableObjects.Bool;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.Visualization
{
    public class ArrowSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private List<GameObject> arrows;
        [SerializeField] private GameObject playerManager;
        [SerializeField] private MutableBool playerOnMasterServer;

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
                        photonView.RPC(nameof(SpawnArrowInClients), RpcTarget.All, playerInput.playerId, 0);
                        break;
                    case Constants.INPUT_SHORT_JUMP_ID:
                        photonView.RPC(nameof(SpawnArrowInClients), RpcTarget.All, playerInput.playerId, 1);
                        break;
                    case Constants.INPUT_CROUCH_ID:
                        photonView.RPC(nameof(SpawnArrowInClients), RpcTarget.All, playerInput.playerId, 2);
                        break;
                }
            }
        }


        [PunRPC]
        public void SpawnArrowInClients(int playerId, int arrowIndex)
        {
            Debug.Log("Spawning arrow: " + playerId);
            var arrowObject =
                Instantiate(arrows[arrowIndex], transform.position, arrows[arrowIndex].transform.rotation);
            var playerColor = playerColors[playerId + (playerOnMasterServer.Value ? 0 : 1) % playerColors.Count];
            playerColor.a = 1f;
            arrowObject.GetComponent<SpriteRenderer>().color = playerColor;
        }
    }
}