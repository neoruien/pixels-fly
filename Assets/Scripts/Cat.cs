using UnityEngine;

public class Cat : MonoBehaviour
{
    private int rand;
    private Animation catAnim;
    private Animation carAnim;

    void Start()
    {
        rand = Random.Range(1, 20) / 10;
        catAnim = GetComponent<Animation>();
        carAnim = gameObject.transform.parent.GetChild(2).gameObject.GetComponent<Animation>();
        catAnim["Cat Runs"].speed = rand;
        carAnim["Car with Cat"].speed = rand;
    }

    void Update()
    {
        catAnim.Play("Cat Runs");
        carAnim.Play("Car with Cat");
        Refresh();
    }

    private void Refresh()
    {
        rand = Random.Range(1, 20) / 10;
        catAnim["Cat Runs"].speed = rand;
        carAnim["Car with Cat"].speed = rand;
    }

}