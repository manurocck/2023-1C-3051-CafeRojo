using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP;
// PISO
public class Piso
{
    private const float S_METRO = TGCGame.S_METRO;
    
    private Vector3 ColorDefault = new Color(19, 38, 47).ToVector3();
    private Texture2D TexturaBaldosa = TGCGame.GameContent.T_PisoMadera;
    private float CantidadBaldosasLargo = 1f; // para hacerlo en "Tiles"
    private float CantidadBaldosasAncho = 1f; // para hacerlo en "Tiles"
    private Matrix World;
    internal Vector3 PosicionInicial;
    private Effect Effect = TGCGame.GameContent.E_BasicShader;
    private readonly float MetrosAncho;
    private readonly float MetrosLargo;
    //private readonly int MetrosCuadrados;

    public Piso(int metrosAncho, int metrosLargo, Vector3 posicionInicial)
    {
        PosicionInicial = posicionInicial;
        PosicionInicial.Y += -15; // Hard-codeada para que el auto esté exáctamente sobre el piso, debería depender de S_METRO
        MetrosAncho = metrosAncho * S_METRO;
        MetrosLargo = metrosLargo * S_METRO;
        
        Matrix Scale = Matrix.CreateScale(MetrosLargo, 0f, MetrosAncho);
        World = Scale 
                * Matrix.CreateTranslation(PosicionInicial);
        var boxito = new Box(MetrosLargo*2,1f, MetrosAncho*2);

        TypedIndex index = TGCGame.Simulation.LoadShape<Box>(boxito);

        Vector3 fixedPosition = PosicionInicial - Vector3.UnitY;
        TGCGame.Simulation.CreateStatic(fixedPosition.ToBepu(), Quaternion.Identity.ToBepu(), index);
    }
    public Vector3 PuntoExtremo(){
        return PosicionInicial + ( new Vector3(MetrosLargo,0f,MetrosAncho) );
    }
    public Vector3 PuntoCentro() => this.PuntoExtremo()*0.5f;
    
    public Piso ConColor(Color color){
        ColorDefault = color.ToVector3();
        return this;
    }
    public Piso ConTextura(Texture2D texturaPiso, float baldosasAncho = 1, float baldosasLargo = 1){
        Effect = TGCGame.GameContent.E_TextureTiles;
        CantidadBaldosasAncho = baldosasAncho; 
        CantidadBaldosasLargo = baldosasLargo;
        TexturaBaldosa = texturaPiso;
        return this;
    }
    public void Draw()
    {
        // var body = TGCGame.Simulation.Statics.GetStaticReference(Handle);
        // var aabb = body.BoundingBox;
        // TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Red);

        Effect.Parameters["DiffuseColor"]?.SetValue(ColorDefault);
        Effect.Parameters["Texture"]?.SetValue(TexturaBaldosa);
        Effect.Parameters["TilesWide"]?.SetValue(CantidadBaldosasAncho);
        Effect.Parameters["TilesBroad"]?.SetValue(CantidadBaldosasLargo);
        Effect.Parameters["World"].SetValue(World);
        TGCGame.GameContent.G_Quad.Draw(Effect);
    }
}