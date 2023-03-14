using UnityEngine;
using UnityEngine.UI;

public class DisplayButton : MonoBehaviour
{
    private Shop shop;
    private int id;
    private Image displayImage;

    void Start()
    {
        shop = GameObject.Find("ShopCanvas").GetComponent<Shop>();
        id = int.Parse(name.Substring(13));
        displayImage = GetComponent<Image>();

        displayImage.color = COLOR_REF.colors[id];
    }

    public void Display()
    {
        // shop.userAvatar.color = displayImage.color;
    }
}
