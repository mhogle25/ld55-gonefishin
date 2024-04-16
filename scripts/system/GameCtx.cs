using System.Collections.Generic;
using System.IO;
using BFO.G.Utilities;
using Godot;

namespace BFO.G.GoneFishin;

public partial class ColorInfo : Resource 
{
	public ColorInfo(string code, string name) 
	{
		this.Code = code;
		this.Name = name;
	}
	
	public string Code { get; init; } 
	public string Name { get; init;}
	
	public string GetCode() => this.Code;
	public string GetName() => this.Name;
}

public partial class GameCtx : Node
{
	const string COLOR_CODES = "#0048BA #B0BF1A #7CB9E8 #F0F8FF #DB2D43 #C46210 #EED9C4 #9F2B68 #F19CBB #AB274F #3B7A57 #FFBF00 #9966CC #3DDC84 #C88A65 #665D1E #915C83 #841B2D #FAEBD7 #FBCEB1 #00FFFF #7FFFD4 #D0FF14 #4B6F44 #E9D66B #B2BEB5 #FF9966 #FDEE00 #007FFF #89CFF0 #A1CAF1 #F4C2C2 #FEFEFA #FF91AF #FAE7B5 #DA1884 #7C0A02 #848482 #BCD4E6 #9F8170 #F5F5DC #2E5894 #9C2542 #FFE4C4 #3D2B1F #967117 #CAE00D #000000 #3D0C02 #54626F #3B3C36 #BFAFB2 #FFEBCD #A57164 #318CE7 #ACE5EE #660000 #0000FF #A2A2D0 #5DADEC #126180 #8A2BE2 #5072A7 #3C69E7 #DE5D83 #79443B #E3DAC9 #CB4154 #D891EF #004225 #CD7F32 #964B00 #AF6E4D #7BB661 #FFC680 #800020 #DEB887 #A17A74 #CC5500 #E97451 #8A3324 #BD33A4 #702963 #5F9EA0 #91A3B0 #006B3C #ED872D #A67B5B #4B3621 #A3C1AD #C19A6B #EFBBCC #FFFF99 #FFEF00 #E4717A #C41E3A #00CC99 #960018 #FFA6C9 #B31B1B #56A0D3 #ED9121 #703642 #C95A49 #ACE1AF #B2FFFF #DE3163 #007BA7 #2A52BE #6D9BC3 #F7E7CE #F1DDCF #36454F #E68FAC #FFB7C5 #954535 #E23D28 #98817B #E34234 #CD607E #E4D00A #9FA91F #7F1734 #6F4E37 #B9D9EB #F88379 #8C92AC #B87333 #AD6F69 #CB6D51 #996666 #FF3800 #FF7F50 #F88379 #893F45 #FBEC5D #6495ED #FFF8DC #2E2D88 #FFF8E7 #81613C #FFBCD9 #FFFDD0 #DC143C #F5F5F5 #00FFFF #58427C #FFD300 #F56FA1 #FED85D #654321 #5D3954 #008B8B #536878 #B8860B #1A2421 #BDB76B #483C32 #8B008B #556B2F #FF8C00 #9932CC #301934 #8B0000 #E9967A #8FBC8F #3C1414 #8CBED6 #483D8B #2F4F4F #177245 #00CED1 #9400D3 #555555 #DA3287 #FAD6A5 #B94E48 #004B49 #FF1493 #FF9933 #00BFFF #4A646C #7E5E60 #1560BD #2243B6 #C19A6B #EDC9AF #696969 #1E90FF #4A412A #00009C #EFDFBB #555D50 #C2B280 #1B1B1B #614051 #F0EAD6 #CCFF00 #BF00FF #8F00FF #50C878 #6C3082 #B48395 #AB4B52 #CC474B #563C5C #00FF40 #96C8A2 #C19A6B #801818 #B53389 #DE5285 #E5AA70 #4F7942 #6C541E #FF5470 #683068 #B22222 #CE2029 #E25822 #EEDC82 #A2006D #FFFAF0 #E936A7 #FF00FF #E48400 #DCDCDC #E49B0F #007F66 #F8F8FF #6082B6 #AB92B3 #85754E #996515 #FCC200 #FFDF00 #DAA520 #00573F #676767 #A8E4A0 #00FF00 #1164B4 #A7F432 #6EAEA1 #E9D66B #3FFF00 #DA9100 #FF7A00 #DF73FF #AA98A9 #006DB0 #49796B #FF1DCE #FF69B4 #355E3B #71A6D2 #319177 #ED2939 #B2EC5D #4C516D #6A5DFF #00416A #B3446C #F4F0EC #FFFFF0 #F8DE7E #A50B5E #343434 #F4CA16 #BDDA57 #29AB87 #4CBB17 #3AB09E #E8F48C #882D17 #E79FC4 #6B4423 #D6CADD #26619C #FFFF66 #A9BA9D #CF1020 #CCCCFF #FFF0F5 #C4C3D0 #7CFC00 #FFF700 #FFFACD #CCA01D #FDFF00 #F6EABE #FFF44F #545AA7 #ADD8E6 #F08080 #93CCEA #E0FFFF #C8AD7F #FAFAD2 #D3D3D3 #90EE90 #FED8B1 #C5CBE1 #FFB6C1 #FFA07A #20B2AA #87CEFA #778899 #B0C4DE #FFFFE0 #C8A2C8 #32CD32 #195905 #FAF0E6 #DECC9C #DE6FA1 #6CA0DC #674C47 #6699CC #CC3336 #FF00FF #9F4576 #AAF0D1 #F2E8D7 #C04000 #FBEC5D #6050DC #0BDA51 #979AAA #F37A48 #FDBE02 #FF8243 #74C365 #EAA221 #E0B0FF #915F6D #EF98AA #47ABCC #30BFBF #ACACE6 #5E8C31 #D9E650 #733380 #D92121 #A63A79 #FAFA37 #F2BA49 #4C9141 #73C2FB #66DDAA #0000CD #E2062C #AF4035 #F3E5AB #BA55D3 #9370DB #3CB371 #7B68EE #00FA9A #48D1CC #C71585 #F8B878 #F8DE7E #FEBAAD #D3AF37 #0A7E8C #9C7C38 #E4007C #7ED4E6 #8DD9CC #8B72BE #8B8680 #4D8C57 #ACBF60 #D982B5 #E58E73 #A55353 #FFEB00 #ECB176 #702670 #191970 #FFC40C #FFDAE9 #E3F988 #36747D #F5E050 #3EB489 #F5FFFA #98FF98 #BBB477 #FFE4E1 #967117 #FF948E #8DA399 #8A9A5B #30BA8F #C54B8C #FFDB58 #317873 #D65282 #AD4379 #000080 #4666FF #39FF14 #FE4164 #214FC6 #727472 #A4DDED #E9FFDB #CC7722 #43302E #CFB53B #FDF5E6 #796878 #673147 #C08081 #848482 #808000 #B5B35C #9AB973 #353839 #A8C3BC #B784A7 #FF7F00 #FF9F00 #FF681F #FA5B3D #F5BD1F #DA70D6 #F2BDCD #FF6E4A #4A0000 #682860 #BED3E5 #9BC4E2 #ED7A9B #FADADD #ECEBBD #78184A #009B7D #FFEFD5 #E63E62 #F1E9D2 #50C878 #DEA5A4 #800080 #1F005E #FFE5B4 #FFDAB9 #D1E231 #B768A2 #CCCCFF #E12C2C #EC5800 #8BA8B7 #DF00FF #000F89 #123524 #2E2787 #C30B4E #01796F #2A2F23 #FFC0CB #FFDDF4 #D8B2D1 #F78FA7 #93C572 #E5E4E2 #8E4585 #5946B2 #5DA493 #86608E #BE4F62 #B0E0E6 #701C1C #DF00FF #CC8899 #FF7518 #6A0DAD #9678B6 #4E5180 #FE4EDA #9A4EAE #436B95 #E8CCD7 #A6A6A6 #8E3A59 #242124 #E30B5D #915F6D #B3446C #D68A59 #826644 #FF33CC #E3256B #8D4E85 #FF0000 #FF5349 #E40078 #FD3A4A #C71585 #A45A52 #002387 #777696 #00CCCC #8A7F80 #838996 #FF007F #F9429E #9E5E6F #674846 #E32636 #FF66CC #ED7A9B #C21E56 #905D5D #AB4E52 #65000B #D40000 #BC8F8F #7851A9 #FADA5E #CE4676 #D10056 #E0115F #9B111E #A81C07 #80461B #B7410E #DA2C43 #8B4513 #FF7800 #EED202 #F4C430 #BCB88A #FA8072 #FF91A4 #C2B280 #967117 #F4A460 #507D2A #0F52BA #0067A5 #FF2400 #FFD800 #66FF66 #2E8B57 #612086 #59260B #FFF5EE #764374 #FFBA00 #704214 #8A795D #778BA5 #009E60 #8FD400 #D98695 #5FA778 #FC0FC0 #882D17 #C0C0C0 #ACACAC #C4AEAD #BFC1C2 #CB410B #FF3855 #FFDB00 #007474 #87CEEB #CF71AF #6A5ACD #708090 #299617 #C84186 #100C08 #FFFAFA #893843 #757575 #1D2951 #A7FC00 #87FF2A #00FF7F #007BB8 #4682B4 #CC33CC #FADA5E #E4D96F #FA5053 #FF9361 #33CC33 #914E75 #FFCC33 #E3AB57 #FAD6A5 #CF6BA9 #A83731 #D2B48C #F28500 #E4717A #FB4D46 #483C32 #8B8589 #D0F0C0 #F4C2C2 #008080 #367588 #00FFBF #CF3476 #E2725B #D8BFD8 #DE6FA1 #FC89AC #0ABAB5 #DBD7D2 #EEE600 #FF6347 #86A1A9 #00755E #2D68C4 #1C05B3 #3E8EDE #DEAA88 #40E0D0 #00FFEF #A0D6B4 #8A9A5B #8A496B #3F00FF #4166F5 #FF6FFF #FC6C85 #635147 #FFDDCA #009EDB #FFFF66 #AE2029 #AFDBF5 #F3E5AB #F38FA9 #C5B358 #C80815 #43B3AE #E34234 #D9381E #A020F0 #8F00FF #324AB2 #F75394 #40826D #009698 #9F1D35 #00CCFF #FFA089 #9F00FF #CEFF00 #004242 #189BCC #F5DEB3 #FFFFFF #A2ADD0 #D470A2 #FF43A4 #FC6C85 #A75502 #722F37 #673147 #FF007C #56887D #C9A0DC #C19A6B #738678 #EEED09 #F1B42F #FFFF00 #9ACD32 #FFAE42 #FFF700 #2E5090 #0014A8 #2C1608 #39A78E #006c8d #ff7fff";
	const string COLOR_NAMES = "Absolute Zero~Acid green~Aero~Alice blue~Alizarin~Alloy orange~Almond~Amaranth deep purple~Amaranth pink~Amaranth purple~Amazon~Amber~Amethyst~Android green~Antique brass~Antique bronze~Antique fuchsia~Antique ruby~Antique white~Apricot~Aqua~Aquamarine~Arctic lime~Artichoke green~Arylide yellow~Ash gray~Atomic tangerine~Aureolin~Azure~Baby blue~Baby blue eyes~Baby pink~Baby powder~Baker-Miller pink~Banana Mania~Barbie Pink~Barn red~Battleship grey~Beau blue~Beaver~Beige~B'dazzled blue~Big dip o’ruby~Bisque~Bistre~Bistre brown~Bitter lemon~Black~Black bean~Black coral~Black olive~Black Shadows~Blanched almond~Blast-off bronze~Bleu de France~Blizzard blue~Blood red~Blue~Blue bell~Blue jeans~Blue sapphire~Blue-violet~Blue yonder~Bluetiful~Blush~Bole~Bone~Brick red~Bright lilac~British racing green~Bronze~Brown~Brown sugar~Bud green~Buff~Burgundy~Burlywood~Burnished brown~Burnt orange~Burnt sienna~Burnt umber~Byzantine~Byzantium~Cadet blue~Cadet grey~Cadmium green~Cadmium orange~Café au lait~Café noir~Cambridge blue~Camel~Cameo pink~Canary~Canary yellow~Candy pink~Cardinal~Caribbean green~Carmine~Carnation pink~Carnelian~Carolina blue~Carrot orange~Catawba~Cedar Chest~Celadon~Celeste~Cerise~Cerulean~Cerulean blue~Cerulean frost~Champagne~Champagne pink~Charcoal~Charm pink~Cherry blossom pink~Chestnut~Chili red~Cinereous~Cinnabar~Cinnamon Satin~Citrine~Citron~Claret~Coffee~Columbia Blue~Congo pink~Cool grey~Copper~Copper penny~Copper red~Copper rose~Coquelicot~Coral~Coral pink~Cordovan~Corn~Cornflower blue~Cornsilk~Cosmic cobalt~Cosmic latte~Coyote brown~Cotton candy~Cream~Crimson~Cultured Pearl~Cyan~Cyber grape~Cyber yellow~Cyclamen~Dandelion~Dark brown~Dark byzantium~Dark cyan~Dark electric blue~Dark goldenrod~Dark jungle green~Dark khaki~Dark lava~Dark magenta~Dark olive green~Dark orange~Dark orchid~Dark purple~Dark red~Dark salmon~Dark sea green~Dark sienna~Dark sky blue~Dark slate blue~Dark slate gray~Dark spring green~Dark turquoise~Dark violet~Davy's grey~Deep cerise~Deep champagne~Deep chestnut~Deep jungle green~Deep pink~Deep saffron~Deep sky blue~Deep Space Sparkle~Deep taupe~Denim~Denim blue~Desert~Desert sand~Dim gray~Dodger blue~Drab dark brown~Duke blue~Dutch white~Ebony~Ecru~Eerie black~Eggplant~Eggshell~Electric lime~Electric purple~Electric violet~Emerald~Eminence~English lavender~English red~English vermillion~English violet~Erin~Eton blue~Fallow~Falu red~Fandango~Fandango pink~Fawn~Fern green~Field drab~Fiery rose~Finn~Firebrick~Fire engine red~Flame~Flax~Flirt~Floral white~Frostbite~Fuchsia~Fulvous~Gainsboro~Gamboge~Generic viridian~Ghost white~Glaucous~Glossy grape~Gold Fusion~Golden brown~Golden poppy~Golden yellow~Goldenrod~Gotham green~Granite gray~Granny Smith apple~Green~Green-blue~Green Lizard~Green Sheen~Hansa yellow~Harlequin~Harvest gold~Heat Wave~Heliotrope~Heliotrope gray~Honolulu blue~Hooker's green~Hot magenta~Hot pink~Hunter green~Iceberg~Illuminating emerald~Imperial red~Inchworm~Independence~Indigo~Indigo dye~Irresistible~Isabelline~Ivory~Jasmine~Jazzberry jam~Jet~Jonquil~June bud~Jungle green~Kelly green~Keppel~Key lime~Kobe~Kobi~Kobicha~Languid lavender~Lapis lazuli~Laser lemon~Laurel green~Lava~Lavender blue~Lavender blush~Lavender gray~Lawn green~Lemon~Lemon chiffon~Lemon curry~Lemon glacier~Lemon meringue~Lemon yellow~Liberty~Light blue~Light coral~Light cornflower blue~Light cyan~Light French beige~Light goldenrod yellow~Light gray~Light green~Light orange~Light periwinkle~Light pink~Light salmon~Light sea green~Light sky blue~Light slate gray~Light steel blue~Light yellow~Lilac~Lime green~Lincoln green~Linen~Lion~Liseran purple~Little boy blue~Liver~Livid~Madder Lake~Magenta~Magenta haze~Magic mint~Magnolia~Mahogany~Maize~Majorelle blue~Malachite~Manatee~Mandarin~Mango~Mango Tango~Mantis~Marigold~Mauve~Mauve taupe~Mauvelous~Maximum blue~Maximum blue green~Maximum blue purple~Maximum green~Maximum green yellow~Maximum purple~Maximum red~Maximum red purple~Maximum yellow~Maximum yellow red~May green~Maya blue~Medium aquamarine~Medium blue~Medium candy apple red~Medium carmine~Medium champagne~Medium orchid~Medium purple~Medium sea green~Medium slate blue~Medium spring green~Medium turquoise~Medium violet-red~Mellow apricot~Mellow yellow~Melon~Metallic gold~Metallic Seaweed~Metallic Sunburst~Mexican pink~Middle blue~Middle blue green~Middle blue purple~Middle grey~Middle green~Middle green yellow~Middle purple~Middle red~Middle red purple~Middle yellow~Middle yellow red~Midnight~Midnight blue~Mikado yellow~Mimi pink~Mindaro~Ming~Minion yellow~Mint~Mint cream~Mint green~Misty moss~Misty rose~Mode beige~Mona Lisa~Morning blue~Moss green~Mountain Meadow~Mulberry~Mustard~Myrtle green~Mystic~Mystic maroon~Navy blue~Neon blue~Neon green~Neon fuchsia~New Car~Nickel~Non-photo blue~Nyanza~Ochre~Old burgundy~Old gold~Old lace~Old lavender~Old mauve~Old rose~Old silver~Olive~Olive green~Olivine~Onyx~Opal~Opera mauve~Orange~Orange peel~Orange-red~Orange soda~Orange-yellow~Orchid~Orchid pink~Outrageous Orange~Oxblood~Palatinate purple~Pale aqua~Pale cerulean~Pale Dogwood~Pale pink~Pale spring bud~Pansy purple~Paolo Veronese green~Papaya whip~Paradise pink~Parchment~Paris Green~Pastel pink~Patriarch~Paua~Peach~Peach puff~Pear~Pearly purple~Periwinkle~Permanent Geranium Lake~Persimmon~Pewter Blue~Phlox~Phthalo blue~Phthalo green~Picotee blue~Pictorial carmine~Pine green~Pine green~Pink~Pink lace~Pink lavender~Pink Sherbet~Pistachio~Platinum~Plum~Plump Purple~Polished Pine~Pomp and Power~Popstar~Powder blue~Prune~Psychedelic purple~Puce~Pumpkin~Purple~Purple mountain majesty~Purple navy~Purple pizzazz~Purpureus~Queen blue~Queen pink~Quick Silver~Quinacridone magenta~Raisin black~Raspberry~Raspberry glacé~Raspberry rose~Raw sienna~Raw umber~Razzle dazzle rose~Razzmatazz~Razzmic Berry~Red~Red-orange~Red-purple~Red Salsa~Red-violet~Redwood~Resolution blue~Rhythm~Robin egg blue~Rocket metallic~Roman silver~Rose~Rose bonbon~Rose Dust~Rose ebony~Rose madder~Rose pink~Rose Pompadour~Rose red~Rose taupe~Rose vale~Rosewood~Rosso corsa~Rosy brown~Royal purple~Royal yellow~Ruber~Rubine red~Ruby~Ruby red~Rufous~Russet~Rust~Rusty red~Saddle brown~Safety orange~Safety yellow~Saffron~Sage~Salmon~Salmon pink~Sand~Sand dune~Sandy brown~Sap green~Sapphire~Sapphire blue~Scarlet~School bus yellow~Screamin' Green~Sea green~Seance~Seal brown~Seashell~Secret~Selective yellow~Sepia~Shadow~Shadow blue~Shamrock green~Sheen green~Shimmering Blush~Shiny Shamrock~Shocking pink~Sienna~Silver~Silver chalice~Silver pink~Silver sand~Sinopia~Sizzling Red~Sizzling Sunrise~Skobeloff~Sky blue~Sky magenta~Slate blue~Slate gray~Slimy green~Smitten~Smoky black~Snow~Solid pink~Sonic silver~Space cadet~Spring bud~Spring Frost~Spring green~Star command blue~Steel blue~Steel pink~Stil de grain yellow~Straw~Strawberry~Strawberry Blonde~Strong Lime Green~Sugar Plum~Sunglow~Sunray~Sunset~Super pink~Sweet Brown~Tan~Tangerine~Tango pink~Tart Orange~Taupe~Taupe gray~Tea green~Tea rose~Teal~Teal blue~Technobotanica~Telemagenta~Terra cotta~Thistle~Thulian pink~Tickle Me Pink~Tiffany Blue~Timberwolf~Titanium yellow~Tomato~Tourmaline~Tropical rainforest~True Blue~Trypan Blue~Tufts blue~Tumbleweed~Turquoise~Turquoise blue~Turquoise green~Turtle green~Twilight lavender~Ultramarine~Ultramarine blue~Ultra pink~Ultra red~Umber~Unbleached silk~United Nations blue~Unmellow yellow~Upsdell red~Uranian blue~Vanilla~Vanilla ice~Vegas gold~Venetian red~Verdigris~Vermilion~Vermilion~Veronica~Violet~Violet-blue~Violet-red~Viridian~Viridian green~Vivid burgundy~Vivid sky blue~Vivid tangerine~Vivid violet~Volt~Warm black~Weezy Blue~Wheat~White~Wild blue yonder~Wild orchid~Wild Strawberry~Wild watermelon~Windsor tan~Wine~Wine dregs~Winter Sky~Wintergreen Dream~Wisteria~Wood brown~Xanadu~Xanthic~Xanthous~Yellow~Yellow-green~Yellow Orange~Yellow Sunshine~YInMn Blue~Zaffre~Zinnwaldite brown~Zomp~Copacabana~pinkpi pink";
	public const string PATH = "/root/GameCtx";
	
