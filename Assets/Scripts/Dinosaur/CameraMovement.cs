using UnityEngine;

namespace DefaultNamespace
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float cameraForwardPosition = 5f;
        [SerializeField] private float moveSpeed = 5f;

        private Transform crowdDino;

        private void LateUpdate()
        {
            var newPosition = new Vector3(crowdDino.position.x + cameraForwardPosition, transform.position.y,
                transform.position.z);
            transform.position =  Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed);
        }

        private void OnEnable()
        {
            crowdDino = FindObjectOfType<DinoPosition>().transform;
        }
    }
}