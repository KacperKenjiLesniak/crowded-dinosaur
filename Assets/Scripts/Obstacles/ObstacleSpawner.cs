using System.Collections.Generic;
using MutableObjects.Int;
using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("Open")] [SerializeField] private List<ScoreThreshold> stages;

        [Header("Restricted")] [SerializeField]
        private List<Obstacle> obstacles;

        [SerializeField] private int birdIndex;

        [SerializeField] private MutableVector3 dinoPosition;
        [SerializeField] private MutableInt score;
        private float birdInitialHeight;
        private Camera camera;

        private Obstacle currentObstacle;
        private int currentStage = -1;
        private float nextObstacleTime;
        private float obstacleTimer;

        private void Awake()
        {
            camera = Camera.main;
        }


        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            currentObstacle = obstacles[0];
            birdInitialHeight = obstacles[birdIndex].transform.position.y;
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

            if (TransformBehindCameraView(currentObstacle.transform))
            {
                obstacleTimer += Time.deltaTime;
            }
        }

        private void SpawnObstacle()
        {
            int obstacleIndex = Random.Range(0, stages[currentStage].maxObstacleIndex);
            currentObstacle = obstacles[obstacleIndex];
            currentObstacle.transform.position = new Vector3(dinoPosition.Value.x + 30f + currentObstacle.spawnOffset, currentObstacle.transform.position.y);
            if (obstacleIndex == birdIndex)
            {
                currentObstacle.transform.position = new Vector3(currentObstacle.transform.position.x + currentObstacle.spawnOffset,
                    birdInitialHeight + Random.Range(-1.5f, 1.5f));
            }
        }

        private bool TransformBehindCameraView(Transform t)
        {
            var viewPos = camera.WorldToViewportPoint(t.position);
            return viewPos.x < 0;
        }
    }
}