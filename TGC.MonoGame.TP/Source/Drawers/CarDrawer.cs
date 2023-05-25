using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Drawers;
internal class CarDrawer : IDrawer
{
    private const float AUTO_SCALE = 0.056f * TGCGame.S_METRO;
    private const float WHEEL_TURNING_LIMIT = 0.5f;
    private const float ERROR_TRASLACION_RUEDAS = AUTO_SCALE*0.01f;
    private static Effect Effect => TGCGame.GameContent.E_SpiralShader;
    protected Model Model => TGCGame.GameContent.M_Auto;

    //UPDATE
    internal Vector3 CarPosition { private get; set; } = Vector3.Zero;
    internal float WheelRotation { get; set; } = 0;
    internal float WheelTurning { get; set; } = 0;

    internal CarDrawer(){}
    void IDrawer.Draw(Matrix GeneralWorld){
        //var simuWorld = this.Body();
        var world = GeneralWorld;
        //var aabb = simuWorld.BoundingBox;
        //TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);
        //var quaternion = simuWorld.Pose.Orientation;
        var worldAux = Matrix.Identity;
        
        // set effect (se puede optimizar)
        foreach(var mesh in Model.Meshes)
        foreach(var meshPart in mesh.MeshParts)
            meshPart.Effect = Effect;

        // acá se están dibujando las ruedas una vez. sacarlas del dibujado.
        foreach(var bone in Model.Bones){
            switch(bone.Name){
                case "Car":
                    foreach(var mesh in bone.Meshes){  
                        foreach(var meshPart in mesh.MeshParts){
                            meshPart.Effect.Parameters["World"]?.SetValue(world);
                        }
                        mesh.Draw();
                    }
                break;
                case "WheelA": // Adelante derecha
                case "WheelB": // Adelante izquierda
                    worldAux = 
                                Matrix.CreateScale(1.5f)
                                * Matrix.CreateRotationY(WheelTurning)                      // giro del volante
                                * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                * Matrix.CreateRotationY(WheelRotation)                     // giro con el auto
                                * world
                                ;
                    foreach(var mesh in bone.Meshes){
                        foreach(var meshPart in mesh.MeshParts){
                            // Escalo -> Rotación extra -> Llevo a su lugar -> Rotación auto -> Traslación auto
                            meshPart.Effect.Parameters["World"]?.SetValue(worldAux);
                        }
                        mesh.Draw();
                    }
                break;
                case "WheelC": // Atras izquierda
                case "WheelD": // Atras derecha
                    worldAux = 
                                Matrix.CreateScale(2f)
                                * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                * Matrix.CreateRotationY(WheelRotation)                   // giro con el auto
                                * world 
                                ;
                    foreach(var mesh in bone.Meshes){
                        foreach(var meshPart in mesh.MeshParts){
                            meshPart.Effect.Parameters["World"]?.SetValue(worldAux);
                        }
                        mesh.Draw();
                    }
                break;
            }
        }
    }

}



