using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby;

class Camera
{
    private float DISTANCIA_AL_AUTO = 4f * PistonDerby.S_METRO;
    internal Vector3 CameraPosition = Vector3.Zero;
    private Vector3 FollowedPosition = Vector3.Zero;
    public Matrix Projection { get; private set; }
    public Matrix View { get; private set; }

    public Camera(float aspectRatio)
    {
        //Matriz de proyeccion casi isometrica, entre mas cerca del 0 este el primer 
        // valor se respeta mas la isometria pero tambien se rompe todo si es muy bajo
        Projection = Matrix.CreatePerspectiveFieldOfView(0.5f, aspectRatio, 0.1f, 100000f);
    }
    public void Mover(KeyboardState keyboardState){
        var multiplicador = 0.025f*PistonDerby.S_METRO;
        if(keyboardState.IsKeyDown(Keys.LeftShift)){
            multiplicador = 1f;
        }   
        if(keyboardState.IsKeyDown(Keys.Down)){
            DISTANCIA_AL_AUTO += 2f*multiplicador;
        }
        if(keyboardState.IsKeyDown(Keys.Up)){
            DISTANCIA_AL_AUTO -= 2f*multiplicador;
        }
    }
    public void Update(Matrix followedWorld)
    {
        FollowedPosition = followedWorld.Translation;
        CameraPosition = FollowedPosition + new Vector3(1, 1, 1) * DISTANCIA_AL_AUTO;
        Vector3 cameraNormal = new Vector3(-1, 1, -1);

        //Matriz de vista isometrica
        View = Matrix.CreateLookAt(CameraPosition, FollowedPosition, cameraNormal);

    }
}

