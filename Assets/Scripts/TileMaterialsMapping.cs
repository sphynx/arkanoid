using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileMaterialsMapping : ScriptableObject
{
    [System.Serializable]
    public class Mapping
    {
        public TileBase tile;
        public Material material;
    }

    // This is what we set up from the Scriptable Object UI.
    // The mapping between good and broken tiles.
    public Mapping[] Data;

    // Internal data.
    private Dictionary<string, Material> data;

    public Material GetMaterial(TileBase brick)
    {
        Init();
        data.TryGetValue(brick.name, out Material material);
        return material;
    }

    private void Init()
    {
        if (data != null)
            return;

        data = new Dictionary<string, Material>();
        foreach (var mapping in Data)
        {
            data[mapping.tile.name] = mapping.material;
        }
    }
}

