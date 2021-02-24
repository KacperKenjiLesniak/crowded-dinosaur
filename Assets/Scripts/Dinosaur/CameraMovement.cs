using UnityEngine;

namespace DefaultNamespace
{
    public class CameraMovement : MonoBehaviour
    {
        private Transform crowdDino;

        private void OnEnable()
        {
            crowdDino = FindObjectOfType<DinoPosition>().transform;
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(crowdDino.position.x, transform.position.y, transform.position.z);
        }
    }
}