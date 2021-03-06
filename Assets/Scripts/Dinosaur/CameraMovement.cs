using UnityEngine;

namespace DefaultNamespace
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float cameraForwardPosition = 5f;

        private Transform crowdDino;

        private void Update()
        {
            transform.position = new Vector3(crowdDino.position.x + cameraForwardPosition, transform.position.y,
                transform.position.z);
        }

        private void OnEnable()
        {
            crowdDino = FindObjectOfType<DinoPosition>().transform;
        }
    }
}