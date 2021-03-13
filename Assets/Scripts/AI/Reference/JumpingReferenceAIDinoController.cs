using System;
using System.Linq;

namespace DefaultNamespace.AI
{
    public class JumpingReferenceAIDinoController : ReferenceAIDinoController
    {
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
                           bird.position.y <= minBirdHeightToCrouch)
                   || smallObstacles
                       .Any(obstacle =>
                           Math.Abs(obstacle.position.x - transform.position.x) <= smallObstacleDistanceToJump * rb.velocity.x / dinoMovement.initialSpeed &&
                           obstacle.position.x > transform.position.x);
        }

        protected override bool ShouldShortJump()
        {
            return false;
        }

        protected override bool ShouldCrouch()
        {
            return birds
                .Any(bird =>
                    Math.Abs(bird.position.x - transform.position.x) <= birdDistanceToCrouch * rb.velocity.x / dinoMovement.initialSpeed &&
                    bird.position.x > transform.position.x &&
                    bird.position.y > minBirdHeightToCrouch);
        }
    }
}