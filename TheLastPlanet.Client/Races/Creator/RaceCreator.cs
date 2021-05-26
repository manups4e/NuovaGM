using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Races.Creator
{
	internal static class RaceCreator
	{
		private enum RotationDummyType
		{
			Heading,
			Roll,
			Pitch,
			Yaw
		}
		private static List<ObjectHash> placement = new()
		{
			ObjectHash.prop_mp_placement,
			ObjectHash.prop_mp_placement_lrg,
			ObjectHash.prop_mp_placement_maxd,
			ObjectHash.prop_mp_placement_med,
			ObjectHash.prop_mp_placement_red,
			ObjectHash.prop_mp_placement_sm
		};

		private static Dictionary<string, List<dynamic>> CategorieEListe = new Dictionary<string, List<dynamic>>()
		{
			["Pista Stunt"] = new List<dynamic>
			{
				"Griglia di Partenza 16",
				"Griglia di Partenza 30",
				"Linea di Partenza 01",
				"Linea di Partenza 01 (30)",
				"Linea di Partenza 02",
				"Linea di Partenza 02 (30)",
				"Linea di Partenza 03",
				"Linea di Partenza 03 (30)",
				"Linea di Partenza con Pit Stop Sx (16)",
				"Linea di Partenza con Pit Stop Dx (16)",
				"Linea di Partenza con Pit Stop Sx (30)",
				"Linea di Partenza con Pit Stop Dx (30)",
				"Dritta Corta",
				"Dritta Media",
				"Dritta Medio-Larga",
				"Dritta Larga",
				"Dritta Larga con Pit Stop",
				"Curva Media",
				"Curva Larga",
				"Curva XL",
				"Curva 5°",
				"Curva 15°",
				"Curva 30°",
				"Curva 45°",
				"Curva Stretta 90°",
				"Curva 135°",
				"Curva 180°",
				"Curva 180° Tornante",
				"Bivio",
				"Incrocio",
				"Collegamento",
				"Gincana Sx",
				"Gincana Sx Piccola",
				"Gincana Dx",
				"Gincana Dx Piccola",
				"Ostacolo Pneumatico",
				"Discesa (5)",
				"Discesa (15)",
				"Discesa (30)",
				"Discesa (45)",
				"Salita (5)",
				"Salita (15)",
				"Salita (30)",
				"Salita (45)",
			},
			["Pista Stunt con Barriere"] = new List<dynamic>()
			{
				"Dritta Corta",
				"Dritta Media",
				"Dritta Medio-Larga",
				"Dritta Larga",
				"Curva Media",
				"Curva Larga",
				"Curva XL",
				"Curva 5°",
				"Curva 15°",
				"Curva 30°",
				"Curva 45°",
				"Curva 90° Stretta",
				"Curva 180°",
				"Curva 180° Tornante",
				"Bivio",
				"Incrocio",
				"Strettoia Pubb. 01",
				"Strettoia Pubb. 02",
				"Strettoia Pubb. 03",
				"Ostacolo ai Lati",
				"Ostacolo al Centro",
			},
			["Pista Stunt Rialzata"] = new List<dynamic>()
			{
				"Griglia di Partenza 16",
				"Griglia Linea di Partenza 16",
				"Griglia di Partenza 30",
				"Griglia Linea di Partenza 30",
				"Stunt Extra Piccola",
				"Stunt Piccola",
				"Stunt Dritta",
				"Stunt Curva",
				"Stunt Curva 15°",
				"Stunt Curva 30°",
				"Stunt Curva 45°",
				"Stunt Curva 45° Stretta",
				"Stunt Inversione a U",
				"Stunt Ponte",
				"Stunt Sorpasso",
				"Stunt Bivio",
				"Stunt Strettoia",
				"Stunt Strettoia Lunga",
				"Stunt Pendenza 15",
				"Stunt Pendenza 30",
				"Stunt Pendenza 45",
				"Stunt Collina Ripida",
				"Stunt Colline Piccole",
				"Stunt Dossi",
				"Stunt Salto",
				"Stunt Salto 15",
				"Stunt Salto 30",
				"Stunt Salto 45",
				"Stunt Collegamento",
				"Stunt Collegamento Largo",
				"Stunt Larga a Collegamento Pista",
				"Stunt Larga Piccola",
				"Stunt Larga Curva 15",
				"Stunt Larga Curva",
				"Stunt Larga Curva U",
				"Stunt Larga Pendenza 15",
				"Stunt Larga Pendenza 30",
				"Stunt Larga Pendenza 45",
				"Stunt Unione Tubo",
			},
			["Barriere Stunt"] = new List<dynamic>()
			{
				"Barriera da Gara 1 Sezione",
				"Barriera da Gara 2 Sezioni",
				"Barriera da Gara 4 Sezioni",
				"Barriera da Gara 8 Sezioni",
				"Barriera da Gara 16 Sezioni",
				"Barriera da Gara 5° Corta",
				"Barriera da Gara 15° Corta",
				"Barriera da Gara 30° Corta",
				"Barriera da Gara 45° Corta",
				"Barriera da Gara 90° Corta",
				"Barriera da Gara 5°",
				"Barriera da Gara 15°",
				"Barriera da Gara 30°",
				"Barriera da Gara 45°",
				"Barriera da Gara 90°",
				"Barriera da Gara 5° Piegata",
				"Barriera da Gara 15° Piegata",
				"Barriera da Gara 30° Piegata",
				"Barriera da Gara 45° Piegata",
				"Barriera da Gara 90° Piegata",
				"Barriera da Gara Dritta Extra Corta",
				"Barriera da Gara Dritta Corta",
				"Barriera da Gara Dritta Media",
				"Barriera da Gara Dritta Media Esterna",
				"Barriera da Gara Dritta Media Interna",
				"Barriera da Gara Dritta Larga Esterna",
				"Barriera da Gara Dritta Larga Interna",
			},
			["Tubi Stunt"] = new List<dynamic>()
			{
				"Tubo Extra Corto",
				"Tubo Corto",
				"Tubo Medio",
				"Tubo Lungo",
				"Tubo Extra Lungo",
				"Tubo Curva",
				"Tubo Curva 5°",
				"Tubo Curva 15°",
				"Tubo Curva 30°",
				"Tubo Curva 45°",
				"Tubo Bivio",
				"Tubo Croce",
				"Tubo Apertura Singola",
				"Tubo Apertura Doppia",
				"Tubo Aprtura Dritta",
				"Tubo Passagio 1/4",
				"Tubo Passaggio 1/2",
				"Tubo Salto 01",
				"Tubo Salto 02",
				"Tubo Passaggio Rotante",
				"Tubo Passaggio Rotante Medio",
				"Tubo 1/2 Passaggio Rotante",
				"Tubo 1/4 Passaggio Rotante",
				"Tubo 1/2 Passaggio Rotante Medio",
				"Tubo Ingresso",
				"Tubo Fine",
				"Tubo Fine con Passaggio",
				"Tubo Accelerazione",
				"Tubo Ricarica Boost",
				"Tubo Unione",
				"Tubo Apertura Dritta Opaco",
				"Tubo Corto Opaco",
			},
			["Tubi Stunt Neon"] = new List<dynamic>()
			{
				"Extra Corto",
				"Corto",
				"Medio",
				"Lungo",
				"Extra Lungo",
				"Curva",
				"Curva 5°",
				"Curva 15°",
				"Curva 30°",
				"Curva 45°",
			},
			["Frecce Neon Grandi"] = new List<dynamic>()
			{
				"Freccia Media Piccola",
				"Freccia Media Grossa",
				"Freccia Larga Piccola",
				"Freccia Larga Grossa",
				"Freccia XL Piccola",
				"Freccia XL Larga",
			},
			["Tubi Stunt Aerei"] = new List<dynamic>()
			{
				"Tubo Piccolo",
				"Tubo Medio",
				"Tubo Largo",
				"Tubo Enorme",
				"Tubo Gigantesco",
				"Tubo Curva 5°",
				"Tubo Curva 15°",
				"Tubo Curva 30°",
				"Tubo Curva 45°",
				"Tubo Passaggio 1/4",
				"Tubo Passaggio 1/2",
				"Tubo Rampa",
				"Tubo Apertura",
				"Tubo Velocià",
				"Tubo Piccolo x2",
				"Tubo Medio x2",
				"Tubo Largo x2",
				"Tubo Enorme x2",
				"Tubo Gigantesco x2",
				"Tubo Apertura x2",
				"Tubo Curva 5° x2",
				"Tubo Velocità x2",
				"Tubo Piccolo x4",
				"Tubo Medio x4",
				"Tubo Largo x4",
				"Tubo Enorme x4",
				"Tubo Gigantesco x4",
				"Tubo Apertura x4",
				"Tubo Curva 5° x4",
			},
			["Anelli Checkpoint Stunt"] = new List<dynamic>()
			{
				"Checkpoint Piccolo",
				"Checkpoint Medio",
				"Checkpoint Largo",
				"Checkpoint Extra Large",
				"Checkpoint XXL",
				"Checkpoint Curva",
				"Checkpoint Curva 5°",
				"Checkpoint Curva 15°",
				"Checkpoint Curva 30°",
				"Checkpoint Curva 45°",
				"Checkpoint Bivio",
			},
			["Passaggi Aerei Stunt"] = new List<dynamic>()
			{
				"Passaggio Pentagono Neon",
				"Passaggio Pentagono Neon Invertito",
				"Passaggio Esagono Neon",
				"Passaggio Esagono Neon Invertito",
				"Passaggio Neon Quadrato",
				"Passaggio Neon Cerchio",
				"Passaggio Neon Forma Aeroplano",
			},
			["Passaggi Gonfiabili Stunt"] = new List<dynamic>()
			{
				"Gonfiabile Traliccio Piccolo",
				"Gonfiabile Traliccio Medio",
				"Gonfiabile Traliccio Largo",
				"Gonfiabile Striscia Piccola",
				"Gonfiabile Striscia Media",
				"Gonfiabile Striscia Larga",
				"Gonfiabile Striscia Larga x3",
				"Gonfiabile Striscia Larga x5",
				"Gonfiabile Striscia Larga x7",
				"Gonfiabile Ponte Piccolo",
				"Gonfiabile Ponte Largo",
				"Gonfiabile Ponte Largo x3",
				"Gonfiabile Ponte Largo x5",
				"Gonfiabile Ponte Largo x7",
				"Gonfiabile Traliccio Sprunk",
				"Gonfiabile Traliccio Rainé",
				"Gonfiabile Traliccio Flow",
				"Gonfiabile Traliccio Shark",
				"Gonfiabile Traliccio Jackal",
				"Gonfiabile Traliccio METV",
				"Gonfiabile Ponte Sprunk",
				"Gonfiabile Ponte Rainé",
				"Gonfiabile Ponte Flow",
				"Gonfiabile Ponte Shark",
				"Gonfiabile Ponte Jackal",
				"Gonfiabile Ponte METV",
			},
			["Blocchi da Costruzione Stunt"] = new List<dynamic>()
			{
				"Blocco 1x2",
				"Blocco 2x2",
				"Blocco 3x2",
				"Blocco 1x3",
				"Blocco 2x3",
				"Blocco 3x3",
				"Blocco 1x4",
				"Blocco 2x4",
				"Blocco 3x4",
				"Blocco 1x5",
				"Blocco 2x5",
				"Blocco 3x5",
				"Blocco 9x11",
				"Blocco 9x21",
				"Blocco 18x21",
				"Blocco 23x28",
				"Blocco 28x48",
				"Pannello Pericolo Largo",
				"Blocco Pericolo Piccolo",
				"Blocco Pericolo Medio",
				"Blocco Pericolo Largo",
				"Blocco Pericolo Extra Large",
			},
			["Blocchi Stunt Neon"] = new List<dynamic>()
			{
				"Blocco Piccolo 1",
				"Blocco Medio 1",
				"Blocco Largo 1",
				"Blocco Extra Large 1",
				"Blocco Piccolo 2",
				"Blocco Medio 2",
				"Blocco Largo 2",
				"Blocco Extra Large 2",
				"Blocco Piccolo 3",
				"Blocco Medio 3",
				"Blocco Largo 3",
				"Blocco Extra Large 3",
				"Blocco Enorme 1",
				"Blocco Enorme 2",
				"Blocco Enorme 3",
				"Blocco Enorme 4",
				"Blocco Enorme 5",
			},
			["Rampe Stunt"] = new List<dynamic>()
			{
				"Salto Piccolo",
				"Salto Medio",
				"Salto Largo",
				"Salto Liscio Piccolo",
				"Salto Liscio Medio",
				"Salto Liscio Largo",
				"Rampa XS",
				"Rampa Piccola",
				"Rampa Media",
				"Rampa Larga",
				"Rampa XL",
				"Rampa XXL",
				"Rampa Larga Ampia",
				"Rampa XL Ampia",
				"Rampa XXL Ampia",
				"Rampa Larga Ampia Stunt",
				"Rampa XL Ampia Stunt",
				"Rampa XXL Ampia Stunt",
				"Rampia Media Flip Sx",
				"Rampia Media Flip Dx",
				"Rampia Piccola Flip Sx",
				"Rampia Piccola Flip Dx",
				"Rampa Stunt Gigante",
				"Rampa Stunt Gigante Ampia",
				"1/4 Pipe Piccolo",
				"1/4 Pipe Medio",
				"1/4 Pipe Largo",
				"Gobba da Salto Piccola",
				"Gobba da Salto Media",
			},
			["Pezzi Set Stunt"] = new List<dynamic>()
			{
				"Mezzo Loop",
				"Loop Intero",
				"Multi Loop",
				"Loop Intero (Rampa)",
				"Loop Intero (No Rampa)",
				"Spirale Dx Piccola",
				"Spirale Sx Piccola",
				"Spirale Dx Media",
				"Spirale Sx Media",
				"Spirale Dx Larga",
				"Spirale Sx Larga",
				"Spirale Dx XXL",
				"Spirale Sx XXL",
				"Wall Ride 45 Dx (Rampa)",
				"Wall Ride 45 Dx",
				"Wall Ride 45 Sx (Rampa)",
				"Wall Ride 45 Sx",
				"Wall Ride 90 Dx (Rampa)",
				"Wall Ride 90 Dx",
				"Wall Ride 90 Sx (Rampa)",
				"Wall Ride 90 Sx",
				"Wall Ride Curva",
				"Wall Ride Curva Sx (Rampa)",
				"Wall Ride Curva Dx (Rampa)",
				"Wall Ride Spirale Sx",
				"Wall Ride Spirale Dx",
				"Wall Ride Spirale Sx (Rampa)",
				"Wall Ride Spirale Dx (Rampa)",
				"Bersaglio Stunt Piccolo",
				"Bersaglio Stunt",
				"Stand Birillo",
				"Porta da Calcio",
				"Zona di Atterraggio Stunt",
				"Pneumatico Ruota",
				"Ostacolo Pista",
				"Pista di Decollo Stunt",
			},
			["Segnali Stunt"] = new List<dynamic>()
			{
				"Segnale di Stop",
				"Segnale 5",
				"Segnale 10",
				"Segnale 50",
				"Segnale 100",
				"Segnale Pericolo",
				"Segnale Tornante Sx",
				"Segnale Tornante Dx",
				"Segnale Curva Sx",
				"Segnale Curva Dx",
				"Segnale Gincana Sx",
				"Segnale Gincana Dx",
				"Segnale !",
				"Segnale >",
				"Segnale <",
				"Segnale Pits Dx",
				"Segnale Pits Sx",
				"Cartello Piega",
				"Cartello Onda",
				"Cartello Rallenta",
				"Cartello Rampa",
				"Cartello Half Loop ",
				"Cartello Loop",
				"Cartello No Auto",
				"Cartello Birillo",
				"Cartello Dirupo",
				"Cartello Salita Ripida",
				"Cartello Curva a gomito Sx",
				"Cartello Dossi",
				"Cartello Curva U",
				"Cartello Strada Inclinata",
				"Cartello !",
				"Segnale Freccia Dritto",
				"Segnale Freccia Sx",
				"Segnale Freccia Dx",
				"Segnale Freccia Sx Medio",
				"Segnale Freccia Sx Largo",
				"Segnale Freccia Dx Medio",
				"Segnale Freccia Dx Largo",
				"Muro di Ruote Corto",
				"Muro di Ruote Corto Dx",
				"Muro di Ruote Corto Sx",
				"Muro di Ruote Pits Sx",
				"Muro di Ruote Pits Dx",
				"Muro di Ruote Tornante Sx",
				"Muro di Ruote Tornante Dx",
				"Muro di Ruote Medio",
				"Muro di Ruote Largo 01",
				"Muro di Ruote Largo 02",
				"Muro di Ruote Largo 03",
				"Muro di Ruote Largo 04",
				"Muro di Ruote Largo 05",
				"Muro di Ruote Largo 06",
				"Muro di Ruote Largo 07",
				"Muro di Ruote Largo 08",
				"Muro di Ruote Largo 09",
				"Muro di Ruote Largo 10",
				"Muro di Ruote Largo 11",
				"Muro di Ruote Largo 12",
				"Muro di Ruote Largo 13",
				"Muro di Ruote Largo 13",
				"Muro di Ruote Medio Dx 01",
				"Muro di Ruote Medio Dx 02",
				"Muro di Ruote Medio Dx 03",
				"Muro di Ruote Medio Dx 04",
				"Muro di Ruote Medio Dx 05",
				"Muro di Ruote Medio Dx 06",
				"Muro di Ruote Medio Dx 07",
				"Muro di Ruote Medio Dx 08",
				"Muro di Ruote Largo Dx 01",
				"Muro di Ruote Largo Dx 02",
				"Muro di Ruote Largo Dx 03",
				"Muro di Ruote Largo Dx 04",
				"Muro di Ruote Largo Dx 05",
				"Muro di Ruote Largo Dx 06",
				"Muro di Ruote Largo Dx 07",
				"Muro di Ruote Largo Dx 08",
				"Muro di Ruote Largo Dx 09",
				"Muro di Ruote Largo Dx 10",
				"Muro di Ruote Medio Sx 01",
				"Muro di Ruote Medio Sx 02",
				"Muro di Ruote Medio Sx 03",
				"Muro di Ruote Medio Sx 04",
				"Muro di Ruote Medio Sx 05",
				"Muro di Ruote Medio Sx 06",
				"Muro di Ruote Medio Sx 07",
				"Muro di Ruote Largo Sx 01",
				"Muro di Ruote Largo Sx 02",
				"Muro di Ruote Largo Sx 03",
				"Muro di Ruote Largo Sx 04",
				"Muro di Ruote Largo Sx 05",
				"Muro di Ruote Largo Sx 06",
				"Muro di Ruote Largo Sx 07",
				"Muro di Ruote Largo Sx 08",
				"Muro di Ruote Largo Sx 09",
				"Muro di Ruote Largo Sx 10",
				"Segnale Cavalletta (Obey)",
				"Segnale Cavalletta (Xero)",
				"Segnale Cavalletta (Fukaru)",
				"Segnale Cavalletta (Chepalle)",
				"Segnale Cavalletta (Globe Oil)",
				"Blimp Xero",
				"Segnale Prendere il Volo",
				"Segnale Ron",
				"Segnale Xero",
				"Segnale PissWasser Appeso",
				"Pneumatico Atomic",
				"Arco Kronos",
				"Segnale Pit Stop In Appeso",
				"Segnale Pit Stop Out Appeso",
			},
			["Speciali Stunt"] = new List<dynamic>()
			{
				"Boost Velocità",
				"Boost Velocità Pista Rialzata",
				"Boost Velocità Pista",
				"Rallentare",
				"Rallentare Pista Rialzata",
				"Rallentare Pista",
				"Rifornimento Boost",
				"Rifornimento Boost Pista Rialzata",
				"Rifornimento Boost Pista",
				"Boost Velocita Aereo",
				"Fuoco D'artificio",
				"Ruota di Fuoco",
				"Ruota di Fuoco Media",
				"Ruota di Fuoco Larga",
				"Sirena Pubblica",
				"Speaker Audio",
			},
			["Dinamici"] = new List<dynamic>()
			{
				"Birillo Gigante", 
				"Palla da Bowling", 
				"Palla da Calcio Piccola",
				"Palla da Calcio Media",
				"Palla da Calcio Grande", 
				"Barili Esplosivi 01",
				"Barili Esplosivi 02",
				"Barili Esplosivi 03", 
			},
		};

		private static Camera enteringCamera;
		private static Prop cross;
		private static Marker placeMarker;
		private static Vector3 curLocation;
		private static float Height = 0;
		private static Vector3 curRotation;
		private static Vector3 cameraPosition;
		//private static Vehicle cameraVeh;
		private static RaceTrack data = new RaceTrack();
		private static Prop DummyProp; // prop temporaneo da posizionare
		private static Vector3 dummyRot;
		private static RotationDummyType rotationDummyType = RotationDummyType.Heading;
		private static List<TrackPieces> Piazzati = new List<TrackPieces>();
		private static float zoom = 75f;
		private static List<dynamic> grigliaVehStart = new List<dynamic>() { "2 X 2" };
		private static int categoriaScelta = 0;
		private static int tipoPropScelto = 0;

		public static async void CreatorPreparation()
		{
			Cache.MyPlayer.Player.CanControlCharacter = false;
			float height = GetHeightmapTopZForPosition(199.4f, -934.3f);
			Vector3 rot = new(-90f, 0f, 0f);
			enteringCamera = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", false));
			enteringCamera.Position = new Vector3(199.4f, -934.3f, height);
			enteringCamera.Rotation = rot;
			enteringCamera.IsActive = true;
			enteringCamera.FarClip = 1000;
			enteringCamera.FieldOfView = 30;
			RenderScriptCams(true, false, 3000, true, false);
			SetFrontendActive(false);
			curRotation = enteringCamera.Rotation;
			Cache.MyPlayer.Ped.IsPositionFrozen = true;
			Cache.MyPlayer.Ped.IsVisible = false;
			Cache.MyPlayer.Ped.IsInvincible = true;
			Cache.MyPlayer.Ped.DiesInstantlyInWater = false;
			Screen.Hud.IsRadarVisible = false;
			placeMarker ??= new Marker(MarkerType.HorizontalCircleSkinny, WorldProbe.CrossairRenderingRaycastResult.HitPosition, new Vector3(6.7f), Colors.GreyDark);

			if (cross == null)
			{
				float pz = 0;
				GetGroundZFor_3dCoord(enteringCamera.Position.X, enteringCamera.Position.Y, enteringCamera.Position.Z, ref pz, false);
				cross = new Prop(CreateObjectNoOffset(Funzioni.HashUint("prop_mp_placement"), enteringCamera.Position.X, enteringCamera.Position.Y, pz, false, false, false));
				cross.IsVisible = true;
				SetEntityLoadCollisionFlag(cross.Handle, true);
				cross.LodDistance = 500;
				SetEntityCollision(cross.Handle, false, false);
				cameraPosition = enteringCamera.Position;
				Height = cross.Position.Z;
			}
			SetFocusEntity(cross.Handle);
			Client.Instance.AddTick(MoveCamera);
			CreatorMainMenu();
			Screen.Fading.FadeIn(10);
		}

		public static async void CreatorMainMenu()
		{
			UIMenu Creator = new("Creatore Gare", "Lo strumento dei creativi");
			Creator.MouseControlsEnabled = false;
			HUD.MenuPool.Add(Creator);
			UIMenu Dettagli = Creator.AddSubMenu("Dettagli");
			UIMenu Posizionamento = Creator.AddSubMenu("Posizionamento");

			#region Dettagli

			UIMenuItem titolo = new("Titolo");
			Dettagli.AddItem(titolo);
			titolo.SetRightBadge(BadgeStyle.Alert);
			UIMenuItem descrizione = new("Descrizione");
			descrizione.SetRightBadge(BadgeStyle.Alert);
			Dettagli.AddItem(descrizione);
			UIMenuItem foto = new("Foto");
			Dettagli.AddItem(foto);
			UIMenuListItem tipoGara = new("Tipo di Gara", new List<dynamic>() { "Standard", "Senza contatto" }, data.TipoGara);
			Dettagli.AddItem(tipoGara);
			UIMenuListItem tipoDiGiri = new("Tipo di Giri", new List<dynamic>() { "Giri", "Da punto a punto" }, data.TipoGiri); // punto a punto disabilita numero laps
			Dettagli.AddItem(tipoDiGiri);
			UIMenuListItem numeroLaps = new("Numero di Giri", new List<dynamic>() { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, data.Laps);
			Dettagli.AddItem(numeroLaps);
			UIMenuListItem numPlayers = new("Max Giocatori", new List<dynamic>() { 32, 64 }, data.MaxPlayers);
			Dettagli.AddItem(numPlayers);
			UIMenu veicoliDisponibili = Dettagli.AddSubMenu("Veicoli Disponibili", "Attenzione, se si sceglie la griglia piccola saranno disponibili solo le MOTO"); // aggiungere le classi dei veicoli e premendo espandi scegliamo i singoli veicoli
			veicoliDisponibili.ParentItem.SetRightBadge(BadgeStyle.Car);
			#region veicoliDisponibili
			UIMenuCheckboxItem Compacts = new UIMenuCheckboxItem("Compacts", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Compacts);
			UIMenuCheckboxItem Sedans = new UIMenuCheckboxItem("Sedans", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Sedans);
			UIMenuCheckboxItem SUVs = new UIMenuCheckboxItem("SUVs", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(SUVs);
			UIMenuCheckboxItem Coupes = new UIMenuCheckboxItem("Coupes", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Coupes);
			UIMenuCheckboxItem Muscle = new UIMenuCheckboxItem("Muscle", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Muscle);
			UIMenuCheckboxItem SportsClassics = new UIMenuCheckboxItem("SportsClassics", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(SportsClassics);
			UIMenuCheckboxItem Sports = new UIMenuCheckboxItem("Sports", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Sports);
			UIMenuCheckboxItem Super = new UIMenuCheckboxItem("Super", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Super);
			UIMenuCheckboxItem Motorcycles = new UIMenuCheckboxItem("Motorcycles", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Motorcycles);
			UIMenuCheckboxItem OffRoad = new UIMenuCheckboxItem("OffRoad", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(OffRoad);
			UIMenuCheckboxItem Industrial = new UIMenuCheckboxItem("Industrial", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Industrial);
			UIMenuCheckboxItem Utility = new UIMenuCheckboxItem("Utility", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Utility);
			UIMenuCheckboxItem Vans = new UIMenuCheckboxItem("Vans", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Vans);
			UIMenuCheckboxItem Cycles = new UIMenuCheckboxItem("Cycles", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Cycles);
			UIMenuCheckboxItem Boats = new UIMenuCheckboxItem("Boats", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Boats);
			UIMenuCheckboxItem Helicopters = new UIMenuCheckboxItem("Helicopters", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Helicopters);
			UIMenuCheckboxItem Planes = new UIMenuCheckboxItem("Planes", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Planes);
			UIMenuCheckboxItem Service = new UIMenuCheckboxItem("Service", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Service);
			UIMenuCheckboxItem Emergency = new UIMenuCheckboxItem("Emergency", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Emergency);
			UIMenuCheckboxItem Commercial = new UIMenuCheckboxItem("Commercial", UIMenuCheckboxStyle.Tick, false, "");
			veicoliDisponibili.AddItem(Commercial);
			#endregion
			UIMenuListItem classeDefault = new("Classe Default", new List<dynamic>() { "Super" }, 0); // da gestire in base alla griglia e alle classi permesse in veicoli disponibili
			Dettagli.AddItem(classeDefault);
			UIMenuListItem veicoloDefault = new("Veicolo Default", new List<dynamic>() { "Prototipo" }, 0); // inserire i veicoli in base al nome del modello e in base alle classi permesse
			Dettagli.AddItem(veicoloDefault);
			UIMenuListItem orario = new("Orario", new List<dynamic>() { "Mattino", "Pomeriggio", "Notte" }, 0); // cambiare l'orario e aggiungere tramonto e alba
			Dettagli.AddItem(orario);
			UIMenuListItem meteo = new("Meteo", new List<dynamic>() { "Soleggiato", "Pioggia", "Smog", "Sereno", "Nuvoloso", "Coperto", "Tempesta", "Nebbia" }, 0); // cambiare il meteo in base alla necesità e magari aggiungere alba e tramonto
			Dettagli.AddItem(meteo);
			UIMenuListItem traffico = new("Traffico", new List<dynamic>() { "Off", "Basso", "Medio", "Alto" }, 0, "Se la gara è a mezz'aria, è consigliato impostare su OFF"); // cambiare il traffico in base alla necesità
			Dettagli.AddItem(traffico);
			#region dettagli item process
			Dettagli.OnItemSelect += async (a, b, c) =>
			{
				if (b == titolo)
				{
					string title = await HUD.GetUserInput("Inserisci Titolo (Max 25 caratteri)", "", 25);
					data.Titolo = title;
					b.SetRightLabel(title.Length > 15 ? title.Substring(0, 13) + "..." : title);
				}
				else if (b == descrizione)
				{
					string desc = await HUD.GetUserInput("Inserisci descrizione (Max 70 caratteri)", "", 70);
					data.Descrizione = desc;
					b.SetRightLabel(desc.Length > 15 ? desc.Substring(0, 13) + "..." : desc);
				}
				if(!(string.IsNullOrWhiteSpace(data.Titolo) && string.IsNullOrWhiteSpace(data.Descrizione)))
				{
					// unlocks everything
					titolo.SetRightBadge(BadgeStyle.None);
					descrizione.SetRightBadge(BadgeStyle.None);
				}
			};

			Dettagli.OnListChange += async (a, b, c) =>
			{
				if (b == tipoGara)
				{
					data.TipoGara = c;
				}
				else if (b == tipoDiGiri)
				{
					data.TipoGiri = c;
				}
				else if (b == numeroLaps)
				{
					data.Laps = c;
				}
				else if (b == numPlayers)
				{
					data.MaxPlayers = c;
					grigliaVehStart.Clear();
					grigliaVehStart.Add("2 X " + data.MaxPlayers);
				}
				else if (b == orario)
				{
					data.Orario = c;
					PauseClock(true);
					switch (c)
					{
						case 0:
							NetworkOverrideClockTime(7, 0, 0);
							break;
						case 1:
							NetworkOverrideClockTime(17, 0, 0);
							break;
						case 2:
							NetworkOverrideClockTime(23, 59, 59);
							break;
					}
				}
				else if (b == meteo)
				{
					data.Meteo = c;
					//"Soleggiato", "Pioggia", "Smog", "Sereno", "Nuvoloso", "Coperto", "Tempesta", "Nebbia"
					switch (c)
					{
						case 0:
							World.TransitionToWeather(Weather.ExtraSunny, 1f);
							break;
						case 1:
							World.TransitionToWeather(Weather.Raining, 1f);
							break;
						case 2:
							World.TransitionToWeather(Weather.Smog, 1f);
							break;
						case 3:
							World.TransitionToWeather(Weather.Clear, 1f);
							break;
						case 4:
							World.TransitionToWeather(Weather.Clouds, 1f);
							break;
						case 5:
							World.TransitionToWeather(Weather.Overcast, 1f);
							break;
						case 6:
							World.TransitionToWeather(Weather.ThunderStorm, 1f);
							break;
						case 7:
							World.TransitionToWeather(Weather.Foggy, 1f);
							break;
					}
				}
				else if (b == traffico)
				{
					data.Traffico = c;
					switch (c)
					{
						case 0:
							break;
						case 1:
							break;
						case 2:
							break;
						case 3:
							break;
					}
				}
				else if (b == classeDefault)
				{
					var veicoli = Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().ToList();
					VehicleClass classe;
					bool success = Enum.TryParse(b.Items[c].ToString(), out classe);
					var disp = veicoli.Where(x => GetVehicleClassFromName((uint)x) == (int)classe).Except(data.VeicoliEsclusi).ToList();
					veicoloDefault.Items.Clear();
					foreach (var d in disp)
						veicoloDefault.Items.Add(d.ToString());
					VehicleClass pippo;
					VehicleHash poppo;
					Enum.TryParse((string)classeDefault.Items.Last(), out pippo);
					Enum.TryParse((string)veicoloDefault.Items.First(), out poppo);
					data.ClasseDefault = pippo;
					data.VeicoloDefault = poppo;
				}
				else if (b == veicoloDefault)
				{
					VehicleHash classe;
					bool success = Enum.TryParse(b.Items[c].ToString(), out classe);
					data.VeicoloDefault = classe;
				}
			};
			#endregion

			#region ClassiVeicoli Process
			veicoliDisponibili.OnCheckboxChange += (a, b, c) =>
			{
				try
				{
					VehicleClass classe;
					bool success = Enum.TryParse(b.Text, out classe);
					if (success)
					{
						if (c)
							data.ClassiPermesse.Add(classe);
						else
							data.ClassiPermesse.Remove(classe);
						classeDefault.Items.Clear();
						var veicoli = Enum.GetValues(typeof(VehicleHash)).Cast<VehicleHash>().ToList();
						foreach (var cla in data.ClassiPermesse)
							classeDefault.Items.Add(cla.ToString());
						var disp = veicoli.Where(x => GetVehicleClassFromName((uint)x) == (int)classe).Except(data.VeicoliEsclusi).ToList();
						veicoloDefault.Items.Clear();
						foreach (var d in disp)
							veicoloDefault.Items.Add(d.ToString());
						VehicleClass pippo;
						VehicleHash poppo;
						Enum.TryParse((string)classeDefault.Items.Last(), out pippo);
						Enum.TryParse((string)veicoloDefault.Items.First(), out poppo);
						data.ClasseDefault = pippo;
						data.VeicoloDefault = poppo;
						Dettagli.RefreshIndex();
					}
				}
				catch (Exception e)
				{
					Client.Logger.Error(e.ToString());
				}
			};

			#endregion
			#endregion

			#region Posizionamento

			UIMenu checkpointsEGriglia = Posizionamento.AddSubMenu("Checkpoints");
			UIMenu propPlacing = Posizionamento.AddSubMenu("Posizionamento tracciato");


			#region CHECKPOINTS E GRIGLIA

			UIMenu grigliaDiPartenza = checkpointsEGriglia.AddSubMenu("Griglia di partenza");
			UIMenu checkPoints = checkpointsEGriglia.AddSubMenu("Posiziona Checkpoint");

			#region GRIGLIA DI PARTENZA

			UIMenuListItem dimensioniGriglia = new UIMenuListItem("Dimensioni Griglia", grigliaVehStart, 0);
			UIMenuListItem tipoGriglia = new UIMenuListItem("Tipo Griglia", new List<dynamic>() { "Grande", "Media", "Piccola" }, 0);
			grigliaDiPartenza.AddItem(dimensioniGriglia);
			grigliaDiPartenza.AddItem(tipoGriglia);

			#endregion

			#region CHECKPOINTS
			UIMenuItem piazzaCheck = new UIMenuItem("Piazza Checkpoint");
			UIMenuListItem tipoCheck = new UIMenuListItem("Tipo", new List<dynamic>() { "Primario", "Secondario" }, 0);
			UIMenuListItem stileCheck = new UIMenuListItem("Stile", new List<dynamic>() { "Classico", "Circolare" }, 0);
			checkPoints.AddItem(piazzaCheck);
			checkPoints.AddItem(tipoCheck);
			checkPoints.AddItem(stileCheck);
			#endregion

			#endregion

			#region PROPS E POSIZIONAMENTO
			UIMenuListItem categoria = new UIMenuListItem("Categoria", CategorieEListe.Keys.Cast<dynamic>().ToList(), 0);
			propPlacing.AddItem(categoria);
			UIMenuListItem tipo = new UIMenuListItem("Tipo", CategorieEListe[CategorieEListe.Keys.ToList()[0]], 0);
			propPlacing.AddItem(tipo);
			UIMenuListItem tipoRot = new UIMenuListItem("Tipo di Rotazione", new List<dynamic>() { "Heading Default", "Roll", "Pitch", "Yaw" }, 0);
			propPlacing.AddItem(tipoRot);
			UIMenuListItem color = new UIMenuListItem("Colore", new List<dynamic>() { "Rosso", "Blu", "Viola", "Nero", "Bianco", "Grigio", "Giallo", "Arancione", "Verde", "Rosa", "Da Gara", "Nero & Giallo", "Arancione & Blu", "Verde & Giallo", "Rosa & Grigio" }, 0);
			propPlacing.AddItem(color);
			UIMenuCheckboxItem stacking = new UIMenuCheckboxItem("Abilita Accatastamento Prop", UIMenuCheckboxStyle.Tick, false, "");
			propPlacing.AddItem(stacking);

			propPlacing.OnListChange += async (a, b, c) =>
			{
				if (b == categoria)
				{
					categoriaScelta = c;
				}
				else if(b == tipo)
				{
					Vector3 pos = Vector3.Zero;
					if (DummyProp != null)
						pos = DummyProp.Position;
					else
						pos = cross.Position;
					if(DummyProp != null)
						DummyProp.Delete();
					tipoPropScelto = c;
					string m = GetModelFromMenu();
					DummyProp = await Funzioni.SpawnLocalProp(m, pos, false, false);
					DummyProp.Heading = cross.Heading;
					DummyProp.IsCollisionEnabled = false;
					dummyRot = new (0, 0, curRotation.Z);
				}
				else if (b == tipoRot)
				{
					rotationDummyType = (RotationDummyType)c;
				}
			};


			UIMenu opzioniAvanzate = propPlacing.AddSubMenu("Opzioni Avanzate");
			#region opzioniAvanzate

			UIMenu overridePos = opzioniAvanzate.AddSubMenu("Override Posizione", "Utilizza una Free Camera i valori X, Y, Z per ~y~posizionare~w~ i componenti nelle esatte posizioni");
			UIMenu overrideRot = opzioniAvanzate.AddSubMenu("Override Posizione", "Utilizza una Free Camera i valori X, Y, Z per ~y~ruotare~w~ i componenti nelle esatte posizioni");
			UIMenuCheckboxItem useOverride = new UIMenuCheckboxItem("Usa Override", UIMenuCheckboxStyle.Tick, false, "");
			UIMenuListItem alignment = new UIMenuListItem("Allineamento", new List<dynamic>() { "Mondo", "Locale" }, 0);
			UIMenuDynamicListItem posX = new UIMenuDynamicListItem("X", curLocation.X.ToString("F3"), (sender, direction) =>
			{
				if (direction == UIMenuDynamicListItem.ChangeDirection.Left) curLocation.X -= 0.1f;
				else curLocation.X += 0.1f;
				return curLocation.X.ToString("F3");
			});
			UIMenuDynamicListItem posY = new UIMenuDynamicListItem("Y", curLocation.Y.ToString("F3"), (sender, direction) =>
			{
				if (direction == UIMenuDynamicListItem.ChangeDirection.Left) curLocation.Y -= 0.1f;
				else curLocation.Y += 0.1f;
				return curLocation.Y.ToString("F3");
			});
			UIMenuDynamicListItem posZ = new UIMenuDynamicListItem("Z", curLocation.Z.ToString("F3"), (sender, direction) =>
			{
				if (direction == UIMenuDynamicListItem.ChangeDirection.Left) curLocation.Z -= 0.1f;
				else curLocation.Z += 0.1f;
				return curLocation.Z.ToString("F3");
			});
			UIMenuDynamicListItem rotX = new UIMenuDynamicListItem("X", dummyRot.X.ToString("F3"), (sender, direction) =>
			{
				if (direction == UIMenuDynamicListItem.ChangeDirection.Left) dummyRot.X -= 0.1f;
				else dummyRot.X += 0.1f;
				return dummyRot.X.ToString("F3");
			});
			UIMenuDynamicListItem rotY = new UIMenuDynamicListItem("Y", dummyRot.Y.ToString("F3"), (sender, direction) =>
			{
				if (direction == UIMenuDynamicListItem.ChangeDirection.Left) dummyRot.Y -= 0.1f;
				else dummyRot.Y += 0.1f;
				return dummyRot.Y.ToString("F3");
			});
			UIMenuDynamicListItem rotZ = new UIMenuDynamicListItem("Z", dummyRot.Z.ToString("F3"), (sender, direction) =>
			{
				if (direction == UIMenuDynamicListItem.ChangeDirection.Left) dummyRot.Z -= 0.1f;
				else dummyRot.Z += 0.1f;
				return dummyRot.Z.ToString("F3");
			});
			overridePos.AddItem(alignment);
			overridePos.AddItem(posX);
			overridePos.AddItem(posY);
			overridePos.AddItem(posZ);
			overrideRot.AddItem(alignment);
			overrideRot.AddItem(rotX);
			overrideRot.AddItem(rotY);
			overrideRot.AddItem(rotZ);

			#endregion

			UIMenuListItem speedPadIntensity = new UIMenuListItem("Intensità Pad Accelerazione", new List<dynamic>() { "Debole", "Normale", "Forte", "Extra Forte", "Ultra Forte" }, 1);
			propPlacing.AddItem(speedPadIntensity);
			UIMenuListItem slowPadIntensity = new UIMenuListItem("Intensità Pad Rallentamento", new List<dynamic>() { "Debole", "Normale", "Forte", "Extra Forte", "Ultra Forte" }, 1);
			propPlacing.AddItem(slowPadIntensity);
			UIMenu soundTriggerMenu = propPlacing.AddSubMenu("Menu Attivazione Suoni");
			#region soundTriggerMenu

			UIMenuListItem soundId = new UIMenuListItem("Sound ID", new List<dynamic>() { "Airhorn", "Roar", "Chitarra 01", "Chitarra 02", "Clacson", "Tuono", "Allarme" }, 0);
			UIMenuItem soundPreview = new UIMenuItem("Play Anteprima Suono");
			UIMenuListItem radius = new UIMenuListItem("Distanza Attivazione", new List<dynamic> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 150 }, 0);
			UIMenuListItem voltePerLap = new UIMenuListItem("(Globale) Volte per Giro", new List<dynamic>() { "Infinite", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70 }, 0);
			UIMenuCheckboxItem oncePerLap = new UIMenuCheckboxItem("Una Volta per Giro", false);
			soundTriggerMenu.AddItem(soundId);
			soundTriggerMenu.AddItem(soundPreview);
			soundTriggerMenu.AddItem(radius);
			soundTriggerMenu.AddItem(voltePerLap);
			soundTriggerMenu.AddItem(oncePerLap);

			#endregion


			#endregion

			#endregion

			UIMenuItem Esci = new("Esci");
			Creator.AddItem(Esci);
			Creator.Visible = true;
			Creator.InstructionalButtons.Add(Creator.Back);
		}

		#region	METODI

		private static string GetModelFromMenu()
		{
			switch (categoriaScelta)
			{
				case 0:
					switch (tipoPropScelto)
					{
						case 0:
							return "stt_prop_track_start";

						case 1:
							return "stt_prop_track_start_02";

						case 2:
							return "stt_prop_race_start_line_01";

						case 3:
							return "stt_prop_race_start_line_01b";

						case 4:
							return "sr_prop_sr_start_line_02";

						case 5:
							return "stt_prop_race_start_line_02b";

						case 6:
							return "stt_prop_race_start_line_03";

						case 7:
							return "stt_prop_race_start_line_03b";

						case 8:
							return "sum_prop_ac_track_pit_stop_16l";

						case 9:
							return "sum_prop_ac_track_pit_stop_16r";

						case 10:
							return "sum_prop_ac_track_pit_stop_30l";

						case 11:
							return "ch_prop_track_pit_stop_01";

						case 12:
							return "sum_prop_ac_track_pit_stop_30r";

						case 13:
							return "stt_prop_track_straight_s";

						case 14:
							return "stt_prop_track_straight_m";

						case 15:
							return "ba_prop_track_straight_lm";

						case 16:
							return "stt_prop_track_straight_l";

						case 17:
							return "sum_prop_ac_track_pit_stop_01";

						case 18:
							return "stt_prop_track_bend_m";

						case 19:
							return "stt_prop_track_bend_l";

						case 20:
							return "ba_prop_track_bend_l_b";

						case 21:
							return "stt_prop_track_bend2_l";

						case 22:
							return "stt_prop_track_bend2_l_b";

						case 23:
							return "stt_prop_track_bend_5d";

						case 24:
							return "stt_prop_track_bend_15d";

						case 25:
							return "stt_prop_track_bend_30d";

						case 26:
							return "ch_prop_track_ch_bend_45";

						case 27:
							return "sum_prop_track_ac_bend_45";

						case 28:
							return "sum_prop_track_ac_bend_lc";

						case 29:
							return "ch_prop_track_ch_bend_135";

						case 30:
							return "sum_prop_track_ac_bend_135";

						case 31:
							return "stt_prop_track_bend_180d";

						case 32:
							return "sum_prop_track_ac_bend_180d";

						case 33:
							return "stt_prop_track_fork";

						case 34:
							return "stt_prop_track_cross";

						case 35:
							return "stt_prop_track_link";

						case 36:
							return "stt_prop_track_chicane_l";

						case 37:
							return "stt_prop_track_chicane_l_02";

						case 38:
							return "stt_prop_track_chicane_r";

						case 39:
							return "stt_prop_track_chicane_r_02";

						case 40:
							return "stt_prop_track_block_03";

						case 41:
							return "sr_prop_track_straight_l_d5";

						case 42:
							return "sr_prop_track_straight_l_d15";

						case 43:
							return "sr_prop_track_straight_l_d30";

						case 44:
							return "sr_prop_track_straight_l_d45";

						case 45:
							return "sr_prop_track_straight_l_u5";

						case 46:
							return "sr_prop_track_straight_l_u15";

						case 47:
							return "sr_prop_track_straight_l_u30";

						case 48:
							return "sr_prop_track_straight_l_u45";

					}
					break;
			}
			return null;
		}

		public static void changeModel(ObjectHash iVar11)
		{
			cross.Delete();
			cross = null;
			ObjectHash model = ObjectHash.prop_mp_placement_sm;

			switch (iVar11)
			{
				case ObjectHash.prop_mp_placement_sm:
					model = ObjectHash.prop_mp_placement_sm;

					break;
				case ObjectHash.prop_mp_placement_lrg:
					model = ObjectHash.prop_mp_placement_lrg;

					break;
				case ObjectHash.prop_mp_cant_place_sm:
					model = ObjectHash.prop_mp_cant_place_sm;

					break;
				case ObjectHash.prop_mp_cant_place_lrg:
					model = ObjectHash.prop_mp_cant_place_lrg;

					break;
				case ObjectHash.prop_mp_max_out_sm:
					model = ObjectHash.prop_mp_max_out_sm;

					break;
				case ObjectHash.prop_mp_max_out_lrg:
					model = ObjectHash.prop_mp_max_out_lrg;

					break;
			}

			if (cross == null)
			{
				cross = new Prop(CreateObjectNoOffset((uint)model, WorldProbe.CrossairRenderingRaycastResult.HitPosition.X, WorldProbe.CrossairRenderingRaycastResult.HitPosition.Y, WorldProbe.CrossairRenderingRaycastResult.HitPosition.Z, false, false, false))
				{
					IsVisible = true,
					LodDistance = 500,
					IsCollisionEnabled = false,
				};
				SetEntityLoadCollisionFlag(cross.Handle, true);
			}
		}

		private static bool func_9339(float iParam0, float fParam1)
		{
			if (IsInputDisabled(2))
				return true;
			if (iParam0 < -fParam1)
				return true;
			else if (iParam0 > fParam1)
				return true;
			return false;
		}

		private static int func_7449()
		{
			if (IsInputDisabled(2))
				return 251;
			return 210;
		}

		private static int func_7450()
		{
			if (IsInputDisabled(2))
				return 250;
			return 209;
		}

		private static int func_9341()
		{
			if (IsInputDisabled(2))
				return 204;
			return 217;
		}

		private static int func_9387()
		{
			if (IsInputDisabled(2))
				return 253;
			return 208;
		}

		private static int func_7660()
		{
			if (IsInputDisabled(2))
				return 252;
			return 207;
		}

		private static float func_909(float fParam0, float fParam1, float fParam2)
		{
			if (fParam0 > fParam2)
			{
				return fParam2;
			}
			else if (fParam0 < fParam1)
			{
				return fParam1;
			}
			return fParam0;
		}


		private static int func_7497(Vector3 vParam0, float fParam1, int iParam2, Vector3 vParam3, float fParam4)
		{
			Vector3 vVar0 = Vector3.Zero;
			Vector3 vVar1 = Vector3.Zero;
			Vector3 vVar2 = Vector3.Zero;
			Vector3 vVar3 = Vector3.Zero;

			GetModelDimensions((uint)iParam2, ref vVar0, ref vVar1);
			vVar2 = vVar1 - vVar0;
			vVar3.X = (vVar2.X / 2f) - vVar1.X;
			vVar3.Y = (vVar2.Y / 2f) - vVar1.Y;
			if (func_7498(vParam3, fParam4, vParam0.X + vVar3.X, vVar2, new	Vector3(fParam1), Vector3.Zero))
				return 1;
			return 0;
		}

		private static bool func_7498(Vector3 Param0, float uParam1, float fParam2, Vector3 Param3, Vector3 vParam4, Vector3 Param5, float uParam6 = 0, float fParam7 = 0)
		{
			float fVar0;
			float fVar1;
			float fVar2;
			float fVar3;
			float fVar4;
			float fVar5;
			float fVar6;
			float fVar7;
			float fVar8;
			float fVar9;
			float fVar10;

			fVar0 = (Cos(fParam7) * (Param0.X - Param3.X)) - (Sin(fParam7) * (Param0.Y - Param3.Y)) + Param3.X;
			fVar1 = (((Sin(fParam7) * (Param0.X - Param3.X)) + (Cos(fParam7) * (Param0.Y - Param3.Y))) + Param3.Y);
			fVar2 = (Param3.X - (Param5.X / 2f));
			fVar3 = (Param3.Y - (Param5.Y / 2f));
			fVar4 = (Param3.X + (Param5.X / 2f));
			fVar5 = (Param3.Y + (Param5.Y / 2f));
			fVar6 = func_909(fVar0, fVar2, fVar4);
			fVar7 = func_909(fVar1, fVar3, fVar5);
			fVar8 = (fVar0 - fVar6);
			fVar9 = (fVar1 - fVar7);
			fVar10 = ((fVar8 * fVar8) + (fVar9 * fVar9));
			return fVar10 < (fParam2 * fParam2);
		}
		#endregion

		#region Ticks

		private static async Task MoveCamera()
		{
			#region	MOVIMENTI TELECAMERA
			if (enteringCamera.Exists())
			{
				DisableInputGroup(2);
				float fVar0 = GetDisabledControlNormal(2, 218);
				float fVar1 = GetDisabledControlNormal(2, 219);
				float fVar2 = GetDisabledControlNormal(2, 220);
				float fVar3 = GetDisabledControlNormal(2, 221);
				float ltNorm = GetDisabledControlNormal(2, 252);
				float rtNorm = GetDisabledControlNormal(2, 253);
				if (!IsLookInverted())
				{
					fVar1 = -fVar1;
					fVar3 = -fVar3;
				}
				if (!IsInputDisabled(2))
				{
					N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
				}
				else if (Input.IsDisabledControlPressed(Control.CreatorLT) || Input.IsDisabledControlPressed(Control.CreatorRT))
				{
					N_0xc8b5c4a79cc18b94(enteringCamera.Handle);
				}
				if (IsInputDisabled(2))
				{
					fVar2 = GetDisabledControlUnboundNormal(2, 1) * 2;
					fVar3 = GetDisabledControlUnboundNormal(2, 2) * -1 * 2;
				}

				float xVectFwd = -fVar1 * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));
				float yVectFwd = fVar1 * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
				float xVectLat = fVar0 * (float)Math.Cos(Funzioni.Deg2rad(curRotation.Z));
				float yVectLat = fVar0 * (float)Math.Sin(Funzioni.Deg2rad(curRotation.Z));


				curRotation = new(fVar3 + enteringCamera.Rotation.X, 0, -fVar2 + enteringCamera.Rotation.Z);

				zoom = Vector3.Distance(cameraPosition, curLocation); // da migliorare assolutamente

				curLocation = new((cameraPosition + zoom * enteringCamera.CamForwardVector()).X, (cameraPosition + zoom * enteringCamera.CamForwardVector()).Y, Height);

				if (ltNorm > 0 || rtNorm > 0)
				{
					if (zoom < 351f)
						zoom += ltNorm;
					if (zoom > 16f)
						zoom -= rtNorm;
					cameraPosition = curLocation - zoom * enteringCamera.CamForwardVector();
				}

				if (IsDisabledControlPressed(2, func_7450()))
				{
					cameraPosition.Z += 1f;
					Height += 1f;
				}
				if (IsDisabledControlPressed(2, func_7449()))
				{
					cameraPosition.Z -= 1f;
					Height -= 1f;
				}

				cameraPosition.X += xVectFwd + xVectLat;
				cameraPosition.Y += yVectFwd + yVectLat;


				float z = 0;
				GetGroundZFor_3dCoord(curLocation.X, curLocation.Y, Height + 300, ref z, false);
				if (Height <= z + 0.3f)
					Height = z + 0.3f;
				cross.Position = curLocation;
				cross.Rotation = new(0, 0, curRotation.Z);
				placeMarker.Position = curLocation + new Vector3(0, 0, 0.1f);
				placeMarker.Draw();

				if (curRotation.X >= -11.5f)
					curRotation.X = -11.5f;
				if (curRotation.X <= -89.9f)
					curRotation.X = -89.9f;

				enteringCamera.Position = cameraPosition;
				enteringCamera.Rotation = curRotation;
			}
			#endregion

			#region MOVIMENTI PROP
			if (DummyProp != null)
			{
				DummyProp.Position = curLocation;
				if (IsDisabledControlPressed(2, 227))
				{
					if (rotationDummyType == RotationDummyType.Heading || rotationDummyType == RotationDummyType.Yaw)
					{
						dummyRot.Z -= 3f % 360f;
						if (dummyRot.Z < 360f)
							dummyRot.Z += 360f;
					}
					if (rotationDummyType == RotationDummyType.Pitch)
					{
						dummyRot.X -= 3f % 360f;
						if (dummyRot.X < 360f)
							dummyRot.X += 360f;
					}
					if (rotationDummyType == RotationDummyType.Roll)
					{
						dummyRot.Y -= 3f % 360f;
						if (dummyRot.Y < 360f)
							dummyRot.Y += 360f;
					}
				}
				if (IsDisabledControlPressed(2, 226))
				{
					if (rotationDummyType == RotationDummyType.Heading || rotationDummyType == RotationDummyType.Yaw)
					{
						dummyRot.Z += 3f % 360f;
						if (dummyRot.Z > 360f)
							dummyRot.Z -= 360f;
					}
					if (rotationDummyType == RotationDummyType.Pitch)
					{
						dummyRot.X += 3f % 360f;
						if (dummyRot.X > 360f)
							dummyRot.X -= 360f;
					}
					if (rotationDummyType == RotationDummyType.Roll)
					{
						dummyRot.Y += 3f % 360f;
						if (dummyRot.Y > 360f)
							dummyRot.Y -= 360f;
					}
				}

				if (rotationDummyType != RotationDummyType.Heading)
				{
					Vector3 vVar2 = Vector3.Zero;
					Vector3 vVar3 = Vector3.Zero;
					Vector3 vVar6 = new(-90f, 0f, 0f);
					Vector3 vVar7 = new (1f, 1f, 1f );
					float fVar8 = 1.25f;
					float fVar9 = 0;
					GetModelDimensions((uint)DummyProp.Model.Hash, ref vVar2, ref vVar3);
					Vector3 vVar4 = vVar3 - vVar2;
					Vector3 vVar10 = new(Math.Abs(vVar4.X), Math.Abs(vVar4.Y), Math.Abs(vVar4.Z));
					if (vVar10.X > vVar10.Y && vVar10.X > vVar10.Z)
					{
						fVar9 = vVar10.X;
					}
					else if (vVar10.Y > vVar10.X && vVar10.Y > vVar10.Z)
					{
						fVar9 = vVar10.Y;
					}
					else if (vVar10.Z > vVar10.X && vVar10.Z > vVar10.Y)
					{
						fVar9 = vVar10.Z;
					}
					if (fVar9 > 10f)
					{
						float fVar11 = (fVar9 / 10f);
						vVar7 *= new Vector3(fVar11);
						fVar8 *= fVar11;
					}
					
					if (rotationDummyType == RotationDummyType.Pitch)
					{
						Vector3 vVar5 = GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 1f, 0f, 0f) - curLocation;
						World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, vVar3.X + fVar8, 0f, 0f), vVar5, vVar6, vVar7, Colors.Purple);
						World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, vVar2.X - fVar8, 0f, 0f), -vVar5, vVar6, vVar7, Colors.Purple);
					}
					if (rotationDummyType == RotationDummyType.Roll)
					{
						Vector3 vVar5 = GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 1f, 0f) - curLocation;
						World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, vVar3.Y + fVar8, 0f), vVar5, vVar6, vVar7, Colors.Purple);
						World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, vVar2.Y - fVar8, 0f), -vVar5, vVar6, vVar7, Colors.Purple);
					}
					if (rotationDummyType == RotationDummyType.Yaw)
					{
						Vector3 vVar5 = GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 0f, 1f) - curLocation;
						World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 0f, vVar3.Z + fVar8), vVar5, vVar6, vVar7, Colors.Purple);
						World.DrawMarker(MarkerType.UpsideDownCone, GetOffsetFromEntityInWorldCoords(DummyProp.Handle, 0f, 0f, vVar2.Z - fVar8), -vVar5, vVar6, vVar7, Colors.Purple);
					}
				}
				DummyProp.Rotation = dummyRot;
			}

			#endregion
			// PER LO SNAP CERCARE "Creator_Snap", "DLC_Stunt_Race_Frontend_Sounds"
		}
		#endregion
	}
}