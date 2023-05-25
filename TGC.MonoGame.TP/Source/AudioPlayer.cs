using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TGC.MonoGame.TP;

public class AudioPlayer
{
    private Song Soundtrack;
    internal AudioPlayer( ){
        MediaPlayer.IsMuted = true;
        MediaPlayer.IsRepeating = true;
        // MediaPlayer.Volume = 0.5f;
    }
    internal void LoadContent(){
        Soundtrack = TGCGame.GameContent.S_SynthWars;
        MediaPlayer.Play(Soundtrack);
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