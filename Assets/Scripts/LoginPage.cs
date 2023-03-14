using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPage : MonoBehaviour
{
    private AuthController controller;

    void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<AuthController>();
    }

    public void LogIn()
    {
        controller.Login();
    }

    public void Guest()
    {
        controller.Guest();
    }

    public void SignUp()
    {
        SceneManager.LoadScene("Signup");
    }

}
