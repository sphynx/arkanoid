using UnityEngine;
using UnityEngine.Tilemaps;

public class Bricks : MonoBehaviour
{
    Tilemap tilemap;

    [SerializeField]
    BricksStateMapping bricksMapping;

    [SerializeField]
    BonusSpawner bonusSpawner;

    [SerializeField]
    GameObject brickExplosionPrefab;

    [SerializeField]
    AudioSource brickHitSound;

    public Vector3 location;

    private void Update()
    {
        location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Awake()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
    }

    private void HandleContact(ContactPoint2D hit)
    {
        // Note that normal vectors are pointing from the ball into the bricks
        // so when we add a little bit of that, we get a point inside the brick area
        // If we just use hit.point, it may be right on the border and not register properly,
        // resolving to a different cell.
        var x = hit.point.x + 0.02f * hit.normal.x;
        var y = hit.point.y + 0.02f * hit.normal.y;
        var hitPos = new Vector3(x, y, 0f);
        Vector3Int cell = tilemap.WorldToCell(hitPos);

        // Either remove the brick completely or replace it with the broken tile:
        TileBase tile = tilemap.GetTile(cell);

        if (tile != null)
        {
            TileBase nextTile = bricksMapping.Break(tile); // can be null
            tilemap.SetTile(cell, nextTile);

            brickHitSound.Play();

            if (nextTile == null)
            {
                Vector3 bonusPos = tilemap.GetCellCenterWorld(cell);
                GameObject bonus = bonusSpawner.SpawnBonus(bonusPos);

                if (bonus == null)
                {
                    ExplodeBrick(hitPos, hit.normal, tile);
                }
            }
        }
    }

    void ExplodeBrick(Vector3 hitPos, Vector3 normal, TileBase tile)
    {
        GameObject brickExplosionObj = Instantiate(brickExplosionPrefab, hitPos, Quaternion.identity);
        Explosion brickExplosion = brickExplosionObj.GetComponent<Explosion>();
        brickExplosion.Play(tile, normal);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObj = collision.gameObject;

        if (gameObj.CompareTag("Ball") || gameObj.CompareTag("LaserBeam"))
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                HandleContact(hit);
            }
        }
        
        if (gameObj.CompareTag("LaserBeam"))
        {
            Destroy(gameObj);
        }
    }
}
