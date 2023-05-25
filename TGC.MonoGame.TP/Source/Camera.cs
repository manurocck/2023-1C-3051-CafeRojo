using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TGC.MonoGame.TP;

class Camera
{
    private float DISTANCIA_AL_AUTO = 4f * TGCGame.S_METRO;
    public Matrix Projection { get; private set; }
    public Matrix View { get; private set; }

    public Camera(float aspectRatio)
    {
        //Matriz de proyeccion casi isometrica, entre mas cerca del 0 este el primer 
        // valor se respeta mas la isometria pero tambien se rompe todo si es muy bajo
        Projection = Matrix.CreatePerspectiveFieldOfView(0.5f, aspectRatio, 0.1f, 100000f);
    }
    public void Mover(KeyboardState keyboardState){
        var multiplicador = 0.025f*TGCGame.S_METRO;
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
        var followedPosition = followedWorld.Translation;
        
        var offsetedPosition = followedPosition 
            + Vector3.Right * DISTANCIA_AL_AUTO
            + Vector3.Up * DISTANCIA_AL_AUTO;

        var forward = (followedPosition - offsetedPosition);
        forward.Normalize();

        var right = Vector3.Cross(forward, Vector3.Up);
        var cameraCorrectUp = Vector3.Cross(right, forward);

        //View = Matrix.CreateLookAt(offsetedPosition, followedPosition, cameraCorrectUp);

        //Matriz de vista isometrica
        View = Matrix.CreateLookAt(followedPosition + new Vector3(1, 1, 1) * DISTANCIA_AL_AUTO, followedPosition, new Vector3(-1, 1, -1));

    }
}

