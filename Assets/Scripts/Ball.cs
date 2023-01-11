using UnityEngine;
using UnityEngine.SceneManagement;

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
            // maybe just return the ball to the pad instead?
            SceneManager.LoadScene(0);
        }
    }
}
