using Sword.Casting;
using Sword.Services;


namespace Sword.Scripting
{
    public class CollideBordersAction : Action
    {
        private AudioService audioService;
        private PhysicsService physicsService;
        
        public CollideBordersAction(PhysicsService physicsService, AudioService audioService)
        {
            this.physicsService = physicsService;
            this.audioService = audioService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Enemy enemy = (Enemy)cast.GetFirstActor(Constants.ENEMY_GROUP);
            Body body = enemy.GetBody();
            Point position = body.GetPosition();
            int x = position.GetX();
            int y = position.GetY();
            Sound bounceSound = new Sound(Constants.BOUNCE_SOUND);
            Sound overSound = new Sound(Constants.OVER_SOUND);

            if (x < Constants.FIELD_LEFT)
            {
                enemy.BounceX();
                audioService.PlaySound(bounceSound);
            }
            else if (x >= Constants.FIELD_RIGHT - Constants.ENEMY_WIDTH)
            {
                enemy.BounceX();
                audioService.PlaySound(bounceSound);
            }

            if (y < Constants.FIELD_TOP)
            {
                enemy.BounceY();
                audioService.PlaySound(bounceSound);
            }
            else if (y >= Constants.FIELD_BOTTOM - Constants.ENEMY_WIDTH)
            {
                Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
                stats.RemoveLife();

                if (stats.GetLives() > 0)
                {
                    callback.OnNext(Constants.TRY_AGAIN);
                }
                else
                {
                    callback.OnNext(Constants.GAME_OVER);
                    audioService.PlaySound(overSound);
                }
            }
        }
    }
}