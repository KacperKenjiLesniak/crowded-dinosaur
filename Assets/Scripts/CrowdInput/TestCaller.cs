﻿using System;
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
        var crowd = new CrowdInputReliability(3, 3, 0.1f, 0.5f);
        var crowd2 = new CrowdInputReliability(10, 3, 0.1f, 0.5f);

        // Display output of functions
        print(crowd.IssueCommands(new []{0, 0, 1}));
        print(crowd.IssueCommands(new []{1, 1, 0}));

        foreach (var reliability in crowd.GetPlayerReliabilities())
        {
            print(reliability);
        }
        
        print("crowd2");
        print(crowd2.IssueCommands(new []{0, 0, 0, 0 , 0, 1, 1, 1, 1, 2}));
        foreach (var reliability in crowd2.GetPlayerReliabilities())
        {
            print(reliability);
        }
    }
}