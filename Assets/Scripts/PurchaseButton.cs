using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    private Shop shop;
    private int id;
    // private Image item;
    private Text priceText;
    private bool purchased;
    private bool changedButton;

    public GameObject useButton;

    void Start()
    {
        shop = GameObject.Find("ShopCanvas").GetComponent<Shop>();
        id = int.Parse(this.name.Substring(14));
        // item = GameObject.Find("DisplayButton" + id).GetComponent<Image>();
        priceText = GameObject.Find("Price" + id).GetComponent<Text>();
        useButton = GameObject.Find("UseButton" + id);
        purchased = false;
        changedButton = false;
    }

    void Update()
    {
        if (!purchased)
        {
            useButton.SetActive(false);
            foreach (int skin in shop.userData.user.skins) if (skin == id)
                {
                    purchased = true;
                    break;
                }
        }
        if (purchased & !changedButton)
        {
            changedButton = true;
            useButton.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Purchase()
    {
        int price = int.Parse(priceText.text);
        int coins = int.Parse(shop.coinsText.text);

        if (purchased)
        {
            print("Item has already been purchased");
        }
        else if (coins >= price)
        {
            shop.Purchase(id, price);
            useButton.GetComponent<UseAvatarButton>().Use();
        }
        else if (coins < price)
        {
            print("Insufficient coins");
        }
    }

}