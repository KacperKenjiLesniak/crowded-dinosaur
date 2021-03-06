using System;
using UnityEngine;

namespace DefaultNamespace.Utils
{
    public class CheatCodes : MonoBehaviour
    {
        private bool timeSlow;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                timeSlow = !timeSlow;
                Time.timeScale = timeSlow ? 0.1f : 1f;
            }
        }
    }
}