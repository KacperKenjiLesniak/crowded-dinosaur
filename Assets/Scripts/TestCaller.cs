using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.InteropServices;
using DefaultNamespace;

public class TestCaller : MonoBehaviour
{
    void Start()
    {
        var crowd = new CrowdInputReliability(3, 3);
        
        // Display output of functions
        print(crowd.IssueCommands(new []{0, 0, 1}));
        print(crowd.IssueCommands(new []{1, 1, 0}));
        foreach (var reliability in crowd.GetPlayerReliabilities())
        {
            print(reliability);
        }
    }
}