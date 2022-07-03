using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace NoTownMusic;

[Label("$Mods.NoTownMusic.Config.Label")]
public class NoTownMusicConfig : ModConfig {
	public static NoTownMusicConfig Instance;

	public override ConfigScope Mode => ConfigScope.ServerSide;

	[Label("$Mods.NoTownMusic.Config.DisableTownMusicBoxRecipesLabel")]
	[Tooltip("$Mods.NoTownMusic.Config.DisableTownMusicBoxRecipesTooltip")]
	[DefaultValue(true)]
	[ReloadRequired]
	public bool TownMusicBoxRecipes { get; set; }
}
