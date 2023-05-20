using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{

    public class HabitacionConferencias : IHabitacion{
        public const int ANCHO = 10;
        public const int LARGO = 10;
        public HabitacionConferencias(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
            
            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

        }
    }    
}