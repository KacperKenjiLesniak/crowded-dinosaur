using System;
using GameEvents.Game;
using GameEvents.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameEvent lostGameEvent;
        [SerializeField] private GameEvent restartGameEvent;
        [SerializeField] private TMP_Text lostGameText;
        [SerializeField] private string youLostText = "You lost!";

        public void LoseGame()
        {
            lostGameText.text = youLostText;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}