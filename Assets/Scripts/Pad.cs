using UnityEngine;

public class Pad : MonoBehaviour
{
    [SerializeField]
    float maxAcceleration;

    [SerializeField]
    float maxSpeed;

    float velocity;

    void Start()
    {
        velocity = 0f;
    }

    void FixedUpdate()
    {
        //float userInput = Input.GetAxis("Horizontal");

        // One way is to treat input as desired velocity
        // and use maxAcceleration to control sensitivity:
        //
        // desiredVelocity = userInput * maxSpeed;
        // float maxSpeedChange = maxAcceleration * Time.deltaTime;
        // velocity = Mathf.MoveTowards(velocity, desiredVelocity, maxSpeedChange);

        // Another way is to directly control velocity:
        //
        // velocity = userInput * maxSpeed; // 35 seems to work fine

        // And the third way is just to use -1 or 1 for velocity:
        float userInput;
        if (Input.GetKey(KeyCode.LeftArrow))
            userInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            userInput = 1f;
        else
            userInput = 0;
        velocity = userInput * maxSpeed; // 30 seems to work fine

        var displacement = velocity * Time.fixedDeltaTime;
        var pos = transform.localPosition;
        var newX = pos.x + displacement;
        newX = Mathf.Clamp(newX, -15.5f, 15.5f);
        var newPos = new Vector3(newX, pos.y, 0);

        transform.localPosition = newPos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collided with an object tagged <color=red>{collision.gameObject.tag}</color>");

        if (collision.gameObject.CompareTag("WidePadBonus"))
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(1.5f * scale.x, scale.y, scale.z);
        }

        Destroy(collision.gameObject);
    }
}