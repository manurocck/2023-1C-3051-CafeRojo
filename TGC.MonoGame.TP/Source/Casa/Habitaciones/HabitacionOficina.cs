using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionOficina : IHabitacion{
        private const int Size = 5;
        public HabitacionOficina(Vector3 posicionInicial):base(Size,Size,posicionInicial){
            Piso.Oficina();
            AddPared(Pared.Abajo (Size, Size, posicionInicial));
            AddPared(Pared.Arriba(Size, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));
            AddPuerta(Puerta.Izquierda(1f, Size, posicionInicial));

            AddElemento(new Mueble(TGCGame.GameContent.M_SillaOficina,new Vector3(1000f,0f,1000f), new Vector3(-MathHelper.PiOver2,MathHelper.PiOver4,0f), 10f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(1000f,500f,1000f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));
        
            AddElemento(new Mueble(TGCGame.GameContent.M_Planta, new Vector3(500f,0f,500f), new Vector3(0f,0f,0f), 15f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Escritorio, new Vector3(1500f,0f,1500f), new Vector3(0f, 0f, 0f), 170f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Plantis, new Vector3(4800f,0f,4000f), new Vector3(0f,0f,0f), 15f));

        }
    }    
}