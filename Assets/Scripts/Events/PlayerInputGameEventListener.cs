using GameEvents.Generic;
using UnityEngine;

namespace DefaultNamespace.Events
{

    [AddComponentMenu("Game Events/PlayerInputGameEventListener")]
    public class PlayerInputGameEventListener : ArgumentGameEventListener<PlayerInputGameEvent, PlayerInputEvent, PlayerInput>
    {
    }
}