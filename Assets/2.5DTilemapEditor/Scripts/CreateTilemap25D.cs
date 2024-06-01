#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace TilemapEditor25D
{
    public class CreateTilemap25D
    {
        [MenuItem("GameObject/2.5DTilemapEditor/Tilemap 2.5D",priority = int.MinValue)]
        private static void Create()
        {
            GameObject obj = new GameObject("Tilemap 2.5D");
            obj.AddComponent(typeof(Tilemap25D));

            Undo.RegisterCreatedObjectUndo(obj, "Create Tilemap 2.5D");
        }

        [MenuItem("Component/2.5DTilemapEditor/Tilemap 2.5D")]
        private static void AddComponent()
        {
            GameObject obj = Selection.activeGameObject;
            Undo.AddComponent(obj, typeof(Tilemap25D));
        }
    }
}
#endif