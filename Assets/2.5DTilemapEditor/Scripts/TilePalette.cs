#if UNITY_EDITOR

using UnityEngine;
using System.Collections.Generic;

namespace TilemapEditor25D
{
    [CreateAssetMenu(menuName = "2.5D Tilemap Editor/Create Tile Palette")]
    public class TilePalette : ScriptableObject
    {
        [SerializeField]
        private List<GameObject> prefabs = new List<GameObject>();
        public List<GameObject> Prefabs {
            get { return prefabs; }
            set { prefabs = value; }
        }
    }
}
#endif