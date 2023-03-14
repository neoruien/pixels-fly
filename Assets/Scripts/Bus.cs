using UnityEngine;

public class Bus : MonoBehaviour
{
    private GameObject player;
    private Vector3 pos;
    private Quaternion rot;
    private bool moved;

    private int rand;
    private Animation anim;
    private int numAni = 2;

    void Start()
    {
        player = GameObject.Find("Player");
        pos = transform.position - transform.parent.position;
        rot = transform.rotation;
        moved = false;
        rand = Random.Range(0, numAni);
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        if (player.transform.position.z > pos.z + transform.parent.position.z - 15f & player.transform.position.z < pos.z + transform.parent.position.z & !moved)
        {
            if (rand == 0)
            {
                anim["Bus Moves"].speed = 0.9f * player.GetComponent<PlayerController>().forwardSpeed / player.GetComponent<PlayerController>().defaultForwardSpeed;
                anim.Play("Bus Moves");
            }
            else if (rand == 1)
            {
                anim["Bus Flips"].speed = 0.9f * player.GetComponent<PlayerController>().forwardSpeed / player.GetComponent<PlayerController>().defaultForwardSpeed;
                anim.Play("Bus Flips");
            }
            moved = true;
            Invoke("Back", 3);
        }
    }

    private void Back()
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z + transform.parent.position.z);
        transform.rotation = rot;
        moved = false;
        rand = Random.Range(0, numAni);
    }

}
