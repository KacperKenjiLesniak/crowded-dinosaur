using GameEvents.Generic;
using UnityEngine;

namespace DefaultNamespace.Events
{
    [CreateAssetMenu(fileName = "PlayerInputGameEvent", menuName = "Game Events/PlayerInputGameEvent", order = 0)]
    public class PlayerInputGameEvent : ArgumentGameEvent<PlayerInput>
    {
    }
}