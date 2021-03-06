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

        private void Awake()
        {
            score.Value = 0;
            initialPosition = 0f;
        }

        private void LateUpdate()
        {
            score.Value = (int) Mathf.Floor(dinoPosition.Value.x - initialPosition);
            scoreText.Value = score.Value.ToString();
        }
    }
}