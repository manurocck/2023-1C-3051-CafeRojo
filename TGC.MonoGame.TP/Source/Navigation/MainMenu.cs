using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Navigation;

internal class MainMenu : IMenuItem
{
    private float elapsedActionTime = 0;
    private bool Transition = false;
    private int OptionSelected = 0; // 0: Todavía ninguna, 1: PlayOption , 2: SettingsOption, 3: Controles 
    private Piso Piso = new Piso(15,15,new Vector3(-PistonDerby.S_METRO * 3f ,0,-PistonDerby.S_METRO * 3f ));
    private Pared Pared1, Pared2;

    public MainMenu(int width, int heigth) : base(width, heigth) {
        Pared1 = new Pared(Piso.PosicionInicial, Piso.PuntoExtremoIzquierdo(), false);
        Pared2 = new Pared(Piso.PosicionInicial, Piso.PuntoExtremoDerecho(), false);
        PistonDerby.Reproductor.Play();
    }

    private void StartGame()
    {
        Transition = true; 
        PistonDerby.Reproductor.Stop(); 
        PistonDerby.Reproductor.EngineCar();        
    }
    internal override IMenuItem Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
    {
        // Console.WriteLine("Mouse Position ( {0:F},{1:F} ) ", mouseState.Position.X/Window.X, mouseState.Position.Y/Window.Y );
        
        int LAST_OPT = 2;
        elapsedActionTime += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

        if( OptionSelected == 0 && keyboardState.GetPressedKeyCount()>0 && elapsedActionTime>1f ) {OptionSelected = 1; elapsedActionTime = 0; };

        if( (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) && elapsedActionTime > 0.2f && OptionSelected != 3){
            elapsedActionTime = 0;
            OptionSelected = (OptionSelected>1)? OptionSelected-1 : LAST_OPT;
            PistonDerby.GameContent.S_Bling.Play();
        }
        if( (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) && elapsedActionTime > 0.2f && OptionSelected != 3){
            elapsedActionTime = 0;
            OptionSelected = (OptionSelected==LAST_OPT)? 1 : OptionSelected+1;
            PistonDerby.GameContent.S_Bling.Play();
        }
        
        if((keyboardState.IsKeyDown(Keys.Enter) || keyboardState.IsKeyDown(Keys.Space)) && elapsedActionTime > 0.2f){
            elapsedActionTime = 0;
            if(OptionSelected == 1) this.StartGame(); // EMPIEZA EL JUEGO
            else if(OptionSelected == 2) OptionSelected = 3;
            else if(OptionSelected == 3) OptionSelected = 2;
            PistonDerby.GameContent.S_Bling.Play();
        }
        return this;
    }
    private void DrawTitle(float secondsElapsed)
    {
        var effect = PistonDerby.GameContent.E_TextureShader;
        effect.Parameters["View"].SetValue(HUDView);
        effect.Parameters["World"].SetValue(AjusteQuad() * QuadSize(0.75f,0.75f) * AjusteFinal(1));
        effect.Parameters["Texture"].SetValue(PistonDerby.GameContent.TM_Start);
        PistonDerby.GameContent.G_Quad.Draw(effect);
        this.DrawPlay(secondsElapsed);
    }
    private void DrawPlay(float secondsElapsed)
    {
        Effect efecto = PistonDerby.GameContent.E_TextureItermitente;
        efecto.Parameters["View"].SetValue(HUDView);
        efecto.Parameters["World"].SetValue(AjusteQuad() * QuadSize(0.75f,0.75f) * AjusteFinal(1));
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
        
        if(OptionSelected==0) 
            this.DrawTitle(secondsElapsed);
        else if (OptionSelected == 1 || OptionSelected == 2) 
            this.DrawOptions(secondsElapsed);
        else 
            this.DrawControles();

        return !Transition;
    }

    private void DrawControles(){
        var effect = PistonDerby.GameContent.E_TextureShader;
        effect.Parameters["View"].SetValue(HUDView);
        effect.Parameters["World"].SetValue(AjusteQuad() * QuadSize(0.75f,0.75f) * AjusteFinal(1));
        effect.Parameters["Texture"].SetValue(PistonDerby.GameContent.TM_Controles);
        PistonDerby.GameContent.G_Quad.Draw(effect);
    }
    private void DrawOptions(float secondsElapsed)
    {
        var effect = PistonDerby.GameContent.E_TextureItermitente; 
        effect.Parameters["View"].SetValue(HUDView);
        var gapOptions = 0.05f;

        
        effect.Parameters["Time"].SetValue(0.25f); // Menu Background
        effect.Parameters["World"].SetValue(AjusteQuad() * QuadSize(1f,0.3f) * AjusteFinal(0));
        effect.Parameters["Texture"].SetValue(PistonDerby.GameContent.TP_Presentacion0);
        PistonDerby.GameContent.G_Quad.Draw(effect);


        var alphaValue = 0f;
        alphaValue = (OptionSelected == 2)? 0.5f : 0;
        effect.Parameters["Time"].SetValue(alphaValue); // Play option
        effect.Parameters["Texture"].SetValue(PistonDerby.GameContent.TM_PlayOption);
        effect.Parameters["World"].SetValue(AjusteQuad() * QuadSize(0.35f,0.1f) * Up(gapOptions) * Right(0.05f) * AjusteFinal(0));
        PistonDerby.GameContent.G_Quad.Draw(effect);

        alphaValue = (OptionSelected == 1)? 0.5f : 0;
        effect.Parameters["Time"].SetValue(alphaValue); // Settings Option
        effect.Parameters["World"].SetValue(AjusteQuad() * QuadSize(0.35f,0.1f) * Down(gapOptions) * Right(0.05f) * AjusteFinal(0));
        effect.Parameters["Texture"].SetValue(PistonDerby.GameContent.TM_SettingsOption);
        PistonDerby.GameContent.G_Quad.Draw(effect);

        effect.Parameters["Time"].SetValue(0f); // Pointer
        var alturaSeleccionado = (OptionSelected == 1)? Up(gapOptions) : Down(gapOptions);
        effect.Parameters["World"].SetValue(AjusteQuad() * QuadSize(0.1f) * alturaSeleccionado * Left(0.15f) * AjusteFinal(0));
        effect.Parameters["Texture"].SetValue(PistonDerby.GameContent.TM_Pointer);
        PistonDerby.GameContent.G_Quad.Draw(effect);
    }
}