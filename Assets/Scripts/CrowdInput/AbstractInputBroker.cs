using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using Reliability;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AbstractInputBroker : MonoBehaviour
    {
        protected ICrowdInputReliability crowdInputReliability;
        protected EvaluatorData evaluatorData;
        protected int numberOfPlayers;

        public abstract void SetUp(CrowdConfig config, int numberOfPlayers, InputReceiver receiver);

        public abstract void PostInput(PlayerInput input);
        
        
        private void Awake()
        {
            evaluatorData = GetComponent<EvaluatorData>();
        }

        public void ResetCrowdInputReliability()
        {
            crowdInputReliability = null;
        }
    }
}