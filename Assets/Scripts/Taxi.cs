using UnityEngine;

public class Taxi : MonoBehaviour
{
    private int rand;
    private Animation anim;
    private int numAni = 2;

    void Start()
    {
        rand = Random.Range(0, numAni);
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        if (rand == 0)
        {
            anim["Taxi Moves"].speed = Random.Range(1, 20) / 10;
            anim.Play("Taxi Moves");
        }
        if (rand == 1)
        {
            anim["Taxi Moves 2"].speed = Random.Range(1, 20) / 10;
            anim.Play("Taxi Moves 2");
        }
        if (!gameObject.activeSelf) Invoke("Refresh", 5);
    }

    private void Refresh()
    {
        rand = Random.Range(0, numAni);
    }

}
