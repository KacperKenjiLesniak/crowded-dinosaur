using System;
using System.Collections.Generic;
using MutableObjects.Int;
using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("Open")] [SerializeField] private List<ScoreThreshold> stages;

        [Header("Restricted")] [SerializeField]
        private List<Transform> obstacles;
        [SerializeField] private int birdIndex;

        [SerializeField] private MutableVector3 dinoPosition;
        [SerializeField] private MutableInt score;

        private int currentStage = -1;
        private float obstacleTimer;
        private float nextObstacleTime;
        private float birdInitialHeight;
        
        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            birdInitialHeight = obstacles[birdIndex].position.y;
        }

        private void Update()
        {
            if (stages.Count > currentStage + 1 && stages[currentStage + 1].score <= score.Value)
            {
                currentStage++;
                nextObstacleTime = Random.Range(
                    stages[currentStage].meanTimeBetweenObstacles - stages[currentStage].timeVariance,
                    stages[currentStage].meanTimeBetweenObstacles + stages[currentStage].timeVariance
                );
                obstacleTimer = 0f;
            }

            if (obstacleTimer >= nextObstacleTime)
            {
                nextObstacleTime = Random.Range(
                    stages[currentStage].meanTimeBetweenObstacles - stages[currentStage].timeVariance,
                    stages[currentStage].meanTimeBetweenObstacles + stages[currentStage].timeVariance
                );
                obstacleTimer = 0f;
                SpawnObstacle();
            }

            obstacleTimer += Time.deltaTime;
        }

        private void SpawnObstacle()
        {
            var obstacleIndex = Random.Range(0, stages[currentStage].maxObstacleIndex);
            var obstacle = obstacles[obstacleIndex];
            obstacle.position = new Vector3(dinoPosition.Value.x + 20f, obstacle.position.y);
            if (obstacleIndex == birdIndex)
            {
                obstacle.position = new Vector3(obstacle.position.x, birdInitialHeight + Random.Range(-1.5f, 1.5f));
            }
        }
    }
}