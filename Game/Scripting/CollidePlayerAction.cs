using Sword.Casting;
using Sword.Services;


namespace Sword.Scripting
{
    public class CollidePlayerAction : Action
    {
        private AudioService audioService;
        private PhysicsService physicsService;
        
        public CollidePlayerAction(PhysicsService physicsService, AudioService audioService)
        {
            this.physicsService = physicsService;
            this.audioService = audioService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Enemy enemy = (Enemy)cast.GetFirstActor(Constants.ENEMY_GROUP);
            Player player = (Player)cast.GetFirstActor(Constants.PLAYER_GROUP);
            Body enemyBody = enemy.GetBody();
            Body playerBody = player.GetBody();

            if (physicsService.HasCollided(playerBody, enemyBody))
            {
                enemy.BounceY();
                Sound sound = new Sound(Constants.BOUNCE_SOUND);
                audioService.PlaySound(sound);
            }
        }
    }
}