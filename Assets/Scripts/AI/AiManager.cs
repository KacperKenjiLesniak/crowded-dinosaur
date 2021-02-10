using System;
using System.Collections.Generic;
using MutableObjects.Int;
using UnityEngine;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(PlayerManager))]
    public class AiManager : MonoBehaviour
    {
        [SerializeField] private AiList aiList;

        public void AddAi(float noise)
        {
            aiList.aiConfigs.Add(new AiConfig(noise));
        }

        public void ClearAis()
        {
            aiList.aiConfigs.Clear();
        }

        public void CreateAis()
        {
            GetComponent<PlayerManager>().CreateAIControllers(aiList.aiConfigs);
        }
    }
}