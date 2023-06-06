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
using PistonDerby.Navigation;

namespace PistonDerby;

public class PistonDerby : Game 
{
    public const float S_METRO = 250f;
    internal static bool DEVELOPER_MODE = false;
    internal static bool DEBUG_GIZMOS = DEVELOPER_MODE;
    // internal static bool FULL_SCREEN = !DEVELOPER_MODE;
    internal static bool FULL_SCREEN = false;
    internal static bool INITIAL_ANIMATION = !DEVELOPER_MODE;
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
        Reproductor = new AudioPlayer();
        Gizmos = new Gizmos();
        Gizmos.Enabled = DEBUG_GIZMOS;
        Casa = new Casa();

        AutosDummy = new List<AutoDummy>();
        base.Initialize();
    }
    protected override void LoadContent() 
    {
        base.LoadContent();

        GameContent = new Content(Content, GraphicsDevice);
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        GameMenu = new GameMenu(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        
        Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "Content"));
        Reproductor.LoadContent();
        Casa.LoadContent();

        foreach (var e in GameContent.Efectos) e.Parameters["Projection"].SetValue(Camera.Projection);
        foreach (var e in GameContent.EfectosHUD) e.Parameters["Projection"].SetValue(Camera.Projection);

        AutosDummy.Add(new AutoDummy (Casa.PuntoCentro(0) * 0.5f));
        AutosDummy.Add(new AutoDummy (Casa.PuntoCentro(1)));
        AutosDummy.Add(new AutoDummy (Casa.PuntoCentro(2)));

        Auto   = new Auto (Casa.PuntoCentro(0));
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

        Reproductor.Update(dTime, Keyboard.GetState());                      

        Casa.Update(dTime, keyboardState);
        Auto.Update(dTime, Keyboard.GetState());
        foreach(AutoDummy a in AutosDummy)
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
}
