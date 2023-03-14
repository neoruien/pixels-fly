using UnityEngine;

public class Armor : MonoBehaviour
{
    private Animation lives;

    void Start()
    {
        lives = GameObject.Find("Lives").GetComponent<Animation>();
        setPosition(Random.Range(0, 2));
    }

    void Update()
    {
        GetComponent<Animation>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lives.Play("Life Gained");
            if (other.GetComponent<PlayerController>().numLives < 3) other.GetComponent<PlayerController>().numLives++;
            Invoke("Back", 3);
            gameObject.SetActive(false);
        }
    }

    private void Back()
    {
        gameObject.SetActive(true);
        setPosition(Random.Range(0, 2));
    }

    private void setPosition(int i)
    {
        if (i == 0) transform.position = new Vector3(-2.5f, 0, transform.parent.position.z);
        // else if (i == 1) transform.position = new Vector3(0, 7, transform.parent.position.z);
        else transform.position = new Vector3(2.5f, 0, transform.parent.position.z);
    }
}