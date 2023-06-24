using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Autos;

namespace PistonDerby.Drawers;
internal class CarDrawer : IDrawer
{
    private const float AUTO_SCALE = 0.056f * PistonDerby.S_METRO;
    private const float WHEEL_TURNING_LIMIT = 0.5f;
    private const float ERROR_TRASLACION_RUEDAS = AUTO_SCALE*0.01f;
    private Effect Effect = PistonDerby.GameContent.E_PBRShader;

    //UPDATE
    internal Vector3 CarPosition { private get; set; } = Vector3.Zero;
    internal float WheelRotation() => Auto.WheelRotation;
    internal float WheelTurning() => Auto.WheelTurning;
    internal Auto Auto;

    internal CarDrawer(Auto auto){
        Auto = auto;
         
    }

    // internal CarDrawer(Auto auto, Effect shader) {Auto = auto; Effect = shader;}
    void IDrawer.Draw(Model Model, Matrix GeneralWorld){
        Matrix world = GeneralWorld;
        Matrix worldAux = Matrix.Identity;
        this.Effect.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.TA_MetalMap);
        SetearTexturas();
        
        if(Model.Bones.Count > 0)
        foreach(ModelBone bone in Model.Bones){
            switch(bone.Name){
                case "Car":
                    worldAux = world;
                break;
                case "WheelA": // FRONT RIGHT
                case "WheelB": // FRONT LEFT
                    worldAux =  Matrix.CreateScale(1.5f)
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
                                * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                * Matrix.CreateRotationY(WheelRotation())                   // giro con el auto
                                * world 
                                ;
                break;
            }

            Effect.Parameters["World"]?.SetValue(worldAux);
            Effect.Parameters["matInverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(worldAux)));
        
            Effect.Parameters["DiffuseColor"].SetValue(new Vector3(255f, 0.5f, 255f));
            foreach(var mesh in bone.Meshes){  
                foreach(var meshPart in mesh.MeshParts){
                    meshPart.Effect = Effect;
                }
                mesh.Draw();
            }
        }
        else{
            Effect.Parameters["World"]?.SetValue(worldAux);
            Effect.Parameters["matInverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(worldAux)));
            Effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0.5f, 0.5f));
            foreach(var mesh in Model.Meshes){  
                foreach(var meshPart in mesh.MeshParts){
                    meshPart.Effect = Effect;
                }
                mesh.Draw();
            }
        }
    }

    private void SetearTexturas()
    {
        Effect.Parameters["albedoTexture"]?.SetValue(PistonDerby.GameContent.TA_BaseColor);
        Effect.Parameters["normalTexture"]?.SetValue(PistonDerby.GameContent.TA_NormalMap);
        Effect.Parameters["metallicTexture"]?.SetValue(PistonDerby.GameContent.TA_MetalMap);
        Effect.Parameters["roughnessTexture"]?.SetValue(PistonDerby.GameContent.TA_RoughnessMap);
        Effect.Parameters["aoTexture"]?.SetValue(PistonDerby.GameContent.TA_CavityMap);
    }
}



