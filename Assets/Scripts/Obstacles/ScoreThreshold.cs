using System;

namespace DefaultNamespace
{
    [Serializable]
    public class ScoreThreshold
    {
        public int score;
        public float meanTimeBetweenObstacles;
        public float timeVariance;
        public int maxObstacleIndex;
    }
}