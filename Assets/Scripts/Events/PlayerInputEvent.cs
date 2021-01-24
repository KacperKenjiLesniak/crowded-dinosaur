using System;
using UnityEngine.Events;

namespace DefaultNamespace.Events
{
    [Serializable]
    public class PlayerInputEvent : UnityEvent<PlayerInput>
    {
    }
}