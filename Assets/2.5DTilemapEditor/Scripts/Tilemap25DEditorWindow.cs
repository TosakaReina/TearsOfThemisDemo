#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using UnityEditor.ShortcutManagement;

namespace TilemapEditor25D
{
	public class Tilemap25DEditorWindow : EditorWindow
	{
		private static Tilemap25DEditorWindow instance;

		public static Tilemap25DEditorWindow Instance {
			get { return instance; }
			private set { instance = value; }
		}

		private Vector2 scrollPosition;
		private int selectedTile = 0;
		private int activeTileMap = 0;
		private int paletteScale = 1;
		private TilePalette tilePalette = null;

		private SerializedObject serializedObjectSettings;
		private EditorData data;

		[MenuItem("Tools/2.5DTilemapEditor/2.5DTilemapEditor Window")]
		private static void Init()
		{
			GetWindow<Tilemap25DEditorWindow>("2.5D Tilemap Editor");
		}

        private void OnEnable()
        {
			Instance = this;
		}


        private void OnGUI()
		{
			if (serializedObjectSettings == null)
			{
				serializedObjectSettings = new SerializedObject(ScriptableSingleton<EditorData>.instance.Settings);
			}
			if (data == null)
			{
				data = ScriptableSingleton<EditorData>.instance;
			}

			SetupEditorWindow();
		}

		private void SetupEditorWindow()
		{
			int selectedTool = 0;
			Tilemap25D[] tileMaps = FindObjectsOfType(typeof(Tilemap25D)) as Tilemap25D[];

			EditorGUI.BeginChangeCheck();

			// Setup the tools
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Tools");

				GUIContent[] tools = {
					new GUIContent("Default"),
					new GUIContent("Pencil", "ShortcutKey: " + ShortcutManager.instance.GetShortcutBinding("2.5DTilemapEditor/Pencil").ToString()),
					new GUIContent("Eraser", "ShortcutKey: " + ShortcutManager.instance.GetShortcutBinding("2.5DTilemapEditor/Eraser").ToString()),
					new GUIContent("Bucket", "ShortcutKey: " + ShortcutManager.instance.GetShortcutBinding("2.5DTilemapEditor/Bucket").ToString())
				};
				selectedTool = GUILayout.Toolbar((int)data.SelectedTool, tools);
			}

			// Setup the active 2.5D tilemap
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Active 2.5D Tilemap");
				int cnt = 1;
				string[] tilemapNames = tileMaps.Select(t => cnt++ + ": " + t.name).ToArray();
				activeTileMap = EditorGUILayout.Popup(Mathf.Clamp(activeTileMap, 0, tilemapNames.Length - 1), tilemapNames);

				if (tileMaps.Length > 0 && (data.ActiveTilemap == null || data.ActiveTilemap != tileMaps[activeTileMap]))
				{
					data.ActiveTilemap = tileMaps[activeTileMap];
				}
			}

			EditorGUILayout.Space();

			// Setup the tile palette object field
			tilePalette = EditorGUILayout.ObjectField("Tile Palette", data.Settings.TilePalette, typeof(TilePalette), false) as TilePalette;

			// Setup the tile palette
			if (tilePalette != null)
			{
				if (tilePalette?.Prefabs?.Count > 0)
				{
					// Get prefab images
					Texture[] prefabImages = tilePalette.Prefabs.Select(p => AssetPreview.GetAssetPreview(p)).ToArray();

					// Setup the tile palette selection grid
					using (EditorGUILayout.ScrollViewScope scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition))
					{
						scrollPosition = scrollView.scrollPosition;

						selectedTile = GUILayout.SelectionGrid(
							selectedTile,
							prefabImages,
							data.Settings.PaletteScale,
							GUILayout.Width(position.width - 19),
							GUILayout.Height(position.width / data.Settings.PaletteScale * Mathf.CeilToInt((float)prefabImages.Length / data.Settings.PaletteScale))
						);

						if (tilePalette != data.Settings.TilePalette)
						{
							selectedTile = 0;
						}
					}

					// Setup the tile palette information
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Label(tilePalette.Prefabs[selectedTile].name, GUILayout.Width(position.width - 50));
						paletteScale = (int)GUILayout.HorizontalSlider(data.Settings.PaletteScale, 1, 10);
					}
				}
			}

			// Update data
			if (EditorGUI.EndChangeCheck())
			{
				data.SelectedTool = (PaintTool)Enum.ToObject(typeof(PaintTool), selectedTool);

				if (tilePalette?.Prefabs?.Count > 0)
				{
					data.SelectedTile = tilePalette.Prefabs[selectedTile];
					data.Settings.PaletteScale = paletteScale;
				}

				data.Settings.TilePalette = tilePalette;

				EditorUtility.SetDirty(data);
			}
		}
	}
}

#endif