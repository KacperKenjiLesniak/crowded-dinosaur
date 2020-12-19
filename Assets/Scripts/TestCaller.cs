using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using DefaultNamespace;

public class TestCaller : MonoBehaviour
{
    
    // Import and expose native c++ functions
    [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "displayNumber")]
    public static extern int displayNumber();
    
    [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "getRandom")]
    public static extern int getRandom();
    
    [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "displaySum")]
    public static extern int displaySum();
    
    void Start()
    {
        var crowd = new CrowdInputReliability();
        
        // Display output of functions
        print(crowd.GetNumber(4));
        print(crowd.GetNumber(4));
    }
}