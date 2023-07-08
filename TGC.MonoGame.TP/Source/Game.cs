using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PistonDerby.Elementos;
using PistonDerby.Collisions;
using PistonDerby.Utils;
using PistonDerby.Gizmo;
using PistonDerby.HUD;
using PistonDerby.Autos;

namespace PistonDerby;

public class PistonDerby : Game 
{
    public const float S_METRO = 250f;
    internal static bool DEVELOPER_MODE = false;
    internal static bool DEBUG_GIZMOS = false;
    internal static bool FULL_SCREEN = false;
    internal static bool INITIAL_ANIMATION = true;
    private GraphicsDeviceManager Graphics;
    private SpriteBatch SpriteBatch;
    internal static GameSimulation Simulation;
    internal static Content GameContent;
    internal static GameMenu GameMenu;
    internal static Gizmos Gizmos;
    private CarHUD CarHUD;
    internal static AudioPlayer Reproductor;
    private Camera Camera; 
    private Casa Casa;
    private Auto Auto; 
    private List<AutoDummy> AutosDummy;
    private List<AutoAI> AutosAI;
    internal static List<ElementoDinamico> ElementosDinamicos = new List<ElementoDinamico>(); //Lista temporal que contiene Elementos Dinamicos de manera Global || Probablemente Casa deba ser Global y contener esta lista

    public PistonDerby() {
        Graphics = new GraphicsDeviceManager(this);
        IsMouseVisible = true;
    }

    protected override void Initialize() 
    {
        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        GraphicsDevice.RasterizerState = rasterizerState; // CullCounterClockwise; para activar el Culling
        GraphicsDevice.BlendState = BlendState.AlphaBlend;     

        Graphics.PreferredBackBufferHeight = (DEVELOPER_MODE)?
                                             GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height : 
                                             GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        Graphics.PreferredBackBufferWidth  = (DEVELOPER_MODE)?
                                             Graphics.PreferredBackBufferHeight*16/9 :
                                             GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        Graphics.IsFullScreen = FULL_SCREEN;

        
        Graphics.ApplyChanges();
    
        Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
        Simulation = new GameSimulation();
        if(!DEVELOPER_MODE) Reproductor = new AudioPlayer();
        Gizmos = new Gizmos();
        Gizmos.Enabled = DEBUG_GIZMOS;
        Casa = new Casa();

        AutosDummy = new List<AutoDummy>();
        AutosAI = new List<AutoAI>();
        base.Initialize();
    }
    protected override void LoadContent() 
    {
        base.LoadContent();

        GameContent = new Content(Content, GraphicsDevice);
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        GameMenu = new GameMenu(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        
        Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "Content"));
        Reproductor?.LoadContent();
        Casa.LoadContent();

        foreach (var e in GameContent.Efectos) e.Parameters["Projection"].SetValue(Camera.Projection);
        foreach (var e in GameContent.EfectosHUD) e.Parameters["Projection"].SetValue(Camera.Projection);

        ConfiguracionBlinnPhong(PistonDerby.GameContent.E_BlinnPhong);
        ConfiguracionBlinnPhong(PistonDerby.GameContent.E_BlinnPhongTiles);

        AutosDummy.Add(new AutoDummy (Casa.PuntoCentro(0) * 0.5f));
        AutosDummy.Add(new AutoDummy (Casa.PuntoCentro(1)));
        AutosDummy.Add(new AutoDummy (Casa.PuntoCentro(2)));

        Auto   = new Auto (Casa.PuntoCentro(0));
        AutosAI.Add(new AutoAI (Auto, Casa.PuntoCentro(2)));
        
        
        CarHUD = new CarHUD(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        Auto.AsociarHUD(CarHUD);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 

        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
        if (Keyboard.GetState().IsKeyDown(Keys.G)) Gizmos.Enabled = !Gizmos.Enabled;
        if(DEBUG_GIZMOS) Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);

        if(GameMenu.isRunning() && INITIAL_ANIMATION) {
            GameMenu.Update(gameTime, keyboardState, Mouse.GetState());
            return;
        }

        // Actualizo la posición de la cámara en el shader de PBR
        foreach(Effect e in PistonDerby.GameContent.Efectos)
            e.Parameters["eyePosition"]?.SetValue(Camera.CameraPosition);

        Reproductor?.Update(dTime, Keyboard.GetState());                      

        Casa.Update(dTime, keyboardState);
        Auto.Update(dTime, Keyboard.GetState());
        foreach(AutoDummy a in AutosDummy)
            a.Update(dTime);
        foreach(AutoAI a in AutosAI)
            a.Update(dTime);

        foreach(ElementoDinamico e in ElementosDinamicos) e.Update(dTime, keyboardState);

        Camera.Mover(keyboardState);
        Camera.Update(Auto.World);

        Simulation.Update();
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        if(GameMenu.isRunning() && INITIAL_ANIMATION) {
            GameMenu.Draw(gameTime);
            return;
        }

        foreach (var e in GameContent.Efectos){
            e.Parameters["View"].SetValue(Camera.View);
            e.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
        }
        foreach (ElementoDinamico elementoDinamico in ElementosDinamicos)
            elementoDinamico.Draw();

        Auto.Draw(); 
        foreach(AutoDummy a in AutosDummy) a.Draw();
        foreach(AutoAI a in AutosAI) a.Draw();
                 
        Casa.Draw();
        Gizmos.Draw();

        this.DebugGizmos();

        CarHUD.Draw();
    }
    private void DebugGizmos()
    {
        Casa.DebugGizmos();
        BoundingBox aabb;
        foreach(ElementoDinamico e in ElementosDinamicos){
            aabb = e.Body().BoundingBox.ToBoundingBox(); 
            PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.DeepPink);
        }
    }
    protected override void UnloadContent()
    {
        Simulation.Dispose();
        base.UnloadContent();
    }
    private void ConfiguracionBlinnPhong(Effect e)
    {
        e.Parameters["ambientColor"].SetValue(new Vector3(255, 255, 255) / 255f);
        e.Parameters["diffuseColor"].SetValue(new Vector3(255, 255, 255) / 255f); // Adjusted the blue component to reduce yellow tint
        e.Parameters["specularColor"].SetValue(new Vector3(0, 148, 148) / 255f);

        e.Parameters["KAmbient"].SetValue(0.15f);
        e.Parameters["KDiffuse"].SetValue(0.3f);
        e.Parameters["KSpecular"].SetValue(0.3f);

        e.Parameters["shininess"].SetValue(60); // No shininess, as there are no specular highlights

    }
}
