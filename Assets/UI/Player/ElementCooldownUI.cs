using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ElementCooldownUI : MonoBehaviour
{
    public Element element;
    public Image imageCD;
    public Image imageBG;

    // Start is called before the first frame update
    void Start()
    {
        imageCD.color = element.GetColor();
        imageBG.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    { 
        if (Player.stats.CanUseSkill(element))
        {
            imageCD.fillAmount = 1;
            imageCD.color = element.GetColor();
        }
        else
        {
            imageCD.fillAmount = Player.stats.GetCooldownProportion(element);
            float h,s,v;
            Color.RGBToHSV(element.GetColor(), out h, out s, out v);
            imageCD.color = Color.HSVToRGB(h,s*0.5f,v*0.8f);
        }
    }
}
