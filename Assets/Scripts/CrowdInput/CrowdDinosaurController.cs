using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Events;
using GameEvents.Generic;
using GameEvents.Int;
using MutableObjects.Int;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(DinoMovement))]
    public class CrowdDinosaurController : MonoBehaviourPunCallbacks, IArgumentGameEventListener<PlayerInput>
    {
        [SerializeField] private PlayerInputGameEvent playerInputGameEvent;
        [SerializeField] private MutableInt numberOfAIs;

        private CrowdInputReliability crowdInputReliability;
        private List<int> currentPlayerInputs;
        private int numberOfPlayers;
        private DinoMovement dinoMovement;

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            dinoMovement = GetComponent<DinoMovement>();
        }

        private void Start()
        {
            playerInputGameEvent.RegisterListener(this);
        }

        public void StartGame()
        {
            numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount + numberOfAIs.Value;
            Debug.Log("Starting game with " + numberOfPlayers + " players.f");
            crowdInputReliability = new CrowdInputReliability(numberOfPlayers, 2, 0.01f, 0.7f);
            currentPlayerInputs = EmptyInputList();
            InvokeRepeating(nameof(ApplyInputs), Constants.INPUT_PERIOD, Constants.INPUT_PERIOD);
        }

        private void ApplyInputs()
        {
            var command = crowdInputReliability.IssueCommands(currentPlayerInputs.ToArray());
            if (command == Constants.INPUT_JUMP_ID)
            {
                dinoMovement.IssueJump();
            }

            Debug.Log("Applying inputs: " + currentPlayerInputs.ToArray()[0] + " input:" + command);
            Debug.Log("Current reliabilities: " + crowdInputReliability.GetPlayerReliabilities()
                .Select(f => f + "")
                .Aggregate((i, j) => i + ", " + j));

            currentPlayerInputs = EmptyInputList();
        }

        public void RaiseGameEvent(PlayerInput input)
        {
            currentPlayerInputs[input.playerId] = input.inputId;
        }

        private void OnDestroy()
        {
            playerInputGameEvent.UnregisterListener(this);
        }


        private List<int> EmptyInputList()
        {
            return Enumerable.Range(0, numberOfPlayers).Select(i => 0).ToList();
        }
    }
}