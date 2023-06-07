using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.HUD.Elements;

public class BulletAmmo : IAmmoHUD {
    internal override Texture2D Texture() => this.TexturaVariable;
    internal override (float Ancho, float Alto) QuadSize() => (40 * scaleFactor.X, 15*scaleFactor.Y); //tamaño imágen
    internal override (float X, float Y) Ubicacion() => (95,-95 * Window.Heigth/Window.Width); //ubicación inicial

    private (float Width, float Heigth) Window;
    private (float X, float Y) scaleFactor => (250/(Window.Width) , 250/(Window.Heigth) /* * aspectRatio */);

    private int TotalAmmo = 30;
    public float ShootedAmmo = 0;
    public float Ammo = 0;

    private SoundEffect Sound = PistonDerby.GameContent.S_Metralleta;
    private SoundEffectInstance Instance;
    private Texture2D TexturaVariable; 
    private Matrix QuadWorld;

    
    public BulletAmmo(float width, float heigth){
        Window.Width = width;
        Window.Heigth = heigth;
        Instance = Sound.CreateInstance();
        Instance.Volume = 0.1f;
    }

    public void PullingTrigger(float dt){
        if(Ammo>0){
        if (Instance.State != SoundState.Playing){
            Instance.IsLooped = true;
            Instance.Play();
        }
        ShootedAmmo += 2*dt;
        }
        else Instance.Stop();
    }
    public void ReleasingTrigger() => Instance.Stop();
    public void Update(Vector3 followedPosition){
            Ammo = TotalAmmo - ShootedAmmo;
            QuadWorld = AjusteQuad() 
                        * Matrix.CreateScale(QuadSize().Ancho,QuadSize().Alto,0)
                        * Matrix.CreateTranslation(followedPosition);
    }
    public void Recargar(){
        ShootedAmmo = 0;
    }
    public void Draw(){
        (float Width, float Heigth) delta = (25,6); // Separacion entre balas
        float variableHeight = this.Ubicacion().Y;  

        delta.Width  *= scaleFactor.X;
        delta.Heigth *= scaleFactor.Y;

        int j = 0;
        for(int i = TotalAmmo; i>=0; i--){
            TexturaVariable = (i<Ammo)? 
                            PistonDerby.GameContent.TH_Bullet :     // primero dibuja las no vacias 
                            PistonDerby.GameContent.TH_EmptyBullet;            
            if(i==TotalAmmo*0.5f) {
                j++;
                variableHeight = this.Ubicacion().Y;
            }
            
            Matrix movimientoHorizontal = Matrix.CreateTranslation(Vector3.UnitX*(Ubicacion().X+delta.Width*j));
            Matrix movimientoVertical   = Matrix.CreateTranslation(Vector3.UnitY*(variableHeight));
            
            Matrix tempWorld = QuadWorld * (movimientoHorizontal * movimientoVertical)* AjusteFinal();
            
            this.Efecto().Parameters["Texture"].SetValue(this.Texture());
            this.Efecto().Parameters["World"].SetValue(tempWorld);
            PistonDerby.GameContent.G_Quad.Draw(this.Efecto());
            
            variableHeight+=delta.Heigth;
        }
    }
}