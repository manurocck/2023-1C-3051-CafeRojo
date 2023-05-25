using System;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP;
public class Pared{
    public const float GROSOR = TGCGame.S_METRO * 0.2f;
    public const float ALTURA = TGCGame.S_METRO * 2f;
    protected float LARGO;
    private Matrix World;
    private (Vector3 Inicio , Vector3 Final) Coordenadas = (Vector3.Zero, Vector3.Zero);
    private Effect Efecto = TGCGame.GameContent.E_TextureTiles;
    
    ///<summary> Pared completamente cerrada</summary>
    public Pared(Vector3 puntoInicio, Vector3 puntoFinal){
        var esHorizontal = (puntoInicio.X == puntoFinal.X);
        Coordenadas = (puntoInicio, puntoFinal);

        LARGO = (esHorizontal)? Coordenadas.Final.Z - Coordenadas.Inicio.Z : Coordenadas.Final.X - Coordenadas.Inicio.X - GROSOR;

        Matrix Rotacion = esHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);

        World = Matrix.CreateScale(ALTURA,GROSOR,LARGO) 
                * Matrix.CreateRotationZ(MathHelper.PiOver2) 
                * Rotacion 
                * Matrix.CreateTranslation(Coordenadas.Inicio.X,-TGCGame.S_METRO*0.1f,Coordenadas.Inicio.Z);
        
        AddToSimulation();
    }

    private void AddToSimulation(){
        var esHorizontal = (this.Coordenadas.Inicio.X == this.Coordenadas.Final.X);
        var esteNumerito = Math.Abs(-Coordenadas.Inicio.X+Coordenadas.Final.X);
        var otroNumerito = Math.Abs(-Coordenadas.Inicio.Z+Coordenadas.Final.Z);

        Box boxito = (!esHorizontal)? new Box(esteNumerito, ALTURA, GROSOR) 
                                    : new Box(GROSOR, ALTURA, otroNumerito);
        Vector3 fixedPosition = (!esHorizontal)? new Vector3((Coordenadas.Inicio.X+Coordenadas.Final.X)/2, ALTURA*0.5f, Coordenadas.Inicio.Z):
                                            new Vector3(Coordenadas.Inicio.X, ALTURA*0.5f, (Coordenadas.Inicio.Z+Coordenadas.Final.Z)/2);

        TypedIndex index = TGCGame.Simulation.LoadShape<Box>(boxito);
        TGCGame.Simulation.CreateStatic(fixedPosition.ToBepu(), Quaternion.Identity.ToBepu(), index);
    }
    public void SetEffect( Effect effect) => this.Efecto = effect;
    public void Draw(Texture2D textura){ 
        
        //var body = TGCGame.Simulation.Statics.GetStaticReference(Handle);
        //var aabb = body.BoundingBox;
        //TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);

        var metrosLargo = LARGO/TGCGame.S_METRO;

        Efecto.Parameters["World"].SetValue(World);
        Efecto.Parameters["Texture"]?.SetValue(textura);
        Efecto.Parameters["Filter"]?.SetValue(TGCGame.GameContent.T_MeshFilter);
        Efecto.Parameters["TilesWide"]?.SetValue(metrosLargo*0.5f);
        Efecto.Parameters["TilesBroad"]?.SetValue(1);
        // Efecto.Parameters["MetrosLargo"]?.SetValue(ALTURA/TGCGame.S_METRO);
        // Efecto.Parameters["MetrosAncho"]?.SetValue(metrosLargo);
        TGCGame.GameContent.G_Cubo.Draw(Efecto);
    }
}
