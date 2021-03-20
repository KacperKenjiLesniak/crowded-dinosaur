using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AbstractInputBroker : MonoBehaviour
    {
        protected CrowdInputReliability crowdInputReliability;
        protected EvaluatorData evaluatorData;

        public abstract void SetUp(CrowdConfig config, int numberOfPlayers, int numberOfReferenceAis, InputReceiver receiver);

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