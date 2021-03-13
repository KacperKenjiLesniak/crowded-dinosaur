using System;
using System.Linq;

namespace DefaultNamespace.AI
{
    public class NotCrouchingReferenceAIDinoController : ReferenceAIDinoController
    {
        protected float minBirdHeightToCrouch = -1.7f;
        private float maxBirdHeightToCrouch = -1.35f;

        protected override bool ShouldLongJump()
        {
            return obstacles
                       .Any(obstacle =>
                           Math.Abs(obstacle.position.x - transform.position.x) <= obstacleDistanceToJump * rb.velocity.x / dinoMovement.initialSpeed &&
                           obstacle.position.x > transform.position.x)
                   || birds
                       .Any(bird =>
                           Math.Abs(bird.position.x - transform.position.x) <= obstacleDistanceToJump * rb.velocity.x / dinoMovement.initialSpeed &&
                           bird.position.x > transform.position.x &&
                           bird.position.y <= minBirdHeightToCrouch);
        }

        protected override bool ShouldShortJump()
        {
            return smallObstacles
                .Any(obstacle =>
                    Math.Abs(obstacle.position.x - transform.position.x) <= smallObstacleDistanceToJump * rb.velocity.x / dinoMovement.initialSpeed &&
                    obstacle.position.x > transform.position.x);
        }

        protected override bool ShouldCrouch()
        {
            return birds
                .Any(bird =>
                    Math.Abs(bird.position.x - transform.position.x) <= birdDistanceToCrouch * rb.velocity.x / dinoMovement.initialSpeed &&
                    bird.position.x > transform.position.x &&
                    bird.position.y > minBirdHeightToCrouch &&
                    bird.position.y <= maxBirdHeightToCrouch);
        }
    }
}