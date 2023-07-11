using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Drawers;
using PistonDerby.Elementos;
using PistonDerby.Utils;

namespace PistonDerby;
// PISO
public class Piso : ElementoEstatico
{
    private Texture2D TexturaBaldosa = PistonDerby.GameContent.T_PisoMadera;
    private Effect Effect;
    private float TextureTilesLargo;
    private float TextureTilesAncho;
    private readonly float MetrosAncho;
    private readonly float MetrosLargo;
    internal Vector3 PosicionInicial;
    internal StaticHandle Handle;
    private Matrix TempWorld;
    protected readonly BoundingBox PeliculaContacto;

    public Piso(int metrosAncho, int metrosLargo, Vector3 posicionInicial) : base(Vector3.Zero, new Box(0.001f,0.001f,0.001f), null, new GeometryTextureDrawer(PistonDerby.GameContent.G_Quad, PistonDerby.GameContent.T_PisoMadera), Vector3.Zero, Vector3.Zero)
    {
        Effect = PistonDerby.GameContent.E_BlinnPhongTiles;
        PosicionInicial = posicionInicial;
        PosicionInicial.Y += -15; // Hard-codeada para que el auto esté exáctamente sobre el piso, debería depender de S_METRO
        MetrosAncho = metrosAncho * PistonDerby.S_METRO;
        MetrosLargo = metrosLargo * PistonDerby.S_METRO;
        
        Matrix Scale = Matrix.CreateScale(MetrosLargo, 0f, MetrosAncho);
        TempWorld = Scale 
                * Matrix.CreateTranslation(PosicionInicial);
        var boxito = new Box(MetrosLargo,1f, MetrosAncho);

        TypedIndex index = PistonDerby.Simulation.LoadShape<Box>(boxito);

        Vector3 fixedPosition = this.PuntoCentro();
        Handle = PistonDerby.Simulation.CreateStatic(fixedPosition.ToBepu(), Quaternion.Identity.ToBepu(), index);

        // PeliculaContacto = new BoundingBox(this.Static().BoundingBox.Min+Vector3.UnitY, this.Static().BoundingBox.Max+Vector3.UnitY);
    }
    public Vector3 PuntoExtremo() => PosicionInicial + ( new Vector3(MetrosLargo,0f,MetrosAncho) );
    public Vector3 PuntoExtremoIzquierdo() => PosicionInicial + ( new Vector3(0f,0f,MetrosAncho) );
    public Vector3 PuntoExtremoDerecho() => PosicionInicial + ( new Vector3(MetrosLargo,0f,0f) );
    public Vector3 PuntoCentro() => PosicionInicial + ( new Vector3(MetrosLargo*0.5f,0f,MetrosAncho*0.5f) );
    public Piso ConTextura(Texture2D texturaPiso, float baldosasAncho = 1, float baldosasLargo = 1){
        TextureTilesAncho = baldosasAncho; 
        TextureTilesLargo = baldosasLargo;
        TexturaBaldosa = texturaPiso;
        return this;
    }
    public void TextureShaderHUD() => this.Effect = PistonDerby.GameContent.E_TextureTiles;
    internal override void Draw()
    {
        //Effect = PistonDerby.GameContent.E_TextureTiles;

        Effect.Parameters["Texture"]?.SetValue(TexturaBaldosa); 
        Effect.Parameters["TilesWide"]?.SetValue(TextureTilesAncho);
        Effect.Parameters["TilesBroad"]?.SetValue(TextureTilesLargo);
        Effect.Parameters["World"].SetValue(TempWorld);
        Effect.Parameters["matInverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(World)));
        
        Effect.Parameters["KAmbient"]?.SetValue(0.15f);
        Effect.Parameters["KDiffuse"]?.SetValue(0.3f);
        Effect.Parameters["KSpecular"]?.SetValue(0.3f);
        Effect.Parameters["shininess"]?.SetValue(30);

        PistonDerby.GameContent.G_Quad.Draw(Effect);
    }

    public new void DebugGizmos()
    {
        var body = PistonDerby.Simulation.GetStaticReference(Handle);
        var aabb = body.BoundingBox;
        PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.ForestGreen);
    }
}