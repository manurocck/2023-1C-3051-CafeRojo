using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP
{
    public class Pared{
        private const float LARGO_PUERTA = TGCGame.S_METRO * 2f;
        public const float GROSOR = TGCGame.S_METRO * 0.1f;
        public const float ALTURA = TGCGame.S_METRO * 2f;
        private List<float> DistanciasPuertas = new List<float>();
        private List<Matrix> Worlds = new List<Matrix>();
        protected Vector3 PuntoInicial = Vector3.Zero;
        protected Vector3 PuntoFinal;
        public readonly bool EsHorizontal;
        private float Largo;
        private Effect Efecto = TGCGame.GameContent.E_TextureTiles;
        private StaticHandle Handle;
        

        public Pared(Vector3 puntoInicio, Vector3 puntoFinal, bool esHorizontal = false){
            PuntoInicial = puntoInicio;
            PuntoFinal = puntoFinal;
            EsHorizontal = esHorizontal;
            Ubicar(puntoInicio, puntoFinal);
 
            var esteNumerito = Math.Abs(-PuntoInicial.X+PuntoFinal.X);
            var otroNumerito = Math.Abs(-PuntoInicial.Z+PuntoFinal.Z);

            Box boxito = (!EsHorizontal)? new Box(esteNumerito, ALTURA, GROSOR) 
                                        : new Box(GROSOR, ALTURA, otroNumerito);
                            
            Vector3 posicion = (!EsHorizontal)? new Vector3((PuntoInicial.X+PuntoFinal.X)/2, ALTURA*0.5f, PuntoInicial.Z):
                                                new Vector3(PuntoInicial.X, ALTURA*0.5f, (PuntoInicial.Z+PuntoFinal.Z)/2);

            Handle = TGCGame.Simulation.Statics.Add( new StaticDescription(
                                                posicion.ToBepu(),
                                                TGCGame.Simulation.Shapes.Add(boxito)));
        }


        ///<summary> Pared Completamente Cerrada. Me dibujo de derecha a izquierda (Horizontal) o de arriba para abajo (Vertical) </summary>
        public void Ubicar(Vector3 puntoInicial, Vector3 puntoFinal){
            PuntoInicial = puntoInicial;
            PuntoFinal = puntoFinal;

            //Largo = largoAbsoluto;
            Largo = (EsHorizontal)? puntoFinal.Z - puntoInicial.Z : puntoFinal.X - puntoInicial.X - GROSOR;
            
            Matrix Escala       = Matrix.CreateScale(ALTURA,GROSOR,Largo);
            Matrix LevantarQuad = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Matrix Rotacion     = EsHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);
            Matrix Traslacion   = Matrix.CreateTranslation(PuntoInicial.X,-TGCGame.S_METRO*0.1f,PuntoInicial.Z);

            var worldCerrada = Escala * LevantarQuad * Rotacion * Traslacion ;
            Worlds.Add(worldCerrada);
        }
        /// <summary> La distancia es un valor entre 0 y 1. Siendo 0 el principio y 1 el final</summary>
        public void AddPuerta(float distanciaPonderada){
            this.DistanciasPuertas.Add(distanciaPonderada);
            this.DistanciasPuertas.Sort();
            Matrix LevantarQuad = Matrix.CreateRotationZ(MathHelper.PiOver2);
            Matrix Rotacion     = EsHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);
            this.Worlds = new List<Matrix>();
            
            
            Matrix Escala;
            Matrix Traslacion;
            Matrix WorldAux;
            int i;
            float largoParedRestante = Largo;
            float ponderacionRestante = 1f;
            float corrimiento = 0f;

            // si agregan 2 puertas muy pegadas solo cambiar el corrimiento, no crear un mundo nuevo
            for(i = 0 ; i < DistanciasPuertas.Count ; i++){
                var largoParedActual = largoParedRestante*this.DistanciasPuertas[i] * ponderacionRestante;

                Escala       = Matrix.CreateScale(ALTURA,GROSOR,largoParedActual);
                Traslacion   = (EsHorizontal)? 
                            Matrix.CreateTranslation(PuntoInicial.X,-TGCGame.S_METRO*0.1f,PuntoInicial.Z + corrimiento ) :
                            Matrix.CreateTranslation(PuntoInicial.X + corrimiento ,-TGCGame.S_METRO*0.1f,PuntoInicial.Z) ;

                WorldAux = Escala * LevantarQuad * Rotacion * Traslacion ;
                this.Worlds.Add(WorldAux);
                

                ponderacionRestante = ((largoParedRestante-LARGO_PUERTA)/Largo);
                DistanciasPuertas.ForEach(d => d -= (largoParedActual/Largo) );
                corrimiento +=(largoParedActual + LARGO_PUERTA);
                largoParedRestante  -= (largoParedActual + LARGO_PUERTA);
            }

            // dibuja la última
            Escala       = Matrix.CreateScale(ALTURA,GROSOR,largoParedRestante);
            Traslacion   = (EsHorizontal)? 
                            Matrix.CreateTranslation(PuntoInicial.X,-0,PuntoInicial.Z + corrimiento ) :
                            Matrix.CreateTranslation(PuntoInicial.X + corrimiento,-0,PuntoInicial.Z) ;
            
            WorldAux = Escala * LevantarQuad * Rotacion * Traslacion ;
                            
            this.Worlds.Add(WorldAux);
                     
            

        }
        public void Draw(){ 
            Draw(TGCGame.GameContent.T_MarmolNegro);
        }
        public void Draw(Texture2D textura){ 
            
            //var body = TGCGame.Simulation.Statics.GetStaticReference(Handle);
            //var aabb = body.BoundingBox;

            //TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);
            var metrosLargo = Largo/TGCGame.S_METRO;

            Efecto.Parameters["Texture"]?.SetValue(textura);
            Efecto.Parameters["Filter"]?.SetValue(TGCGame.GameContent.T_MeshFilter);

            foreach( var w in Worlds){
                Efecto.Parameters["World"].SetValue(w); 
                Efecto.Parameters["TilesWide"]?.SetValue(metrosLargo*0.5f);
                Efecto.Parameters["TilesBroad"]?.SetValue(1);
                // Efecto.Parameters["MetrosLargo"]?.SetValue(ALTURA/TGCGame.S_METRO);
                // Efecto.Parameters["MetrosAncho"]?.SetValue(metrosLargo);
                TGCGame.GameContent.G_Cubo.Draw(Efecto);
            }

            // Generalizar para tener mas puertas
            // if(ConPuerta){
            //     Efecto.Parameters["World"].SetValue(WorldLateral1); 
            //     TGCGame.GameContent.G_Cubo.Draw(Efecto);
            //     Efecto.Parameters["World"].SetValue(WorldLateral2); 
            //     TGCGame.GameContent.G_Cubo.Draw(Efecto);
            // }else{
            //     Efecto.Parameters["World"].SetValue(WorldCerrada); 
            //     TGCGame.GameContent.G_Cubo.Draw(Efecto);
            // }            
        }
        public void SetEffect(Effect nuevoEfecto){
            Efecto = nuevoEfecto;
        }
        
        public void SetPuntoInicio(Vector3 nuevoPunto) => PuntoInicial = nuevoPunto;
    }
}