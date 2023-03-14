using UnityEngine;

public class Car : MonoBehaviour
{
    private GameObject player;
    private Vector3 pos;
    private bool moved;

    void Start()
    {
        player = GameObject.Find("Player");
        pos = transform.position - transform.parent.position;
        moved = false;
    }

    void Update()
    {
        if (player.transform.position.z > pos.z + transform.parent.position.z - 15f && player.transform.position.z < pos.z + transform.parent.position.z && !moved)
        {
            GetComponent<Animation>().Play();
            moved = true;
            Invoke("Back", 5);
        }
    }

    private void Back()
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z + transform.parent.position.z);
        moved = false;
    }

}
