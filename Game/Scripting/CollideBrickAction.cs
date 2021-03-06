using System.Collections.Generic;
using Sword.Casting;
using Sword.Services;


namespace Sword.Scripting
{
    public class CollideBrickAction : Action
    {
        private AudioService audioService;
        private PhysicsService physicsService;
        
        public CollideBrickAction(PhysicsService physicsService, AudioService audioService)
        {
            this.physicsService = physicsService;
            this.audioService = audioService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Enemy enemy = (Enemy)cast.GetFirstActor(Constants.ENEMY_GROUP);
            List<Actor> bricks = cast.GetActors(Constants.BRICK_GROUP);
            Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
            
            foreach (Actor actor in bricks)
            {
                Brick brick = (Brick)actor;
                Body brickBody = brick.GetBody();
                Body enemyBody = enemy.GetBody();

                if (physicsService.HasCollided(brickBody, enemyBody))
                {
                    enemy.BounceY();
                    Sound sound = new Sound(Constants.BOUNCE_SOUND);
                    audioService.PlaySound(sound);
                    int points = brick.GetPoints();
                    stats.AddPoints(points);
                    cast.RemoveActor(Constants.BRICK_GROUP, brick);
                }
            }
        }
    }
}