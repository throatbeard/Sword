using Sword.Casting;
using Sword.Services;


namespace Sword.Scripting
{
    public class DrawHudAction : Action
    {
        private VideoService videoService;
        
        public DrawHudAction(VideoService videoService)
        {
            this.videoService = videoService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
            //DrawLabel(cast, Constants.SCORE_GROUP, Constants.SCORE_FORMAT, stats.GetLevel());
            //DrawLabel(cast, Constants.HEALTH_GROUP, Constants.HEALTH_FORMAT, stats.GetLives());
            //DrawLabel(cast, Constants.SCORE_GROUP, Constants.SCORE_FORMAT, stats.GetScore());
        }

        private void DrawLabel(Cast cast, string group, string format, int data)
        {
            Label label = (Label)cast.GetFirstActor(group);
            Text text = label.GetText();
            text.SetValue(string.Format(format, data));
            Point position = label.GetPosition();
            videoService.DrawText(text, position);
        }
    }
}