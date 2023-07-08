using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Drawers;
using PistonDerby.Utils;

namespace PistonDerby.Elementos;
public class ElementoBuilder{
    private Model Model;
    private Vector3 PosicionRelativa;
    private Vector3 Corrimiento = Vector3.Zero;
    private Vector3 Rotacion = Vector3.Zero;
    private IDrawer Drawer = new ColorDrawer(Color.Magenta); // drawer como static?
    private float Escala = 1f;

    /// <summary> Recibe una posicion pivot <paramref name="inicioHabitacion"/> sobre la cuál dibuja todos los elementos + el corrimiento indicado por los métodos <paramref name="Posicion"/> y <paramref name="Altura"/></summary> 
    public ElementoBuilder(Vector3 inicioHabitacion) => PosicionRelativa = inicioHabitacion;

    public void Reset(){
        Corrimiento = Vector3.Zero;
        Rotacion = Vector3.Zero;
        Escala = 1f;
        Drawer = new ColorDrawer(Color.Magenta);
    }
    public ElementoBuilder Modelo(Model modelo3d){
        Model = modelo3d;
        Reset();
        return this;
    }
    public ElementoBuilder ConEscala(float escala){
        Escala = escala;
        return this;
    }
    /// <summary> La <paramref name="altura"/> del elemento. Su unidad es <paramref name="S_METRO"/></summary>
    public ElementoBuilder ConAltura(float metrosAltura){
        Corrimiento.Y = metrosAltura * PistonDerby.S_METRO;
        return this;
    }
    public ElementoBuilder ConRotacion(float rotacionX, float rotacionY, float rotacionZ){
        Rotacion.X = rotacionX;
        Rotacion.Y = rotacionY;
        Rotacion.Z = rotacionZ;
        return this;
    }
    /// <summary> Establece el corrimiento del objeto sobre el plano. <paramref name="metrosDistanciaAlOrigenEnX"/> y <paramref name="metrosDistanciaAlOrigenEnZ"/>  tienen como unidad a <paramref name="S_METRO"/> <code></code> <example><code> ElementoBuilder.ConPosicion(2,3); //Lo mueve 2 unidades para abajo y 3 para la izquierda desde el origen </code></example></summary> 
    public ElementoBuilder ConPosicion(float metrosDistanciaAlOrigenEnX, float metrosDistanciaAlOrigenEnZ){
        Corrimiento.X = metrosDistanciaAlOrigenEnX * PistonDerby.S_METRO;
        Corrimiento.Z = metrosDistanciaAlOrigenEnZ * PistonDerby.S_METRO;
        return this;
    }
    public ElementoBuilder ConShader(Effect shaderSinParametros){
        Drawer = new PlainEffectDrawer(shaderSinParametros);
        return this;
    }
    public ElementoBuilder ConPBRempaquetado(Texture2D texturaPBRempaquetada, Texture2D texturaBaseColor, Texture2D texturaNormalMap){
        Drawer = new PBRDrawer(texturaPBRempaquetada, texturaBaseColor, texturaNormalMap);
        return this;
    }
    public ElementoBuilder ConTextura(Texture2D textura){
        Drawer = new TextureDrawer(textura);
        return this;
    }
    internal ElementoBuilder ConColor(Color color){
        Drawer = new ColorDrawer(color);
        return this;
    }
    public ElementoEstatico BuildMueble(){
        Corrimiento += PosicionRelativa;
        ElementoEstatico elemento = new ElementoEstatico(Model, Drawer, Corrimiento, Rotacion, Escala);

        return elemento;
    }
    public ElementoDinamicoIndependiente BuildMuebleDinamico(){
        Corrimiento += PosicionRelativa;
        ElementoDinamicoIndependiente elemento = new ElementoDinamicoIndependiente(Model, Drawer, Corrimiento, Rotacion, Escala);

        return elemento;
    }
    // public ElementoEstatico BuildDinamico(){
    //     Corrimiento += PosicionRelativa;
    //     return new ElementoDinamico(Drawer, Corrimiento, Rotacion, Escala);
    // }
}
