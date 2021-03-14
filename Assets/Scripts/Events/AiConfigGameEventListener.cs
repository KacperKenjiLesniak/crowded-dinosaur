using DefaultNamespace.AI;
using GameEvents.Generic;
using UnityEngine;

namespace DefaultNamespace.Events
{
    [AddComponentMenu("Game Events/AiConfigGameEventListener")]
    public class
        AiConfigGameEventListener : ArgumentGameEventListener<AiConfigGameEvent, AiConfigEvent, AiConfig>
    {
    }
}