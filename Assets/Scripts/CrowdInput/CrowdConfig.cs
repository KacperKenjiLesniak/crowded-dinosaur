using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "CrowdConfig", menuName = "ScriptableObjects/CrowdConfig", order = 0)]
    public class CrowdConfig : ScriptableObject
    {
        public int numberOfCommands;
        public float reliabilityCoefficient;
        public float agreementThreshold;
        public float inputTimeToLive;
    }
}