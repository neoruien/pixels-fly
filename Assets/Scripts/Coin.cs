using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    private User user;
    private Animation coinBg;
    private Vector3 pos;

    // Training mode
    private GameObject player;
    private bool training = false;

    private float moveSpeed = 40f;

    public static bool isAbility2 = false;

    void Start()
    {
        user = GameObject.Find("Controller").GetComponent<UserData>().user;
        coinBg = GameObject.Find("CoinsBg").GetComponent<Animation>();
        pos = transform.position - transform.parent.position;
        if (transform.position.y == 1) setPosition(Random.Range(0, 3));

        player = GameObject.Find("Player");
        if (SceneManager.GetActiveScene().name.Equals("Level (Training)")) training = true;
    }

    void Update()
    {
        GetComponent<Animation>().Play();

        if (isAbility2 && transform.position.z - player.transform.position.z < 15)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
            moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            coinBg.Play("Coin Gained");
            other.GetComponent<PlayerController>().numCoins++;
            user.coins++;
            if (training)
            {
                player.GetComponent<PlayerControllerAI>().AddReward(50f);
                Debug.Log(player.GetComponent<PlayerControllerAI>().GetCumulativeReward());
            }
            Invoke("Back", 3);
            gameObject.SetActive(false);
        }
    }

    private void Back()
    {
        gameObject.SetActive(true);
        if (transform.position.y == 1) setPosition(Random.Range(0, 3));
    }

    private void setPosition(int i)
    {
        if (i == 0) transform.position = new Vector3(-2.5f, 1, pos.z + transform.parent.position.z);
        else if (i == 1) transform.position = new Vector3(2.5f, 1, pos.z + transform.parent.position.z);
        else
        {
            if (transform.parent.name == "Tile 4(Clone)") transform.position = new Vector3(0, 8.5f, pos.z + transform.parent.position.z);
            else if (transform.parent.name == "Tile 1(Clone)" || transform.parent.name == "Tile 3(Clone)" || transform.parent.name == "Tile 10(Clone)")
                transform.position = new Vector3(0, 7, pos.z + transform.parent.position.z);
            else transform.position = new Vector3(0, 1, pos.z + transform.parent.position.z);
        }
    }
}