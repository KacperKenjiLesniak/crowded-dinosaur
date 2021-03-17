using System;
using DefaultNamespace.AI;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private CrowdConfig crowdConfig;
        [SerializeField] private TMP_InputField inputTimeToLive;
        [SerializeField] private TMP_InputField reliabilityCoefficient;
        [SerializeField] private TMP_InputField agreementThreshold;

        private void Start()
        {
            inputTimeToLive.text = crowdConfig.inputTimeToLive.ToString("F");
            reliabilityCoefficient.text = crowdConfig.reliabilityCoefficient.ToString("F");
            agreementThreshold.text = crowdConfig.agreementThreshold.ToString("F");
        }

        public void SaveSettings()
        {
            crowdConfig.inputTimeToLive = float.Parse(inputTimeToLive.text);
            crowdConfig.reliabilityCoefficient = float.Parse(reliabilityCoefficient.text);
            crowdConfig.agreementThreshold = float.Parse(agreementThreshold.text);
        }
    }
}