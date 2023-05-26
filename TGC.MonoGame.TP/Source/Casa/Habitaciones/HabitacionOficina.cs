using Microsoft.Xna.Framework;
using PistonDerby.Elementos;

namespace PistonDerby
{
    public class HabitacionOficina : IHabitacion{
        public const int ANCHO = 6;
        public const int LARGO = 10;
        public HabitacionOficina(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
            Piso = Piso.ConTextura(PistonDerby.GameContent.T_PisoMadera, ANCHO, LARGO);
            // Piso = Piso.ConTextura(PistonDerby.GameContent.T_PisoMaderaClaro, ANCHO, LARGO/2);

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

            Amueblar();
        }
        // public override void DrawElementos(){
        //     var bShader = PistonDerby.GameContent.E_BasicShader;
        //     var tShader = PistonDerby.GameContent.E_TextureShader;
        //     foreach(var e in Elementos){
        //         switch(e.GetTag()){
        //             case "SillaOficina":
        //                 tShader.Parameters["Texture"].SetValue(PistonDerby.GameContent.T_SillaOficina);
        //             break;
        //             case "Escritorio":
        //                 tShader.Parameters["Texture"].SetValue(PistonDerby.GameContent.T_PisoMadera);
        //             break;
        //             case "Cafe-Rojo":
        //                 bShader.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
        //             break;
        //             default:
        //             break;
        //         }
        //         e.Draw();
        //     }
        // }

        private void Amueblar(){
            var carpintero = new ElementoBuilder(this.PuntoInicio());

            carpintero.Modelo(PistonDerby.GameContent.M_SillaOficina)
                .ConPosicion(4000f, 1000f)
                // .ConShader(tShader)
                .ConRotacion(-MathHelper.PiOver2,-MathHelper.PiOver4,0f)
                .ConEscala(10f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_CafeRojo)
                .ConPosicion(4000f, 1000f)
                .ConRotacion(-MathHelper.PiOver2,0f,0f)
                .ConEscala(10f)
                .ConAltura(500f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_Planta)
                .ConPosicion(500f,500f)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_Escritorio)
                .ConPosicion(3500f, 1500f)
                // .ConShader(tShader)
                .ConRotacion(0f, MathHelper.Pi, 0f)
                .ConEscala(170f)
                .ConAltura(50f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_Plantis)
                .ConPosicion(4700f, 3500f)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
        }
    }    
}