	private const string SAVE_ID = "save_01";
	private const string SAVE_DIR = "save";
	private readonly FileManager saveFileManager = new(FilePath.User, SAVE_DIR, "json");
	
	private readonly List<ColorInfo> colorInfos = new();
	private readonly RandomNumberGenerator rng = new();
	
	private SaveData saveData = null;
	private AudioStreamPlayer globalMusic = null;

	public override void _Ready()
	{	
		string savePath = $"{FileManager.USER_PATH}{SAVE_DIR}";
		
		if (!DirAccess.DirExistsAbsolute(savePath))
			DirAccess.MakeDirRecursiveAbsolute(savePath);
			
		string[] codesList = COLOR_CODES.Split(' ');
		string[] namesList = COLOR_NAMES.Split('~');
			
		if (codesList.Length != namesList.Length)
			BFCtx.PrintErr("Color files must have the same number of lines");
			
		for (int i = 0; i < codesList.Length; i++) 
		{
			string code = codesList[i];
			string name = namesList[i];
			this.colorInfos.Add(new ColorInfo(code, name));
		}
			
		AudioStreamOggVorbis stream = GD.Load<AudioStreamOggVorbis>("res://assets/music/idle - 123.ogg");
		this.globalMusic = new();
		AddChild(this.globalMusic);
		this.globalMusic.Stream = stream;
		
		PlayMusic();
			
		GetSaveData();
	}

