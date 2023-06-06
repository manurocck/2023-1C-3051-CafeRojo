using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PistonDerby;

public class AudioPlayer
{
    private SoundEffectInstance Motor;
    private SoundEffectInstance MotorAcelerando;
    private bool Stopping = false;
    private bool Playing = false;
    private Song Soundtrack;
    internal AudioPlayer( ){
        MediaPlayer.IsMuted = false;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.05f;
    }
    internal void LoadContent(){
        Soundtrack = PistonDerby.GameContent.S_SynthWars;
        // MediaPlayer.Play(Soundtrack);
    }
    public void EngineCar() {
        MediaPlayer.Volume = 0.4f;
        PistonDerby.GameContent.S_MotorEncendido.Play(0.3f, 1,0);
        Motor = PistonDerby.GameContent.S_MotorRegulando.CreateInstance();
        Motor.IsLooped = true;
        Motor.Volume = 0.5f;
        Motor.Play();

        MotorAcelerando =  PistonDerby.GameContent.S_MotorAcelerando.CreateInstance();
        MotorAcelerando.IsLooped = true;
        MotorAcelerando.Volume = 0;
        MotorAcelerando.Play();

        Soundtrack = PistonDerby.GameContent.S_ItsAFight;
        MediaPlayer.IsRepeating = true;
        this.Play();
    }
    public void Stop() => Stopping = true;
    public void Play() {
        MediaPlayer.Volume = 0.5f;
        MediaPlayer.Play(Soundtrack);
        Playing = true;
    } 
    internal void Update(float dTime, KeyboardState keyboardState)
    {
        
        if(Motor.State == SoundState.Playing)
            Motor.Volume = Math.Min(Motor.Volume + 0.1f*dTime*3, 0.1f);

        if(keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.S)){
            if(MotorAcelerando.State == SoundState.Stopped) MotorAcelerando.Play();
            MotorAcelerando.Volume = Math.Min(MotorAcelerando.Volume + 0.1f*dTime*3, 0.15f);
            Motor.Volume = Math.Max(Motor.Volume - 0.1f*dTime*3, 0f);
        }else{
            MotorAcelerando.Volume = Math.Max(MotorAcelerando.Volume - 0.1f*dTime*3, 0f);
            Motor.Volume = Math.Min(Motor.Volume + 0.1f*dTime*3, 0.1f);
        }

        if(MotorAcelerando.Volume == 0) MotorAcelerando.Stop();

        if(Stopping) {
            MediaPlayer.Volume = Math.Max(MediaPlayer.Volume-0.01f, 0);
            if(MediaPlayer.Volume == 0) Stopping = false;
        }else if(Playing){
            MediaPlayer.Volume = Math.Min(MediaPlayer.Volume+0.1f*dTime, 0.5f);
            if(MediaPlayer.Volume == 0) Playing = false;
        }
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