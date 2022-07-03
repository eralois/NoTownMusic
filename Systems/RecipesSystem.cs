using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NoTownMusic.Systems;

public class RecipesSystem : ModSystem {
	public override void AddRecipes() {
		const float MinimumTownNPCs = 2f;

		if (!NoTownMusicConfig.Instance.TownMusicBoxRecipes)
			return;

		Recipe.Create(ItemID.MusicBoxTownDay)
			.AddIngredient(ItemID.MusicBox)
			.AddTile(TileID.TinkerersWorkbench)
			.AddCondition(NetworkText.FromKey("Mods.NoTownMusic.RecipeConditions.InsideTownDuringDay"), (_) => Main.LocalPlayer.townNPCs >= MinimumTownNPCs && Main.dayTime)
			.Register();

		Recipe.Create(ItemID.MusicBoxTownNight)
			.AddIngredient(ItemID.MusicBox)
			.AddTile(TileID.TinkerersWorkbench)
			.AddCondition(NetworkText.FromKey("Mods.NoTownMusic.RecipeConditions.InsideTownDuringNight"), (_) => Main.LocalPlayer.townNPCs >= MinimumTownNPCs && !Main.dayTime)
			.Register();
	}
}
