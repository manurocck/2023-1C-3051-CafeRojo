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
    private const bool DEBUG_GIZMOS = false;
    private const bool FULL_SCREEN = false;
    private GraphicsDeviceManager Graphics;
    private SpriteBatch SpriteBatch;
    internal static GameSimulation Simulation;
    internal static Content GameContent;
    internal static Gizmos Gizmos;
    private PistonDerbyHUD HUD;
    private AudioPlayer Reproductor;
    private Camera Camera; 
    private Casa Casa;
    private Auto Auto;
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

        Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        Graphics.PreferredBackBufferWidth  = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        Graphics.IsFullScreen=FULL_SCREEN;

        HUD = new PistonDerbyHUD(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        
        Graphics.ApplyChanges();
    
        Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
        Simulation = new GameSimulation();
        Reproductor = new AudioPlayer();
        Gizmos = new Gizmos();
        Gizmos.Enabled = DEBUG_GIZMOS;
        Casa = new Casa();


        base.Initialize();
    }
    protected override void LoadContent() 
    {
        base.LoadContent();

        GameContent = new Content(Content, GraphicsDevice);
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "Content"));
        Reproductor.LoadContent();
        Casa.LoadContent();

        foreach (var e in GameContent.Efectos) e.Parameters["Projection"].SetValue(Camera.Projection);
        foreach (var e in GameContent.EfectosHUD) e.Parameters["Projection"].SetValue(Camera.Projection);

        Auto  = new Auto (Casa.PuntoCentro(0));
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 

        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        Reproductor.Update(Keyboard.GetState());
        Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);
                                        
        Casa.Update(dTime, keyboardState);
        Auto.Update(dTime, Keyboard.GetState());
        foreach(ElementoDinamico e in ElementosDinamicos) e.Update(dTime, keyboardState);
        
        Camera.Mover(keyboardState);

        Camera.Update(Auto.World());
        HUD.Update(Auto.World());
        
        Simulation.Update();

        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DimGray);

        foreach (var e in GameContent.Efectos){
            e.Parameters["View"].SetValue(Camera.View);
            e.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
        }
        foreach (ElementoDinamico elementoDinamico in ElementosDinamicos)
            elementoDinamico.Draw();

        Auto.Draw();          
        Casa.Draw();
        Gizmos.Draw();

        HUD.Draw();

        this.DebugGizmos();
    }
    private void DebugGizmos()
    {
        Auto.DebugGizmos();
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
