using System;
using System.Collections.Generic;
using System.IO;
using Sword.Casting;
using Sword.Scripting;
using Sword.Services;


namespace Sword.Directing
{
    public class SceneManager
    {
        public static AudioService AudioService = new RaylibAudioService();
        public static KeyboardService KeyboardService = new RaylibKeyboardService();
        public static MouseService MouseService = new RaylibMouseService();
        public static PhysicsService PhysicsService = new RaylibPhysicsService();
        public static VideoService VideoService = new RaylibVideoService(Constants.GAME_NAME,
            Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Constants.BLACK);

        public SceneManager()
        {
        }

        public void PrepareScene(string scene, Cast cast, Script script)
        {
            if (scene == Constants.NEW_GAME)
            {
                PrepareNewGame(cast, script);
            }
            // else if (scene == Constants.NEXT_SCORE)
            // {
            //     PrepareNextScore(cast, script);
            // }
            else if (scene == Constants.TRY_AGAIN)
            {
                PrepareTryAgain(cast, script);
            }
            else if (scene == Constants.IN_PLAY)
            {
                PrepareInPlay(cast, script);
            }
            else if (scene == Constants.GAME_OVER)
            {
                PrepareGameOver(cast, script);
            }
        }

        private void PrepareNewGame(Cast cast, Script script)
        {
            AddStats(cast);
            //AddScore(cast);
            //AddScore(cast);
            AddLives(cast);
            AddEnemy(cast);
            //AddBricks(cast);
            AddPlayer(cast);
            AddDialog(cast, Constants.ENTER_TO_START);

            script.ClearAllActions();
            AddInitActions(script);
            AddLoadActions(script);

            //ChangeSceneAction a = new ChangeSceneAction(KeyboardService, Constants.NEXT_SCORE);
            //script.AddAction(Constants.INPUT, a);

            AddOutputActions(script);
            AddUnloadActions(script);
            AddReleaseActions(script);
        }

        // private void PrepareNextScore(Cast cast, Script script)
        // {
        //     AddEnemy(cast);
        //     AddBricks(cast);
        //     AddPlayer(cast);
        //     AddDialog(cast, Constants.PREP_TO_LAUNCH);

        //     script.ClearAllActions();

        //     TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
        //     script.AddAction(Constants.INPUT, ta);

        //     AddOutputActions(script);

        //     PlaySoundAction sa = new PlaySoundAction(AudioService, Constants.WELCOME_SOUND);
        //     script.AddAction(Constants.OUTPUT, sa);
        // }
        private void PlaceEnemies(Cast cast, Script script)
        {

        }
        
        private void PrepareTryAgain(Cast cast, Script script)
        {
            AddEnemy(cast);
            AddPlayer(cast);
            AddDialog(cast, Constants.PREP_TO_LAUNCH);

            script.ClearAllActions();
            
            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);
            
            AddUpdateActions(script);
            AddOutputActions(script);
        }

        private void PrepareInPlay(Cast cast, Script script)
        {
            //ActivateEnemy(cast);
            cast.ClearActors(Constants.DIALOG_GROUP);

            script.ClearAllActions();

            ControlPlayerAction action = new ControlPlayerAction(KeyboardService);
            script.AddAction(Constants.INPUT, action);

            AddUpdateActions(script);    
            AddOutputActions(script);
        
        }

        private void PrepareGameOver(Cast cast, Script script)
        {
            AddEnemy(cast);
            AddPlayer(cast);
            AddDialog(cast, Constants.WAS_GOOD_GAME);

            script.ClearAllActions();

            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.NEW_GAME, 5, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);

