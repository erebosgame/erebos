using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {NoElement, Fire, Earth, Water, Air};

class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public int experience;
    public int expToNextLevel = 10;

    public Element elementState;

    public Dictionary<Element, float> lastUse = new Dictionary<Element, float>();
    public Dictionary<Element, float> cooldowns = new Dictionary<Element, float>();

    public void Awake() {
        Player.stats = this;
        Player.gameObject = this.gameObject;
    }

    public void Start()
    {
        health = maxHealth;
        experience = 0;

        cooldowns.Add(Element.Fire, 8f);
        cooldowns.Add(Element.Earth, 10f);
        cooldowns.Add(Element.Water, 5f);
        cooldowns.Add(Element.Air, 7f); 
        cooldowns.Add(Element.NoElement, 1.2f);        
    }

    public void UseSkill(Element e)
    {
        lastUse[e] = Time.time;
    }

    public bool CanUseSkill(Element e)
    {
        if (lastUse.ContainsKey(e))
        {
            return (Time.time - lastUse[e]) > cooldowns[e];
        }
        else
        {
            return true;
        }
    }

    public void AddExperience (int amount)
    {
        experience += amount;
        if(experience > expToNextLevel)
        {
            expToNextLevel += experience;
        }
    }
}