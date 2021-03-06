using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
    public class AIDinoController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float obstacleDistanceToJump;
        [SerializeField] private float smallObstacleDistanceToJump;
        [SerializeField] private float birdDistanceToCrouch;

        private int aiIndex;
        private List<Transform> birds;
        private float currentBirdDistanceToCrouch;
        private float currentObstacleDistanceToJump;
        private float currentSmallObstacleDistanceToJump;
        private DinoInputSender dinoInputSender;
        private DinoMovement dinoMovement;
        private float maxJumpNoise;
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
            currentObstacleDistanceToJump = obstacleDistanceToJump + Random.Range(-maxJumpNoise, maxJumpNoise);
            currentSmallObstacleDistanceToJump = smallObstacleDistanceToJump + Random.Range(-maxJumpNoise, maxJumpNoise);
            currentBirdDistanceToCrouch = birdDistanceToCrouch + Random.Range(-maxJumpNoise, maxJumpNoise);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (ShouldLongJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump(false);
                    dinoInputSender.SendInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount, Constants.INPUT_JUMP_ID);
                    currentObstacleDistanceToJump = obstacleDistanceToJump * rb.velocity.x / 10 +
                                                    Random.Range(-maxJumpNoise, maxJumpNoise);
                }

                if (ShouldShortJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump(true);
                    dinoInputSender.SendInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount, Constants.INPUT_SHORT_JUMP_ID);
                    smallObstacleDistanceToJump = obstacleDistanceToJump * rb.velocity.x / 10 +
                                                    Random.Range(-maxJumpNoise, maxJumpNoise);
                }
                
                if (ShouldCrouch() && !dinoMovement.isCrouching)
                {
                    dinoMovement.IssueCrouch();
                    dinoInputSender.SendInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount,
                        Constants.INPUT_CROUCH_ID);
                    currentBirdDistanceToCrouch = obstacleDistanceToJump * rb.velocity.x / 10 +
                                                  Random.Range(-maxJumpNoise, maxJumpNoise);
                }
            }
        }

        public void Configure(int index, float jumpNoise)
        {
            aiIndex = index;
            maxJumpNoise = jumpNoise;
        }

        private bool ShouldLongJump()
        {
            return obstacles
                       .Any(obstacle =>
                           Math.Abs(obstacle.position.x - transform.position.x) <= currentObstacleDistanceToJump &&
                           obstacle.position.x > transform.position.x)
                   || birds
                       .Any(bird =>
                           Math.Abs(bird.position.x - transform.position.x) <= currentObstacleDistanceToJump &&
                           bird.position.x > transform.position.x &&
                           bird.position.y <= minBirdHeightToCrouch);
        }
        
        private bool ShouldShortJump()
        {
            return smallObstacles
                .Any(obstacle =>
                    Math.Abs(obstacle.position.x - transform.position.x) <= currentSmallObstacleDistanceToJump &&
                    obstacle.position.x > transform.position.x);
        }

        private bool ShouldCrouch()
        {
            return birds
                .Any(bird =>
                    Math.Abs(bird.position.x - transform.position.x) <= currentBirdDistanceToCrouch &&
                    bird.position.x > transform.position.x &&
                    bird.position.y > minBirdHeightToCrouch);
        }
    }
}