using System;
using GameEvents.Game;
using GameEvents.Generic;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.Events
{
    public class EventBroadcaster : MonoBehaviourPunCallbacks, IGameEventListener
    {
        [SerializeField] private GameEvent gameEvent;

        private void Start()
        {
            gameEvent.RegisterListener(this);
        }

        public void RaiseGameEvent()
        {
            photonView.RPC(nameof(RaiseGameEventRpc), RpcTarget.Others);
        }

        [PunRPC]
        private void RaiseGameEventRpc()
        {
            gameEvent.UnregisterListener(this);
            gameEvent.RaiseGameEvent();
            gameEvent.RegisterListener(this);
        }

        private void OnDestroy()
        {
            gameEvent.UnregisterListener(this);
        }
    }
}