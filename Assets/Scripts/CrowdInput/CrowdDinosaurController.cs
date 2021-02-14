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
        [SerializeField] private GameEvent lostGameEvent;
        [SerializeField] private InputBroker inputBroker;

        private int numberOfPlayers;
        private DinoMovement dinoMovement;

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            inputBroker = FindObjectOfType<InputBroker>();
            dinoMovement = GetComponent<DinoMovement>();
        }

        public void StartGame()
        {
            numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount + aiList.aiConfigs.Count;
            inputBroker.SetUp(new InputBrokerConfig(
                    numberOfPlayers,
                    2,
                    0.01f,
                    0.7f,
                    0.4f
                ),
                this);
            Debug.Log("Starting game with " + numberOfPlayers + " players.f");
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

        public void ApplyInput(int input)
        {
            if (input == Constants.INPUT_JUMP_ID && dinoMovement.grounded)
            {
                dinoMovement.IssueJump();
            }
        }
    }
}