using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
    public class AIDinoController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float obstacleDistanceToJump;

        private int aiIndex;
        private float maxJumpNoise;
        private DinoMovement dinoMovement;
        private DinoInputSender dinoInputSender;
        private List<Transform> obstacles;
        private float currentObstacleDistanceToJump;
        private Rigidbody2D rb;

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            dinoMovement = GetComponent<DinoMovement>();
            dinoInputSender = GetComponent<DinoInputSender>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            obstacles = GameObject.FindGameObjectsWithTag("Obstacle").Select(o => o.transform).ToList();
            currentObstacleDistanceToJump = obstacleDistanceToJump + Random.Range(-maxJumpNoise, maxJumpNoise);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (ShouldJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump();
                    dinoInputSender.SendJumpInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount);
                    currentObstacleDistanceToJump = obstacleDistanceToJump * rb.velocity.x / 10 + Random.Range(-maxJumpNoise, maxJumpNoise);
                }
            }
        }

        public void Configure(int index, float jumpNoise)
        {
            aiIndex = index;
            maxJumpNoise = jumpNoise;
        }

        private bool ShouldJump()
        {
            return obstacles
                .Any(obstacle => 
                    Vector3.Distance(obstacle.position, transform.position) <= currentObstacleDistanceToJump &&
                    obstacle.position.x > transform.position.x
                    );
        }
    }
}