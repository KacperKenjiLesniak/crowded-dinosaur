using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class FrameToggle : MonoBehaviour
{
    [SerializeField] private GameObject dynamicFrameInputBroker;
    [SerializeField] private GameObject staticFrameInputBroker;

    private void Start()
    {
        var toggle = GetComponent<Toggle>();
        
        int count = toggle.onValueChanged.GetPersistentEventCount();
        
        for (var i = 0; i < count; i++)
        {
            toggle.onValueChanged.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.Off);
        }
        
        toggle.isOn = CrowdInputManager.instance.gameObject.GetComponent<DynamicFrameInputBroker>() != null;
              
        for (var i = 0; i < count; i++)
        {
            toggle.onValueChanged.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }

    }

    public void Toggle(bool isDynamic)
    {
        Destroy(CrowdInputManager.instance.gameObject);
        CrowdInputManager.instance = null;
        Instantiate(isDynamic ? dynamicFrameInputBroker : staticFrameInputBroker);
    }
}