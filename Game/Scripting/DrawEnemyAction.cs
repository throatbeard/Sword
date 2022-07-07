using Sword.Casting;
using Sword.Services;


namespace Sword.Scripting
{
    public class DrawEnemyAction : Action
    {
        private VideoService videoService;
        
        public DrawEnemyAction(VideoService videoService)
        {
            this.videoService = videoService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Enemy enemy = (Enemy)cast.GetFirstActor(Constants.ENEMY_GROUP);
            Body body = enemy.GetBody();

            if (enemy.IsDebug())
            {
                Rectangle rectangle = body.GetRectangle();
                Point size = rectangle.GetSize();
                Point pos = rectangle.GetPosition();
                videoService.DrawRectangle(size, pos, Constants.PURPLE, false);
            }

            Image image = enemy.GetImage();
            Point position = body.GetPosition();
            videoService.DrawImage(image, position);
        }
    }
}