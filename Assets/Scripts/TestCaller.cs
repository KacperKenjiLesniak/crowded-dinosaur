using System;
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
        var crowd = new CrowdInputReliability(3, 3);
        
        // Display output of functions
        print(displaySum());
        print(crowd.IssueCommands(new []{0, 0, 1}));
        print(crowd.IssueCommands(new []{1, 1, 0}));
    }
}