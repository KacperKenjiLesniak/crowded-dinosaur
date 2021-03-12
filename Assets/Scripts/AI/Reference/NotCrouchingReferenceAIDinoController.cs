using System;
using System.Linq;

namespace DefaultNamespace.AI
{
    public class NotCrouchingReferenceAIDinoController : ReferenceAIDinoController
    {
        protected override bool ShouldLongJump()
        {
            return obstacles
                       .Any(obstacle =>
                           Math.Abs(obstacle.position.x - transform.position.x) <= obstacleDistanceToJump &&
                           obstacle.position.x > transform.position.x)
                   || birds
                       .Any(bird =>
                           Math.Abs(bird.position.x - transform.position.x) <= obstacleDistanceToJump &&
                           bird.position.x > transform.position.x &&
                           bird.position.y <= minBirdHeightToCrouch);
        }

        protected override bool ShouldShortJump()
        {
            return smallObstacles
                .Any(obstacle =>
                    Math.Abs(obstacle.position.x - transform.position.x) <= smallObstacleDistanceToJump &&
                    obstacle.position.x > transform.position.x);
        }

        protected override bool ShouldCrouch()
        {
            return birds
                .Any(bird =>
                    Math.Abs(bird.position.x - transform.position.x) <= birdDistanceToCrouch &&
                    bird.position.x > transform.position.x &&
                    bird.position.y > minBirdHeightToCrouch);
        }
    }
}