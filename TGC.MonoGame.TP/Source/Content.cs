using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PistonDerby.Geometries;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System;

namespace PistonDerby;

public class Content
{
    private readonly ContentManager ContentManager;
    
    #region string carpetas
    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSounds = "Sounds/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    public const string ContentFolderTextures = "Textures/";
    #endregion
    
    #region modelos
    internal Model 
            M_Alfil, M_Torre, M_Auto, M_AutoEnemigo, M_Inodoro, M_Misil, M_SillaOficina, M_CafeRojo, M_Silla, M_Mesa, M_Sillon, 
            M_Televisor , M_MuebleTV, M_Planta , M_Escritorio, M_Cocine, M_Plantis, M_Lego, M_Baniera,M_Sofa, M_Mesita, M_Aparador, 
            M_Bacha, M_Organizador, M_Cajonera, M_CamaMarinera , M_MesadaCentral, M_MesadaLateral, M_MesadaLateral2, M_Alacena, 
            M_Botella, M_Maceta, M_Maceta2, M_Maceta3, M_Maceta4, M_Olla, M_ParedCocina, M_Plato, M_PlatoGrande, M_PlatosApilados,
            M_Mesada, M_AutoPegni, M_Heladera, M_Dragon, M_Dragona, M_Cama, M_Juego, M_Puff, M_Armario1, M_Gato, M_Hombrepiedra, M_Lena, 
            M_Lamparamesita, M_Lobo, M_Libro, M_Piano, M_Robot, M_BrMisil;
    #endregion
    
    #region efectos
    internal readonly Effect E_BasicShader, E_TextureShader, E_SpiralShader, E_BlacksFilter, 
                        E_TwoTextureMix, E_TextureTiles, E_Traslucid, E_TextureItermitente, E_TextureMirror,
                        E_PBRShader, E_BulletShader, E_PBRpackedShader, E_BlinnPhong, E_BlinnPhongTiles;
    internal readonly Effect HE_HealthHUD, HE_TurboHUD, HE_TextureHUD;
    #endregion
    
    #region texturas
    internal readonly Texture2D T_Alfombra, T_PisoMadera, T_PisoCeramica, T_PisoAlfombrado, T_AlfombraHabitacion,
                        T_MeshFilter, T_MaderaNikari, T_SillaOficina, T_PisoMaderaClaro, T_Dragon,
                        T_RacingCar, T_CombatVehicle, T_Ladrillos, T_Marmol, T_MarmolNegro, T_Reboque, T_Concreto,
                        T_CubosMadera, T_PisoMaderaElegante, T_PisoPasto, T_MysteryBox, T_MisilLanzado;
    internal readonly Texture2D TH_Bullet, TH_EmptyBullet;
    internal readonly Texture2D TM_Start, TM_Play, TM_Pointer, TM_PlayOption, TM_SettingsOption, TM_Controles;
    internal readonly Texture2D TP_Presentacion1, TP_Presentacion2, TP_Presentacion3, TP_Presentacion0;  
    internal readonly Texture2D TA_MetalMap, TA_BaseColor, TA_RoughnessMap, TA_NormalMap, TA_CavityMap, TA_LightEmissionMap;
    internal readonly Texture2D T_Planta_BaseColorMap, T_Planta_NormalMap, T_Planta_RoughnessMetallicOpacityMap,
                                T_Plantis_BaseColorMap, T_Plantis_NormalMap, T_Plantis_RoughnessMetallicOpacityMap;
    #endregion
    
    internal readonly SoundEffect S_Metralleta, S_MotorEncendido, S_MotorRegulando, S_MotorAcelerando, S_Bling, S_Pickup;
    internal readonly Song S_SynthWars, S_MeiHuaSan, S_ItsAFight;
    
    internal readonly List<Effect> Efectos = new List<Effect>();
    internal readonly List<Effect> EfectosHUD = new List<Effect>();
    internal readonly QuadPrimitive G_Quad;
    internal readonly CuboPrimitive G_Cubo;
    internal readonly CylinderPrimitive G_Cilindro;

