using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementUI : MonoBehaviour
{
    public GameObject currentElementPrefab;
    private GameObject currentElement;

    public GameObject elementPrefab;
    private Dictionary<Element, GameObject> elements;

    // Start is called before the first frame update
    void Start()
    {
        elements = new Dictionary<Element, GameObject>();
        currentElement = Instantiate(currentElementPrefab, this.transform);

        AddElement(Element.Fire);
        AddElement(Element.Earth);
        AddElement(Element.Water);
        AddElement(Element.Air);

        UpdateElementPosition();
    }

    // Update is called once per frame
    void Update()
    {
         
    }
    void AddElement(Element e)
    {
        elements[e] = Instantiate(elementPrefab, this.transform);
        elements[e].GetComponent<ElementCooldownUI>().element = e;
    }

    void UpdateElementPosition()
    {
        int count = elements.Count;
        int found = 0;
        for (Element e = (Element)1; e < (Element)5; e++)
        {
            if (!elements.ContainsKey(e))
                continue;
            found++;
            float angle = (Mathf.PI/2) * found / (count+1);
            //TODO: migliorare posizione
            //print(angle);
            //print(new Vector3(-Mathf.Cos(angle), Mathf.Sin(angle), 0)*15);
            elements[e].GetComponent<RectTransform>().anchoredPosition =
                new Vector3(-Mathf.Cos(angle)*300 - 25 + 30, 
                            Mathf.Sin(angle)*300 - 25 - 30, 0);
        }
    }
}
