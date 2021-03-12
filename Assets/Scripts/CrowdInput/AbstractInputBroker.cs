using DefaultNamespace.Events;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AbstractInputBroker : MonoBehaviour
    {
        public abstract void SetUp(CrowdConfig config, int numberOfPlayers, int numberOfReferenceAis, InputReceiver receiver);

        public abstract void PostInput(PlayerInput input);
    }
}