using System;
using Microsoft.Xna.Framework;

namespace PistonDerby
{

    public class HabitacionConferencias : IHabitacion{
        public const int ANCHO = 10;
        public const int LARGO = 10;
        public HabitacionConferencias(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
            Piso = Piso.ConTextura(PistonDerby.GameContent.T_PisoMadera, ANCHO, LARGO);
            // Piso = Piso.ConTextura(PistonDerby.GameContent.T_PisoAlfombrado, ANCHO/5, LARGO/2);


            var posicionInicial = new Vector3(posicionX,0f,posicionZ);
        }
    }    
}