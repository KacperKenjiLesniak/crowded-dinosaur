using MutableObjects.Bool;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOnServerToggle : MonoBehaviour
{
    [SerializeField] private MutableBool playerOnMasterServer;
    
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }
        
        var toggle = GetComponent<Toggle>();
    
        int count = toggle.onValueChanged.GetPersistentEventCount();
        
        for (var i = 0; i < count; i++)
        {
            toggle.onValueChanged.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.Off);
        }
    
        toggle.isOn = playerOnMasterServer.Value;
              
        for (var i = 0; i < count; i++)
        {
            toggle.onValueChanged.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }
    }
    
    public void Toggle(bool value)
    {
        playerOnMasterServer.Value = value;
    }
}
