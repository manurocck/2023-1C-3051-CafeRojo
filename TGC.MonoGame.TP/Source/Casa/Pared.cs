using System;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP
{
    public class Pared{
        public const float GROSOR = TGCGame.S_METRO * 0.2f;
        public const float ALTURA = TGCGame.S_METRO * 2f;
        protected float LARGO;
        private Matrix World;
        protected Vector3 PuntoInicial = Vector3.Zero;
        protected Vector3 PuntoFinal;
        private readonly bool EsHorizontal;
        private Effect Efecto = TGCGame.GameContent.E_TextureTiles;
        
        ///<summary> Pared completamente cerrada</summary>
        public Pared(Vector3 puntoInicio, Vector3 puntoFinal){
            EsHorizontal = (puntoInicio.X == puntoFinal.X);
            PuntoInicial = puntoInicio;
            PuntoFinal = puntoFinal;

            LARGO = (EsHorizontal)? PuntoFinal.Z - PuntoInicial.Z : PuntoFinal.X - PuntoInicial.X - GROSOR;

            Matrix Rotacion     = EsHorizontal ? Matrix.CreateRotationY(0) : Matrix.CreateRotationY(MathHelper.PiOver2);

            World = Matrix.CreateScale(ALTURA,GROSOR,LARGO) 
                    * Matrix.CreateRotationZ(MathHelper.PiOver2) 
                    * Rotacion 
                    * Matrix.CreateTranslation(PuntoInicial.X,-TGCGame.S_METRO*0.1f,PuntoInicial.Z);
            
            AddToSimulation();
        }

        private void AddToSimulation(){
            var esteNumerito = Math.Abs(-PuntoInicial.X+PuntoFinal.X);
            var otroNumerito = Math.Abs(-PuntoInicial.Z+PuntoFinal.Z);
            Box boxito = (!EsHorizontal)? new Box(esteNumerito, ALTURA, GROSOR) 
                                        : new Box(GROSOR, ALTURA, otroNumerito);
            Vector3 fixedPosition = (!EsHorizontal)? new Vector3((PuntoInicial.X+PuntoFinal.X)/2, ALTURA*0.5f, PuntoInicial.Z):
                                                new Vector3(PuntoInicial.X, ALTURA*0.5f, (PuntoInicial.Z+PuntoFinal.Z)/2);

            TypedIndex index = TGCGame.Simulation.LoadShape<Box>(boxito);
            TGCGame.Simulation.CreateStatic(fixedPosition.ToBepu(), Quaternion.Identity.ToBepu(), index);
        }
        public void Draw(Texture2D textura){ 
            
            //var body = TGCGame.Simulation.Statics.GetStaticReference(Handle);
            //var aabb = body.BoundingBox;
            //TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);

            var metrosLargo = LARGO/TGCGame.S_METRO;

            Efecto.Parameters["Texture"]?.SetValue(textura);
            Efecto.Parameters["Filter"]?.SetValue(TGCGame.GameContent.T_MeshFilter);
            Efecto.Parameters["World"].SetValue(World); 
            Efecto.Parameters["TilesWide"]?.SetValue(metrosLargo*0.5f);
            Efecto.Parameters["TilesBroad"]?.SetValue(1);
            // Efecto.Parameters["MetrosLargo"]?.SetValue(ALTURA/TGCGame.S_METRO);
            // Efecto.Parameters["MetrosAncho"]?.SetValue(metrosLargo);
            TGCGame.GameContent.G_Cubo.Draw(Efecto);
        }
    }
}