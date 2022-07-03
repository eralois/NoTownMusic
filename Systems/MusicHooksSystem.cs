using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace NoTownMusic.Systems;

public class MusicHooksSystem : ModSystem {
	private static readonly MethodInfo _LocalPlayerGetMethod = typeof(Main).GetProperty("LocalPlayer", BindingFlags.Static | BindingFlags.Public).GetMethod;
	private static readonly FieldInfo _TownNPCsField = typeof(Player).GetField("townNPCs", BindingFlags.Instance | BindingFlags.Public);

	public override void Load() {
		if (_LocalPlayerGetMethod == null)
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
}
