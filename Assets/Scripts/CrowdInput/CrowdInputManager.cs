using UnityEngine;

namespace DefaultNamespace
{
    public class CrowdInputManager : MonoBehaviour
    {
        public static CrowdInputManager instance;

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
    }
}