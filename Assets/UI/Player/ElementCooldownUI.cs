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
            image.color = Color.green;
        }
        else
        {
            image.color = Color.red;
        }
    }
}
