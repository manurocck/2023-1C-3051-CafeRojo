using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{

    public class Casa {

        private const float S_METRO = TGCGame.S_METRO;
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
                if (i>=indexInicioExteriores) Esqueleto[i].SetEffect(TGCGame.GameContent.E_BlacksFilter);
            
            foreach(var h in Habitaciones) h.Draw();
            foreach(Puerta p in Puertas) p.Draw(TGCGame.GameContent.T_Concreto);
            foreach(Pared p in Esqueleto) p.Draw(TGCGame.GameContent.T_Concreto);

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
            
            foreach(var h in Habitaciones)
                Console.WriteLine("Habitacion cargada con {0:D}"+ " modelos.", h.cantidadElementos());
        }

        private void construirParedes(){
            var hPrincipal = Habitaciones[0];
            var hCocina    = Habitaciones[1];
            var hPasillo   = Habitaciones[2];
            var hDormiLego = Habitaciones[3];

            /*  
                >>> ¿Cómo identificar paredes cortadas? 
                    Su primer punto es el final de un segmento      */
            
            Esqueleto.Add(new Pared(hPasillo  .SegmentoDerecha  ().inicio, hPasillo  .SegmentoDerecha  ().final ));
            Esqueleto.Add(new Pared(hDormiLego.SegmentoIzquierda().inicio, hDormiLego.SegmentoIzquierda().final ));
            Esqueleto.Add(new Pared(hCocina   .SegmentoDerecha  ().inicio, hCocina   .SegmentoDerecha  ().final ));
            Esqueleto.Add(new Pared(hCocina   .SegmentoSuperior ().inicio, hCocina   .SegmentoSuperior ().final ));
            Esqueleto.Add(new Pared(hDormiLego.SegmentoDerecha  ().inicio, hDormiLego.SegmentoDerecha  ().final ));
            Esqueleto.Add(new Pared(hDormiLego.SegmentoSuperior ().inicio, hDormiLego.SegmentoSuperior ().final ));
            indexInicioExteriores = Esqueleto.Count;

            Puertas.Add(new Puerta(hPrincipal.SegmentoDerecha ().inicio, hPrincipal.SegmentoDerecha ().final, 0.3f));
            Puertas.Add(new Puerta(hDormiLego.SegmentoInferior().inicio, hDormiLego.SegmentoInferior().final, 0.5f));
            Puertas.Add(new Puerta(hPrincipal.SegmentoSuperior().inicio, hPrincipal.SegmentoSuperior().final, 0.6f));

            //      Paredes exteriores
            Esqueleto.Add(new Pared(hPasillo  .SegmentoInferior ().inicio, hPasillo  .SegmentoInferior ().final ));
            Esqueleto.Add(new Pared(hPrincipal.SegmentoInferior ().inicio, hPrincipal.SegmentoInferior ().final));
            Esqueleto.Add(new Pared(hPrincipal.SegmentoIzquierda().inicio, hPrincipal.SegmentoIzquierda().final));
            Esqueleto.Add(new Pared(hPrincipal.SegmentoSuperior ().final,  hCocina   .SegmentoInferior ().final));
            Esqueleto.Add(new Pared(hCocina   .SegmentoIzquierda().inicio, hCocina   .SegmentoIzquierda().final ));
            Esqueleto.Add(new Pared(hDormiLego.SegmentoInferior ().final,  hPasillo  .SegmentoSuperior ().final ));

        }
    }
}