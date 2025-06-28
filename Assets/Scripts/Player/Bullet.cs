using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    public int damageAmount = 10;   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Shoot.Instance.Speed * Time.deltaTime, 0f);
        if (transform.position.x > 9 || transform.position.x < -9)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Enemies"))
        {
            Debug.Log("Bullet hit Enemy!");

            EnemyController enemyHealth = collision.GetComponent<EnemyController>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount); 
            }
            Destroy(gameObject); 

        }
    }
}
