using System;
using Microsoft.Xna.Framework;
using PistonDerby.Elementos;

namespace PistonDerby;
public class HabitacionPasillo : IHabitacion{
    public const int ANCHO = 8;
    public const int LARGO = 4;
    public HabitacionPasillo(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){  
        Piso.ConTextura(PistonDerby.GameContent.T_PisoAlfombrado, ANCHO, LARGO*0.5f);

        var posicionInicial = new Vector3(posicionX,0f,posicionZ);

       // Amueblar();

    }
/*
     private void Amueblar(){
        var carpintero = new ElementoBuilder(this.PuntoInicio());
        
        carpintero.Modelo(PistonDerby.GameContent.M_Gato)
            .ConPosicion(2.25f,0.4f)
            .ConAltura(0.45f)
            .ConColor(Color.White)
            .ConEscala(0.45f);
            AddElemento(carpintero.BuildMueble());

    }*/

}
