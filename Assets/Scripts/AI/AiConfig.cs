using System;
using UnityEngine;

namespace DefaultNamespace.AI
{
    [Serializable]
    [CreateAssetMenu(fileName = "AiConfig", menuName = "ScriptableObjects/AiConfig", order = 0)]
    public class AiConfig : ScriptableObject
    {
        public float maxNoise;
        public float noiseShift;
    }
}