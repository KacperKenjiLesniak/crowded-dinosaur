using System;
using DefaultNamespace.AI;
using DefaultNamespace.Events;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class AddCustomAiButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField maxNoise;
        [SerializeField] private TMP_InputField noiseShift;
        [SerializeField] private AiConfigGameEvent createAiEvent;
        
        public void Click()
        {
            var aiConfig = ScriptableObject.CreateInstance<AiConfig>();
            try
            {
                aiConfig.maxNoise = float.Parse(maxNoise.text);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                aiConfig.maxNoise = 0f;
            }

            try
            {
                aiConfig.noiseShift = float.Parse(noiseShift.text);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                aiConfig.noiseShift = 0f;
            }
            createAiEvent.RaiseGameEvent(aiConfig);
        }
    }
}