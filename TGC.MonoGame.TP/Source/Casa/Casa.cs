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
            //Esqueleto[5].SetEffect(TGCGame.GameContent.E_BlacksFilter); // principal abajo
            //Esqueleto[6].SetEffect(TGCGame.GameContent.E_BlacksFilter); // principal izquierda
            
            foreach(var h in Habitaciones) h.Draw();
            foreach(Pared p in Esqueleto) p.Draw(TGCGame.GameContent.T_Concreto);
            foreach(Puerta p in Puertas) p.Draw(TGCGame.GameContent.T_Concreto);

            // Esqueleto[10].SetEffect(TGCGame.GameContent.E_TextureShader);
            // Esqueleto[10].Draw(TGCGame.GameContent.T_Concreto);
            
            // for(int i = 0 ; i<8 ; i++)
            //     Esqueleto[i].Draw(TGCGame.GameContent.T_Ladrillos);
            // for(int i = 8 ; i<11 ; i++){   
            //     //Esqueleto[i].SetEffect(TGCGame.GameContent.E_TextureShader);
            //     //Esqueleto[i].Draw(TGCGame.GameContent.T_Concreto);
            // }
            // for(int i = 11; i<15 ; i++)
            //     Esqueleto[i].Draw(TGCGame.GameContent.T_Concreto);

            // for(int i = 15; i< Esqueleto.Count; i++){
            //     Esqueleto[i].Draw();
            // }

            // Esqueleto[9].SetEffect(TGCGame.GameContent.E_BlacksFilter);
            // Esqueleto[9].Draw(TGCGame.GameContent.T_Ladrillos);

            // Esqueleto[8].SetEffect(TGCGame.GameContent.E_BlacksFilter);
            // Esqueleto[8].Draw(TGCGame.GameContent.T_Ladrillos);


        }

        public Vector3 GetCenter(int indexHabitacion){
            if(indexHabitacion>0 && indexHabitacion < Habitaciones.Count)
                return Habitaciones[indexHabitacion].GetMiddlePoint();
            else
                return Habitaciones[0].GetMiddlePoint();
        }
        private void disponerHabitaciones(){
            // DISPOSICIÓN RELATIVA DE HABITACIONES. 
            //      TAREA :  Sacar el S_METRO de acá y de Elemento. 
            //               Dejarlo solo que dependa de la Habitación definir las posiciones
            //               de los objetos (en AddElemento y cuandodefinimos la PosicionInicial)
            Habitaciones.Add( new HabitacionPrincipal(0f,0f));
            Habitaciones.Add( new HabitacionCocina(-HabitacionCocina.LARGO * S_METRO, S_METRO * HabitacionPrincipal.ANCHO/2) );
            Habitaciones.Add( new HabitacionPasillo(0f, -HabitacionPasillo.ANCHO * S_METRO) );
            // Habitaciones.Add( new HabitacionToilette(-HabitacionToilette.Size * S_METRO, 0));
            // Habitaciones.Add( new HabitacionPasillo2(0f, -Habitaciones[2].Ancho*2 * S_METRO));
            // Habitaciones.Add( new HabitacionOficina(0f, -S_METRO *(Habitaciones[2].Ancho*2+HabitacionOficina.Size)));
            Habitaciones.Add( new HabitacionDormitorioLegos(-S_METRO * HabitacionDormitorioLegos.LARGO , -HabitacionPasillo.ANCHO* S_METRO ));
            // Habitaciones.Add( new HabitacionDormitorio2(Habitaciones[2].Ancho * S_METRO , -Habitaciones[2].Ancho*2 * S_METRO));
            // Habitaciones.Add( new HabitacionToilette(Habitaciones[5].GetVerticeExtremo().X,Habitaciones[5].GetVerticeExtremo().Z - HabitacionToilette.Size*S_METRO));

            foreach(var h in Habitaciones)
                Console.WriteLine("Habitacion cargada con {0:D}"+ " modelos.", h.cantidadElementos());
        }

        private void construirParedes(){
            var hPrincipal = Habitaciones[0];
            var hCocina    = Habitaciones[1];
            var hPasilloPr = Habitaciones[2];
            var hDormiLego = Habitaciones[3];
            // var hBanioPr   = Habitaciones[3];
            // var hOficina   = Habitaciones[5];
            // var hDormiLego = Habitaciones[6];
            // var hDormiDrag = Habitaciones[7];
            // var hBanioOf   = Habitaciones[8];

            // Todas las paredes que tengan como punto de inicio el final de un segmento, son paredes "cortadas"

            // Paredes exteriores
            Esqueleto.Add(new Pared(hPrincipal.GetSegmentoSuperior ().final,  hCocina   .GetSegmentoInferior ().final, true));
            Esqueleto.Add(new Pared(hCocina   .GetSegmentoIzquierda().inicio, hCocina   .GetSegmentoIzquierda().final, false ));
            // Esqueleto.Add(new Pared(hBanioPr  .GetSegmentoIzquierda().inicio, hBanioPr  .GetSegmentoIzquierda().final, false ));
            Esqueleto.Add(new Pared(hPasilloPr.GetSegmentoInferior().inicio,   hPasilloPr.GetSegmentoInferior().final, true ));
            Esqueleto.Add(new Pared(hPasilloPr.GetSegmentoDerecha().inicio,   hPasilloPr.GetSegmentoDerecha().final, true ));
            // Esqueleto.Add(new Pared(hDormiDrag .GetSegmentoSuperior().final,   hPasilloPr.GetSegmentoInferior().final, true ));
            Esqueleto.Add(new Pared(hDormiLego.GetSegmentoIzquierda().inicio, hDormiLego.GetSegmentoIzquierda().final, false ));
            // Esqueleto.Add(new Pared(hDormiDrag.GetSegmentoInferior ().inicio, hDormiDrag.GetSegmentoInferior ().final, true ));
            // Esqueleto.Add(new Pared(hDormiDrag.GetSegmentoIzquierda().inicio, hDormiDrag.GetSegmentoIzquierda().final, false ));
            // Esqueleto.Add(new Pared(hBanioOf  .GetSegmentoInferior ().inicio, hBanioOf  .GetSegmentoInferior ().final, true ));
            
            // Exteriores de la principal 
            //-Esqueleto.Add(new Pared(hPrincipal.GetSegmentoIzquierda().inicio, hPrincipal.GetSegmentoIzquierda().final));
            //-Esqueleto.Add(new Pared(hPrincipal.GetSegmentoInferior ().inicio, hPrincipal.GetSegmentoInferior ().final, true));

            // // Paredes HabitacionPrincipal
            //Esqueleto.Add(new Pared(hPrincipal.GetSegmentoSuperior().inicio, hPrincipal.GetSegmentoSuperior().final, true));
            Puertas.Add(new Puerta(hPrincipal.GetSegmentoSuperior().inicio, hPrincipal.GetSegmentoSuperior().final, 0.6f, true));
            //Esqueleto[5].AddPuerta(0.50f);
            //Esqueleto[5].AddPuerta(0.10f);
            //Esqueleto.Add(new Pared(hPrincipal.GetSegmentoDerecha ().inicio, hPrincipal.GetSegmentoDerecha ().final));
            Puertas.Add(new Puerta(hPrincipal.GetSegmentoDerecha().inicio, hPrincipal.GetSegmentoDerecha().final, 0.3f, false));
            // Esqueleto[11].AddPuerta(0.10f);
            // Paredes cortadas del Pasillo 1 (el más cercano a la principal)
            //Esqueleto.Add(new Pared(hDormiLego.GetSegmentoInferior().inicio, hDormiLego.GetSegmentoInferior().final, true ));
            Puertas.Add(new Puerta(hDormiLego.GetSegmentoInferior().inicio, hDormiLego.GetSegmentoInferior().final, 0.5f, true));
            // Esqueleto[12].AddPuerta(0.25f);
            Esqueleto.Add(new Pared(hDormiLego.GetSegmentoInferior ().final,  hPasilloPr.GetSegmentoSuperior ().final, true ));
            // Esqueleto.Add(new Pared(hOficina.GetSegmentoIzquierda ().inicio, hDormiDrag.GetSegmentoDerecha ().inicio, false ));
            // Esqueleto[14].AddPuerta(0.25f);
            
            // // Paredes cocina ( primera cortada )
            
            Esqueleto.Add(new Pared(hCocina.GetSegmentoDerecha ().inicio, hCocina.GetSegmentoDerecha ().final, false ));
            //Puertas.Add(new Puerta(hCocina.GetSegmentoDerecha ().inicio, hCocina.GetSegmentoDerecha ().final, 0.5f, false ));
            Esqueleto.Add(new Pared(hCocina.GetSegmentoSuperior().inicio, hCocina.GetSegmentoSuperior().final, true ));
            

            // // Paredes baños
            // Esqueleto.Add(new Pared(hBanioPr.GetSegmentoDerecha ().inicio, hBanioPr.GetSegmentoDerecha ().final, false ));
            // Esqueleto.Add(new Pared(hBanioPr.GetSegmentoSuperior().inicio, hBanioPr.GetSegmentoSuperior().final, true ));
            // Esqueleto.Add(new Pared(hBanioOf.GetSegmentoDerecha ().inicio, hBanioOf.GetSegmentoDerecha ().final, false ));
            // // Paredes Dormitorio1 (Lego)
            
            Esqueleto.Add(new Pared(hDormiLego.GetSegmentoDerecha ().inicio, hDormiLego.GetSegmentoDerecha ().final, false ));
            Esqueleto.Add(new Pared(hDormiLego.GetSegmentoSuperior().inicio, hDormiLego.GetSegmentoSuperior().final, true ));
            
            // // Paredes Dormitorio2 (Dragones)
            // Esqueleto.Add(new Pared(hDormiDrag.GetSegmentoSuperior().inicio, hDormiDrag.GetSegmentoSuperior().final, true ));
            // Esqueleto[22].AddPuerta(0.25f);
            // Esqueleto.Add(new Pared(hDormiDrag.GetSegmentoDerecha ().inicio, hDormiDrag.GetSegmentoDerecha ().final, false ));
            // Esqueleto[23].AddPuerta(0.25f);
            // // Paredes oficina
            // Esqueleto.Add(new Pared(hOficina.GetSegmentoSuperior().inicio, hOficina.GetSegmentoSuperior().final, true ));
            // Esqueleto.Add(new Pared(hOficina.GetSegmentoDerecha ().inicio, hOficina.GetSegmentoDerecha ().final, false ));
            // Esqueleto.Add(new Pared(hOficina.GetSegmentoInferior().inicio, hOficina.GetSegmentoInferior().final, true ));
            // Esqueleto[26].AddPuerta(0.25f);

            Console.WriteLine("CheckPoint");
        }
    }
}