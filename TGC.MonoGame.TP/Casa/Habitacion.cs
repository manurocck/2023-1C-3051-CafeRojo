using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class Habitacion
    {
        private int Ancho;
        private int Alto;
        private Vector3 PosicionInicial;
        private Piso Piso;
        private List<Pared> Paredes;
        private List<IElementoDinamico> ElementosDinamicos;
        private List<IElemento> Elementos;

        private Pared ParedQueAnda;

        //Ancho y Alto en Cantidad de Baldosas
        private Habitacion(int ancho, int alto, Vector3 posicionInicial)
        {
            Ancho = ancho;
            Alto = alto;
            PosicionInicial = posicionInicial;

            ElementosDinamicos = new List<IElementoDinamico>();
            Elementos = new List<IElemento>();
            Piso = new Piso(ancho, alto, posicionInicial); // Se carga el default

            Paredes = new List<Pared>();
            Paredes.Add(Pared.Arriba (10, 10, Vector3.Zero));
           /*  Paredes.Add(Pared.Derecha   (ancho, alto, posicionInicial + new Vector3(0, 0, ancho * 5000f)));          
            Paredes.Add(Pared.Izquierda    (ancho, alto, posicionInicial + new Vector3(ancho * 5000f, 0, ancho * 5000f)));
            Paredes.Add(Pared.Abajo     (ancho, alto, posicionInicial + new Vector3(0, 0, ancho * 5000f))); */          

            ParedQueAnda = Pared.Arriba(10, 10, Vector3.Zero);
        }
        public void AddDinamico( IElementoDinamico elem ){
            elem.newPosicionInicial(PosicionInicial);
            ElementosDinamicos.Add(elem);
        }
        private void AddElemento( IElemento elem ){
            elem.newPosicionInicial(PosicionInicial);
            Elementos.Add(elem);
        }

        public static Habitacion Oficina(int ancho, int alto, Vector3 posicionInicial){
            var oficina = new Habitacion(alto, ancho, posicionInicial);

            oficina.Piso.Oficina();
            oficina.AddElemento(new Mueble("Silla-Oficina", 10f,new Vector3(100f,0f,100f), new Vector3(-MathHelper.PiOver2,MathHelper.PiOver4,0f)));
//            oficina.AddElemento(new Mueble("Gabinete", 10f,Vector3.Zero, new Vector3(0f,0f,MathHelper.Pi)));
            oficina.AddElemento(new Mueble("Cafe-Rojo", 10f,new Vector3(100f,50f,100f), new Vector3(-MathHelper.PiOver2,0f,0f)));

            
            return oficina;
        }
        public static Habitacion Cocina(int ancho, int alto, Vector3 posicionInicial){
            Habitacion cocina = new Habitacion(ancho,alto,posicionInicial);
            cocina.Piso.Cocina();
            cocina.AddElemento( new Mueble("Inodoro", 15f, new Vector3(100f,0f,100f), new Vector3(0, MathHelper.PiOver2*3f, 0) ));
            cocina.AddElemento( new Mueble("Inodoro", 15f, new Vector3(200f,0f,200f), new Vector3(0, 0, 0) ));
            cocina.AddElemento( new Mueble("Inodoro", 15f, new Vector3(300f,100f,300f), new Vector3(0, MathHelper.PiOver2, MathHelper.Pi) ));
            cocina.AddElemento( new Mueble("Inodoro", 15f, new Vector3(400f,0f,400f), new Vector3(0, MathHelper.PiOver2, 0) ));
            return cocina;
        }
        public static Habitacion Principal(int ancho, int alto, Vector3 posicionInicial){
            Habitacion principal = new Habitacion(ancho,alto,posicionInicial);
            #region CargaElementosDinámicos
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;
                principal.AddDinamico(new EnemyCar("Models/CombatVehicle/Vehicle", escala, posicionesAutosIA, Vector3.Zero));
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion
            
            #region CargaElementosEstáticos
            for(int i = 0 ; i<100*20 ; i+=100){
                principal.AddElemento(new Mueble("Chair", 10f, new Vector3(   i , 40f , -30f ), Vector3.Zero));
            }
            principal.AddElemento( new Mueble("Mesa", 12f, new Vector3(415f,0f,415f), new Vector3(0, MathHelper.PiOver2, 0) ));
            principal.AddElemento( new Mueble("Mesa", 12f, new Vector3(30f,0f,415f), Vector3.Zero ));
            principal.AddElemento( new Mueble("Inodoro", 15f, new Vector3(30f,0f,215f), new Vector3(0, MathHelper.PiOver2, 0) ));
            principal.AddElemento( new Mueble("Sillon", 10f, new Vector3(250f,30f,250f), Vector3.Zero));
            #endregion

            return principal;
        }

        public void Load(ContentManager content)
        {
            ParedQueAnda.Load(content);
            Piso.Load(content);

            foreach(var pared in Paredes)
                pared.Load(content);
            foreach(var elemento in ElementosDinamicos)
                elemento.Load(content);
            foreach(var elemento in Elementos)
                elemento.Load(content);  
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState){
            foreach(var e in ElementosDinamicos){
                e.Update(gameTime, keyboardState);
            }
            return;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Piso.Draw(view, projection);

            foreach(var pared in Paredes)
                pared.Draw(view, projection);

            foreach(var elemento in ElementosDinamicos)
                elemento.Draw(view, projection);
            foreach(var elemento in Elementos)
                elemento.Draw(view, projection);
        }
    }
}