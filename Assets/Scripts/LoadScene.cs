using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSignUpScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Signup");
    }

    public void LoadMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
