using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadTraining()
    {
        SceneManager.LoadScene("Level (Training)");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void ShowScores()
    {
        SceneManager.LoadScene("Scores");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Logout()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            FirebaseAuth.DefaultInstance.SignOut();
            SceneManager.LoadScene("Login");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
