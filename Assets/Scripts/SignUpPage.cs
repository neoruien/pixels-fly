using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpPage : MonoBehaviour
{
    private AuthController controller;

    void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<AuthController>();
    }

    public void Back()
    {
        SceneManager.LoadScene("Login");
    }

    public void SignUp()
    {
        controller.Signup();
    }

}
