using Microsoft.Xna.Framework;
using PistonDerby.Elementos;

namespace PistonDerby
{
    public class HabitacionPrincipal : IHabitacion{
        public const int ANCHO = 10;
        public const int LARGO = 10;
        public HabitacionPrincipal(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
            Piso.ConTextura(PistonDerby.GameContent.T_PisoMadera, 10, 10);
            Amueblar();
        }
        private void Amueblar(){
            var carpintero = new ElementoBuilder(this.PuntoInicio());
            
            carpintero.Modelo(PistonDerby.GameContent.M_Mesa)
                .ConPosicion(5, 4.2f)
                .ConTextura(PistonDerby.GameContent.T_MarmolNegro)
                .ConEscala(2.4f)
                .ConRotacion(0, MathHelper.PiOver2, 0);
                
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(PistonDerby.GameContent.M_Silla)
                .ConTextura(PistonDerby.GameContent.T_MaderaNikari)
                .ConAltura(0.3f)
                .ConEscala(2f)
                .ConRotacion(-MathHelper.PiOver2, 0, 0)
                .ConPosicion(5, 3); // cabecera
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(5,5.15f) // contra-cabecera
                .ConRotacion(-MathHelper.PiOver2, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(4.5f,3.75f) // lado superior cerca de cabecera
                .ConRotacion(-MathHelper.PiOver2, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );
            
                carpintero
                .ConPosicion(4.5f,4.5f) // lado superior cerca de contra-cabecera
                .ConRotacion(-MathHelper.PiOver2, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(5.5f,3.75f) // lado inferior cerca de cabecera
                .ConRotacion(-MathHelper.PiOver2, -MathHelper.PiOver2*0.7f, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(5.5f,4.5f) // lado inferior cerca de contra-cabecera
                .ConRotacion(-MathHelper.PiOver2, -MathHelper.PiOver2*1.3f, 0);
                AddElemento( carpintero.BuildMueble() );
            
        
            // carpintero.Modelo(PistonDerby.GameContent.M_Sillon)
            //     .ConPosicion(0,0) //no se estan dibujando
            //     .ConAltura(0)
            //     .ConTextura(PistonDerby.GameContent.T_Alfombra)
            //     .ConRotacion(0, MathHelper.Pi, 0)
            //     .ConEscala(1f);
            //     AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(PistonDerby.GameContent.M_MuebleTV)
                .ConPosicion(LARGO-1f, 0.5f) // el del mueble
                .ConRotacion(0, MathHelper.Pi, 0)
                .ConTextura(PistonDerby.GameContent.T_Reboque)
                .ConEscala(0.25f);
                AddElemento( carpintero.BuildMueble() );
            
            
            carpintero.Modelo(PistonDerby.GameContent.M_Sofa)
                .ConPosicion(LARGO-2f, ANCHO-0.75f)
                .ConTextura(PistonDerby.GameContent.T_Alfombra)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.Pi, 0)
                .ConEscala(0.2f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(PistonDerby.GameContent.M_Mesita)
                .ConEscala(4f)
                .ConTextura(PistonDerby.GameContent.T_MaderaNikari)
                .ConPosicion(LARGO-2.15f, ANCHO-1.75f)
                .ConRotacion(-MathHelper.PiOver2, 0, 0);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(PistonDerby.GameContent.M_CafeRojo)
                .ConColor(Color.DarkRed)
                .ConEscala(2f)
                .ConAltura(0.355f)
                .ConPosicion(LARGO-2.15f, ANCHO-1.75f); // el acostado

                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(LARGO-2.25f, ANCHO-1.9f)
                .ConAltura(0.355f)
                .ConRotacion(-MathHelper.PiOver2,0f,0f); // el parado

                AddElemento( carpintero.BuildMueble() );
                
                carpintero
                .ConPosicion(5, 4.2f)
                .ConAltura(0.6f); // el de la mesa

                AddElemento( carpintero.BuildMueble() );

            carpintero.Modelo(PistonDerby.GameContent.M_Televisor)
                .ConEscala(1.75f)
                .ConShader(PistonDerby.GameContent.E_SpiralShader)

                .ConPosicion(LARGO-1.425f, 0.5f) // el del mueble
                .ConAltura(0.15f);
                
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConPosicion(0.25f, ANCHO-1f) // el de la pared
                .ConAltura(1.25f);
                
                AddElemento( carpintero.BuildMueble() );

            #region Autos Enemigos

                for(int i=1; i<6; i++){
                    PistonDerby.ElementosDinamicos.Add(new EnemyCar(i*1f,1f,6f, Vector3.Zero)); // Fila vertical
                    PistonDerby.ElementosDinamicos.Add(new EnemyCar(2f,1f,i*1f, Vector3.Zero));  // Fila horizontal
                }
            
            #endregion            
        }
    }    
}