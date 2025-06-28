using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;   
    private Material defaultMaterial;

    [SerializeField] private Material whiteMaterial;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask roofLayer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool facingRight = true;
    private float roofJumpCooldown;

    // Player Health
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    } 

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        health = maxHealth;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (roofJumpCooldown > 0.2f)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

            if(onRoof() && !isGrounded())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = 7;
            }

            if (Input.GetKey(KeyCode.Space))
            {
            Jump();
            }
        }
        else
        {
            roofJumpCooldown += Time.deltaTime;
        }

            // flip face
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
    facingRight = !facingRight;
    transform.Rotate(0f, -180f, 0f);
    }

    private void Jump()
    {
        if(isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        else if(onRoof() && !isGrounded())
        {
            roofJumpCooldown = 0;
            rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
        }
        
        
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onRoof()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, 
        new Vector2(transform.localScale.x, 0), 0.1f, roofLayer);
        return raycastHit.collider != null;
    }  

    public void TakeDamage(int damage)
    {
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        AudioManager.Instance.PlaySound(AudioManager.Instance.hit);
        spriteRenderer.material = whiteMaterial;    
        StartCoroutine("ResetMaterial");
        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.ComputerWin();
            AudioManager.Instance.PlaySound(AudioManager.Instance.die);
        }
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }
}