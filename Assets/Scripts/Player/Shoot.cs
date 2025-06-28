using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public static Shoot Instance;
    public Transform shootingPoint;

    [SerializeField] private GameObject bulletPrefab;

    public float Speed;
    
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
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Instantiate (bulletPrefab, shootingPoint.position, transform.rotation);
        }
    }
}
