#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace TilemapEditor25D
{
    [ExecuteInEditMode]
    public class Tilemap25D : MonoBehaviour
    {
        private EditorData data;

        [SerializeField]
        [Range(1, 100)]
        private int tileSize = 1;
        public int TileSize {
            get { return tileSize; }
            set { tileSize = value; }
        }

        // Create 1000 x 1000 grid
        private const int GridScale = 1000;

        private void OnEnable()
        {
            data = ScriptableSingleton<EditorData>.instance;

            SceneView.duringSceneGui += OnSceneGUI;
            Tilemap25DEditorWindow.Instance?.Repaint();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            Tilemap25DEditorWindow.Instance?.Repaint();
        }


        private void OnDrawGizmos()
        {
            // Process only when active
            if (!EditorWindow.HasOpenInstances<Tilemap25DEditorWindow>() || data.ActiveTilemap != this || data.SelectedTile == null) return;

            if (data.SelectedTool != PaintTool.Default)
            {
                // Draw grid gizmos
                float start = -GridScale * TileSize + (float)TileSize / 2;
                float end = GridScale * TileSize - (float)TileSize / 2;

                Gizmos.color = new Color(1, 1, 1, 0.25f);
                for (float i = start; i <= end; i += TileSize)
                {
                    Gizmos.DrawLine(new Vector3(start, i, 0) + transform.position, new Vector3(end, i, 0) + transform.position);
                    Gizmos.DrawLine(new Vector3(i, start, 0) + transform.position, new Vector3(i, end, 0) + transform.position);
                }

                // Draw selector gizmos
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(data.CurrentGrid, Vector3.one * TileSize);

            }
        }

        private void OnSceneGUI(SceneView scene)
        {
            // Process only when active
            if (!EditorWindow.HasOpenInstances<Tilemap25DEditorWindow>() || data.ActiveTilemap != this || data.SelectedTile == null) return;

            if (Tools.current != Tool.None && data.SelectedTool != PaintTool.Default)
            {
                data.SelectedTool = PaintTool.Default;
            }

            if (data.SelectedTool != PaintTool.Default)
            {
                scene.in2DMode = true;

                // Disable handle
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                // Update selector gizmos
                HandleUtility.Repaint();

                Vector3 mousePosition = Event.current.mousePosition;
                // For Retina display
                mousePosition.x *= EditorGUIUtility.pixelsPerPoint;
                mousePosition.y = scene.camera.pixelHeight - mousePosition.y * EditorGUIUtility.pixelsPerPoint;

                // Get current selection grid
                data.CurrentGrid = ConvertToGridPosition(scene.camera.ScreenToWorldPoint(mousePosition)); ;

                // Click left button
                if (Event.current.button == 0)
                {
                    if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                    {
                        switch (data.SelectedTool)
                        {
                            case PaintTool.Pencil:
                                PaintTools.Pencil();
                                break;
                            case PaintTool.Eraser:
                                PaintTools.Eraser();
                                break;
                            case PaintTool.Bucket:
                                if (Event.current.type == EventType.MouseDown) PaintTools.Bucket();
                                break;
                        }
                        Event.current.Use();
                    }
                    else if (Event.current.type == EventType.MouseUp)
                    {
                        data.PrePaintPosition = null;
                        Event.current.Use();
                    }
                }
            }
        }


        private Vector3 ConvertToGridPosition(Vector2 position)
        {
            Vector2 diff = position - (Vector2)transform.position;
            Vector3 gridLocalPosition
                = new Vector3(
                    Mathf.RoundToInt(diff.x / TileSize) * TileSize,
                    Mathf.RoundToInt(diff.y / TileSize) * TileSize,
                    0);
            return transform.TransformPoint(gridLocalPosition);
        }
    }
}
#endif

