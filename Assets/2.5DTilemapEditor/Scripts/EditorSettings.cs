#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace TilemapEditor25D
{
    public class EditorSettings : ScriptableObject
    {
        [SerializeField]
        private TilePalette tilePalette;
        public TilePalette TilePalette {
            get { return tilePalette; }
            set { tilePalette = value; }
        }

        [SerializeField]
        [Range(1, 10)]
        private int paletteScale = 4;
        public int PaletteScale {
            get { return paletteScale; }
            set { paletteScale = value; }
        }
    }
}
#endif