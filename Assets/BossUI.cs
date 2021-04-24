using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public Image hpFill;
    public static BossUI instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public static void SetActive(bool value)
    {
        instance.gameObject.SetActive(value);
    }

    public static void UpdateHealth(float percentage)
    {
        instance.hpFill.fillAmount = percentage;
    }
}
