using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float pitThreshold = -10f; // Y position below which is considered a pit
    private Animator animator;
    private bool isGrounded;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();

    }
    void Start()
    {
        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDied += OnPlayerDied;
        }
    }

    private void OnPlayerDied(Health health)
    {
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    private void OnDestroy()
    {
        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDied -= OnPlayerDied;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin())
        {
            return; // Skip movement and animation updates if the game is over
        }
        CheckForPit();
        HandleMovement();
        HandleJump();
        UpdateAnimations();
    }
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            audioManager.PlayJumpSound();
        }
    }

    private void UpdateAnimations()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        animator.SetBool("isRunning", isRunning);
        bool isJumping = !isGrounded;
        animator.SetBool("isJumping", isJumping);
    }

    private void CheckForPit()
    {
        if (transform.position.y < pitThreshold)
        {
            gameManager.RespawnAtCheckpoint();
        }
    }
}
