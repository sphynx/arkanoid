using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

using System.Collections.Generic;

public class Pad : MonoBehaviour
{
    [SerializeField]
    float maxAcceleration;

    [SerializeField]
    float maxSpeed;

    [SerializeField]
    GameObject ballPrefab;

    [SerializeField]
    GameObject laserBeamPrefab;

    [SerializeField]
    float ballImpulse;

    [SerializeField]
    float widePadTime;

    [SerializeField]
    float laserTime;

    [SerializeField]
    TilemapCollider2D tileMapCollider;

    [SerializeField]
    int numOfMultiBalls;

    [SerializeField]
    float multiballsSpawnRadius;

    [SerializeField]
    IntVar score;

    [SerializeField]
    IntVar lives;

    [SerializeField]
    AudioSource bonusSound;

    [SerializeField]
    AudioSource ballHitSound;

    SpriteRenderer spriteRenderer;

    Animator animator;

    float velocity;

    bool glueBall;
    bool useLaser;
    bool useWidePad;
    float laserActiveTime;
    float widePadActiveTime;

    List<Ball> ballsOnPad;

    void Start()
    {
        velocity = 0f;

        // Bonuses:
        glueBall = true;
        useLaser = false;
        useWidePad = false;
        laserActiveTime = 0f;
        widePadActiveTime = 0f;

        ballsOnPad = new List<Ball>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        SpawnBallOnPad();
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

        velocity = userInput * maxSpeed;

        var displacement = velocity * Time.fixedDeltaTime;
        var pos = transform.localPosition;
        var newX = pos.x + displacement;

        // TODO: set this based on resolution and current width of pad.
        newX = Mathf.Clamp(newX, -15.5f, 15.5f);

        var newPos = new Vector3(newX, pos.y, 0);

        transform.localPosition = newPos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MultiballBonus"))
        {
            SpawnMultiBalls(numOfMultiBalls, multiballsSpawnRadius);
            bonusSound.Play();
        }
        else if (collision.gameObject.CompareTag("WidePadBonus"))
        {
            WidenPad();
            bonusSound.Play();
        }
        else if (collision.gameObject.CompareTag("StickyBonus"))
        {
            MakeSticky();
            bonusSound.Play();
        }
        else if (collision.gameObject.CompareTag("LaserBonus"))
        {
            UseLaser();
            bonusSound.Play();
        }

        Destroy(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball;

        if (collision.gameObject.TryGetComponent<Ball>(out ball))
        {
            if (glueBall && !ballsOnPad.Contains(ball))
            {
                ball.GlueToPad(this.gameObject);
                ballsOnPad.Add(ball);
            }
            else if (!ballsOnPad.Contains(ball))
            {
                ballHitSound.Play();
            }
        }
    }

    void SpawnMultiBalls(int numBalls, float spawnRadius)
    {
        // Find some ball (it can return any with that tag):
        GameObject ball = GameObject.FindWithTag("Ball");

        if (ball == null)
            return;

        // Spawn new N balls around it in a circular pattern
        for (int i = 0; i < numBalls; i++)
        {
            float angle = 2 * Mathf.PI * i / numBalls;

            float dirX = Mathf.Cos(angle);
            float dirY = Mathf.Sin(angle);

            Vector3 center = ball.transform.position;
            Vector3 where = center + new Vector3(dirX, dirY, 0f) * spawnRadius;

            // This is some tricky business to prevent balls appearing within the bricks collider :)
            // Note we can do it differently: using OverlapCircle from Physics2D and check that
            // tileMap collider is not in the returned array of colliders.
            if (!tileMapCollider.composite.OverlapPoint(where))
            {
                Vector2 closestPoint = tileMapCollider.composite.ClosestPoint(where);
                float distance = Vector2.Distance(closestPoint, where);

                if (distance > 0.75f)
                {
                    GameObject newBallObj = Instantiate(ballPrefab, where, Quaternion.identity);
                    Ball newBall = newBallObj.GetComponent<Ball>();
                    newBall.Fire(new Vector2(dirX, dirY) * ballImpulse);
                }
            }
        }
    }

    void MakeSticky()
    {
        glueBall = true;
    }

    void WidenPad()
    {
        useWidePad = true;
        spriteRenderer.size = new Vector2(6f, 1f);
        widePadActiveTime = Time.time + widePadTime;
    }

    void UseLaser()
    {
        useLaser = true;
        animator.SetBool("Has Laser", useLaser);
        laserActiveTime = Time.time + laserTime;
    }

    void PowerDownWidePad()
    {
        useWidePad = false;
        Bounds bounds = spriteRenderer.sprite.bounds;
        var defaultWidth = bounds.extents.x / bounds.extents.y;
        spriteRenderer.size = new Vector2(defaultWidth, 1f);
    }

    void PowerDownLaser()
    {
        useLaser = false;
        animator.SetBool("Has Laser", useLaser);
    }

    void FireBallsInRandomDirections()
    {
        glueBall = false;

        foreach (Ball b in this.ballsOnPad)
        {
            float angle = Random.Range(Mathf.PI / 4f, Mathf.PI * 3f / 4f);
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            b.Fire(new Vector2(x, y) * ballImpulse);
        };

        this.ballsOnPad = new List<Ball>();
    }

    void SpawnBallOnPad()
    {
        GameObject ballObj = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ballObj.transform.parent = transform;
        Ball ball = ballObj.GetComponent<Ball>();
        ballObj.transform.localPosition = ball.CenterOnPad(this.gameObject);
        ballsOnPad.Add(ball);
    }

    void FireLaser()
    {
        float height = spriteRenderer.sprite.bounds.size.y;
        float width = height * spriteRenderer.size.x;

        Vector3 leftPos = new Vector3(-width / 2f + 0.18f, height - 0.2f, 0f);
        Vector3 rightPos = new Vector3(width / 2f - 0.18f, height - 0.2f, 0f);

        Instantiate(laserBeamPrefab, transform.position + leftPos, Quaternion.identity);
        Instantiate(laserBeamPrefab, transform.position + rightPos, Quaternion.identity);
    }

    void OnLostLife()
    {
        lives.Value -= 1;
        if (lives.Value == 0)
        {
            GameOver();
        }
        else
        {
            SpawnBallOnPad();
        };
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireBallsInRandomDirections();

            if (useLaser)
                FireLaser();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (ballsOnPad.Count == 0)
                SpawnBallOnPad();
        }

        if (useLaser && Time.time > laserActiveTime)
        {
            PowerDownLaser();
        }

        if (useWidePad && Time.time > widePadActiveTime)
        {
            PowerDownWidePad();
        }

        var balls = Object.FindObjectsOfType<Ball>();
        if (balls.Length == 0)
            OnLostLife();
    }
}