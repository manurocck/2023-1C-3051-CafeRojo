using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Autos;

namespace PistonDerby.Navigation;

internal class MainMenu : IMenuItem
{
    private bool Transition = false;
    private Piso Piso = new Piso(15,15,new Vector3(-PistonDerby.S_METRO * 3f ,0,-PistonDerby.S_METRO * 3f ));
    private Pared Pared1, Pared2;
    private AutoIA AutoIA;

    private (float X, float Y ) Window;
    public MainMenu(int width, int heigth) : base(width, heigth) {
        Window.X = width;
        Window.Y = heigth;
        Pared1 = new Pared(Piso.PosicionInicial, Piso.PuntoExtremoIzquierdo());
        Pared2 = new Pared(Piso.PosicionInicial, Piso.PuntoExtremoDerecho());
    }

    internal override IMenuItem Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
    {
        Console.WriteLine("Mouse Position ( {0:F},{1:F} ) ", mouseState.Position.X/Window.X, mouseState.Position.Y/Window.Y );
        Transition = keyboardState.IsKeyDown(Keys.Enter);
        return this;
    }
    private void DrawTitle(){
        var effect = PistonDerby.GameContent.E_TextureShader;
        effect.Parameters["View"].SetValue(HUDView);
        effect.Parameters["World"].SetValue(FullScreenWorld());
        effect.Parameters["Texture"].SetValue(PistonDerby.GameContent.TM_Start);
        PistonDerby.GameContent.G_Quad.Draw(effect);
    }

    private void DrawPlay(float secondsElapsed)
    {
        Effect efecto = PistonDerby.GameContent.E_TextureItermitente;
        efecto.Parameters["View"].SetValue(HUDView);
        efecto.Parameters["World"].SetValue(FullScreenWorld());
        efecto.Parameters["Texture"].SetValue(PistonDerby.GameContent.TM_Play);
        efecto.Parameters["Time"].SetValue(secondsElapsed%1);

        PistonDerby.GameContent.G_Quad.Draw(efecto);
        efecto.Parameters["Time"].SetValue(1);
    }
    private void DrawSpinningCar(float secondsElapsed, Matrix isometricView)
    {
        Effect effect = PistonDerby.GameContent.E_SpiralShader;
        // var isometricView = Matrix.CreateLookAt(Vector3.One *500f, Vector3.Zero, new Vector3(-1,1,-1));

        effect.Parameters["View"].SetValue(isometricView);

        Model model = PistonDerby.GameContent.M_Auto;
        Matrix world;
        float transitionFactor = secondsElapsed*2/MathHelper.TwoPi;
        Vector3 transition = new Vector3(PistonDerby.S_METRO*4 * MathF.Sin(transitionFactor * 2) , 0 , PistonDerby.S_METRO*2 * MathF.Cos(transitionFactor));
        transition+= Piso.PuntoCentro()/2;

        foreach(ModelBone bone in model.Bones){
            world = bone.ModelTransform * Matrix.CreateRotationY(MathHelper.Pi + transitionFactor * 2) * Matrix.CreateScale(30) * Matrix.CreateTranslation(transition);
            // effect.Parameters["World"].SetValue(world*AjusteFinal(40));
            effect.Parameters["World"].SetValue(world);
            foreach(ModelMesh mesh in bone.Meshes){
                foreach(ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
                mesh.Draw();
            }
        }
    }
    internal void DrawEmptyHouse(float secondsElapsed)
    {

        Piso.ConTextura(PistonDerby.GameContent.T_Concreto,2,4);

        Effect effect = PistonDerby.GameContent.E_TextureTiles;
        Matrix isometricView = Matrix.CreateLookAt( Piso.PuntoCentro() + Vector3.UnitY * 1 * PistonDerby.S_METRO, 
                                                    Piso.PosicionInicial + Vector3.UnitX * 1 * PistonDerby.S_METRO, 
                                                    new Vector3(-0.33f,1,-0.33f));
        effect.Parameters["View"].SetValue(isometricView);


        this.DrawSpinningCar(secondsElapsed, isometricView);
        Piso.Draw();
        Pared1.Draw(PistonDerby.GameContent.T_Ladrillos);
        Pared2.Draw(PistonDerby.GameContent.T_Ladrillos);

    }
    internal override bool Draw(float secondsElapsed) { 
        

        this.DrawEmptyHouse(secondsElapsed);
               
        this.DrawTitle();
        this.DrawPlay(secondsElapsed);
        // effect = PistonDerby.GameContent.E_BasicShader;
        // effect.Parameters["World"].SetValue(FullScreenWorld());
        // effect.Parameters["View"].SetValue(HUDView);

        // effect.Parameters["Intensidad"].SetValue(0.5f);
        // effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
        
        return !Transition;
    }
}