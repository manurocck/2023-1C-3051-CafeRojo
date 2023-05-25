
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Elementos;

namespace TGC.MonoGame.TP;
public abstract class IHabitacion
{
    private const float S_METRO = TGCGame.S_METRO;
    public readonly int MetrosAncho;
    public readonly int MetrosLargo;
    internal Vector3 PosicionInicial;
    internal Piso Piso;
    internal List<ElementoEstatico> Muebles;

    // Ancho y Alto en Cantidad de Baldosas
    public IHabitacion(int metrosAncho, int metrosLargo, Vector3 traslacionEnMetros)
    {
        PosicionInicial = traslacionEnMetros* TGCGame.S_METRO;
        MetrosAncho = metrosAncho;
        MetrosLargo = metrosLargo;
        Muebles = new List<ElementoEstatico>();
        Piso = new Piso(metrosAncho, metrosLargo, PosicionInicial);
    }
    public void AddElemento( ElementoEstatico e ){
        Muebles.Add(e);
    }
    public int cantidadElementos(){
        // Para debuggear
        return this.Muebles.Count;
    }
    public void Update(float dTime, KeyboardState keyboardState){
        // Es necesaria ??
        return;
    }

    public void Draw()
    {
        Piso.Draw();
        foreach(var e in Muebles) e.Draw();
    }

    /*                 *|
    |> > > UTILS        |
    |*                 */

    /// <summary> Coordenadas del punto tomado como origen de la habitación (superior derecho)</summary>
    public Vector3 PuntoInicio() => this.PosicionInicial;
    /// <summary> Coordenadas del centro de la habitación (sobre el piso)</summary>
    public Vector3 PuntoCentro() => Piso.PuntoCentro();
    /// <summary> Coordenadas del punto más alejado al origen de la habitación (inferior izquierdo)</summary>
    public Vector3 PuntoExtremo() => this.PuntoCentro()*2;

    /// <summary> Coordenadas del lado superior de la habitación (no confundir lado con pared) <code><paramref name="inicio"/> es el punto superior de la derecha</code><code><paramref name="final"/> es el punto superior de la izquierda</code></summary>
    public (Vector3 inicio, Vector3 final) SegmentoSuperior() =>
                ( this.PuntoInicio(),
                new Vector3(this.PuntoInicio().X, 0f , this.PuntoExtremo().Z));
    /// <summary> Coordenadas del lado inferior de la habitación (no confundir lado con pared) <code><paramref name="inicio"/> es el punto inferior de la derecha</code><code><paramref name="final"/> es el punto inferior de la izquierda</code></summary>
    public (Vector3 inicio, Vector3 final) SegmentoInferior() =>
                (new Vector3(this.PuntoExtremo().X, 0f , this.PuntoInicio().Z),
                this.PuntoExtremo());
    /// <summary> Coordenadas del lado derecho de la habitación (no confundir lado con pared) <code><paramref name="inicio"/> es el punto superior de la derecha</code><code><paramref name="final"/> es el punto inferior de la derecha</code></summary>
    public (Vector3 inicio, Vector3 final) SegmentoDerecha() =>
                (this.PuntoInicio(),
                new Vector3(this.PuntoExtremo().X, 0f , this.PuntoInicio().Z));
    /// <summary> Coordenadas del lado izquierdo de la habitación (no confundir lado con pared) <code><paramref name="inicio"/> es el punto superior de la izquierda</code><code><paramref name="final"/> es el punto inferior de la izquierda</code></summary>
    public (Vector3 inicio, Vector3 final) SegmentoIzquierda() =>
                (new Vector3(this.PuntoInicio().X, 0f , this.PuntoExtremo().Z),
                this.PuntoExtremo());
}
