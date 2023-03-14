using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;
using Firebase.Extensions;
using Proyecto26;
using UnityEditor;
using System;

public class UserData : MonoBehaviour
{
    public User user;

    private User[] allUsers;// for sorting ranks
    private int numUsers;

    private string databaseURL = "https://pixels-fly.firebaseio.com/";

    private DatabaseReference databaseReference;

    public static UserData singleton = null;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (singleton == null)
        {
            singleton = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        user = new User();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        allUsers = new User[1000];
        for (int i = 1; i <= 1000; i++)
        {
            allUsers[i - 1] = new User();
        }
    }

    public void SaveData(string username, string email, string password) // new user
    {
        int[] skins = new int[5];
        skins[0] = -1; // default skin
        float[] volumes = new float[2];
        volumes[0] = 0.8f;
        volumes[1] = 1f;

        user = new User(username, email, password, 0, 250, 999, skins, volumes);
        string json = JsonUtility.ToJson(user);
        databaseReference.Child(email).
            SetRawJsonValueAsync(json);
    }

    public void UpdateData(string email)
    {
        databaseReference.Child(email).SetValueAsync(email);
    }

    public void LoadData(string email, string password) // old user
    {
        FirebaseDatabase.DefaultInstance.GetReferenceFromUrl(databaseURL).GetValueAsync().
            ContinueWithOnMainThread((task) =>
            {
                if (task.IsCanceled)
                {
                    print("canceled");
                }

                if (task.IsFaulted)
                {
                    print("faulted");
                }

                if (task.IsCompleted)
                {
                    string storedUnder = email.Trim(); // removes leading and trailing whitespace
                    storedUnder = storedUnder.Replace(".", "000");
                    RestClient.Get<User>($"{databaseURL}users/{storedUnder}.json")
                        .Then(data =>
                        {
                            user = new User(data.username, data.email, data.password, data.highscore, data.coins, data.rank, data.skins, data.volumes);
                            print(user.username + "," + user.email + "," + user.password + "," + user.highscore);
                            print("done setting up user!");
                        });
                }
            });
    }

    public void UpdateRank()
    {
        print("updating rank");

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
                    int rank = 1;

                    foreach (DataSnapshot child in snapshot.Children)
                    {
                        if (user.highscore < int.Parse(child.Child("highscore").GetRawJsonValue())) rank += 1;
                    }

                    user.rank = rank;

                    numUsers = 0;
                    foreach (DataSnapshot child in snapshot.Children)
                    {
                        // if their rank < 999, store them locally
                        if (int.Parse(child.Child("rank").GetRawJsonValue()) < 999 && !child.Child("username").GetRawJsonValue().Replace("\"", "").Equals(user.username)
                                                                                   && !child.Child("email").GetRawJsonValue().Replace("\"", "").Equals("Guest"))
                        {
                            allUsers[numUsers].username = child.Child("username").GetRawJsonValue().Replace("\"", "");
                            allUsers[numUsers].email = child.Child("email").GetRawJsonValue().Replace("\"", "");
                            allUsers[numUsers].password = child.Child("password").GetRawJsonValue().Replace("\"", "");
                            allUsers[numUsers].highscore = int.Parse(child.Child("highscore").GetRawJsonValue().Replace("\"", ""));
                            allUsers[numUsers].coins = int.Parse(child.Child("coins").GetRawJsonValue().Replace("\"", ""));
                            allUsers[numUsers].rank = int.Parse(child.Child("rank").GetRawJsonValue().Replace("\"", ""));
                            numUsers++;
                        }
                        else if (child.Child("username").GetRawJsonValue().Replace("\"", "").Equals(user.username))
                        {
                            allUsers[numUsers].username = user.username;
                            allUsers[numUsers].email = user.email;
                            allUsers[numUsers].password = user.password;
                            allUsers[numUsers].highscore = user.highscore;
                            allUsers[numUsers].coins = user.coins;
                            allUsers[numUsers].rank = user.rank;
                            allUsers[numUsers].skins = user.skins;
                            allUsers[numUsers].volumes = user.volumes;
                            numUsers++;
                        }
                    }

                    SortByScores();

                    print("done updating rank: " + user.rank);
                }
            });
    }

    public void PostToDatabase()
    {
        print("posting to database");

        string storeUnder = user.email.Trim(); // removes leading and trailing whitespace
        storeUnder = storeUnder.Replace(".", "000");

        print("stored under: " + storeUnder);

        RestClient.Put<User>($"{databaseURL}users/{storeUnder}.json", user);

        print("done posting to database");
    }

    public void PostToDatabaseWithRankings()
    {
        print("posting to database");

        string storeUnder = user.email.Trim(); // removes leading and trailing whitespace
        storeUnder = storeUnder.Replace(".", "000");

        print("stored under: " + storeUnder);

        RestClient.Put<User>($"{databaseURL}users/{storeUnder}.json", user);

        for (int i = 0; i < numUsers; i++)
        {
            User temp = allUsers[i];
            RestClient.Get<User>($"{databaseURL}users/{allUsers[i].email.Replace(".", "000")}.json")
                .Then(data =>
                {
                    temp.skins = data.skins;
                    print(temp.username + ", score: " + temp.highscore + ", rank: " + temp.rank);
                    RestClient.Put<User>($"{databaseURL}users/{temp.email.Replace(".", "000")}.json", temp);
                });
        }

        print(user.username + ", score: " + user.highscore + ", rank: " + user.rank);

        print("done posting to database");
    }

    public void SortByScores()
    {
        for (int pos = 0; pos < numUsers; pos++)
        {
            if (pos == 0)
            {
                // do nothing
            }
            else
            {
                for (int i = 0; i < pos; i++)
                {
                    if (allUsers[pos].highscore >= allUsers[i].highscore)
                    {
                        // swap the two users
                        User temp = allUsers[pos];
                        allUsers[pos] = allUsers[i];
                        allUsers[i] = temp;
                    }
                }
            }
        }

        for (int i = 0; i < numUsers; i++)
        {
            allUsers[i].rank = i + 1;
            print(allUsers[i].username + ", score: " + allUsers[i].highscore + ", rank: " + allUsers[i].rank);
        }
    }
}
