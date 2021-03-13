using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

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
        private float currentNoise;
        private DinoInputSender dinoInputSender;
        private DinoMovement dinoMovement;
        private float maxJumpNoise;
        private float minBirdHeightToCrouch = -2.7f;
        private List<Transform> obstacles;
        private List<Transform> smallObstacles;
        private Rigidbody2D rb;
        private Random random = new Random();

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
            currentNoise = NextFloat(maxJumpNoise);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (ShouldLongJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump(false);
                    dinoInputSender.SendInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount, Constants.INPUT_JUMP_ID);
                    currentNoise = NextFloat(maxJumpNoise);
                }

                if (ShouldShortJump() && dinoMovement.grounded)
                {
                    dinoMovement.IssueJump(true);
                    dinoInputSender.SendInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount,
                        Constants.INPUT_SHORT_JUMP_ID);
                    currentNoise = NextFloat(maxJumpNoise);
                }

                if (ShouldCrouch() && !dinoMovement.isCrouching)
                {
                    dinoMovement.IssueCrouch();
                    dinoInputSender.SendInput(aiIndex + PhotonNetwork.CurrentRoom.PlayerCount,
                        Constants.INPUT_CROUCH_ID);
                    currentNoise = NextFloat(maxJumpNoise);
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
                           Math.Abs(obstacle.position.x - transform.position.x) <= obstacleDistanceToJump * rb.velocity.x / dinoMovement.initialSpeed &&
                           obstacle.position.x > transform.position.x)
                   || birds
                       .Any(bird =>
                           Math.Abs(bird.position.x - transform.position.x) <= obstacleDistanceToJump * rb.velocity.x / dinoMovement.initialSpeed &&
                           bird.position.x > transform.position.x &&
                           bird.position.y <= minBirdHeightToCrouch);
        }

        private bool ShouldShortJump()
        {
            return smallObstacles
                .Any(obstacle =>
                    Math.Abs(obstacle.position.x - transform.position.x) <= smallObstacleDistanceToJump * rb.velocity.x / dinoMovement.initialSpeed &&
                    obstacle.position.x > transform.position.x);
        }

        private bool ShouldCrouch()
        {
            return birds
                .Any(bird =>
                    Math.Abs(bird.position.x - transform.position.x) <= birdDistanceToCrouch * rb.velocity.x / dinoMovement.initialSpeed &&
                    bird.position.x > transform.position.x &&
                    bird.position.y > minBirdHeightToCrouch);
        }

        float NextFloat(float scale)
        {
            double f = random.NextDouble() * 2.0 - 1.0;
            return (float) f * scale;
        }

        public void SetSeed(int seed)
        {
            random = new Random(seed);
        }
    }
}