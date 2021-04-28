using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCooldownUI : MonoBehaviour
{
    public Image imageCD;
    public Image imageBG;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float perc;
        if (Player.stats.weapon)
        {
            perc = Player.stats.GetCooldownProportion(Element.NoElement);
            Debug.Log(perc);
        }
        else
        {
            perc = (Time.time - Player.stats.lastSlingUse)/Player.stats.slingCooldown;
            if (perc > 1)
                perc = 1;
            if (perc < 0)
                perc = 0;
        }

        imageCD.fillAmount = perc;
        if (perc < 1) 
        {
            imageCD.color = new Color(1f,1f,1f, 0.5f);
        }
        else
        {
            imageCD.color = new Color(1f,1f,1f);
        }
    }
}
