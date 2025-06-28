using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserBullet : MonoBehaviour
{

    public float speed = 10f;
    private Vector2 moveDirection;
    
    public int damageAmount = 5;

    public void SetDirection(Vector2 direction, float bulletSpeed)
    {
        moveDirection = direction.normalized; 
        speed = bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;

        if (Mathf.Abs(transform.position.x) > 12) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Bullet hit Player!");

            PlayerMovement playerHealth = collision.GetComponent<PlayerMovement>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount); 
            }
            Destroy(gameObject); 

        }
    }
    
}
