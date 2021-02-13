using System;
using MutableObjects.Vector3;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private MutableVector3 dinoPosition;

        private void FixedUpdate()
        {
            transform.position = new Vector3(dinoPosition.Value.x, transform.position.y, transform.position.z);
        }
    }
}