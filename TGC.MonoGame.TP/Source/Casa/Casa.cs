using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Autos.PowerUps;
using PistonDerby.Mapa;
using PistonDerby.Utils.Iluminacion;

namespace PistonDerby;
public class Casa {

    private const float S_METRO = PistonDerby.S_METRO;
    private List<IHabitacion> Habitaciones;
    private List<Pared> Esqueleto;
    private List<Puerta> Puertas;
    private List<Light> Luces;
    private int indexInicioExteriores;
    private int indexInicioPuertasExteriores;
    public Casa(){
        this.Habitaciones = new List<IHabitacion>();
        this.Esqueleto = new List<Pared>();
        this.Puertas = new List<Puerta>();
    }
    public void LoadContent(){
        DisponerHabitaciones();
        ConstruirParedes();
        InstalarLuces();
        DesplegarPowerUps();

        for(int i=indexInicioExteriores ; i<Esqueleto.Count; i++) // paredes traslúcidas
            Esqueleto[i].SetEffect(PistonDerby.GameContent.E_BlacksFilter);
        for(int i=indexInicioPuertasExteriores ; i<Puertas.Count; i++) // puertas traslúcidas
            Puertas[i].SetEffect(PistonDerby.GameContent.E_BlacksFilter);
    }

    private void DesplegarPowerUps()
    {
        foreach(var h in Habitaciones){
            PistonDerby.ElementosDinamicos.Add(new PowerUpBox(h.PuntoCentro()/S_METRO+Vector3.UnitX, Vector3.Zero));
            PistonDerby.ElementosDinamicos.Add(new PowerUpBox(h.PuntoCentro()/S_METRO-Vector3.UnitX, Vector3.Zero));
            PistonDerby.ElementosDinamicos.Add(new PowerUpBox(h.PuntoCentro()/S_METRO+Vector3.UnitZ, Vector3.Zero));
            PistonDerby.ElementosDinamicos.Add(new PowerUpBox(h.PuntoCentro()/S_METRO-Vector3.UnitZ, Vector3.Zero));
        }
    }

    private void InstalarLuces()
    {
        Luces = new List<Light>();

        foreach(var h in Habitaciones)
            Luces.AddRange(h.Luces); 

        Luces = Luces.ConvertAll(l => l.GenerateShowColor());
        // Mostrar en consola la cantidad de luces
        Console.WriteLine("Cantidad de Luces: " + Luces.Count);
        InicializarShaderPBR();
        InicializarBlinnPhong(PistonDerby.GameContent.E_BlinnPhong);
        InicializarBlinnPhong(PistonDerby.GameContent.E_BlinnPhongTiles);
    }

    private void InicializarBlinnPhong(Effect e)
    {
        var Effect = e;
        var posiciones = Effect.Parameters["lightPositions"].Elements;
        
        for (var index = 0; index < Luces.Count; index++)
        {
            var light = Luces[index];
            posiciones[index].SetValue(light.Position);
        }
    }

    private void InicializarShaderPBR()
    {
        var Effect = PistonDerby.GameContent.E_PBRShader;
        Effect.CurrentTechnique = Effect.Techniques["PBR"];

        var posiciones = Effect.Parameters["lightPositions"].Elements;
        var colores = Effect.Parameters["lightColors"].Elements;

        for (var index = 0; index < Luces.Count; index++)
        {
            var light = Luces[index];
            posiciones[index].SetValue(light.Position);
            colores[index].SetValue(light.Color);
        }
    }

