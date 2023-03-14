using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;

public class AuthController : MonoBehaviour
{
    private InputField emailInput, passwordInput, usernameInput;
    private Text errorText;
    public UserData userData;
    private string databaseURL = "https://pixels-fly.firebaseio.com/";
    private Animation redAnim;
    private Animation yellowAnim;
    private Animation greenAnim;

    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);
    }

    void Update()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users").OrderByChild("highscore")
            .ValueChanged += HandleValueChanged;
    }

    public void Login()
    {
        emailInput = GameObject.Find("Email").GetComponent<InputField>();
        passwordInput = GameObject.Find("Password").GetComponent<InputField>();
        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        redAnim = GameObject.Find("Red").GetComponent<Animation>();
        yellowAnim = GameObject.Find("Yellow").GetComponent<Animation>();
        greenAnim = GameObject.Find("Green").GetComponent<Animation>();
        print("Starting to log in");
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).
            ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    errorText.text = e.Message;
                    redAnim.Play();
                    return;
                }
                if (task.IsFaulted)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    errorText.text = e.Message;
                    redAnim.Play();
                    return;
                }
                if (task.IsCompleted)
                {
                    redAnim.Play();
                    yellowAnim.Play();
                    userData.LoadData(emailInput.text, passwordInput.text);
                    greenAnim.Play();
                }
            });
    }

    public void Signup()
    {
        emailInput = GameObject.Find("Email").GetComponent<InputField>();
        passwordInput = GameObject.Find("Password").GetComponent<InputField>();
        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        redAnim = GameObject.Find("Red").GetComponent<Animation>();
        yellowAnim = GameObject.Find("Yellow").GetComponent<Animation>();
        greenAnim = GameObject.Find("Green").GetComponent<Animation>();

        if ((usernameInput = GameObject.Find("Username").GetComponent<InputField>()) != null)
        {
            print("Starting to sign up");
            if (emailInput.text.Equals("") || passwordInput.text.Equals(""))
            {
                errorText.text = "Please enter email/password to register";
                redAnim.Play();
                return;
            }

            FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).
                ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                        errorText.text = e.Message;
                        redAnim.Play();
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                        errorText.text = e.Message;
                        redAnim.Play();
                        return;
                    }
                    if (task.IsCompleted)
                    {
                        redAnim.Play();
                        yellowAnim.Play();
                        userData.SaveData(usernameInput.text, emailInput.text, passwordInput.text);
                        userData.PostToDatabase();
                        print(userData.user.username + "," + userData.user.email + "," + userData.user.password + "," + userData.user.highscore);
                        greenAnim.Play();
                    }
                });
        }
    }

    public void Guest()
    {
        redAnim = GameObject.Find("Red").GetComponent<Animation>();
        yellowAnim = GameObject.Find("Yellow").GetComponent<Animation>();
        greenAnim = GameObject.Find("Green").GetComponent<Animation>();
        FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().
            ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    errorText.text = e.Message;
                    redAnim.Play();
                    return;
                }
                if (task.IsFaulted)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    errorText.text = e.Message;
                    redAnim.Play();
                    return;
                }
                if (task.IsCompleted)
                {
                    redAnim.Play();
                    yellowAnim.Play();
                    userData.SaveData("Guest", "Guest", "Guest");
                    greenAnim.Play();
                }
            });
    }

    public void Logout()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            FirebaseAuth.DefaultInstance.SignOut();
            SceneManager.LoadScene("Login");
        }
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
    }

    void PlayGreenAnim()
    {
        greenAnim.Play();
    }
}