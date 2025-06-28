using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    // Player
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    // Enemy
    [SerializeField] private Slider EnemyHealthSlider;
    [SerializeField] private TMP_Text EnemyHealthText;

    public GameObject pausePanel;
    
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

    public void UpdateHealthSlider(float current, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = Mathf.RoundToInt(current);
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
    }
    
    public void UpdateComputerHealthSlider(float current, float max)
    { 
        EnemyHealthSlider.maxValue = max;
        EnemyHealthSlider.value = Mathf.RoundToInt(current);
        EnemyHealthText.text = EnemyHealthSlider.value + "/" + EnemyHealthSlider.maxValue;
    }
}
