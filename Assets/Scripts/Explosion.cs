using UnityEngine;
using UnityEngine.Tilemaps;

public class Explosion : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystemRenderer psRenderer;

    [SerializeField]
    private TileMaterialsMapping materialMapping;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        psRenderer = GetComponent<ParticleSystemRenderer>();
    }

    public void Play(TileBase tile, Vector3 hitNormal)
    {
        var shape = ps.shape;

        float zRotation = Vector2.SignedAngle(Vector2.right, -hitNormal) - shape.arc / 2f;
        shape.rotation = new Vector3(shape.rotation.x, shape.rotation.y, zRotation);

        Material material = materialMapping.GetMaterial(tile);
        if (material != null)
        {
            psRenderer.material = material;
        }

        ps.Play();
    }
}
