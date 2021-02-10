using MutableObjects.Int;
using MutableObjects.String;
using MutableObjects.Vector3;
using UnityEngine;

namespace DefaultNamespace.Score
{
    public class Scorer : MonoBehaviour
    {
        [SerializeField] private MutableVector3 dinoPosition;
        [SerializeField] private MutableString scoreText;
        [SerializeField] private MutableInt score;

        private float initialPosition;

        private void Start()
        {
            initialPosition = dinoPosition.Value.x;
        }

        private void Update()
        {
            score.Value = (int) Mathf.Floor(dinoPosition.Value.x - initialPosition);
            scoreText.Value = score.Value.ToString();
        }
    }
}