            AddOutputActions(script);
        }

        // -----------------------------------------------------------------------------------------
        // casting methods
        // -----------------------------------------------------------------------------------------

        private void AddEnemy(Cast cast)
        {
            cast.ClearActors(Constants.ENEMY_GROUP);
        
            int x = Constants.CENTER_X - Constants.ENEMY_WIDTH / 2;
            int y = Constants.SCREEN_HEIGHT - Constants.ENEMY_HEIGHT;
        
            Point position = new Point(x, y);
            Point size = new Point(Constants.ENEMY_WIDTH, Constants.ENEMY_HEIGHT);
            Point velocity = new Point(0, 0);
        
            Body body = new Body(position, size, velocity);
            Image image = new Image(Constants.ENEMY_IMAGE);
            Enemy enemy = new Enemy(body, image, false);
        
        
            cast.AddActor(Constants.ENEMY_GROUP, enemy);
        }

        /*private void AddBricks(Cast cast)
        {
            cast.ClearActors(Constants.BRICK_GROUP);

            Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
            int level = stats.GetScore() % Constants.BASE_SCORES;
            string filename = string.Format(Constants.SCORE_FILE, level);
            List<List<string>> rows = LoadScore(filename);

            for (int r = 0; r < rows.Count; r++)
            {
                for (int c = 0; c < rows[r].Count; c++)
                {
                    int x = Constants.FIELD_LEFT + c * Constants.BRICK_WIDTH;
                    int y = Constants.FIELD_TOP + r * Constants.BRICK_HEIGHT;

                    string color = rows[r][c][0].ToString();
                    int frames = (int)Char.GetNumericValue(rows[r][c][1]);
                    int points = Constants.BRICK_POINTS;

                    Point position = new Point(x, y);
                    Point size = new Point(Constants.BRICK_WIDTH, Constants.BRICK_HEIGHT);
                    Point velocity = new Point(0, 0);
                    List<string> images = Constants.BRICK_IMAGES[color].GetRange(0, frames);

                    Body body = new Body(position, size, velocity);
                    Animation animation = new Animation(images, Constants.BRICK_RATE, 1);
                    
                    Brick brick = new Brick(body, animation, points, false);
                    cast.AddActor(Constants.BRICK_GROUP, brick);
                }
            }
        }*/

        private void AddDialog(Cast cast, string message)
        {
            cast.ClearActors(Constants.DIALOG_GROUP);

            Text text = new Text(message, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.CENTER_Y);

            Label label = new Label(text, position);
            cast.AddActor(Constants.DIALOG_GROUP, label);   
        }

        private void AddScore(Cast cast)
        {
            cast.ClearActors(Constants.SCORE_GROUP);

            Text text = new Text(Constants.SCORE_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_LEFT, Constants.WHITE);
            Point position = new Point(Constants.HUD_MARGIN, Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.SCORE_GROUP, label);
        }

        private void AddLives(Cast cast)
        {
            cast.ClearActors(Constants.HEALTH_GROUP);

            Text text = new Text(Constants.HEALTH_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_RIGHT, Constants.WHITE);
            Point position = new Point(Constants.SCREEN_WIDTH - Constants.HUD_MARGIN, 
                Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.HEALTH_GROUP, label);   
        }

        private void AddPlayer(Cast cast)
        {
            cast.ClearActors(Constants.PLAYER_GROUP);
        
            int x = Constants.CENTER_X - Constants.PLAYER_WIDTH / 2;
            int y = Constants.SCREEN_HEIGHT - Constants.PLAYER_HEIGHT;
        
            Point position = new Point(x, y);
            Point size = new Point(Constants.PLAYER_WIDTH, Constants.PLAYER_HEIGHT);
            Point velocity = new Point(0, 0);
        
            Body body = new Body(position, size, velocity);
            Animation animation = new Animation(Constants.PLAYER_IMAGES, Constants.PLAYER_RATE, 0);
            Player player = new Player(body, animation, false);
        
            cast.AddActor(Constants.PLAYER_GROUP, player);
        }

        /*private void AddScore(Cast cast)
        {
            cast.ClearActors(Constants.SCORE_GROUP);

            Text text = new Text(Constants.SCORE_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.HUD_MARGIN);
            
            Label label = new Label(text, position);
            cast.AddActor(Constants.SCORE_GROUP, label);   
        }*/

        private void AddStats(Cast cast)
        {
            cast.ClearActors(Constants.STATS_GROUP);
            Stats stats = new Stats();
            cast.AddActor(Constants.STATS_GROUP, stats);
        }

        private List<List<string>> LoadScore(string filename)
        {
            List<List<string>> data = new List<List<string>>();
            using(StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    List<string> columns = new List<string>(row.Split(',', StringSplitOptions.TrimEntries));
                    data.Add(columns);
                }
            }
            return data;
        }

        // -----------------------------------------------------------------------------------------
        // scriptig methods
        // -----------------------------------------------------------------------------------------

        private void AddInitActions(Script script)
        {
            script.AddAction(Constants.INITIALIZE, new InitializeDevicesAction(AudioService, 
                VideoService));
        }

        private void AddLoadActions(Script script)
        {
            script.AddAction(Constants.LOAD, new LoadAssetsAction(AudioService, VideoService));
        }

        private void AddOutputActions(Script script)
        {
            script.AddAction(Constants.OUTPUT, new StartDrawingAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawHudAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawEnemyAction(VideoService));
            //script.AddAction(Constants.OUTPUT, new DrawBricksAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawPlayerAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawDialogAction(VideoService));
            script.AddAction(Constants.OUTPUT, new EndDrawingAction(VideoService));
        }

        private void AddUnloadActions(Script script)
        {
            script.AddAction(Constants.UNLOAD, new UnloadAssetsAction(AudioService, VideoService));
        }

        private void AddReleaseActions(Script script)
        {
            script.AddAction(Constants.RELEASE, new ReleaseDevicesAction(AudioService, 
                VideoService));
        }

        private void AddUpdateActions(Script script)
        {
            script.AddAction(Constants.UPDATE, new MoveEnemyAction());
            script.AddAction(Constants.UPDATE, new MovePlayerAction());
            script.AddAction(Constants.UPDATE, new CollideBordersAction(PhysicsService, AudioService));
            //script.AddAction(Constants.UPDATE, new CollideBrickAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CollidePlayerAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CheckOverAction());     
        }
    }
}