using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Autos;
using PistonDerby.Geometries;

namespace PistonDerby.Drawers;
internal class CarDrawer : IDrawer
{
    private const float AUTO_SCALE = 0.056f * PistonDerby.S_METRO;
    private Effect Effect = PistonDerby.GameContent.E_PBRShader;
    private CylinderPrimitive BulletCylinder = PistonDerby.GameContent.G_Cilindro;

    //UPDATE
    internal float WheelRotation() => Auto.WheelRotation;
    internal float WheelTurning() => Auto.WheelTurning;
    internal float WheelFloorRotation() => Auto.WheelFloorRotation;
    internal Auto Auto;

    internal CarDrawer(Auto auto){
        Auto = auto;  
    }

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
        
        // DrawBulletCyllinder(world);

    }

    // BulletCyllinder drawer function : 
    //                          Draws a cyllinder using the car world matrix for the translation 
    //                          and the function WheelRotation() for the rotation.
    //                          It  goes from the roof of the car and ends at the floor 
    //                          at a certain distance away from the car.
    //                          It is used to draw the bullet trajectory so its radius is very small.
    //                          The bullets are going to be drawn inside the cyllinder using a shader
    //                          that will make them look like they are inside the cyllinder.
    private void DrawBulletCyllinder(Matrix carWorld){
        Effect bulletEffect = this.Auto.isShooting()?
                             PistonDerby.GameContent.E_BulletShader : 
                             PistonDerby.GameContent.E_BasicShader;

        Matrix world = 
                          Matrix.CreateScale(0.1f, 10f, 0.1f)
                        * Matrix.CreateRotationX(MathHelper.PiOver2+MathHelper.PiOver4*0.25f)
                        * Matrix.CreateRotationY(WheelRotation())
                        * Matrix.CreateTranslation(new Vector3(0, 1f, 6f))
                        * carWorld
                        ;
        bulletEffect.Parameters["World"]?.SetValue(world);
        bulletEffect.Parameters["DiffuseColor"]?.SetValue(Color.DarkRed.ToVector3());
        BulletCylinder.Draw(bulletEffect);
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



