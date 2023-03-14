using Proyecto26;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform player;
    public UserData userData;
    public bool gameOver;
    public bool paused;
    public bool rankUpdated;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        paused = false;
        rankUpdated = false;
        Time.timeScale = 1; // time passes at normal rate
        userData = GameObject.Find("Controller").GetComponent<UserData>();

        // set player color
        /*
        GameObject playerBody = GameObject.Find("Body");
        if (playerBody.transform.parent.name.Equals("Pigeon"))
            playerBody.GetComponent<Renderer>().materials[2].color = userData.user.skins[0] > 0 
                ? COLOR_REF.colors[userData.user.skins[0]]
                : COLOR_REF.colors[0]; */
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0; // time freezes
            gameOverPanel.SetActive(true);
            gamePanel.SetActive(false);
            if (!rankUpdated)
            {
                rankUpdated = true;
                userData.user.highscore = Mathf.Max(userData.user.highscore, Mathf.RoundToInt((player.position.z - 15) / 30));
                userData.UpdateRank();
            }
        } else if (paused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            gamePanel.SetActive(false);
            if (!rankUpdated)
            {
                rankUpdated = true;
                userData.user.highscore = Mathf.Max(userData.user.highscore, Mathf.RoundToInt((player.position.z - 15) / 30));
                userData.UpdateRank();
            }
        } else if (!paused)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            gamePanel.SetActive(true);
        }
    }

    public void OnSubmit()
    {
        if (!userData.user.email.Equals("Guest")) userData.PostToDatabaseWithRankings();
    }
}
