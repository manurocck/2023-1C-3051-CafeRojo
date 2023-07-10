using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Autos;
using PistonDerby.Geometries;

namespace PistonDerby.Drawers;
internal class DeadCarDrawer : IDrawer
{
    private Effect Effect = PistonDerby.GameContent.E_DeadEnemyShader;

    //UPDATE
    internal float WheelRotation() => Auto.WheelRotation;
    internal float WheelTurning() => Auto.WheelTurning;
    internal float WheelFloorRotation() => Auto.WheelFloorRotation;
    internal Auto Auto;

    internal DeadCarDrawer(Auto auto) => Auto = auto; 

    // internal CarDrawer(Auto auto, Effect shader) {Auto = auto; Effect = shader;}
    void IDrawer.Draw(Model Model, Matrix GeneralWorld){
        Matrix world = GeneralWorld;
        Matrix worldAux = Matrix.Identity;
                
        if(Model.Bones.Count > 0)
        foreach(ModelBone bone in Model.Bones){
            switch(bone.Name){
                case "Car":
                    worldAux = world;
                break;
                case "WheelA": // FRONT RIGHT
                case "WheelB": // FRONT LEFT
                    worldAux =  Matrix.CreateScale(1.5f)
                                * Matrix.CreateRotationX(WheelFloorRotation())              // giro con el piso (funcará?)
                                * Matrix.CreateRotationY(WheelTurning())                      // giro del volante
                                * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                * Matrix.CreateRotationY(WheelRotation())                     // giro con el auto
                                * world
                                ;
                break;
                case "WheelC": // BACK LEFT
                case "WheelD": // BACK RIGHT
                    worldAux = 
                                Matrix.CreateScale(2f)
                                * Matrix.CreateRotationX(WheelFloorRotation())              // giro con el piso (funcará?)
                                * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                * Matrix.CreateRotationY(WheelRotation())                   // giro con el auto
                                * world 
                                ;
                break;
            }

            Effect.Parameters["World"]?.SetValue(worldAux);
            foreach(var mesh in bone.Meshes){  
                foreach(var meshPart in mesh.MeshParts){
                    meshPart.Effect = Effect;
                }
                mesh.Draw();
            }
        }
        else{
            Effect.Parameters["World"]?.SetValue(worldAux);
            foreach(var mesh in Model.Meshes){  
                foreach(var meshPart in mesh.MeshParts){
                    meshPart.Effect = Effect;
                }
                mesh.Draw();
            }
        }
    }
}



