using UnityEngine;

public class Mrt : MonoBehaviour
{
    private Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        anim["MRT Moves"].speed = Random.Range(1, 20) / 10;
        anim.Play("MRT Moves");
    }
}
