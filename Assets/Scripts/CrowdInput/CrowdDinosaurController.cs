﻿using System.Linq;
using DefaultNamespace.AI;
using GameEvents.Game;
using MutableObjects.Bool;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(DinoMovement))]
    public class CrowdDinosaurController : MonoBehaviourPunCallbacks, InputReceiver
    {
        [SerializeField] private AiList aiList;
        [SerializeField] private CrowdConfig crowdConfig;
        [SerializeField] private GameEvent lostGameEvent;
        [SerializeField] private MutableBool playerOnMasterServer;

        private DinoMovement dinoMovement;
        private AbstractInputBroker inputBroker;
        private int numberOfPlayers;

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            dinoMovement = GetComponent<DinoMovement>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (photonView.IsMine)
            {
                if (other.collider.CompareTag("Obstacle") || other.collider.CompareTag("SmallObstacle") || other.collider.CompareTag("Bird"))
                {
                    foreach (var movement in FindObjectsOfType<DinoMovement>())
                    {
                        movement.Die();
                    }
                    lostGameEvent.RaiseGameEvent();
                }
            }
        }

        public void ApplyInput(int input)
        {
            switch (input)
            {
                case Constants.INPUT_JUMP_ID when dinoMovement.grounded:
                    dinoMovement.IssueJump(false);
                    break;
                case Constants.INPUT_SHORT_JUMP_ID when dinoMovement.grounded:
                    dinoMovement.IssueJump(true);
                    break;
                case Constants.INPUT_CROUCH_ID when !dinoMovement.isCrouching:
                    dinoMovement.IssueCrouch();
                    break;
            }
        }

        public void StartGame()
        {
            inputBroker = FindObjectsOfType<AbstractInputBroker>().First(broker => broker.enabled);
            numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount - (playerOnMasterServer.Value ? 0 : 1) + aiList.aiConfigs.Count;
            inputBroker.SetUp(crowdConfig, numberOfPlayers, this);
            Debug.Log("Starting game with " + numberOfPlayers + " players.");
        }
    }
}