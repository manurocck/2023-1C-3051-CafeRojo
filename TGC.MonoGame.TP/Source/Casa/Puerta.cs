
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby;
public class Puerta{

    private List<Pared> Paredes = new List<Pared>();
    private const float ANCHO_PUERTA = 0.25f;
    private Effect Efecto = PistonDerby.GameContent.E_TextureShader;

    // Ubicacion es la distancia a la que está del origen (0 es lo cerca posible y 1 es lo más lejos posible)
    public Puerta(Vector3 puntoInicio, Vector3 puntoFinal, float ubicacionPuerta){

        var esHorizontal = (puntoInicio.X == puntoFinal.X);

        // Si la puerta está al revés, se invierten los puntos
        // (para que el punto inicial siempre sea el más cercano al origen)
        // (y el punto final siempre sea el más lejano al origen)

        if(!esHorizontal && Math.Abs(puntoFinal.X) > 0)
        {
            var temp = puntoInicio;
                puntoInicio = puntoFinal;
                puntoFinal = temp;
        }
        // else if(esHorizontal && puntoFinal.Z > 0)
        // {
        //     var temp = puntoInicio;
        //         puntoInicio = puntoFinal;
        //         puntoFinal = temp;
        // }
        
        Vector3 finPrimerSegmento = (!esHorizontal) ? 
                                    new Vector3(puntoInicio.X * ubicacionPuerta, puntoInicio.Y, puntoInicio.Z):
                                    new Vector3(puntoFinal.X, puntoFinal.Y, puntoFinal.Z * ubicacionPuerta);

        Vector3 inicioSegundoSegmento = (!esHorizontal) ? 
                                    new Vector3(puntoInicio.X * (ubicacionPuerta - ANCHO_PUERTA), puntoInicio.Y, puntoInicio.Z):
                                    new Vector3(puntoFinal.X, puntoFinal.Y, puntoFinal.Z * (ubicacionPuerta + ANCHO_PUERTA));
        
        Pared primerSegmento  = new Pared(puntoInicio, finPrimerSegmento);
        Pared segundoSegmento = new Pared(inicioSegundoSegmento, puntoFinal);

        Paredes.Add(primerSegmento);
        Paredes.Add(segundoSegmento);

    }

    public void SetEffect(Effect effect) { 
        foreach(Pared p in Paredes) p.SetEffect(effect);
    }
    public void Draw(Texture2D textura){
        foreach(Pared p in Paredes) p.Draw(textura);
    }
    public void DebugGizmos() { foreach(Pared p in Paredes) p.DebugGizmos(); }
}
