using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Ball : MonoBehaviour
{
    bool onPad = true;

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    float startImpulse;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onPad)
        {
            onPad = false;
            transform.parent = transform.parent.parent;
            body.AddForce(new Vector2(startImpulse, startImpulse), ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with <color=red>{collision.gameObject.name}</color>");
        var tilemap = collision.gameObject.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                // Why do we need to have those normals?
                var x = hit.point.x - 0.01f * hit.normal.x;
                var y = hit.point.y - 0.01f * hit.normal.y;
                var hitPos = new Vector3(x, y, 0f);
                var cell = tilemap.WorldToCell(hitPos);
                Debug.Log($"Hit cell: {cell}");
                tilemap.SetTile(cell, null);
            }
        }
    }
}
