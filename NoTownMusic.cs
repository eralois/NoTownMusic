using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NoTownMusic;

public class NoTownMusic : Mod {
	private static readonly MethodInfo _LocalPlayerGetMethod = typeof(Main).GetProperty("LocalPlayer", BindingFlags.Static | BindingFlags.Public).GetMethod;
	private static readonly FieldInfo _TownNPCsField = typeof(Player).GetField("townNPCs", BindingFlags.Instance | BindingFlags.Public);

	public override void Load() {
		if (_LocalPlayerGetMethod == null || _TownNPCsField == null)
			throw new Exception("Couldn't find get_LocalPlayer");

		if (_TownNPCsField == null)
			throw new Exception("Couldn't find _TownNPCsField");

		IL.Terraria.Main.UpdateAudio_DecideOnNewMusic += ILUpdateAudio_DecideOnNewMusic;
	}

	public override void Unload() {
		IL.Terraria.Main.UpdateAudio_DecideOnNewMusic -= ILUpdateAudio_DecideOnNewMusic;
	}

	private void ILUpdateAudio_DecideOnNewMusic(ILContext il) {
		var cursor = new ILCursor(il);

		if (cursor.TryGotoNext(MoveType.After, i => i.MatchCall(_LocalPlayerGetMethod), i => i.MatchLdfld(_TownNPCsField), i => i.MatchLdcR4(2f), i => i.MatchCgt(), i => i.MatchStloc(16))) {
			cursor.Emit(OpCodes.Ldc_I4_0);
			cursor.Emit(OpCodes.Stloc_S, cursor.Body.Variables[16]);
		} else {
			throw new Exception("Couldn't apply NoTownMusic hook");
		}
	}

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
