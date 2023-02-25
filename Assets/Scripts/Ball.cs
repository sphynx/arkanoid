using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Fire(Vector2 impulse)
    {
        body.isKinematic = false;
        if (transform.parent != null)
            transform.parent = transform.parent.parent; // Free it from the pad-parent.
        body.AddForce(impulse, ForceMode2D.Impulse);
    }

    public void GlueToPad(GameObject pad)
    {
        body.isKinematic = true;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
        transform.parent = pad.transform; // Bind to pad, so that it moves with it.
    }

    public Vector3 CenterOnPad(GameObject pad)
    {
        Vector3 ballSize = spriteRenderer.bounds.size;
        Vector3 padSize = pad.GetComponent<SpriteRenderer>().bounds.size;

        float xRelativeToPad = 0f;
        float yRelativeToPad = (padSize.y + ballSize.y) / 2f;

        return new Vector3(xRelativeToPad, yRelativeToPad, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AdjustVelocity(body.velocity);
        }
    }

    private void AdjustVelocity(Vector2 velocity)
    {
        const float ADJUST_THRESHOLD = 3f;
        const float ADJUST_AMOUNT = 10f;

        Vector2[] axes = { Vector2.down, Vector2.right, Vector3.left };

        foreach (Vector2 ax in axes)
        {
            float angle = Vector2.SignedAngle(ax, velocity);

            if (Mathf.Abs(angle) < ADJUST_THRESHOLD)
            {
                var adjustedAngle = ADJUST_AMOUNT * Mathf.Sign(angle);

                // rotate velocity `angle` degrees counterclockwise along Z axis:
                Vector2 newVelDirection = Quaternion.Euler(0, 0, adjustedAngle) * body.velocity.normalized;
                Vector2 newVelocity = body.velocity.magnitude * newVelDirection;
                body.velocity = newVelocity;
                return;
            }
        }
    }
}
