using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Transform player;
    public Text text;
    public Text gameOverUIScore;

    // Update is called once per frame
    void Update()
    {
        text.text = ((player.position.z - 15) / 30).ToString("00000");
        gameOverUIScore.text = "Score: " + ((player.position.z - 15) / 30).ToString("0");
    }
}