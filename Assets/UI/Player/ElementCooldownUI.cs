using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ElementCooldownUI : MonoBehaviour
{
    public Element element;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (Player.stats.CanUseSkill(element))
        {
            image.color = element.GetColor();
        }
        else
        {
            float h,s,v;
            Color.RGBToHSV(element.GetColor(), out h, out s, out v);
            image.color = Color.HSVToRGB(h,s*0.5f,v*0.8f);
        }
    }
}
