using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;

public class DataBridge : MonoBehaviour
{
    public Text usernameInput, passwordInput;

    private Player data;

    private string dataURL = "https://pixels-fly.firebaseio.com/";

    private DatabaseReference databaseReference;

    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dataURL);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveData()
    {
        if (usernameInput.text.Equals("") || passwordInput.text.Equals(""))
        {
            print("no data");
            return;
        }

        data = new Player(usernameInput.text, passwordInput.text);

        string jsonData = JsonUtility.ToJson(data);

        databaseReference.Child("User_" + Random.Range(0, 1000000)).
            SetRawJsonValueAsync(jsonData);
    }

    public void LoadData()
    {
        FirebaseDatabase.DefaultInstance.GetReferenceFromUrl(dataURL).GetValueAsync().
            ContinueWith((task) =>
            {
            if (task.IsCanceled)
            {

            }

            if (task.IsFaulted)
            {

            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string playerData = snapshot.GetRawJsonValue();
                foreach (var child in snapshot.Children)
                {
                        string t = child.GetRawJsonValue();
                        Player extractedData = JsonUtility.FromJson<Player>(t);
                        print("The player is " + extractedData.username);
                        print("The player's pass is " + extractedData.password);
                    }
                    Player m = JsonUtility.FromJson<Player>(playerData);
                }
            });
    }
}
