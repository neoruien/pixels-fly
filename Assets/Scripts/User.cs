using System;
using UnityEngine.UI;

[Serializable]
public class User
{
    public string username;
    public string email;
    public string password;
    public int highscore;
    public int coins;
    public int rank;
    public int[] skins; // cell 1 contains the skin that is currently in use
    public float[] volumes;

    public User()
    {
        username = "Loading...";
        email = "error";
        password = "error";
        highscore = 0;
        coins = 0;
        rank = 999;
        skins = new int[5];
        skins[0] = -1;
        volumes = new float[2];
        volumes[0] = 0.8f;
        volumes[1] = 1f;
    }

    public User(string username, string email, string password, int highscore, int coins, int rank, int[] skins, float[] volumes)
    {
        if (email.Equals(""))
        {
            this.username = "error";
            this.email = "error";
            this.password = "error";
            this.highscore = 0;
            this.coins = 0;
            this.rank = 999;
            this.skins = new int[5];
            this.skins[0] = -1;
            this.volumes = new float[2];
            this.volumes[0] = 0.8f;
            this.volumes[1] = 1f;
        }
        else
        {
            this.username = username;
            this.email = email;
            this.password = password;
            this.highscore = highscore;
            this.coins = coins;
            this.rank = rank;
            this.skins = skins;
            this.volumes = volumes;
        }
    }
}
