using Microsoft.Xna.Framework;
using PistonDerby.Elementos;

namespace PistonDerby.Mapa;

public class HabitacionOficina : IHabitacion{
   public const int ANCHO = 5;
   public const int LARGO = 5;
    public HabitacionOficina(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
        Piso = Piso.ConTextura(PistonDerby.GameContent.T_PisoMaderaClaro, 5);

        Amueblar();
    }


    private void Amueblar(){
        var carpintero = new ElementoBuilder(this.PuntoInicio());

        carpintero.Modelo(PistonDerby.GameContent.M_SillaOficina)
            .ConPosicion(4f, 3f)
            .ConTextura(PistonDerby.GameContent.T_SillaOficina)
            .ConRotacion(-MathHelper.PiOver2,-MathHelper.PiOver4,0f)
            .ConEscala(2f);
        AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_CafeRojo)
            .ConPosicion(3.2f, 3.5f)
            .ConRotacion(-MathHelper.PiOver2,0f,0f)
            .ConColor(Color.Red)
            .ConEscala(2f)
            .ConAltura(0.7f);
        AddElemento(carpintero.BuildMueble());
                
                
        carpintero.Modelo(PistonDerby.GameContent.M_Escritorio)
            .ConPosicion(3.5f, 3f)
            .ConTextura(PistonDerby.GameContent.T_Marmol)
            .ConPatas(50f, 0, 170f, 20f, false)
            .ConRotacion(0f, MathHelper.Pi, 0f)
            .ConEscala(40f);
            //.ConAltura(5f);
        AddElemento(carpintero.BuildMueble());
        
        carpintero.Modelo(PistonDerby.GameContent.M_Planta)
            .ConPBRempaquetado(PistonDerby.GameContent.T_Planta_RoughnessMetallicOpacityMap, PistonDerby.GameContent.T_Planta_BaseColorMap, PistonDerby.GameContent.T_Planta_NormalMap)
            .ConPosicion(0.5f, LARGO-0.5f)
            .ConCaja(25f,150f,25f) // Ancho (x), Alto (y), Profundidad (z)
            .ConCorrimientoCaja(0,-10,0) // Corrimiento de la caja
            .ConEscala(4f);
        AddElemento(carpintero.BuildMueble());
                
        carpintero.Modelo(PistonDerby.GameContent.M_Plantis)
            .ConPosicion(ANCHO-2, 1)
            .ConPBRempaquetado(PistonDerby.GameContent.T_Plantis_RoughnessMetallicOpacityMap, PistonDerby.GameContent.T_Plantis_BaseColorMap, PistonDerby.GameContent.T_Plantis_NormalMap)
            .ConCaja(65f,200f,65f) // Ancho (x), Alto (y), Profundidad (z)
            .ConCorrimientoCaja(0,-10,0)
            .ConEscala(5f);
        AddElemento(carpintero.BuildMueble());
    }


 }