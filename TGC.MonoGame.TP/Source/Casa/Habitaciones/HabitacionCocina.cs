using Microsoft.Xna.Framework;
using PistonDerby.Elementos;

namespace PistonDerby.Mapa;
public class HabitacionCocina : IHabitacion{
    public const int ANCHO = 6;
    public const int LARGO = 6;
    private const float SeparacionDePared = 0; 
    public HabitacionCocina(float posicionX, float posicionZ):base(ANCHO,LARGO,new Vector3(posicionX,0f,posicionZ)){
        Piso.ConTextura(PistonDerby.GameContent.T_PisoCeramica, 10, 10);

        var posicionInicial = new Vector3(posicionX,0f,posicionZ);
        
        Amueblar();
    }
    private void Amueblar(){
        var carpintero = new ElementoBuilder(this.PuntoInicio());
        
        var alturaMesada = 0.6f;

        carpintero.Modelo(PistonDerby.GameContent.M_Mesada)
            .ConAltura(alturaMesada+0.0025f)
            .ConPosicion(SeparacionDePared,SeparacionDePared)
            .ConRotacion(0,MathHelper.PiOver2,0)
            .ConTextura(PistonDerby.GameContent.T_MarmolNegro)
            .ConEscala(5f);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_MesadaLateral2)
            .ConPosicion(SeparacionDePared+0.05f,0.85f)
            .ConMaterial(0.1f,0.8f,0.8f,30f)
            .ConCaja(200f,180f,530f) // Ancho (x), Alto (y), Profundidad (z)
            .ConCorrimientoCaja(70,0,-200)
            .ConTextura(PistonDerby.GameContent.T_Marmol)
            .ConRotacion(0,MathHelper.PiOver2,0)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());
        
        carpintero.Modelo(PistonDerby.GameContent.M_PlatosApilados)
            .ConPosicion(SeparacionDePared+0.25f,2.5f)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());
        
        carpintero.Modelo(PistonDerby.GameContent.M_MesadaCentral)
            .ConPosicion(SeparacionDePared+0.05f,1.45f)
            .ConCaja(900f,180f,230f) // Ancho (x), Alto (y), Profundidad (z)
            .ConMaterial(0.1f,0.8f,0.8f,30f)
            .ConRotacion(0,MathHelper.PiOver2,0)
            .ConTextura(PistonDerby.GameContent.T_Marmol)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());
        
        carpintero.Modelo(PistonDerby.GameContent.M_Olla)
            .ConPosicion(2.2f,SeparacionDePared+0.45f)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());
            
            carpintero
            .ConPosicion(SeparacionDePared+0.3f,1.25f)
            .ConAltura(alturaMesada+0.08f);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_Plato)
            .ConPosicion(SeparacionDePared+0.5f,1.5f)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada+1)
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
            .ConMaterial(0.1f,0.8f,0.8f,30f)
            .ConRotacion(0,MathHelper.PiOver2,0)
            .ConTextura(PistonDerby.GameContent.T_Marmol)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_PlatoGrande)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConPosicion(1,SeparacionDePared+0.1f)
            .ConAltura(alturaMesada + 1)
            .ConRotacion(0,MathHelper.PiOver4 / 2,0)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_Botella)
            .ConPosicion(1,SeparacionDePared + 0.2f)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada+0.05f)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_Maceta2)
            .ConPosicion(SeparacionDePared+0.2f,3)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());
        
        carpintero.Modelo(PistonDerby.GameContent.M_Maceta3)
            .ConPosicion(SeparacionDePared+0.2f,3)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
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
            .ConMaterial(0.1f,0.8f,0.8f,30f)
            .ConAltura(0.40f)
            .ConCaja(190f,200,90f) // Ancho (x), Alto (y), Profundidad (z)
            .ConCorrimientoCaja(0,-50,0) // Corrimiento de la caja
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConEscala(0.45f);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_Alacena)
            .ConPosicion(2.6f,0)
            .ConMaterial(0.1f,0.8f,0.8f,30f)
            .ConAltura(alturaMesada * 2.5f)
            .ConTextura(PistonDerby.GameContent.T_Marmol)
            .ConEscala(5);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(PistonDerby.GameContent.M_Maceta)
            .ConPosicion(SeparacionDePared+0.2f,2.5f)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada+1.05f)
            .ConEscala(6);
            AddElemento(carpintero.BuildMueble());
        
        carpintero.Modelo(PistonDerby.GameContent.M_Maceta4)
            .ConPosicion(SeparacionDePared+0.25f,0.55f)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada+1.05f)
            .ConEscala(6);
            AddElemento(carpintero.BuildMueble());
        
        carpintero
            .ConPosicion(2.25f,SeparacionDePared+0.25f)
            .ConTextura(PistonDerby.GameContent.T_Concreto)
            .ConAltura(alturaMesada+1.5f);
            AddElemento(carpintero.BuildMueble());
    }
}    