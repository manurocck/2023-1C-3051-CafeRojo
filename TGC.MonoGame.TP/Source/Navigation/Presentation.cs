using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PistonDerby.Navigation;

internal class Presentation : IMenuItem{
    private IMenuItem MainMenu;
    private Effect efecto = PistonDerby.GameContent.E_TwoTextureMix;
    private Matrix World;
    private bool PressedKeys = false;

    private const int PRESENTATION_LENGTH = END_TRANS4 + 3;
    private const int   START_TRANS1 = 3, START_TRANS2 = 2 + START_TRANS1, START_TRANS3 = 1 + START_TRANS2, 
                        START_TRANS4 = 3 + START_TRANS3,  END_TRANS4 = 4 + START_TRANS4;

    public Presentation(int width, int heigth) : base(width, heigth){
        World = FullScreenWorld();

        MediaPlayer.IsRepeating = false;
        
    }
    internal override IMenuItem Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState){
        PressedKeys = (keyboardState.GetPressedKeyCount() > 0)? true : PressedKeys;
        Console.WriteLine("Ancho y largo de pantalla : ({0:F}:{1:F})",Window.Heigth, Window.Width);
        if(PressedKeys) { return new MainMenu(this.Window.Width, Window.Heigth);}
        
        return this;
    } 
    internal override bool Draw(float secondsElapsed){
        // FONDO PANTALLA COMPLETA
        if(secondsElapsed < PRESENTATION_LENGTH){
            efecto.Parameters["World"].SetValue(World);
            efecto.Parameters["View"]?.SetValue(HUDView);

            if(secondsElapsed < START_TRANS1){ // Black Screen
                if (MediaPlayer.State == MediaState.Stopped)
                    MediaPlayer.Play(PistonDerby.GameContent.S_MeiHuaSan);
                efecto.Parameters["LerpAmount"]?.SetValue(0);
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.TP_Presentacion0);
                efecto.Parameters["Texture2"]?.SetValue(PistonDerby.GameContent.TP_Presentacion0);
            }else if(secondsElapsed < START_TRANS2){ // TRANS1 : CAFE aparece
                efecto.Parameters["LerpAmount"]?.SetValue(0);
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.TP_Presentacion1);
            } else if(secondsElapsed < START_TRANS3){ // TRANS2 : CAFE ROJO se muestra
                efecto.Parameters["LerpAmount"]?.SetValue((secondsElapsed-START_TRANS2)/(START_TRANS3-START_TRANS2));
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.TP_Presentacion1);
                efecto.Parameters["Texture2"]?.SetValue(PistonDerby.GameContent.TP_Presentacion2);
            } else if(secondsElapsed < START_TRANS4){ // TRANS3 : MANTENGO
                efecto.Parameters["LerpAmount"]?.SetValue(1);
            } else if(secondsElapsed < END_TRANS4){ // TRANS4 : HUMITO
                efecto.Parameters["LerpAmount"]?.SetValue(Math.Min((secondsElapsed-START_TRANS4)/(START_TRANS4-START_TRANS3),1));
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.TP_Presentacion2);
                efecto.Parameters["Texture2"]?.SetValue(PistonDerby.GameContent.TP_Presentacion3);
            }
            PistonDerby.GameContent.G_Quad.Draw(efecto);

            return (true && !PressedKeys) ;
        }
        PressedKeys = true;
        return true;
    }
}

