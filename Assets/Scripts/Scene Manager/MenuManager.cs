using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NewGame(){
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
