using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.AI
{
    [CreateAssetMenu(fileName = "AiList", menuName = "ScriptableObjects/AiList", order = 0)]
    public class AiList : ScriptableObject
    {
        public List<AiConfig> aiConfigs;
        public List<ReferenceAiConfig> referenceAiConfigs;
    }
}