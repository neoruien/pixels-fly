using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    private PlayerManager playerManager;
    private AudioManager audioManager;

    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void PauseGame()
    {
        playerManager.paused = true;
    }

    public void ResumeGame()
    {
        playerManager.paused = false;
        playerManager.rankUpdated = false;
    }

    public void ReplayGame()
    {
        playerManager.rankUpdated = false;
        SceneManager.LoadScene("Level");
        audioManager.Replay();
    }

    public void ReplayTraining()
    {
        SceneManager.LoadScene("Level (Training)");
    }

    public void QuitGame()
    {
        playerManager.rankUpdated = false;
        audioManager.Click();
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ClearPrompt()
    {
        // To be implemented
    }

}
