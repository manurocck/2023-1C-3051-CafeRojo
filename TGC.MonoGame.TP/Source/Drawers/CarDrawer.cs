using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Drawers
{
    internal class CarDrawer : IDrawer
    {
        private const float AUTO_SCALE = 0.056f * TGCGame.S_METRO;
        private const float WHEEL_TURNING_LIMIT = 0.5f;
        private const float ERROR_TRASLACION_RUEDAS = AUTO_SCALE*0.01f;
        private static Effect Effect => TGCGame.GameContent.E_TextureShader;
        protected Model Model => TGCGame.GameContent.M_Auto;

        internal Vector3 CarPosition { private get; set; } = Vector3.Zero;
        internal float WheelRotation { get; set; } = 0;
        internal float WheelTurning { get; set; } = 0;

        /*internal override void Draw(Matrix GeneralWorld)
        {
            //Effect.Parameters["Texture"].SetValue(Texture[0]);
            DrawMesh(Model.Meshes[0], GeneralWorld);
            DrawFrontWheel(FrontWheelWorld);
            DrawBackWheel(FrontWheelWorld);
        }

        private void DrawMesh(ModelMesh mesh, Matrix GeneralWorld)
        {
            Matrix World = mesh.ParentBone.Transform * GeneralWorld;
            Effect.Parameters["World"].SetValue(World);
            mesh.Draw();
        }

        private void DrawFrontWheel(){
            World = 
                    // Matrix.CreateRotationX(VelocidadTablero*WHEEL_SPEED_AMOUNT) *
                    Matrix.CreateRotationX(WheelRotation) *
                    Matrix.CreateRotationY(WheelTurning) *
                    Matrix.CreateScale(AUTO_SCALE) * 
                    Matrix.CreateTranslation(bone.Transform.Translation*ERROR_TRASLACION_RUEDAS) *
                    //Matrix.CreateRotationY(Rotation) *
                    Matrix.CreateTranslation(Position);
            meshPart.Effect.Parameters["World"]?.SetValue(World);
        }*/


        void IDrawer.Draw(Matrix GeneralWorld){
            // acá se están dibujando las ruedas una vez. sacarlas del dibujado.
            Matrix World = GeneralWorld;
            foreach(var bone in Model.Bones){
                switch(bone.Name){
                    case "Car":
                        foreach(var mesh in bone.Meshes){  
                            foreach(var meshPart in mesh.MeshParts){
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            mesh.Draw();
                        }
                    break;
                    case "WheelA": // Adelante derecha
                    case "WheelB": // Adelante izquierda
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Rotación extra -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                World = 
                                        // Matrix.CreateRotationX(VelocidadTablero*WHEEL_SPEED_AMOUNT) *
                                        Matrix.CreateRotationX(WheelRotation) *
                                        Matrix.CreateRotationY(WheelTurning) *
                                        Matrix.CreateScale(AUTO_SCALE) * 
                                        Matrix.CreateTranslation(bone.Transform.Translation*ERROR_TRASLACION_RUEDAS) *
                                        //Matrix.CreateRotationY(Rotation) *
                                        Matrix.CreateTranslation(CarPosition);
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            // mesh.Draw();
                        }
                    break;
                    case "WheelC": // Atras izquierda
                    case "WheelD": // Atras derecha
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                World = 
                                        Matrix.CreateRotationX(WheelRotation) *
                                        Matrix.CreateScale(AUTO_SCALE) * 
                                        Matrix.CreateTranslation(bone.Transform.Translation*ERROR_TRASLACION_RUEDAS) *
                                        //Matrix.CreateRotationY(Rotation) *
                                        Matrix.CreateTranslation(CarPosition);
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            // mesh.Draw();
                        }
                    break;
                    default: 
                    break;
                }
            }
        }

    }
}



 