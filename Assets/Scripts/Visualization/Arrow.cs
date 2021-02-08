using System;
using System.Security.Cryptography;
using UnityEngine;

namespace DefaultNamespace.Visualization
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float speed;

        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var color = spriteRenderer.color;
            color.a -= Time.deltaTime;
            spriteRenderer.color = color;

            if (color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}