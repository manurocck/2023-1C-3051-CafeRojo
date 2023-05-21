using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Elementos;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorioLegos : IHabitacion{
        public const int ANCHO = 6;
        public const int LARGO = 7;

        public HabitacionDormitorioLegos(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
            Amueblar();
        }
        
        private void Amueblar(){
            var carpintero = new ElementoBuilder(this.PuntoInicio());

            carpintero.Modelo(TGCGame.GameContent.M_Organizador)
                .ConPosicion(1.5f,0.75f)
                .ConTextura(TGCGame.GameContent.T_MaderaNikari)
                .ConRotacion(-MathHelper.PiOver2,0f, 0f)
                .ConEscala(500f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Cajonera)
                .ConPosicion(1,ANCHO-0.5f)
                .ConTextura(TGCGame.GameContent.T_PisoMadera)
                // .ConRotacion(0f,MathHelper.Pi, 0f)
                .ConRotacion(-MathHelper.PiOver2,0f, 0f)
                .ConEscala(400f);
                AddElemento(carpintero.BuildMueble());
                
            carpintero.Modelo(TGCGame.GameContent.M_CamaMarinera)
                .ConPosicion(LARGO-1.25f,0.5f)
                // .ConTextura(TGCGame.GameContent.T_PisoMadera) // se desarma
                .ConColor(Color.DarkRed)
                .ConRotacion(0f,MathHelper.Pi,0f)
                .ConEscala(2f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Lego);
    

            #region LEGOS GRANDES
            carpintero.Modelo(TGCGame.GameContent.M_Lego);
            carpintero
                .ConEscala(5f)
                .ConColor(Color.DarkRed)
                .ConAltura(1f)
                
                .ConPosicion(0.75f,0.85f)
                .ConRotacion(MathHelper.PiOver4/2,MathHelper.PiOver4/2, 0f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero
                .ConAltura(1.135f)
                .ConColor(Color.DarkGreen)
                .ConPosicion(1.5f,0.85f)
                .ConRotacion(MathHelper.PiOver2,MathHelper.PiOver4, 0f);
                AddElemento(carpintero.BuildMueble());

            carpintero
                .ConColor(Color.DarkBlue)
                .ConPosicion(2.25f,0.85f);
                AddElemento(carpintero.BuildMueble());
                

            #endregion LEGOS GRANDES

            #region LEGOS CHIQUITOS
            carpintero.Modelo(TGCGame.GameContent.M_Lego);
            Vector2 desplazamientoRandom = new Vector2(LARGO*0.5f, ANCHO*0.5f); // donde arranca el bardo
            Vector3 randomColor;
            Vector3 randomRotation;
            float random1, random2, random3; // Entropía

            const float ESPARCIMIENTO = 1f;

            for(int i=0; i<100; i++){
                random1 = (Random.Shared.NextSingle());
                random2 = (Random.Shared.NextSingle()-0.5f);
                random3 = (Random.Shared.NextSingle()-0.5f);

                randomRotation = new Vector3(0f,MathHelper.Pi*random3, 0f);
                randomColor = new Vector3( i%3%2*115 , (i+1)%3%2*115 , (i+2)%3%2*115 );
                
                desplazamientoRandom += new Vector2((ESPARCIMIENTO*MathF.Cos(random1*MathHelper.TwoPi))*random2,ESPARCIMIENTO*(MathF.Sin(random1*MathHelper.TwoPi)*random2));
                
                
                carpintero
                    .ConPosicion(desplazamientoRandom.X,desplazamientoRandom.Y)
                    .ConRotacion(randomRotation.X,randomRotation.Y,randomRotation.Z)
                    .ConColor(new Color(randomColor))
                    .ConEscala(1f);
                    AddElemento(carpintero.BuildMueble());
            }
            #endregion LEGOS CHIQUITOS

             
                
        }
    }    
}