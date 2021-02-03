using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Events;
using GameEvents.Generic;
using GameEvents.Int;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(DinoMovement))]
    public class CrowdDinosaurController : MonoBehaviourPunCallbacks, IArgumentGameEventListener<PlayerInput>
    {
        [SerializeField] private PlayerInputGameEvent playerInputGameEvent;

        private CrowdInputReliability crowdInputReliability;
        private List<int> currentPlayerInputs;
        private int numberOfPlayers = 1;
        private DinoMovement dinoMovement;

        private void Awake()
        {
            dinoMovement = GetComponent<DinoMovement>();
        }

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            playerInputGameEvent.RegisterListener(this);
            
            crowdInputReliability = new CrowdInputReliability(numberOfPlayers, 2, 0.1f, 0.5f);
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