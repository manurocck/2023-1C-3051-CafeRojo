using System;
using BepuPhysics;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Viewer.Gizmos;
using TGC.MonoGame.TP.Collisions;

using System.Collections.Generic;
using TGC.MonoGame.TP.Elementos;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        public const float S_METRO = 250f; // Prueben con 250 y con 1000
        internal static TGCGame Game;
        internal static Content GameContent;
        internal static GameSimulation Simulation;
        internal static Gizmos Gizmos;
        //Lista temporal que contiene Elementos Dinamicos de manera Global || Probablemente Casa deba ser Global y contener esta lista
        internal static List<ElementoDinamico> ElementoDinamicos = new List<ElementoDinamico>();
        
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Auto Auto;
        private Camera Camera; 
        private Casa Casa;
        //private Song Soundtrack;

        public TGCGame()
        {
            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
            Casa = new Casa();

            Game.Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height*3/4;
            Game.Graphics.PreferredBackBufferWidth  = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width*3/4;
            Game.Graphics.ApplyChanges();
        
            Gizmos = new Gizmos();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Simulation = new GameSimulation();
            GameContent = new Content(Content, GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;     

            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "Content"));    
            // Culling
            // GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            // Soundtrack = GameContent.S_SynthWars;
            // MediaPlayer.IsRepeating = true;
            // MediaPlayer.Volume = 0.5f;

            foreach (var e in GameContent.Efectos)
                e.Parameters["Projection"].SetValue(Camera.Projection);

            Vector3 origen = Vector3.Zero;
            Vector3 fin = new Vector3(1f,1f,1f);
            
            Vector3 desplazamiento = new Vector3(5f,0f,5f);

            Casa.LoadContent();
            
            Auto  = new Auto (Casa.PuntoCentro(0));
            //Auto2 = new Auto2(Casa.PuntoCentro(0));
        }

        protected override void Update(GameTime gameTime)
        {

            Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);

            Simulation.Update();

            KeyboardState keyboardState =Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 

            Auto.Update(dTime, keyboardState);
            //Auto2.Update(gameTime, keyboardState);
                                    
            // Control de la música
            // if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Stopped)
            //     MediaPlayer.Play(Soundtrack);
            // else if (keyboardState.IsKeyUp(Keys.W) && MediaPlayer.State == MediaState.Playing)
            //     MediaPlayer.Pause();
            // else if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Paused)
            //     MediaPlayer.Resume();
            // else if (keyboardState.IsKeyDown(Keys.P) && MediaPlayer.State == MediaState.Playing)
            //     MediaPlayer.Stop();

            Casa.Update(dTime, keyboardState);
            
            Camera.Mover(keyboardState);
            Camera.Update(Auto.World());

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            foreach (var e in GameContent.Efectos){
                e.Parameters["View"].SetValue(Camera.View);
                e.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
            foreach (ElementoDinamico elementoDinamico in ElementoDinamicos)
                elementoDinamico.Draw();

            Auto.Draw();          
            //Auto2.Draw();          
            Casa.Draw();
            Gizmos.Draw();
        }
        
        protected override void UnloadContent()
        {
            Simulation.Dispose();
            base.UnloadContent();
        }
    }
}