using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Events;
using GameEvents.Generic;
using GameEvents.Int;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class CrowdDinosaurController : MonoBehaviourPunCallbacks, IArgumentGameEventListener<PlayerInput>
    {
        [SerializeField] private PlayerInputGameEvent playerInputGameEvent;

        private CrowdInputReliability crowdInputReliability;
        private List<int> currentPlayerInputs;
        private int numberOfPlayers = 3;

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            playerInputGameEvent.RegisterListener(this);
            crowdInputReliability = new CrowdInputReliability(numberOfPlayers, 2, 0.1f, 0.5f);
            currentPlayerInputs = Enumerable.Range(0, numberOfPlayers).Select(i => 0).ToList();

            InvokeRepeating(nameof(ApplyInputs), Constants.INPUT_PERIOD, Constants.INPUT_PERIOD);
        }

        private void ApplyInputs()
        {
            var command = crowdInputReliability.IssueCommands(currentPlayerInputs.ToArray());
            if (command == Constants.INPUT_JUMP_ID)
            {
                //jump
            }

            currentPlayerInputs = Enumerable.Range(0, numberOfPlayers).Select(i => 0).ToList();
        }

        public void RaiseGameEvent(PlayerInput input)
        {
            currentPlayerInputs[input.playerId] = input.inputId;
        }

        private void OnDestroy()
        {
            playerInputGameEvent.UnregisterListener(this);
        }
    }
}