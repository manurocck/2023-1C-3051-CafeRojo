using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PistonDerby;

public class AudioPlayer
{
    private Song Soundtrack;
    internal AudioPlayer( ){
        MediaPlayer.IsMuted = false;
        MediaPlayer.IsRepeating = false;
        MediaPlayer.Volume = 0.05f;
    }
    internal void LoadContent(){
        Soundtrack = PistonDerby.GameContent.S_SynthWars;
        // MediaPlayer.Play(Soundtrack);
    }
    internal void Update(KeyboardState keyboardState)
    {
        // Control de la música
        // if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Stopped)
        //     MediaPlayer.Play(Soundtrack);
        // else if (keyboardState.IsKeyUp(Keys.W) && MediaPlayer.State == MediaState.Playing)
        //     MediaPlayer.Pause();
        // else if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Paused)
        //     MediaPlayer.Resume();
        // else if (keyboardState.IsKeyDown(Keys.P) && MediaPlayer.State == MediaState.Playing)
        //     MediaPlayer.Stop();
    }
}