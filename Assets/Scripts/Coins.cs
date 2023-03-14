using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    public Transform player;
    public Text text;
    public Text gameOverUICoins;

    // Update is called once per frame
    void Update()
    {
        text.text = player.GetComponent<PlayerController>().numCoins.ToString("000");
        gameOverUICoins.text = "Coins: " + player.GetComponent<PlayerController>().numCoins.ToString("0");
    }
}