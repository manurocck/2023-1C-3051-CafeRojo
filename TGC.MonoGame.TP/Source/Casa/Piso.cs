using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Elementos;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP;
// PISO
public class Piso : ElementoEstatico
{
    private Texture2D TexturaBaldosa = TGCGame.GameContent.T_PisoMadera;
    private float TextureTilesLargo;
    private float TextureTilesAncho;
    private readonly float MetrosAncho;
    private readonly float MetrosLargo;
    internal Vector3 PosicionInicial;
    internal StaticHandle Handle;
    private Matrix TempWorld;

    public Piso(int metrosAncho, int metrosLargo, Vector3 posicionInicial) : base(new GeometryTextureDrawer(TGCGame.GameContent.G_Quad, TGCGame.GameContent.T_PisoMadera), Vector3.Zero, Vector3.Zero)
    {
        PosicionInicial = posicionInicial;
        PosicionInicial.Y += -15; // Hard-codeada para que el auto esté exáctamente sobre el piso, debería depender de S_METRO
        MetrosAncho = metrosAncho * TGCGame.S_METRO;
        MetrosLargo = metrosLargo * TGCGame.S_METRO;
        
        Matrix Scale = Matrix.CreateScale(MetrosLargo, 0f, MetrosAncho);
        TempWorld = Scale 
                * Matrix.CreateTranslation(PosicionInicial);
        var boxito = new Box(MetrosLargo,1f, MetrosAncho);

        TypedIndex index = TGCGame.Simulation.LoadShape<Box>(boxito);

        Vector3 fixedPosition = this.PuntoCentro() - Vector3.UnitY;
        Handle = TGCGame.Simulation.CreateStatic(fixedPosition.ToBepu(), Quaternion.Identity.ToBepu(), index);
    }
    public Vector3 PuntoExtremo() => PosicionInicial + ( new Vector3(MetrosLargo,0f,MetrosAncho) );
    public Vector3 PuntoCentro() => PosicionInicial + ( new Vector3(MetrosLargo*0.5f,0f,MetrosAncho*0.5f) );
    public Piso ConTextura(Texture2D texturaPiso, float baldosasAncho = 1, float baldosasLargo = 1){
        TextureTilesAncho = baldosasAncho; 
        TextureTilesLargo = baldosasLargo;
        TexturaBaldosa = texturaPiso;
        return this;
    }
    internal override void Draw()
    {
        this.DebugGizmos();
        Effect Effect = TGCGame.GameContent.E_TextureTiles;

        Effect.Parameters["Texture"]?.SetValue(TexturaBaldosa);
        Effect.Parameters["TilesWide"]?.SetValue(TextureTilesAncho);
        Effect.Parameters["TilesBroad"]?.SetValue(TextureTilesLargo);
        Effect.Parameters["World"].SetValue(TempWorld);

        TGCGame.GameContent.G_Quad.Draw(Effect);
    }

    private void DebugGizmos()
    {
        var body = TGCGame.Simulation.GetStaticReference(Handle);
        var aabb = body.BoundingBox;
        TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Red);
    }
}