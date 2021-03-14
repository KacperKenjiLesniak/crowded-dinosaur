using System;
using DefaultNamespace.AI;
using UnityEngine.Events;

namespace DefaultNamespace.Events
{
    [Serializable]
    public class AiConfigEvent : UnityEvent<AiConfig>
    {
    }
}