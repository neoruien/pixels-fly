using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public UserData userData;
    // public Image userAvatar;
    public Text coinsText;

    public Image displayImage;

    void Start()
    {
        userData = GameObject.Find("Controller").GetComponent<AuthController>().userData;
        // userAvatar = GameObject.Find("Avatar").GetComponent<Image>();
        coinsText = GameObject.Find("MyMoney").GetComponent<Text>();

        /* userAvatar.color = userData.user.skins[0] > 0
            ? COLOR_REF.colors[userData.user.skins[0]]
            : COLOR_REF.colors[0]; */
        coinsText.text = userData.user.coins.ToString();
    }

    public void LoadMenu()
    {
        if (!userData.user.email.Equals("Guest")) userData.PostToDatabase();
        SceneManager.LoadScene("Menu");
    }

    public void Purchase(int id, int price)
    {
        userData.user.coins -= price;
        for (int i = 0; i < userData.user.skins.Length; i++)
        {
            if (userData.user.skins[i] == 0)
            {
                userData.user.skins[i] = id;
                break;
            }
        }
        print("Purchased " + id);

        coinsText.text = userData.user.coins.ToString();

        if (!userData.user.email.Equals("Guest")) userData.PostToDatabase();
    }
}