    internal Content(ContentManager Content, GraphicsDevice GraphicsDevice)
    {
        ContentManager = Content;
        ContentManager.RootDirectory = "Content";

        // Geometrias
        G_Quad = new QuadPrimitive(GraphicsDevice);
        G_Cubo = new CuboPrimitive(GraphicsDevice);
        G_Cilindro = new CylinderPrimitive(GraphicsDevice);

        // Efectos
        Efectos.Add(E_BasicShader        = LoadEffect("BasicShader")            );
        Efectos.Add(E_TextureShader      = LoadEffect("TextureShader")          );
        Efectos.Add(E_TextureTiles       = LoadEffect("TextureTiles")           );
        Efectos.Add(E_SpiralShader       = LoadEffect("SpiralShader")           );
        Efectos.Add(E_BlacksFilter       = LoadEffect("BlacksFilter")           );
        Efectos.Add(E_Traslucid          = LoadEffect("TextureTraslucida")      );
        Efectos.Add(E_TwoTextureMix      = LoadEffect("TwoTextureMix")          );
        Efectos.Add(E_TextureItermitente = LoadEffect("TextureItermitente")     );
        Efectos.Add(E_TextureMirror      = LoadEffect("DeTextura/TextureMirror"));
        Efectos.Add(E_PBRShader          = LoadEffect("PBR")                    );
        Efectos.Add(E_PBRpackedShader    = LoadEffect("PBRpacked")              );
        Efectos.Add(E_BulletShader       = LoadEffect("BulletShader")           );
        Efectos.Add(E_BlinnPhong         = LoadEffect("BlinnPhong")             );
        Efectos.Add(E_BlinnPhongTiles    = LoadEffect("BlinnPhongTiles")             );
        
        EfectosHUD.Add(HE_HealthHUD   = LoadEffect("HealthHUD"));
        EfectosHUD.Add(HE_TurboHUD    = LoadEffect("TurboHUD"));
        EfectosHUD.Add(HE_TextureHUD  = LoadEffect("TextureHUD"));


        // Texturas
        T_Alfombra          = LoadTexture("Alfombra");
        T_Concreto          = LoadTexture("Concreto");
        T_Ladrillos         = LoadTexture("Bricks");
        T_Marmol            = LoadTexture("Marmol");
        T_MarmolNegro       = LoadTexture("MarmolNegro");
        T_Reboque           = LoadTexture("Reboque");
        T_MeshFilter        = LoadTexture("MeshFilter");
        T_MaderaNikari      = LoadTexture("MaderaNikari");
        T_PisoMadera        = LoadTexture("PisoMadera");
        T_PisoMaderaClaro   = LoadTexture("PisoMaderaClaro");
        T_PisoMaderaElegante= LoadTexture("PisoMaderaElegante");
        T_CubosMadera       = LoadTexture("CubosMadera");
        T_PisoCeramica      = LoadTexture("PisoCeramica");
        T_PisoAlfombrado    = LoadTexture("PisoAlfombra");
        T_AlfombraHabitacion= LoadTexture("AlfombraHabitacion");
        T_MysteryBox        = LoadTexture("MysteryBox");
        T_SillaOficina      = LoadTexture("Muebles/SillaOficina");
        T_Dragon            = LoadTexture("Muebles/Dragon");
        T_CombatVehicle     = LoadTexture("Autos/CombatVehicle");

        // Texturas del auto RacingCar (PBR)
        TA_BaseColor       = LoadTexture("Autos/RacingCar/BaseColor");
        TA_MetalMap        = LoadTexture("Autos/RacingCar/Metalizado");
        TA_RoughnessMap    = LoadTexture("Autos/RacingCar/Rugosidad");
        TA_CavityMap       = LoadTexture("Autos/RacingCar/Cavidad");
        TA_LightEmissionMap= LoadTexture("Autos/RacingCar/EmisionLuz");
        TA_NormalMap       = LoadTexture("Autos/RacingCar/NormalMap");

        // Texturas de la planta (PBR)
        T_Planta_BaseColorMap                = LoadTexture("Muebles/Planta/BaseColorMap");
        T_Planta_NormalMap                   = LoadTexture("Muebles/Planta/NormalMap");
        T_Planta_RoughnessMetallicOpacityMap = LoadTexture("Muebles/Planta/RoughnessMetallicOpacityMap");

        // Texturas de la plantis (PBR)
        T_Plantis_BaseColorMap                = LoadTexture("Muebles/Plantis/BaseColorMap");
        T_Plantis_NormalMap                   = LoadTexture("Muebles/Plantis/NormalMap");
        T_Plantis_RoughnessMetallicOpacityMap = LoadTexture("Muebles/Plantis/RoughnessMetallicOpacityMap");

        // Imágenes del HUD
        TH_Bullet           = LoadTexture("HUD/BulletAmmo");
        TH_EmptyBullet      = LoadTexture("HUD/EmptyBulletAmmo");

        // Imágenes Presentación
        TP_Presentacion0    = LoadTexture("Presentation/Presentacion0");
        TP_Presentacion1    = LoadTexture("Presentation/Presentacion1");
        TP_Presentacion2    = LoadTexture("Presentation/Presentacion2");
        TP_Presentacion3    = LoadTexture("Presentation/Presentacion3");

        // Imágenes Presentación
        TM_Start            = LoadTexture("Menu/Start");
        TM_Play             = LoadTexture("Menu/Play");
        TM_Pointer          = LoadTexture("Menu/Pointer");
        TM_PlayOption       = LoadTexture("Menu/PlayOption");
        TM_SettingsOption   = LoadTexture("Menu/SettingsOption");
        TM_Controles        = LoadTexture("Menu/Controles");


        // Sonidos
        S_SynthWars         = LoadSong("SynthWars");
        S_MeiHuaSan         = LoadSong("MeiHuaSen");
        S_ItsAFight         = LoadSong("ItsAFight");

        S_Metralleta        = LoadSound("Metralleta");
        S_MotorEncendido    = LoadSound("MotorEncendido");
        S_MotorRegulando    = LoadSound("MotorRegulando");
        S_MotorAcelerando   = LoadSound("MotorAcelerando");
        S_Bling             = LoadSound("Bling");
        S_Pickup            = LoadSound("PickUp");
        
        #region Modelos ( Shader , CarpetaUbicacion, Etiqueta )
        M_Auto              = LoadModel("Autos/", "RacingCar"     );
        M_AutoPegni         = LoadModel("Autos/", "PegniZonda"    );
        M_AutoEnemigo       = LoadModel("Autos/", "CombatVehicle" );
        
        M_Misil             = LoadModel("Autos/", "Misil"         );
        //M_BrMisil           = LoadModel("Autos/", "BrMisil"       );
        
        //Living
        M_Silla             = LoadModel("Muebles/", "Chair"       );
        M_Mesa              = LoadModel("Muebles/", "Mesa"        );
        M_Sillon            = LoadModel("Muebles/", "Sillon"      );
        M_Televisor         = LoadModel("Muebles/", "Televisor"   );
        M_MuebleTV          = LoadModel("Muebles/", "MuebleTV"    );
        M_Mesita            = LoadModel("Muebles/", "Mesita"      );
        M_Sofa              = LoadModel("Muebles/", "Sofa"        );
        M_Aparador          = LoadModel("Muebles/", "Aparador"    );
        M_Piano             = LoadModel("Muebles/", "Piano"       );

        //Oficina
        M_SillaOficina      = LoadModel("Muebles/", "SillaOficina");
        M_CafeRojo          = LoadModel("Muebles/", "Cafe-Rojo"   );
        M_Escritorio        = LoadModel("Muebles/", "Escritorio"  );
        M_Planta            = LoadModel("Muebles/", "Planta"      );
        M_Plantis           = LoadModel("Muebles/", "Plantis"     );

        //Dormitorios
        M_CamaMarinera      = LoadModel("Muebles/", "CamaMarinera");
        M_Cajonera          = LoadModel("Muebles/", "Cajonera"    );
        M_Organizador       = LoadModel("Muebles/", "Organizador" );
        M_Lego              = LoadModel("Muebles/", "Lego"        );
        M_Alfil             = LoadModel("Muebles/", "Alfil"       );
        M_Torre             = LoadModel("Muebles/", "Torre"       );
        M_Dragon            = LoadModel("Muebles/", "Dragon"      );
        M_Dragona           = LoadModel("Muebles/", "Dragona"     );
        M_Cama              = LoadModel("Muebles/", "Cama"        );
        M_Juego             = LoadModel("Muebles/", "Juego"       );
        M_Puff              = LoadModel("Muebles/", "Puff"        );
        M_Armario1          = LoadModel("Muebles/", "Armario1"    );

        //Baño
        M_Baniera           = LoadModel("Muebles/", "Baniera"     );
        M_Bacha             = LoadModel("Muebles/", "Bacha"       );
        M_Inodoro           = LoadModel("Muebles/", "Inodoro"     );
        
    
        //Cocina
        M_Cocine            = LoadModel("Muebles/", "Cocine"      );
        M_MesadaCentral     = LoadModel("Muebles/SetCocina/", "MesadaCentral");
        M_MesadaLateral     = LoadModel("Muebles/SetCocina/", "MesadaLateral");
        M_MesadaLateral2    = LoadModel("Muebles/SetCocina/", "MesadaLateral2");
        M_Mesada            = LoadModel("Muebles/SetCocina/", "Mesada");
        M_Alacena           = LoadModel("Muebles/SetCocina/", "Alacena");
        M_Botella           = LoadModel("Muebles/SetCocina/", "Botella");
        M_Maceta            = LoadModel("Muebles/SetCocina/", "Maceta");
        M_Maceta2           = LoadModel("Muebles/SetCocina/", "Maceta2");
        M_Maceta3           = LoadModel("Muebles/SetCocina/", "Maceta3");
        M_Maceta4           = LoadModel("Muebles/SetCocina/", "Maceta4");
        M_Olla              = LoadModel("Muebles/SetCocina/", "Olla");
        M_ParedCocina       = LoadModel("Muebles/SetCocina/", "ParedCocina");
        M_Plato             = LoadModel("Muebles/SetCocina/", "Plato");
        M_PlatoGrande       = LoadModel("Muebles/SetCocina/", "PlatoGrande");
        M_PlatosApilados    = LoadModel("Muebles/SetCocina/", "PlatosApilados");
        M_Heladera          = LoadModel("Muebles/SetCocina/", "Heladera");

        //Pasillo
        #endregion
    }


    public Model LoadModel(string dir) => ContentManager.Load<Model>(ContentFolder3D + dir);
    public Effect LoadEffect(string dir) => ContentManager.Load<Effect>(ContentFolderEffects + dir);
    public Texture2D LoadTexture(string dir) => ContentManager.Load<Texture2D>(ContentFolderTextures + dir);
    public Song LoadSong(string dir) => ContentManager.Load<Song>(ContentFolderMusic + dir);
    private SoundEffect LoadSound(string dir) => ContentManager.Load<SoundEffect>(ContentFolderSounds + dir);
    public Model LoadModel(string carpeta, string tag){
        var model = LoadModel(carpeta+tag+'/'+tag);
        model.Tag = tag;
        return model;
    }
}
