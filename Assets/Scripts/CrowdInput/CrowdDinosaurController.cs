using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.AI;
using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using GameEvents.Game;
using GameEvents.Generic;
using MutableObjects.Int;
using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(DinoMovement))]
    public class CrowdDinosaurController : MonoBehaviourPunCallbacks, IArgumentGameEventListener<PlayerInput>
    {
        [SerializeField] private PlayerInputGameEvent playerInputGameEvent;
        [SerializeField] private AiList aiList;
        [SerializeField] private MutableVector3 dinoPosition;
        [SerializeField] private GameEvent lostGameEvent;

        private CrowdInputReliability crowdInputReliability;
        private List<int> currentPlayerInputs;
        private int numberOfPlayers;
        private DinoMovement dinoMovement;
        private EvaluatorData evaluatorData = new EvaluatorData();
        
        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            dinoMovement = GetComponent<DinoMovement>();
            dinoPosition.Value = transform.position;
        }

        private void Start()
        {
            playerInputGameEvent.RegisterListener(this);
        }

        private void Update()
        {
            dinoPosition.Value = transform.position;
        }

        public void StartGame()
        {
            numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount + aiList.aiConfigs.Count;
            Debug.Log("Starting game with " + numberOfPlayers + " players.f");
            crowdInputReliability = CrowdInputManager.instance.GetCrowdInputReliability(numberOfPlayers);
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
            
            evaluatorData.AppendReliabilities(crowdInputReliability.GetPlayerReliabilities());
            evaluatorData.AppendInput(currentPlayerInputs);
            
            currentPlayerInputs = EmptyInputList();
        }

        public void RaiseGameEvent(PlayerInput input)
        {
            currentPlayerInputs[input.playerId] = input.inputId;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (photonView.IsMine)
            {
                if (other.collider.CompareTag("Obstacle"))
                {
                    photonView.RPC(nameof(Die), RpcTarget.AllViaServer);
                }
            }
        }

        [PunRPC]
        private void Die()
        {
            lostGameEvent.RaiseGameEvent();
            GetComponent<Animator>().enabled = false;
            dinoMovement.enabled = false;
        }

        private void OnDestroy()
        {
            playerInputGameEvent.UnregisterListener(this);
        }

        private void OnApplicationQuit()
        {
            evaluatorData.SaveData();
        }

        private List<int> EmptyInputList()
        {
            return Enumerable.Range(0, numberOfPlayers).Select(i => 0).ToList();
        }
    }
}