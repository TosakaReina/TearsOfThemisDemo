#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace TilemapEditor25D
{
	public class Shortcut
	{
		[Shortcut("2.5DTilemapEditor/Pencil", KeyCode.B)]
		private static void Pencil()
		{
			SelectTool(PaintTool.Pencil);
		}

		[Shortcut("2.5DTilemapEditor/Eraser", KeyCode.D)]
		private static void Eraser()
		{
			SelectTool(PaintTool.Eraser);
		}

		[Shortcut("2.5DTilemapEditor/Bucket", KeyCode.G)]
		private static void Bucket()
		{
			SelectTool(PaintTool.Bucket);
		}


		private static void SelectTool(PaintTool tool)
		{
			ScriptableSingleton<EditorData>.instance.SelectedTool = tool;
			EditorUtility.SetDirty(ScriptableSingleton<EditorData>.instance);
		}
	}
}
#endif