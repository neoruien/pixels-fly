using UnityEngine;

public class Erp : MonoBehaviour
{
    private User user;
    private GameObject player;
    private Vector3 pos;
    private Quaternion rot;
    private bool taxCollected;
    private bool collapsed;
    private int rand;
    private Animation anim;
    private GameObject erpFallingPrompt;

    private Animation coinBg;
    private Animation birdDies;

    void Start()
    {
        user = GameObject.Find("Controller").GetComponent<UserData>().user;
        player = GameObject.Find("Player");
        pos = transform.position - transform.parent.position;
        rot = transform.rotation;
        taxCollected = false;
        collapsed = false;
        rand = Random.Range(0, 10);
        anim = GetComponent<Animation>();
        erpFallingPrompt = GameObject.Find("Game").transform.GetChild(5).gameObject;

        coinBg = GameObject.Find("CoinsBg").GetComponent<Animation>();
        birdDies = GameObject.Find("Canvas").GetComponent<Animation>();
    }

    void Update()
    {
        if ((player.transform.position.z > pos.z + transform.parent.position.z & player.transform.position.z < pos.z + transform.parent.position.z + 10f & !taxCollected) &&
            player.transform.position.y < 15)
        {
            taxCollected = true;
            if (player.GetComponent<PlayerController>().numCoins > 0)
            {
                coinBg.Play("Coin Lost");
                player.GetComponent<PlayerController>().numCoins--;
                user.coins--;
            }
            else
            {
                birdDies.Play();
                player.GetComponent<PlayerController>().numLives--;
            }
            Invoke("Back", 5);
        }

        if (rand == 0 & !collapsed && player.transform.position.z > pos.z + transform.parent.position.z - 45f && player.transform.position.z < pos.z + transform.parent.position.z)
        {
            anim["Erp Falls Forward"].speed = 0.5f * player.GetComponent<PlayerController>().forwardSpeed / player.GetComponent<PlayerController>().defaultForwardSpeed;
            anim.Play("Erp Falls Forward");
            collapsed = true;
        }
        else if (rand == 1 & !collapsed && player.transform.position.z > pos.z + transform.parent.position.z - 30f && player.transform.position.z < pos.z + transform.parent.position.z)
        {
            anim["Erp Falls Backward"].speed = 0.5f * player.GetComponent<PlayerController>().forwardSpeed / player.GetComponent<PlayerController>().defaultForwardSpeed;
            anim.Play("Erp Falls Backward");
            collapsed = true;
        }
        else if (rand == 0 & !collapsed && player.transform.position.z > pos.z + transform.parent.position.z - 60f && player.transform.position.z < pos.z + transform.parent.position.z)
        {
            erpFallingPrompt.SetActive(true);
        }
        else if (rand == 1 & !collapsed && player.transform.position.z > pos.z + transform.parent.position.z - 45f && player.transform.position.z < pos.z + transform.parent.position.z)
        {
            erpFallingPrompt.SetActive(true);
        }
        else if (collapsed & transform.position.y == 1)
        {
            erpFallingPrompt.SetActive(false);
        }
    }
    private void Back()
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z + transform.parent.position.z);
        transform.rotation = rot;
        taxCollected = false;
        collapsed = false;
        rand = Random.Range(0, 10);
    }
}