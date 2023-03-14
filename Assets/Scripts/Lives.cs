using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public Transform player;
    private PlayerManager playerManager;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.GetComponent<PlayerController>().numLives) hearts[i].sprite = fullHeart;
            else hearts[i].sprite = emptyHeart;
        }
        if (player.GetComponent<PlayerController>().numLives == 0) playerManager.gameOver = true;
    }
}