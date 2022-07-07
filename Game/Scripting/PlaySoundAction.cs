using Sword.Casting;
using Sword.Services;


namespace Sword.Scripting
{
    public class PlaySoundAction : Action
    {
        private AudioService audioService;
        private string filename;

        public PlaySoundAction(AudioService audioService, string filename)
        {
            this.audioService = audioService;
            this.filename = filename;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Sound sound = new Sound(filename);
            audioService.PlaySound(sound);
            script.RemoveAction(Constants.OUTPUT, this);
        }
    }
}