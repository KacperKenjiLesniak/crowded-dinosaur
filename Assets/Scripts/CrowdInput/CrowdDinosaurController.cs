using DefaultNamespace.AI;
using GameEvents.Game;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(DinoMovement))]
    public class CrowdDinosaurController : MonoBehaviourPunCallbacks, InputReceiver
    {
        [SerializeField] private AiList aiList;
        [SerializeField] private CrowdConfig crowdConfig;
        
        private InputBroker inputBroker;
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

        public void StartGame()
        {
            inputBroker = FindObjectOfType<InputBroker>();
            numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount + aiList.aiConfigs.Count;
            inputBroker.SetUp(crowdConfig, numberOfPlayers, this);
            Debug.Log("Starting game with " + numberOfPlayers + " players.");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (photonView.IsMine)
            {
                if (other.collider.CompareTag("Obstacle") || other.collider.CompareTag("Bird"))
                {
                    foreach (var movement in FindObjectsOfType<DinoMovement>())
                    {
                        movement.Die();
                    }
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
                case Constants.INPUT_CROUCH_ID:
                    dinoMovement.IssueCrouch();
                    break;
            }
        }
    }
}