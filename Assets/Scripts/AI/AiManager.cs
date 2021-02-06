using System.Collections.Generic;
using MutableObjects.Int;
using UnityEngine;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(PlayerManager))]
    public class AiManager : MonoBehaviour
    {
        [SerializeField] private MutableInt numberOfAis;

        private List<AiConfig> aiConfigs = new List<AiConfig>();

        public void AddAi(float noise)
        {
            aiConfigs.Add(new AiConfig(noise));
            numberOfAis.Value += 1;
        }

        public void ClearAis()
        {
            aiConfigs.Clear();
        }

        public void CreateAis()
        {
            GetComponent<PlayerManager>().CreateAIControllers(aiConfigs);
        }
    }
}