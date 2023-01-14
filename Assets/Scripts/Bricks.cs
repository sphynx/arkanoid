using UnityEngine;
using UnityEngine.Tilemaps;

public class Bricks : MonoBehaviour
{
    Tilemap tilemap;

    [SerializeField]
    BricksStateMapping bricksMapping;

    [SerializeField]
    BonusSpawner bonusSpawner;

    public Vector3 location;

    private void Update()
    {
        location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Awake()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                // Note that normal vectors are pointing from the ball into the bricks
                // so when we add a little bit of that, we get a point inside the brick area
                // If we just use hit.point, it may be right on the border and not register properly,
                // resolving to a different cell.
                var x = hit.point.x + 0.02f * hit.normal.x;
                var y = hit.point.y + 0.02f * hit.normal.y;
                var hitPos = new Vector3(x, y, 0f);
                var cell = tilemap.WorldToCell(hitPos);

                // Either remove the brick completely or replace it with the broken tile:
                TileBase tile = tilemap.GetTile(cell);

                if (tile != null)
                {
                    TileBase nextTile = bricksMapping.Break(tile); // can be null
                    tilemap.SetTile(cell, nextTile);

                    if (nextTile == null)
                    {
                        Vector3 bonusPos = tilemap.GetCellCenterWorld(cell);
                        bonusSpawner.SpawnBonus(bonusPos);
                    }
                }
            }
        }
    }
}