	public void ResetSave() 
	{
		this.saveData = new();
		SaveDataToDisk();
	}
	
	public int GetDemonCount() => 
		GetSaveData().GetDemonCount();
	
	public bool GetEncounteredBoss() =>
		GetSaveData().GetEncounteredBoss();
		
	public IEnumerable<SummonData> GetSummons() =>
		GetSaveData().GetSummons();
	
	public void AddSummon(SummonData summon) 
	{
		GetSaveData().AddSummon(summon);
		SaveDataToDisk();
	}
	
	public void IncrementDemonCount() 
	{
		GetSaveData().IncrementDemonCount();
		SaveDataToDisk();
	}
	
	public void DecrementDemonCount() 
	{
		GetSaveData().DecrementDemonCount();
		SaveDataToDisk();
	}
	
	public void EncounterBoss() => 
		GetSaveData().EncounterBoss();
		
	public float GetMusicVol() => this.saveData.MusicVol; 
	public float GetMasterVol() => this.saveData.MasterVol;
	public bool GetLimitMouseMovement() => this.saveData.LimitMouseMovement;
	public int GetDifficultySelected() => this.saveData.DifficultySelected; 
	public bool GetFullscreen() => this.saveData.Fullscreen; 
	public void SetMusicVol(float value) 
	{
		this.saveData.MusicVol = value; 
		SaveDataToDisk();
	}
	public void SetMasterVol(float value) 
	{
		this.saveData.MasterVol = value; 
		SaveDataToDisk();
	}
	public void SetLimitMouseMovement(bool value)
	{
		this.saveData.LimitMouseMovement = value; 
		SaveDataToDisk();
	}
	public void SetDifficultySelected(int value) 
	{
		this.saveData.DifficultySelected = value;
		SaveDataToDisk();
	}
	public void SetFullscreen(bool value) 
	{
		this.saveData.Fullscreen = value;
		SaveDataToDisk();
	}

	public ColorInfo GetRandColorInfo() 
	{
		return this.colorInfos[this.rng.RandiRange(0, this.colorInfos.Count - 1)];
	}
	
	public void PlayMusic() 
	{
		this.globalMusic.Play();
	}
	
	public void PauseMusic() 
	{
		this.globalMusic.Stop();
	}

	
	private SaveData GetSaveData() 
	{
		this.saveData ??= JSON
			.Deserialize<SaveData>(this.saveFileManager.Read(SAVE_ID))
			.ReduceTo(() => 
			{
				SaveData newSave = new();
				SaveDataToDisk(newSave);
				BFCtx.Print("Created new save file");
				return newSave;
			});
		return this.saveData;
	}
	
	private void SaveDataToDisk() => SaveDataToDisk(GetSaveData());
	
	private void SaveDataToDisk(SaveData save) 
	{
		string data = JSON.Serialize(save);
		
		if (string.IsNullOrWhiteSpace(data))
			return;
		
		this.saveFileManager.Write(SAVE_ID, data);
	}
}
