using DefaultNamespace.AI;
using GameEvents.Generic;
using UnityEngine;

namespace DefaultNamespace.Events
{
    [CreateAssetMenu(fileName = "AiConfigGameEvent", menuName = "Game Events/AiConfigGameEvent", order = 0)]
    public class AiConfigGameEvent : ArgumentGameEvent<AiConfig>
    {
    }
}