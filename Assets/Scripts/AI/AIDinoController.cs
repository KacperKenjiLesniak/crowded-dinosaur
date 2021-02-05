using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
    public class AIDinoController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float obstacleDistanceToJump;

        public int aiIndex { get; set; }
        private DinoMovement dinoMovement;
        private DinoInputSender dinoInputSender;
        private List<Transform> obstacles;

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            dinoMovement = GetComponent<DinoMovement>();
            dinoInputSender = GetComponent<DinoInputSender>();
        }

        private void Start()
        {
            obstacles = GameObject.FindGameObjectsWithTag("Obstacle").Select(o => o.transform).ToList();
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (ShouldJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump();
                    dinoInputSender.SendJumpInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount);
                }
            }
        }

        private bool ShouldJump()
        {
            return obstacles
                .Any(obstacle => 
                    Vector3.Distance(obstacle.position, transform.position) <= obstacleDistanceToJump &&
                    obstacle.position.x > transform.position.x
                    );
        }
    }
}