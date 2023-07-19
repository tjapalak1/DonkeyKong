using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] overlaps = new Collider2D[4];
    private Vector2 direction;

    private bool grounded;
    private bool climbing;

    public float moveSpeed = 3f;
    public float jumpStrength = 4f;

    private GameManager gmRef;

    public AudioSource walkSound;
    public AudioSource jumpSound;
    public AudioSource coinSound;
    public AudioSource deathSound;
    public AudioSource winSound;
    public AudioSource startLevelSound;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Start()
    {
        gmRef = FindObjectOfType<GameManager>();
        enabled = true;
        startLevelSound.Play();
    }

    private void Update()
    {
        if(gmRef.levelStarted==true)
        {
            CheckCollision();
            SetDirection();
        }
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector3 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0, overlaps);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = overlaps[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                // Only set as grounded if the platform is below the player
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);

                // Turn off collision on platforms the player is not grounded to
                Physics2D.IgnoreCollision(overlaps[i], collider, !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    private void SetDirection()
    {
        if (climbing) 
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        } 
        else if (grounded && Input.GetButtonDown("Jump")) 
        {
            direction = Vector2.up * jumpStrength;
            jumpSound.Play();
        } 
        else 
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Prevent gravity from building up infinitely
        if (grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if (direction.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (direction.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        //walk sound handling
        if(grounded==true)
        {
            if(direction.x!=0f && enabled==true)
            {
                walkSound.volume = 1;
            }
            else
            {
                walkSound.volume = 0;
            }
        }
        else
        {
            walkSound.volume = 0;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex++;
            if (spriteIndex >= runSprites.Length) {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective")&&enabled==true)
        {
            enabled = false;
            gmRef.DisableEverything();
            winSound.time = 5.5f;
            winSound.Play();
            gmRef.SaveScore();
            Invoke(nameof(LevelWin), 6);
            walkSound.volume = 0;
        }
        else if (collision.gameObject.CompareTag("Obstacle")&&enabled==true)
        {
            enabled = false;
            deathSound.Play();
            gmRef.DisableEverything();
            Invoke(nameof(LevelFailed), 5);
            walkSound.volume = 0;
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            gmRef.AddToScore(collision.gameObject.GetComponent<Coin>().coinScoreValue);
            Destroy(collision.gameObject);
            coinSound.Play();
        }
        else if (collision.gameObject.CompareTag("Connector"))
        {
            gmRef.AddToScore(100);
            Destroy(collision.gameObject);
            coinSound.Play();
            gmRef.RemoveConnector();
        }
    }

    private void LevelFailed()
    {
        gmRef.LevelFailed();
    }

    private void LevelWin()
    {
        gmRef.LevelComplete();
    }
}
