using UnityEngine;

public class COLOR_REF : MonoBehaviour
{
    public static Color[] colors;
    // Start is called before the first frame update
    void Awake()
    {
        colors = new Color[5];
        colors[0] = new Color(67f / 255f, 64f / 255f, 22f / 255f); // original green
        colors[1] = new Color(106f / 255f, 28f / 255f, 88f / 255f); // purple
        colors[2] = new Color(93f / 255f, 103f / 255f, 255f / 255f); // blue
        colors[3] = new Color(255f / 255f, 60f / 255f, 60f / 255f); // red
        colors[4] = new Color(255f / 255f, 196f / 255f, 9f / 255f); // yellow
    }
}
