using UnityEngine;

namespace DefaultNamespace
{
    public class CrowdInputManager : MonoBehaviour
    {
        public static CrowdInputManager instance;
        private CrowdInputReliability crowdInputReliability;

        void Awake()
        {
            DontDestroyOnLoad(this);

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public CrowdInputReliability GetCrowdInputReliability(int numberOfPlayers)
        {
            if (crowdInputReliability != null && crowdInputReliability.numberOfPlayers == numberOfPlayers)
            {
                return crowdInputReliability;
            }

            crowdInputReliability = new CrowdInputReliability(numberOfPlayers, 2, 0.01f, 0.7f);
            return crowdInputReliability;
        }
    }
}