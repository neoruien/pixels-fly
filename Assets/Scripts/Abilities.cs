using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{

    public Image abilityImage1;
    public float cooldown1 = 5;
    private bool isCooldown1 = false;
    public float castDuration1 = 1;
    private bool isCasting1 = false;
    public GameObject glow1;

    public Image abilityImage2;
    public float cooldown2 = 50;
    private bool isCooldown2 = false;
    public float castDuration2 = 10;
    private bool isCasting2 = false;
    public GameObject glow2;

    public Image abilityImage3;
    public float cooldown3 = 40;
    private bool isCooldown3 = false;
    public float castDuration3 = 10;
    private bool isCasting3 = false;
    public GameObject glow3;

    private GameObject player;

    void Start()
    {
        abilityImage1.fillAmount = 1;
        abilityImage2.fillAmount = 1;
        abilityImage3.fillAmount = 1;

        player = GameObject.Find("Player");
    }

    void Update()
    {
        AbilityReset1();
        AbilityReset2();
        AbilityReset3();
    }

    public void Ability1()
    {
        if (!isCooldown1 && player.GetComponent<PlayerController>().numLives < 3)
        {
            isCasting1 = true;
            glow1.SetActive(true);
            player.GetComponent<PlayerController>().numLives++;
            StartCoroutine(CastAbility(1));
        }
    }

    void AbilityReset1()
    {
        if (!isCasting1 && isCooldown1)
        {
            abilityImage1.fillAmount += 1 / cooldown1 * Time.deltaTime;

            if (abilityImage1.fillAmount >= 1)
            {
                abilityImage1.fillAmount = 1;
                isCooldown1 = false;
            }
        }
    }

    public void Ability2()
    {
        if (!isCooldown2)
        {
            isCasting2 = true;
            glow2.SetActive(true);
            Coin.isAbility2 = true;
            StartCoroutine(CastAbility(2));
        }
    }

    void AbilityReset2()
    {
        if (!isCasting2 && isCooldown2)
        {
            abilityImage2.fillAmount += 1 / cooldown2 * Time.deltaTime;

            if (abilityImage2.fillAmount >= 1)
            {
                abilityImage2.fillAmount = 1;
                isCooldown2 = false;
            }
        }
    }

    public void Ability3()
    {
        if (!isCooldown3)
        {
            isCooldown3 = true;
            glow3.SetActive(true);
            player.GetComponent<PlayerController>().maxHeight += 5;
            StartCoroutine(CastAbility(3));
        }
    }

    void AbilityReset3()
    {
        if (!isCasting3 && isCooldown3)
        {
            abilityImage3.fillAmount += 1 / cooldown3 * Time.deltaTime;

            if (abilityImage3.fillAmount >= 1)
            {
                abilityImage3.fillAmount = 1;
                isCooldown3 = false;
            }
        }
    }

    public IEnumerator CastAbility(int abilityIndex)
    {
        if (abilityIndex == 1)
        {
            yield return new WaitForSeconds(castDuration1);

            glow1.SetActive(false);

            isCasting1 = false;
            isCooldown1 = true;
            abilityImage1.fillAmount = 0;
        }

        if (abilityIndex == 2)
        {
            yield return new WaitForSeconds(castDuration2);

            glow2.SetActive(false);

            Coin.isAbility2 = false;
            isCasting2 = false;
            isCooldown2 = true;
            abilityImage2.fillAmount = 0;
        }

        if (abilityIndex == 3)
        {
            yield return new WaitForSeconds(castDuration3);

            glow3.SetActive(false);

            player.GetComponent<PlayerController>().maxHeight -= 5;
            isCasting3 = false;
            isCooldown3 = true;
            abilityImage3.fillAmount = 0;
        }
    }
}
