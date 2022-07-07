using Sword.Casting;

namespace Sword.Scripting
{
    public class MovePlayerAction : Action
    {
        public MovePlayerAction()
        {
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Player player = (Player)cast.GetFirstActor(Constants.PLAYER_GROUP);
            Body body = player.GetBody();
            Point position = body.GetPosition();
            Point velocity = body.GetVelocity();
            int x = position.GetX();

            position = position.Add(velocity);
            if (x < 0)
            {
                position = new Point(0, position.GetY());
            }
            else if (x > Constants.SCREEN_WIDTH - Constants.PLAYER_WIDTH)
            {
                position = new Point(Constants.SCREEN_WIDTH - Constants.PLAYER_WIDTH, 
                    position.GetY());
            }

            body.SetPosition(position);       
        }
    }
}