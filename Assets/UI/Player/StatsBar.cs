using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    Canvas canvas;
    public Image hpBar;
    public Image xpBar;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = (float) Player.stats.health / Player.stats.maxHealth;
        xpBar.fillAmount = (float) Player.stats.experience / Player.stats.expToNextLevel;
    }
}
