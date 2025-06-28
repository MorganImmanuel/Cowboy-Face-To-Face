using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (UIController.Instance.pausePanel.activeSelf == false)
        {
            UIController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
            AudioManager.Instance.PlaySound(AudioManager.Instance.pause);
        }
        else
        {
            UIController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            AudioManager.Instance.PlaySound(AudioManager.Instance.unpause);
        }
    }

    public void Resume()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry(){
        SceneManager.LoadScene("GamePlay");
    }

    public void PlayerWin()
    {
        StartCoroutine(ShowPlayerWinScreen());
    }

    IEnumerator ShowPlayerWinScreen()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("PlayerWin");
    }

    public void ComputerWin()
    {
        StartCoroutine(ShowComputerWinScreen());
    }

    IEnumerator ShowComputerWinScreen()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ComputerWin");   
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
