using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoresPage : MonoBehaviour
{
    public GameObject warning;
    private UserData userData;
    private Text myRank, myName, myScore;
    private string databaseURL = "https://pixels-fly.firebaseio.com/";

    private User[] otherUsers = new User[5];
    private Text[] rankedNames = new Text[5],
                   rankedScores = new Text[5];

    void Start()
    {
        // Finding the GameObjects
        userData = GameObject.Find("Controller").GetComponent<AuthController>().userData;
        myName = GameObject.Find("MyName").GetComponent<Text>();
        myScore = GameObject.Find("MyScore").GetComponent<Text>();
        myRank = GameObject.Find("MyRank").GetComponent<Text>();
        for (int i = 1; i <= 5; i++)
        {
            otherUsers[i - 1] = new User();
            rankedNames[i - 1] = GameObject.Find("R" + i + "Name").GetComponent<Text>();
            rankedScores[i - 1] = GameObject.Find("R" + i + "Score").GetComponent<Text>();
        }

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);

        // Get the root reference location of the database.
        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    print("faulted");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    int childRank = 999;
                    
                    foreach (DataSnapshot child in snapshot.Children)
                    {
                        childRank = Int32.Parse(child.Child("rank").GetRawJsonValue());
                        if (childRank <= 5)
                        {
                            otherUsers[childRank - 1].username = child.Child("username").GetRawJsonValue().Replace("\"", "");
                            otherUsers[childRank - 1].highscore = int.Parse(child.Child("highscore").GetRawJsonValue());
                            print(otherUsers[childRank - 1].username + " " + otherUsers[childRank - 1].highscore);
                        }
                    }
                }
            });

        // Fill in the usernames and scores
        myName.text = userData.user.username;
        myScore.text = userData.user.highscore.ToString();
        myRank.text = userData.user.rank.ToString();
    }

    void Update()
    {
        if (userData.user.email.Equals("Guest")) warning.SetActive(true);
        // Update the leaderboard
        for (int i = 0; i < 5; i++)
        {
            rankedNames[i].text = otherUsers[i].username;
            rankedScores[i].text = otherUsers[i].highscore.ToString();
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

}
