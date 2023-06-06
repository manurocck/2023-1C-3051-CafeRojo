using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Utils;

namespace PistonDerby;
public class Pared{
    public const float GROSOR = PistonDerby.S_METRO * 0.2f;
    public const float ALTURA = PistonDerby.S_METRO * 2f;
    protected float LARGO;
    private Matrix World;    
    internal StaticHandle Handle;
    private (Vector3 Inicio , Vector3 Final) Coordenadas = (Vector3.Zero, Vector3.Zero);
    private Effect Efecto = PistonDerby.GameContent.E_TextureTiles;
    
    public Pared(Vector3 puntoInicio, Vector3 puntoFinal, bool paraSimular = true){
        var esHorizontal = (puntoInicio.X == puntoFinal.X);
        Coordenadas = (puntoInicio, puntoFinal);

        LARGO = (esHorizontal)? Coordenadas.Final.Z - Coordenadas.Inicio.Z : Coordenadas.Final.X - Coordenadas.Inicio.X - GROSOR;

        Matrix Rotacion = esHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);

        World = Matrix.CreateScale(ALTURA,GROSOR,LARGO) 
                * Matrix.CreateRotationZ(MathHelper.PiOver2) 
                * Rotacion 
                * Matrix.CreateTranslation(Coordenadas.Inicio.X,-PistonDerby.S_METRO*0.1f,Coordenadas.Inicio.Z);
        
        if(paraSimular) AddToSimulation();
    }

    private void AddToSimulation(){
        var esHorizontal = (this.Coordenadas.Inicio.X == this.Coordenadas.Final.X);
        var esteNumerito = Math.Abs(-Coordenadas.Inicio.X + Coordenadas.Final.X);
        var otroNumerito = Math.Abs(-Coordenadas.Inicio.Z + Coordenadas.Final.Z);
        
        float coordenadaAlturaInicio = -20f; // FIX PARA QUE ESTE A LA ALTURA DEL PISO

        Box boxito = (!esHorizontal)? new Box(esteNumerito+GROSOR, ALTURA, GROSOR) 
                                    : new Box(GROSOR, ALTURA, otroNumerito);

        Vector3 fixedPosition = (!esHorizontal)? 
                                new Vector3((Coordenadas.Inicio.X+Coordenadas.Final.X)*0.5f-GROSOR*0.5f, ALTURA*0.5f + coordenadaAlturaInicio, Coordenadas.Inicio.Z+GROSOR*0.5f):
                                new Vector3(Coordenadas.Inicio.X-GROSOR*0.5f, ALTURA*0.5f, (Coordenadas.Inicio.Z+Coordenadas.Final.Z)*0.5f);

        
        TypedIndex index = PistonDerby.Simulation.LoadShape<Box>(boxito);
        Handle = PistonDerby.Simulation.CreateStatic(fixedPosition.ToBepu(), Quaternion.Identity.ToBepu(), index);
    }
    public void SetEffect( Effect effect) => this.Efecto = effect;
    public void Draw(Texture2D textura){ 

        var metrosLargo = LARGO/PistonDerby.S_METRO;

        Efecto.Parameters["World"].SetValue(World);
        Efecto.Parameters["Texture"]?.SetValue(textura);
        Efecto.Parameters["Filter"]?.SetValue(PistonDerby.GameContent.T_MeshFilter);
        Efecto.Parameters["TilesWide"]?.SetValue(metrosLargo*0.5f);
        Efecto.Parameters["TilesBroad"]?.SetValue(1);
        PistonDerby.GameContent.G_Cubo.Draw(Efecto);
    }
    public void DebugGizmos()
    {
        var body = PistonDerby.Simulation.GetStaticReference(Handle);
        var aabb = body.BoundingBox;
        PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Turquoise);
    }
}
