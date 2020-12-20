using Photon.Pun;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text lostGameText;
        [SerializeField] private string youLostText = "You lost!";

        public void LoseGame()
        {
            lostGameText.text = youLostText;
        }

        public void Restart()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("EmptyScene");
            }
        }
    }
}