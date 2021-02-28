using System;

namespace DefaultNamespace.AI
{
    [Serializable]
    public class AiConfig
    {
        public AiConfig(float jumpNoise)
        {
            this.jumpNoise = jumpNoise;
        }

        public float jumpNoise { get; }
    }
}