using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Mapa;

namespace PistonDerby;
public class Casa {

    private const float S_METRO = PistonDerby.S_METRO;
    private List<IHabitacion> Habitaciones;
    private List<Pared> Esqueleto;
    private List<Puerta> Puertas;
    private int indexInicioExteriores;
    public Casa(){
        this.Habitaciones = new List<IHabitacion>();
        this.Esqueleto = new List<Pared>();
        this.Puertas = new List<Puerta>();
    }
    public void LoadContent(){
        disponerHabitaciones();
        construirParedes();
    }
    public void Update(float dTime, KeyboardState keyboardState){
        foreach(var h in Habitaciones)
            h.Update(dTime, keyboardState);
    }
    public void Draw(){
        
        for(int i=0 ; i<Esqueleto.Count; i++) // paredes traslúcidas
            if (i>=indexInicioExteriores) Esqueleto[i].SetEffect(PistonDerby.GameContent.E_BlacksFilter);
        
        foreach(var h in Habitaciones) h.Draw();
        foreach(Puerta p in Puertas) p.Draw(PistonDerby.GameContent.T_CubosMadera);
        foreach(Pared p in Esqueleto) p.Draw(PistonDerby.GameContent.T_CubosMadera);

    }

    public Vector3 PuntoCentro(int indiceHabitacion){
        if(indiceHabitacion>0 && indiceHabitacion < Habitaciones.Count)
            return Habitaciones[indiceHabitacion].PuntoCentro();
        else
            return Habitaciones[0].PuntoCentro();
    }
    private void disponerHabitaciones(){
        /*  
        Ejemplo :
            >>> new HabitacionX ( traslacionVertical, traslacionHorizontal ) 

                traslacionVertical   : Cuántos METROS se va a mover para arriba o para abajo        
                traslacionHorizontal : Cuántos METROS se va a mover para la izquierda o para la derecha        
        */

        Habitaciones.Add( new HabitacionPrincipal       (0f,0f));
        Habitaciones.Add( new HabitacionCocina          (-HabitacionCocina.LARGO, HabitacionPrincipal.ANCHO/2) );
        Habitaciones.Add( new HabitacionPasillo         (0f, -HabitacionPasillo.ANCHO) );
        Habitaciones.Add( new HabitacionDormitorioLegos (-HabitacionDormitorioLegos.LARGO , -HabitacionPasillo.ANCHO));
        Habitaciones.Add( new HabitacionToilette        (-HabitacionToilette.LARGO , 0f));
        Habitaciones.Add( new HabitacionOficina         (HabitacionPasillo.LARGO , -HabitacionOficina.ANCHO));
        Habitaciones.Add( new HabitacionDormitorio      (0f, -HabitacionPasillo.ANCHO-HabitacionDormitorio.ANCHO));
        
        foreach(var h in Habitaciones)
            Console.WriteLine("Habitacion cargada con {0:D}"+ " modelos.", h.cantidadElementos());
    }

    private void construirParedes(){
        var hPrincipal = Habitaciones[0];
        var hCocina    = Habitaciones[1];
        var hPasillo   = Habitaciones[2];
        var hDormiLego = Habitaciones[3];
        var hToilette  = Habitaciones[4];
        var hOficina   = Habitaciones[5];
        var hDormitorio= Habitaciones[6];
        

        /*  
            >>> ¿Cómo identificar paredes cortadas? 
                Su primer punto es el final de un segmento      */
        
        //Esqueleto.Add(new Pared(hPasillo  .SegmentoDerecha  ().inicio, hPasillo  .SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hDormiLego.SegmentoIzquierda().inicio, hDormiLego.SegmentoIzquierda().final ));
        Esqueleto.Add(new Pared(hCocina   .SegmentoDerecha  ().inicio, hCocina   .SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hCocina   .SegmentoSuperior ().inicio, hCocina   .SegmentoSuperior ().final ));
        Esqueleto.Add(new Pared(hDormiLego.SegmentoDerecha  ().inicio, hDormiLego.SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hDormiLego.SegmentoSuperior ().inicio, hDormiLego.SegmentoSuperior ().final ));
        Esqueleto.Add(new Pared(hToilette .SegmentoIzquierda().inicio, hToilette .SegmentoIzquierda().final ));
        Esqueleto.Add(new Pared(hToilette .SegmentoDerecha  ().inicio, hToilette .SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hToilette .SegmentoSuperior ().inicio, hToilette .SegmentoSuperior ().final ));
        Esqueleto.Add(new Pared(hDormitorio .SegmentoSuperior().inicio, hDormitorio .SegmentoSuperior().final ));
        Esqueleto.Add(new Pared(hDormitorio .SegmentoDerecha  ().inicio, hDormitorio .SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hDormitorio .SegmentoInferior ().inicio, hDormitorio .SegmentoInferior ().final ));
        Esqueleto.Add(new Pared(hOficina .SegmentoDerecha  ().inicio, hOficina .SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hOficina .SegmentoInferior ().inicio, hOficina .SegmentoInferior ().final ));
        indexInicioExteriores = Esqueleto.Count;

        Puertas.Add(new Puerta(hPrincipal.SegmentoDerecha ().inicio, hPrincipal.SegmentoDerecha ().final, 0.3f));
        Puertas.Add(new Puerta(hDormiLego.SegmentoInferior().inicio, hDormiLego.SegmentoInferior().final, 0.5f));
        Puertas.Add(new Puerta(hCocina   .SegmentoInferior().inicio, hPrincipal.SegmentoSuperior().final, 0.6f));
        Puertas.Add(new Puerta(hPrincipal.SegmentoSuperior().inicio, hCocina  .SegmentoInferior().inicio, 0.4f));
        Puertas.Add(new Puerta(hPasillo  .SegmentoDerecha ().inicio, hPasillo  .SegmentoDerecha ().final, 0.4f));
        Puertas.Add(new Puerta(hOficina  .SegmentoSuperior().inicio, hPasillo  .SegmentoInferior().final, 0.6f));

        //      Paredes exteriores
        Esqueleto.Add(new Pared(hPasillo  .SegmentoInferior ().inicio, hOficina  .SegmentoSuperior ().inicio ));
        Esqueleto.Add(new Pared(hPrincipal.SegmentoInferior ().inicio, hPrincipal.SegmentoInferior ().final));
        Esqueleto.Add(new Pared(hPrincipal.SegmentoIzquierda().inicio, hPrincipal.SegmentoIzquierda().final));
        Esqueleto.Add(new Pared(hPrincipal.SegmentoSuperior ().final,  hCocina   .SegmentoInferior ().final));
        Esqueleto.Add(new Pared(hCocina   .SegmentoIzquierda().inicio, hCocina   .SegmentoIzquierda().final ));
        Esqueleto.Add(new Pared(hDormiLego.SegmentoInferior ().final,  hPasillo  .SegmentoSuperior ().final ));
        Esqueleto.Add(new Pared(hPasillo.SegmentoDerecha ().final,  hDormitorio  .SegmentoIzquierda ().final ));
        

    }
    public void DebugGizmos(){
        foreach(IHabitacion h in Habitaciones) h.DebugGizmos();
        foreach(Pared p in Esqueleto) p.DebugGizmos();
        // foreach(Puerta p in Puertas) p.DebugGizmos();
    }
}
