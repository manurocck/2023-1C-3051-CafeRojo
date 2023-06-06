using Microsoft.Xna.Framework;
using PistonDerby.Elementos;

namespace PistonDerby.Mapa;

public class HabitacionToilette : IHabitacion{
       public const int ANCHO = 5;
       public const int LARGO = 6;
      public HabitacionToilette(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
 
            Piso.ConTextura(PistonDerby.GameContent.T_MarmolNegro, ANCHO, LARGO);
                       
            Amueblar();
      }


      private void Amueblar(){
            var carpintero = new ElementoBuilder(this.PuntoInicio());

            carpintero.Modelo(PistonDerby.GameContent.M_Inodoro)
                .ConPosicion(1f, 0.5f)
                .ConRotacion(-MathHelper.PiOver2,0,0)
                .ConColor(Color.White)
                .ConAltura(0.5f)
                .ConEscala(4f);
            AddElemento(carpintero.BuildMueble());
                    

            carpintero.Modelo(PistonDerby.GameContent.M_Baniera)
                .ConPosicion(1.5f, 3.5f)
                .ConTextura(PistonDerby.GameContent.T_Marmol)
                //.ConRotacion(0f, MathHelper.PiOver2, 0f)
                .ConEscala(16f);
            AddElemento(carpintero.BuildMueble());
                    

            carpintero.Modelo(PistonDerby.GameContent.M_Bacha)
                .ConPosicion(2.5f, 0.1f)
                .ConRotacion(-MathHelper.PiOver2, 0f, 0f)
                .ConColor(Color.White)
                .ConAltura(1f)
                .ConEscala(0.5f);
            AddElemento(carpintero.BuildMueble());
      }

 }

        
//         public override void DrawElementos(){
//             var bShader = PistonDerby.GameContent.E_BasicShader;
//             var mShader = PistonDerby.GameContent.E_TextureMirror;
//             foreach(var e in Elementos){
//                 switch(e.GetTag()){
//                     case "Bacha":
//                     case "Inodoro":
//                         bShader.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
//                     break;
//                     case "Baniera":
//                         mShader.Parameters["Texture"].SetValue(PistonDerby.GameContent.T_Ladrillos);
//                     break;

//                     default:
//                     break;
//                 }
//                 e.Draw();
//             }
//         }
//     }    
