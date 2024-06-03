#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TilemapEditor25D
{
    public enum PaintTool
    {
        Default,
        Pencil,
        Eraser,
        Bucket
    }

    public class EditorData : ScriptableSingleton<EditorData>
    {
        private bool preIn2DMode;
        private Tool preTool;

        [SerializeField]
        private GameObject selectedTile;
        public GameObject SelectedTile {
            get { return selectedTile; }
            set { selectedTile = value; }
        }

        [SerializeField]
        private Vector3? prePaintPosition;
        public Vector3? PrePaintPosition {
            get { return prePaintPosition; }
            set { prePaintPosition = value; }
        }

        [SerializeField]
        private Vector3 currentGrid;
        public Vector3 CurrentGrid {
            get { return currentGrid; }
            set { currentGrid = value; }
        }

        
        [SerializeField]
        private PaintTool selectedTool = PaintTool.Default;

        public PaintTool SelectedTool {
            get { return selectedTool; }
            set {
                // Save information when using the unity tool
                if (selectedTool == PaintTool.Default)
                {
                    preTool = Tools.current;
                    preIn2DMode = SceneView.lastActiveSceneView.in2DMode;
                }

                selectedTool = value;

                // Setup as a 2.5D tile map tool
                if (selectedTool != PaintTool.Default)
                {
                    Tools.current = Tool.None;
                    SceneView.lastActiveSceneView.in2DMode = true;
                }
                // Setup as a unity tool
                else
                {
                    SceneView.lastActiveSceneView.in2DMode = preIn2DMode;
                    Tools.current = preTool;
                }

                // Apply to editor window
                Tilemap25DEditorWindow.Instance?.Repaint();

            }
        }

        [SerializeField]
        private Tilemap25D activeTilemap;
        public Tilemap25D ActiveTilemap {
            get { return activeTilemap; }
            set {
                activeTilemap = value;
                HandleUtility.Repaint();
            }
        }

        private const string Path = "Assets/2.5DTilemapEditor/EditorSettings/EditorSettings.asset";
        [SerializeField]
        private EditorSettings settings;
        public EditorSettings Settings {
            get {
                if (settings == null)
                {
                    settings = AssetDatabase.LoadAssetAtPath<EditorSettings>(Path);

                    //If no settings file exists, create one
                    if (settings == null)
                    {
                        settings = CreateInstance<EditorSettings>();
                        AssetDatabase.CreateAsset(settings, Path);
                        AssetDatabase.Refresh();
                    }
                    return settings;
                }
                else
                {
                    return settings;
                }
            }
            set { settings = value; }
        }
    }
}
# endif