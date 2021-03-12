using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
    public class ReferenceAIDinoController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float obstacleDistanceToJump;
        [SerializeField] private float smallObstacleDistanceToJump;
        [SerializeField] private float birdDistanceToCrouch;

        private int aiIndex;
        private List<Transform> birds;
        private DinoInputSender dinoInputSender;
        private DinoMovement dinoMovement;
        private float minBirdHeightToCrouch = -2.7f;
        private List<Transform> obstacles;
        private List<Transform> smallObstacles;
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
            smallObstacles = GameObject.FindGameObjectsWithTag("SmallObstacle").Select(o => o.transform).ToList();
            birds = GameObject.FindGameObjectsWithTag("Bird").Select(o => o.transform).ToList();
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (ShouldLongJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump(false);
                    dinoInputSender.SendInput(aiIndex, Constants.INPUT_JUMP_ID, true);
                }

                if (ShouldShortJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump(true);
                    dinoInputSender.SendInput(aiIndex,
                        Constants.INPUT_SHORT_JUMP_ID, true);
                    smallObstacleDistanceToJump = obstacleDistanceToJump * rb.velocity.x / 10;
                }

                if (ShouldCrouch() && !dinoMovement.isCrouching)
                {
                    dinoMovement.IssueCrouch();
                    dinoInputSender.SendInput(aiIndex,
                        Constants.INPUT_CROUCH_ID, true);
                    birdDistanceToCrouch = obstacleDistanceToJump * rb.velocity.x / 10;
                }
            }
        }

        public void Configure(int index)
        {
            aiIndex = index;
        }

        private bool ShouldLongJump()
        {
            return obstacles
                       .Any(obstacle =>
                           Math.Abs(obstacle.position.x - transform.position.x) <= obstacleDistanceToJump &&
                           obstacle.position.x > transform.position.x)
                   || birds
                       .Any(bird =>
                           Math.Abs(bird.position.x - transform.position.x) <= obstacleDistanceToJump &&
                           bird.position.x > transform.position.x &&
                           bird.position.y <= minBirdHeightToCrouch);
        }

        private bool ShouldShortJump()
        {
            return smallObstacles
                .Any(obstacle =>
                    Math.Abs(obstacle.position.x - transform.position.x) <= smallObstacleDistanceToJump &&
                    obstacle.position.x > transform.position.x);
        }

        private bool ShouldCrouch()
        {
            return birds
                .Any(bird =>
                    Math.Abs(bird.position.x - transform.position.x) <= birdDistanceToCrouch &&
                    bird.position.x > transform.position.x &&
                    bird.position.y > minBirdHeightToCrouch);
        }
    }
}