    public void Update(float dTime, KeyboardState keyboardState){
        foreach(var h in Habitaciones)
            h.Update(dTime, keyboardState);
    }
    public void Draw(){
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
    private void DisponerHabitaciones(){
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
        // Habitaciones.Add( new HabitacionDormitorio      (0f, -HabitacionPasillo.ANCHO-HabitacionDormitorio.ANCHO));
        
        foreach(var h in Habitaciones)
            Console.WriteLine("Habitacion cargada con {0:D}"+ " modelos.", h.cantidadElementos());
    }

    private void ConstruirParedes(){
        var hPrincipal = Habitaciones[0];
        var hCocina    = Habitaciones[1];
        var hPasillo   = Habitaciones[2];
        var hDormiLego = Habitaciones[3];
        var hToilette  = Habitaciones[4];
        var hOficina   = Habitaciones[5];
        // var hDormitorio= Habitaciones[6];
        

        /*  
            >>> ¿Cómo identificar paredes cortadas? 
                Su primer punto es el final de un segmento      */
        
        Esqueleto.Add(new Pared(hCocina   .SegmentoSuperior ().inicio, hCocina   .SegmentoSuperior ().final ));
        Esqueleto.Add(new Pared(hDormiLego.SegmentoDerecha  ().inicio, hDormiLego.SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hDormiLego.SegmentoSuperior ().inicio, hDormiLego.SegmentoSuperior ().final ));
        Esqueleto.Add(new Pared(hToilette .SegmentoSuperior ().inicio, hToilette .SegmentoSuperior ().final ));
        Esqueleto.Add(new Pared(hOficina .SegmentoDerecha  ().inicio, hOficina .SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hPasillo  .SegmentoDerecha ().inicio, hPasillo  .SegmentoDerecha ().final ));
        indexInicioExteriores = Esqueleto.Count;

        indexInicioPuertasExteriores = Puertas.Count;
        // Puertas.Add(new Puerta(hDormiLego.SegmentoInferior().inicio, hDormiLego.SegmentoInferior().final,  0.3f));
        Puertas.Add(new Puerta(hPrincipal.SegmentoSuperior().inicio, hCocina   .SegmentoInferior().inicio, 0.4f));
        // Puertas.Add(new Puerta(hOficina  .SegmentoSuperior().inicio, hOficina  .SegmentoSuperior().final,  -0.3f));
        Puertas.Add(new Puerta(hCocina   .SegmentoInferior().inicio, hPrincipal.SegmentoSuperior().final,  0.6f));
        Puertas.Add(new Puerta(hPrincipal.SegmentoDerecha ().inicio, hPrincipal.SegmentoDerecha ().final,  0.3f));

        //      Paredes exteriores
        Esqueleto.Add(new Pared(hPasillo  .SegmentoInferior ().inicio, hOficina  .SegmentoSuperior ().inicio ));
        Esqueleto.Add(new Pared(hDormiLego.SegmentoIzquierda().inicio, hDormiLego.SegmentoIzquierda().final ));
        Esqueleto.Add(new Pared(hCocina   .SegmentoDerecha  ().inicio, hCocina   .SegmentoDerecha  ().final ));
        Esqueleto.Add(new Pared(hPrincipal.SegmentoInferior ().inicio, hPrincipal.SegmentoInferior ().final));
        Esqueleto.Add(new Pared(hPrincipal.SegmentoIzquierda().inicio, hPrincipal.SegmentoIzquierda().final));
        Esqueleto.Add(new Pared(hPrincipal.SegmentoSuperior ().final,  hCocina   .SegmentoInferior ().final));
        Esqueleto.Add(new Pared(hCocina   .SegmentoIzquierda().inicio, hCocina   .SegmentoIzquierda().final ));
        Esqueleto.Add(new Pared(hOficina .SegmentoInferior ().inicio, hOficina .SegmentoInferior ().final ));
        

    }
    public void DebugGizmos(){
        foreach(IHabitacion h in Habitaciones) h.DebugGizmos();
        foreach(Pared p in Esqueleto) p.DebugGizmos();
        // foreach(Puerta p in Puertas) p.DebugGizmos();
    }

    // Draw all furniture with BlackDrawer
    public void DrawBlack(){
        foreach(var h in Habitaciones) h.DrawBlack();
    }

}
