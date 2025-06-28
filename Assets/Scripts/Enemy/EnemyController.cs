using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform playerTransform; 

    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;
    public float detectionRange = 15f; 
    public float moveSpeed = 3f;
    public float JarakStop = 10f;    

    private bool playerDetected = false;
    private bool isFacingRight;

    // Health
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    [Header("Enemy Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        health = maxHealth;
        UIController.Instance.UpdateComputerHealthSlider(health, maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player GameObject tidak ditemukan! Pastikan player memiliki tag 'Player'.");
            }
        }

        isFacingRight = (transform.localScale.x < 0);
        Debug.Log("Enemy initial facing: " + (isFacingRight ? "Right" : "Left") + " (Scale.x: " + transform.localScale.x + ")");
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }

        if (playerDetected)
        {

            if (distanceToPlayer > JarakStop)
            {
                MoveTowardsPlayer();
            }
            else
            {
                TryShootPlayer();
            }

            FacePlayer();
        }

    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
    }

    void FacePlayer()
    {
        float directionToPlayerX = playerTransform.position.x - transform.position.x;
        if (directionToPlayerX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (directionToPlayerX < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight; 

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1; 
        transform.localScale = currentScale;

        Debug.Log("FLIPPED! New Scale.x: " + transform.localScale.x + ", New isFacingRight: " + isFacingRight); 
    }

    void TryShootPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab not assigned to EnemyController!");
            return;
        }

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        GameObject bulletGO = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        PhaserBullet bulletScript = bulletGO.GetComponent<PhaserBullet>();

        if (bulletScript != null)
        {
            float bulletSpeed = (PhaserWeapon.Instance != null) ? PhaserWeapon.Instance.speed : 10f;
            bulletScript.SetDirection(direction, bulletSpeed);
        }
        else
        {
            Debug.LogError("PhaserBullet script not found on instantiated bullet prefab! Make sure it's attached.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, JarakStop);

    }

    public void TakeDamage(int damage){
        health -= damage;
        UIController.Instance.UpdateComputerHealthSlider(health, maxHealth);
        AudioManager.Instance.PlaySound(AudioManager.Instance.hit);
        spriteRenderer.material = whiteMaterial;    
        StartCoroutine("ResetMaterial");
        if(health <= 0){
            gameObject.SetActive(false);
            GameManager.Instance.PlayerWin();
            AudioManager.Instance.PlaySound(AudioManager.Instance.die);

        }
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }
}
