﻿using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Utils;
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
        private float noiseShift;
        private float chanceForMistake;
        private float currentMistake;
        private bool isMistaken;
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
                if (isMistaken) return;
                if (ShouldLongJump() && dinoMovement.grounded)
                {
                    RandomizeVariables();
                    if (isMistaken) return;
                    dinoMovement.IssueJump(false);
                    dinoInputSender.SendInput(GetAiOffset(), Constants.INPUT_JUMP_ID);
                }
                else if (ShouldShortJump() && dinoMovement.grounded)
                {
                    RandomizeVariables();
                    if (isMistaken) return;
                    dinoMovement.IssueJump(true);
                    dinoInputSender.SendInput(GetAiOffset(), Constants.INPUT_SHORT_JUMP_ID);
                }
                else if (ShouldCrouch() && !dinoMovement.isCrouching)
                {
                    RandomizeVariables();
                    if (isMistaken) return;
                    dinoMovement.IssueCrouch();
                    dinoInputSender.SendInput(GetAiOffset(), Constants.INPUT_CROUCH_ID);
                }
            }
        }

        private int GetAiOffset()
        {
            return aiIndex + PhotonNetwork.CurrentRoom.PlayerCount;
        }

        public void Configure(int index, AiConfig aiConfig)
        {
            aiIndex = index;
            maxJumpNoise = aiConfig.maxNoise;
            noiseShift = aiConfig.noiseShift;
            chanceForMistake = aiConfig.chanceForMistake;
        }

        private bool ShouldLongJump()
        {
            return obstacles
                       .Any(obstacle =>
                           Math.Abs(obstacle.position.x - transform.position.x) <=
                           NoisedDistance(obstacleDistanceToJump) &&
                           obstacle.position.x > transform.position.x)
                   || birds
                       .Any(bird =>
                           Math.Abs(bird.position.x - transform.position.x) <= NoisedDistance(obstacleDistanceToJump) &&
                           bird.position.x > transform.position.x &&
                           bird.position.y <= minBirdHeightToCrouch);
        }

        private bool ShouldShortJump()
        {
            return smallObstacles
                .Any(obstacle =>
                    Math.Abs(obstacle.position.x - transform.position.x) <=
                    NoisedDistance(smallObstacleDistanceToJump) &&
                    obstacle.position.x > transform.position.x);
        }

        private bool ShouldCrouch()
        {
            return birds
                .Any(bird =>
                    Math.Abs(bird.position.x - transform.position.x) <= NoisedDistance(birdDistanceToCrouch) &&
                    bird.position.x > transform.position.x &&
                    bird.position.y > minBirdHeightToCrouch);
        }

        private void IssueRandomInput()
        {
            switch (random.Next(0, 4))
            {
                case 0:
                    dinoMovement.IssueJump(false);
                    dinoInputSender.SendInput(GetAiOffset(), Constants.INPUT_JUMP_ID);
                    break;
                case 1:
                    dinoMovement.IssueJump(true);
                    dinoInputSender.SendInput(GetAiOffset(), Constants.INPUT_SHORT_JUMP_ID);
                    break;
                case 2:
                    dinoMovement.IssueCrouch();
                    dinoInputSender.SendInput(GetAiOffset(), Constants.INPUT_CROUCH_ID);
                    break;
                case 3:
                    break;
            }
        }

        private void RandomizeVariables()
        {
            currentNoise = NextFloat(maxJumpNoise);
            currentMistake = Math.Abs(NextFloat(1));
            Debug.Log("Current mistake: " + currentMistake + " / " + chanceForMistake);
            if (currentMistake < chanceForMistake)
            {
                isMistaken = true;
                IssueRandomInput();
                this.Invoke(() => isMistaken = false, Math.Abs(NextFloat(1)) + 1);
            }
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

        private float NoisedDistance(float distance)
        {
            return distance * rb.velocity.x / dinoMovement.initialSpeed + currentNoise + noiseShift;
        }
    }
}