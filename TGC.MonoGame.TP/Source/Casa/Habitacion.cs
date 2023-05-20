
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;
using TGC.MonoGame.TP.Elementos;

namespace TGC.MonoGame.TP
{
    public abstract class IHabitacion
    {
        private const float S_METRO = TGCGame.S_METRO;
        public readonly int MetrosAncho;
        public readonly int MetrosLargo;
        internal Vector3 PosicionInicial;
        internal Piso Piso;
        internal List<ElementoDinamico> ElementosDinamicos;
        internal List<ElementoEstatico> ElementosEstaticos;

        //Ancho y Alto en Cantidad de Baldosas
        public IHabitacion(int metrosAncho, int metrosLargo, Vector3 posicionInicial)
        {
            MetrosAncho = metrosAncho;
            MetrosLargo = metrosLargo;
            PosicionInicial = posicionInicial;
            ElementosDinamicos = new List<ElementoDinamico>();
            ElementosEstaticos = new List<ElementoEstatico>();
            Piso = new Piso(metrosAncho, metrosLargo, posicionInicial);
        }
        public void AddElemento( ElementoEstatico e ){
            ElementosEstaticos.Add(e);
        }
        public void AddElementoDinamico( ElementoDinamico e ){
            ElementosDinamicos.Add(e);
        }
        public int cantidadElementos(){
            // Para debuggear
            return this.ElementosEstaticos.Count + this.ElementosDinamicos.Count;
        }
        public void Update(float dTime, KeyboardState keyboardState){
            foreach(var e in ElementosDinamicos){
                e.Update(dTime, keyboardState);
            }
            return;
        }

        public void Draw()
        {
            Piso.Draw();
            foreach(var e in ElementosEstaticos) e.Draw();
            foreach(var e in ElementosDinamicos) e.Draw();
        }
        

        public Vector3 GetMiddlePoint() => Piso.GetMiddlePoint();
        public Vector3 PuntoCentro() => Piso.getCenter();
        public Vector3 PuntoInicio() => this.PosicionInicial;
        public Vector3 PuntoExtremo() => this.PuntoCentro()*2;
        public (Vector3 inicio, Vector3 final) GetSegmentoSuperior() =>
                    ( this.PuntoInicio(),
                    new Vector3(this.PuntoInicio().X, 0f , this.PuntoExtremo().Z));
        public (Vector3 inicio, Vector3 final) GetSegmentoInferior() =>
                    (new Vector3(this.PuntoExtremo().X, 0f , this.PuntoInicio().Z),
                    this.PuntoExtremo());
        public (Vector3 inicio, Vector3 final) GetSegmentoDerecha() =>
                    (this.PuntoInicio(),
                    new Vector3(this.PuntoExtremo().X, 0f , this.PuntoInicio().Z));
        public (Vector3 inicio, Vector3 final) GetSegmentoIzquierda() =>
                    (new Vector3(this.PuntoInicio().X, 0f , this.PuntoExtremo().Z),
                    this.PuntoExtremo());
    }
}