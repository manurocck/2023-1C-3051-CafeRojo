using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP
{
    public class Puerta{

        private List<Pared> Paredes = new List<Pared>();
        private const float ANCHO_PUERTA = 0.15f;

        public Puerta(Vector3 puntoInicio, Vector3 puntoFinal, float ubicacionPuerta, bool esHorizontal){

        if(!esHorizontal){
            if(puntoFinal.X > 0){
                var temp = puntoInicio;
                puntoInicio = puntoFinal;
                puntoFinal = temp;
            }
        }
        else if(puntoFinal.Z < 0){
            var temp = puntoInicio;
                puntoInicio = puntoFinal;
                puntoFinal = temp;
        }
        

        Vector3 finPrimerSegmento = (!esHorizontal) ? new Vector3(puntoInicio.X * ubicacionPuerta, puntoInicio.Y, puntoInicio.Z):
                                                      new Vector3(puntoFinal.X, puntoFinal.Y, puntoFinal.Z * ubicacionPuerta);

        Vector3 inicioSegundoSegmento = (!esHorizontal) ? new Vector3(puntoInicio.X * (ubicacionPuerta - ANCHO_PUERTA), puntoInicio.Y, puntoInicio.Z):
                                                          new Vector3(puntoFinal.X, puntoFinal.Y, puntoFinal.Z * (ubicacionPuerta + ANCHO_PUERTA));
        
        Pared primerSegmento = new Pared(puntoInicio, finPrimerSegmento, esHorizontal);
        Pared segundoSegmento = new Pared(inicioSegundoSegmento, puntoFinal, esHorizontal);

        Paredes.Add(primerSegmento);
        Paredes.Add(segundoSegmento);

        }

        public void Draw(Texture2D textura){
            foreach(Pared p in Paredes) p.Draw(textura);
        }
    }
}