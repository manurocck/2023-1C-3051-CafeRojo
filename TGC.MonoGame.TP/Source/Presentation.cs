using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace PistonDerby.Navigation;

internal class Presentation{
    internal static Matrix AjusteQuad() => Matrix.CreateTranslation(new Vector3(-0.5f,0,-0.5f)) //centrar 
                                         * Matrix.CreateRotationX(MathHelper.PiOver2) ;         //levantar
    internal static Matrix AjusteFinal(int z) => Matrix.CreateTranslation(-Vector3.UnitZ * (200f+z*5f));    //standard
    internal Matrix QuadSize(float X,float Y) => Matrix.CreateScale(X * scaleFactor.X , Y *scaleFactor.Y , 0); //tamaño imágen
    private (float X, float Y) scaleFactor;
    private Matrix World, HUDView;

    private const int PRESENTATION_LENGTH = END_TRANS4 + 3;
    private const int   START_TRANS1 = 3, START_TRANS2 = 2 + START_TRANS1, START_TRANS3 = 1 + START_TRANS2, 
                        START_TRANS4 = 3 + START_TRANS3,  END_TRANS4 = 4 + START_TRANS4;

    public Presentation(int width, int heigth){
        scaleFactor = ((width)*0.2f , (heigth)*0.2f);

        World =      AjusteQuad()
                    * QuadSize(1,1)
                    * AjusteFinal(1);

        HUDView = Matrix.CreateLookAt(Vector3.Zero,- Vector3.UnitZ, Vector3.UnitY);
        MediaPlayer.IsRepeating = false;
    }
    public void Draw(float secondsElapsed){
        // FONDO PANTALLA COMPLETA
        if(secondsElapsed < PRESENTATION_LENGTH){
            Effect efecto = PistonDerby.GameContent.E_TwoTextureMix;
            efecto.Parameters["World"].SetValue(World);
            efecto.Parameters["View"]?.SetValue(HUDView);

            if(secondsElapsed < START_TRANS1){ // Black Screen
                if (MediaPlayer.State == MediaState.Stopped)
                    MediaPlayer.Play(PistonDerby.GameContent.S_MeiHuaSan);
                efecto.Parameters["LerpAmount"]?.SetValue(0);
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.PT_Presentacion0);
                efecto.Parameters["Texture2"]?.SetValue(PistonDerby.GameContent.PT_Presentacion0);
            }else if(secondsElapsed < START_TRANS2){ // TRANS1 : CAFE aparece
                efecto.Parameters["LerpAmount"]?.SetValue(0);
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.PT_Presentacion1);
            } else if(secondsElapsed < START_TRANS3){ // TRANS2 : CAFE ROJO se muestra
                efecto.Parameters["LerpAmount"]?.SetValue((secondsElapsed-START_TRANS2)/(START_TRANS3-START_TRANS2));
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.PT_Presentacion1);
                efecto.Parameters["Texture2"]?.SetValue(PistonDerby.GameContent.PT_Presentacion2);
            } else if(secondsElapsed < START_TRANS4){ // TRANS3 : MANTENGO
                efecto.Parameters["LerpAmount"]?.SetValue(1);
            } else if(secondsElapsed < END_TRANS4){ // TRANS4 : HUMITO
                efecto.Parameters["LerpAmount"]?.SetValue(Math.Min((secondsElapsed-START_TRANS4)/(START_TRANS4-START_TRANS3),1));
                efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.PT_Presentacion2);
                efecto.Parameters["Texture2"]?.SetValue(PistonDerby.GameContent.PT_Presentacion3);
            }
            PistonDerby.GameContent.G_Quad.Draw(efecto);
        }
    }
}

