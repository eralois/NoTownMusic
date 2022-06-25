using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace NoTownMusic;

[Label("$Mods.NoTownMusic.Config.Label")]
public class NoTownMusicConfig : ModConfig {
	public static NoTownMusicConfig Instance { get; private set; }

	public override ConfigScope Mode => ConfigScope.ServerSide;

	public override void OnLoaded() {
		Instance = this;
	}

	[Label("$Mods.NoTownMusic.Config.DisableTownMusicBoxRecipesLabel")]
	[Tooltip("$Mods.NoTownMusic.Config.DisableTownMusicBoxRecipesTooltip")]
	[DefaultValue(true)]
	[ReloadRequired]
	public bool TownMusicBoxRecipes { get; set; }
}
