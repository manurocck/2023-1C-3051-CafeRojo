using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Elementos;

namespace TGC.MonoGame.TP
{
    public class HabitacionPrincipal : IHabitacion{
        public const int ANCHO = 10;
        public const int LARGO = 10;
        public HabitacionPrincipal(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
            Piso.ConTextura(TGCGame.GameContent.T_PisoMadera, 10);
            Amueblar();
        }
        private void Amueblar(){
            var carpintero = new ElementoBuilder(this.PuntoInicio());
            
            carpintero.Modelo(TGCGame.GameContent.M_Mesa)
                .ConPosicion(0, 0)
                .ConTextura(TGCGame.GameContent.T_MaderaNikari)
                .ConEscala(2f)
                .ConRotacion(0, MathHelper.PiOver2, 0);
                
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Silla)
                .ConTextura(TGCGame.GameContent.T_MaderaNikari)
                .ConAltura(0.3f)
                .ConEscala(2f)
                .ConRotacion(-MathHelper.PiOver2, 0, 0)
                .ConPosicion(1, 1);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(1,1.5f)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(1,2.5f)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );
            
                carpintero
                .ConPosicion(-1,1)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(-1,1.5f)
                .ConRotacion(-MathHelper.PiOver2, -MathHelper.PiOver2*0.7f, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(-1,1)
                .ConRotacion(-MathHelper.PiOver2, -MathHelper.PiOver2*1.3f, 0);
                AddElemento( carpintero.BuildMueble() );
            
        
            carpintero.Modelo(TGCGame.GameContent.M_Sillon)
                .ConPosicion(0,0)
                .ConAltura(0)
                .ConTextura(TGCGame.GameContent.T_Alfombra)
                .ConRotacion(0, MathHelper.Pi, 0)
                .ConEscala(1f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_MuebleTV);
                carpintero.ConPosicion(0,0)
                .ConRotacion(0, MathHelper.Pi, 0)
                .ConEscala(0.01f);
                AddElemento( carpintero.BuildMueble() );
            
            
            carpintero.Modelo(TGCGame.GameContent.M_Sofa)
                .ConPosicion(5,5)
                .ConTextura(TGCGame.GameContent.T_Alfombra)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.Pi, 0)
                .ConEscala(0.1f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Mesita)
                .ConEscala(2f)
                .ConTextura(TGCGame.GameContent.T_MaderaNikari)
                .ConPosicion(4, 4)
                .ConRotacion(-MathHelper.PiOver2, 0, 0);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_CafeRojo)
                .ConColor(Color.DarkRed)
                .ConEscala(1f)
                .ConAltura(0)
                .ConRotacion(-MathHelper.PiOver2,0f,0f)
                .ConPosicion(0, 0);

                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(0, 0);

                AddElemento( carpintero.BuildMueble() );

            carpintero.Modelo(TGCGame.GameContent.M_Televisor)
                .ConEscala(2f)
                .ConShader(TGCGame.GameContent.E_SpiralShader)
                .ConPosicion(0, 0)
                .ConAltura(0);
                
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConAltura(0)
                .ConPosicion(0, 0)
                .ConRotacion(0f,MathHelper.PiOver2,0f);
                
                AddElemento( carpintero.BuildMueble() );

            #region Autos Enemigos

                var posicionesAutosIA = new Vector3(1000f,0f,6000f);           
                for(int i=1; i<6; i++){
                    AddElementoDinamico(new EnemyCar(i*200f,200f,1200f, Vector3.Zero));
                    AddElementoDinamico(new EnemyCar(220f,200f,i*200f, Vector3.Zero));
                }
            
            #endregion            
        }
    }    
}