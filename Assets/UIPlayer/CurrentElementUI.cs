using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentElementUI : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(Player.stats.elementState)
        {
            case Element.Fire:
                image.color = Color.red;
                break;
            case Element.Water:
                image.color = Color.blue;
                break;
            case Element.Earth:
                image.color = new Color(0.8f, 0.6f, 0.1f);
                break;
            case Element.Air:
                image.color = Color.cyan;
                break;
            case Element.NoElement:
                image.color = Color.white;
                break;
            default:
                image.color = Color.magenta;
                break;
        }
    }
}
