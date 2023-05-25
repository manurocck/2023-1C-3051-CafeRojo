using Microsoft.Xna.Framework;
using PistonDerby.Elementos;

namespace PistonDerby
{
    public class HabitacionCocina : IHabitacion{
        public const int ANCHO = 6;
        public const int LARGO = 6;
        private const float SeparacionDePared = 0; 
        public HabitacionCocina(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
            Piso.ConTextura(PistonDerby.GameContent.T_PisoCeramica, 10, 10);

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);
            
            Amueblar();
        }
       //public override void DrawElementos(){
         //   var mShader = PistonDerby.GameContent.E_TextureMirror;
           // var bShader = PistonDerby.GameContent.E_BasicShader;
            //var tShader = PistonDerby.GameContent.E_TextureShader;
            //foreach(var mueble in Elementos){
              //  switch(mueble.GetTag()){
                //    case "Olla":
                  //      bShader.Parameters["DiffuseColor"].SetValue(Color.DarkSlateBlue.ToVector3());
                    //break;
                    //break;
                   // case "ParedCocina":
                     //   tShader.Parameters["Texture"].SetValue(PistonDerby.GameContent.T_Ladrillos);
                    //break;
                    //default:
                      //  mShader.Parameters["Texture"].SetValue(PistonDerby.GameContent.T_Marmol);
                   // break;
               // }
             //   mueble.Draw();
            //}
        //}
        private void Amueblar(){
            var carpintero = new ElementoBuilder(this.PuntoInicio());
            
            var alturaMesada = 0.6f;

            carpintero.Modelo(PistonDerby.GameContent.M_Mesada)
                .ConAltura(alturaMesada)
                .ConPosicion(SeparacionDePared+0.25f,2.25f)
                .ConRotacion(0,MathHelper.PiOver2,0)
                .ConTextura(PistonDerby.GameContent.T_MarmolNegro)
                .ConEscala(2.25f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_MesadaLateral2)
                .ConPosicion(SeparacionDePared+0.05f,0.85f)
                .ConTextura(PistonDerby.GameContent.T_Marmol)
                .ConRotacion(0,MathHelper.PiOver2,0)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_PlatosApilados)
                .ConPosicion(SeparacionDePared+0.25f,2.5f)
                .ConColor(Color.LightBlue)
                .ConAltura(alturaMesada)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_MesadaCentral)
                .ConPosicion(SeparacionDePared+0.05f,1.45f)
                .ConRotacion(0,MathHelper.PiOver2,0)
                .ConTextura(PistonDerby.GameContent.T_Marmol)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_Olla)
                .ConPosicion(2.2f,SeparacionDePared+0.45f)
                .ConColor(Color.DarkSlateBlue)
                .ConAltura(alturaMesada)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(SeparacionDePared+0.3f,1.25f)
                .ConAltura(alturaMesada+0.08f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_Plato)
                .ConPosicion(SeparacionDePared+0.5f,1.5f)
                .ConAltura(alturaMesada+1)
                .ConColor(Color.Aquamarine)
                .ConRotacion(0,MathHelper.PiOver2,0)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(SeparacionDePared+0.5f,1.30f)
                .ConAltura(alturaMesada+1)
                .ConRotacion(0,MathHelper.PiOver2,0);
                AddElemento(carpintero.BuildMueble());
            
                carpintero
                .ConPosicion(SeparacionDePared+0.5f,1.7f)
                .ConAltura(alturaMesada+1)
                .ConRotacion(0,MathHelper.PiOver2,0);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_MesadaLateral)
                .ConPosicion(SeparacionDePared+0.05f,2.05f)
                .ConRotacion(0,MathHelper.PiOver2,0)
                .ConTextura(PistonDerby.GameContent.T_Marmol)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_PlatoGrande)
                .ConPosicion(1,SeparacionDePared+0.1f)
                .ConColor(Color.WhiteSmoke)
                .ConAltura(alturaMesada + 1)
                .ConRotacion(0,MathHelper.PiOver4 / 2,0)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_Botella)
                .ConPosicion(1,SeparacionDePared + 0.2f)
                .ConAltura(alturaMesada+0.05f)
                .ConColor(Color.Azure)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_Maceta2)
                .ConPosicion(SeparacionDePared+0.2f,3)
                .ConColor(Color.IndianRed)
                .ConAltura(alturaMesada)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_Maceta3)
               .ConPosicion(SeparacionDePared+0.2f,3)
                .ConColor(Color.IndianRed)
                .ConAltura(alturaMesada+1.05f)
                .ConRotacion(0,MathHelper.PiOver2,0)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_ParedCocina)
                .ConPosicion(0,0)
                .ConRotacion(0,MathHelper.PiOver2,0)
                .ConTextura(PistonDerby.GameContent.T_MarmolNegro)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_Cocine)
                .ConPosicion(2.25f,0.4f)
                .ConAltura(0.45f)
                .ConColor(Color.Silver)
                .ConEscala(0.45f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_Alacena)
                .ConPosicion(2.6f,0)
                .ConAltura(alturaMesada * 2.5f)
                .ConTextura(PistonDerby.GameContent.T_Marmol)
                .ConEscala(5);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(PistonDerby.GameContent.M_Maceta)
                .ConPosicion(SeparacionDePared+0.2f,2.5f)
                .ConColor(Color.IndianRed)
                .ConAltura(alturaMesada+1.05f)
                .ConEscala(6);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(PistonDerby.GameContent.M_Maceta4)
                .ConPosicion(SeparacionDePared+0.25f,0.55f)
                .ConColor(Color.IndianRed)
                .ConAltura(alturaMesada+1.05f)
                .ConEscala(6);
                AddElemento(carpintero.BuildMueble());
            
            carpintero
                .ConPosicion(2.25f,SeparacionDePared+0.25f)
                .ConColor(Color.IndianRed)
                .ConAltura(alturaMesada+1.5f);
                AddElemento(carpintero.BuildMueble());
        }
    }    
}