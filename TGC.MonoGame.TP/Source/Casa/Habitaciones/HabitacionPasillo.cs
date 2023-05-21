using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionPasillo : IHabitacion{
        public const int ANCHO = 8;
        public const int LARGO = 4;
        public HabitacionPasillo(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){  
            Piso.ConTextura(TGCGame.GameContent.T_PisoAlfombrado, 3);

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

        }

    }
}