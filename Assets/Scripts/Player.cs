using System;

[Serializable]
public class Player
{
    public string username;
    public string password;

    public Player() { }

    public Player(string name, string pass)
    {
        username = name;
        password = pass;
    }
}
