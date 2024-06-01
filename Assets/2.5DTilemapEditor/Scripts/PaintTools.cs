#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace TilemapEditor25D
{
    public static class PaintTools
    {
        private static readonly EditorData data = ScriptableSingleton<EditorData>.instance;
        private const int MaxPaintTileWidth = 1000;

        // Pencil tool
        public static void Pencil()
        {
            if (data.CurrentGrid == data.PrePaintPosition) return;

            DestroyTile(data.CurrentGrid);
            CreateTile(data.CurrentGrid);

            data.PrePaintPosition = data.CurrentGrid;
        }

        // Eraser tool
        public static void Eraser()
        {
            DestroyTile(data.CurrentGrid);
        }


        // Bucket tool
        public static void Bucket()
        {
            List<Vector3> bucketPositions = new List<Vector3>();
            string bucketTileName = GetSelectedGridTileName();

            if (!GetBucketPositions(bucketTileName, bucketPositions))
            {
                Debug.Log("Reached the limit for painting at once.\n" +
                    "The maximum width of a tile that can be it is " + MaxPaintTileWidth + ".");
                return;
            }

            // Paint bucket positions
            foreach (Vector3 position in bucketPositions)
            {
                DestroyTile(position);
            }
            foreach (Vector3 position in bucketPositions)
            {
                CreateTile(position);
            }

        }

        private static string GetSelectedGridTileName()
        {
            // Get the selected grid objects
            GameObject rootTile = GetRootTileObject(data.CurrentGrid);
            if (rootTile == null)
            {
                return "";
            }
            else
            {
                return rootTile.name;
            }
        }

        private static bool GetBucketPositions(string bucketTileName, List<Vector3> bucketPositions)
        {
            List<Vector3> buf = new List<Vector3>();
            Vector3 pos = data.CurrentGrid;
            buf.Add(pos);

            while (buf.Count != 0)
            {
                pos = buf[buf.Count - 1];
                buf.RemoveAt(buf.Count - 1);

                bucketPositions.Add(pos);

                int cnt = 1;

                // Get the edges of the connected tiles
                Vector3 leftPos = pos;
                while (CheckIsTile(leftPos + Vector3.left * data.ActiveTilemap.TileSize, bucketTileName, bucketPositions))
                {
                    leftPos += Vector3.left * data.ActiveTilemap.TileSize;
                    bucketPositions.Add(leftPos);

                    if (cnt++ >= MaxPaintTileWidth) return false;
                }

                Vector3 rightPos = pos;
                while (CheckIsTile(rightPos + Vector3.right * data.ActiveTilemap.TileSize, bucketTileName, bucketPositions))
                {
                    rightPos += Vector3.right * data.ActiveTilemap.TileSize;
                    bucketPositions.Add(rightPos);

                    if (cnt++ >= MaxPaintTileWidth) return false;
                }


                // Buffer the right edge of the connected tiles
                pos = leftPos + Vector3.up * data.ActiveTilemap.TileSize;
                while (pos != rightPos + Vector3.up * data.ActiveTilemap.TileSize)
                {
                    if (CheckIsTile(pos, bucketTileName, bucketPositions) && !CheckIsTile(pos + Vector3.right * data.ActiveTilemap.TileSize, bucketTileName, bucketPositions))
                    {
                        buf.Add(pos);
                    }
                    pos += Vector3.right * data.ActiveTilemap.TileSize;
                }
                if (CheckIsTile(pos, bucketTileName, bucketPositions))
                {
                    buf.Add(pos);
                }

                pos = leftPos + Vector3.down * data.ActiveTilemap.TileSize;
                while (pos != rightPos + Vector3.down * data.ActiveTilemap.TileSize)
                {
                    if (CheckIsTile(pos, bucketTileName, bucketPositions) && !CheckIsTile(pos + Vector3.right * data.ActiveTilemap.TileSize, bucketTileName, bucketPositions))
                    {
                        buf.Add(pos);
                    }
                    pos += Vector3.right * data.ActiveTilemap.TileSize;
                }
                if (CheckIsTile(pos, bucketTileName, bucketPositions))
                {
                    buf.Add(pos);
                }
            }

            return true;
        }

        private static void CreateTile(Vector3 pos)
        {
            if (GetRootTileObject(pos) != null) return;

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(data.SelectedTile);
            obj.name = data.SelectedTile.name;
            obj.transform.parent = data.ActiveTilemap.transform;
            obj.transform.position = pos;

            Undo.RegisterCreatedObjectUndo(obj, "Create Tile");
        }

        private static void DestroyTile(Vector3 pos)
        {
            GameObject rootTile = GetRootTileObject(pos);
            if (rootTile != null) Undo.DestroyObjectImmediate(rootTile);
        }


        private static GameObject GetRootTileObject(Vector3 pos)
        {
            Transform activeTilemapRoot = data.ActiveTilemap.transform;
            Vector3 tilePos = pos;

            // Get the selected grid objects
            for (int j = 0; j < activeTilemapRoot.childCount; j++)
            {
                Transform rootTile = activeTilemapRoot.GetChild(j);

                if (rootTile.position == tilePos)
                {
                    return rootTile.gameObject;
                }
            }

            return null;
        }

        private static bool CheckIsTile(Vector3 pos, string tileName, List<Vector3> bucketPositions)
        {
            if (bucketPositions.Contains(pos)) return false;

            // Check if there are no tiles
            if (tileName == "")
            {
                // Get the specified grid objects
                GameObject rootTile = GetRootTileObject(pos);

                if(rootTile == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // Check if there are specified tiles.
            else
            {
                // Get the specified grid objects
                GameObject rootTile = GetRootTileObject(pos);

                if (rootTile != null)
                {
                    return rootTile.name == tileName;
                }
                else
                {
                    return false;
                }   
            }
        }
    }
}
#endif