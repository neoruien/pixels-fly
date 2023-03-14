using UnityEngine;
using UnityEngine.UI;

public class UseAvatarButton : MonoBehaviour
{
    private Shop shop;
    private int id;
    private Text useText;
    // private Image item;

    void Start()
    {
        shop = GameObject.Find("ShopCanvas").GetComponent<Shop>();
        id = int.Parse(name.Substring(9));
        useText = GameObject.Find("UseText" + id).GetComponent<Text>();
        // if (id > 0) item = GameObject.Find("DisplayButton" + id).GetComponent<Image>();
    }

    void Update()
    {
        if (id == 0)
        {
            if (shop.userData.user.skins[0] == -1)
            {
                gameObject.GetComponent<Image>().color = new Color(255f / 255f, 220f / 255f, 255f / 255f);
                useText.text = "In Use";
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                useText.text = "Use";
            }
        } else
        {
            if (shop.userData.user.skins[0] == id)
            {
                gameObject.GetComponent<Image>().color = new Color(255f / 255f, 220f / 255f, 255f / 255f);
                useText.text = "In Use";
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                useText.text = "Use";
            }
        }
        /*
        if (id > 0)
        {
            if (shop.userAvatar.color.Equals(item.color))
            {
                gameObject.GetComponent<Image>().color = new Color(224f / 255f, 66f / 255f, 149f / 255f);
                useText.text = "In Use";
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(224f / 255f, 183f / 255f, 240f / 255f);
                useText.text = "Use";
            }
        }
        else
        {
            if (shop.userAvatar.color.Equals(COLOR_REF.colors[0]))
            {
                gameObject.GetComponent<Image>().color = new Color(224f / 255f, 66f / 255f, 149f / 255f);
                useText.text = "In Use";
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(224f / 255f, 183f / 255f, 240f / 255f);
                useText.text = "Default";
            }
        }
        */
    }

    public void Use()
    {
        // shop.userAvatar.color = item.color;
        ChangeSkin();
    }

    public void Default()
    {
        // shop.userAvatar.color = COLOR_REF.colors[0];
        ChangeSkin();
    }

    private void ChangeSkin()
    {
        int skinID = id > 0 ? id : -1;
        int currSkin = shop.userData.user.skins[0];
        int prevSkinLoc = 0;
        for (int i = 0; i < shop.userData.user.skins.Length; i++)
        {
            if (skinID == shop.userData.user.skins[i]) prevSkinLoc = i;
        }
        shop.userData.user.skins[0] = skinID;
        shop.userData.user.skins[prevSkinLoc] = currSkin;
    }

}