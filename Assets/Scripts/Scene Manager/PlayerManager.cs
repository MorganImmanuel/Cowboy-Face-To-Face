using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Retry(){
        SceneManager.LoadScene("GamePlay");
    }

    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    
}
