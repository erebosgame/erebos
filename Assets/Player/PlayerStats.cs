using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {NoElement, Fire, Earth, Water, Air};

public static class ElementMethods
{
    public static Color GetColor(this Element e)
    {
        switch (e)
        {
            case Element.Fire:
                return Color.red;
            case Element.Water:
                return Color.blue;
            case Element.Earth:
                return new Color(0.8f, 0.6f, 0.1f);
            case Element.Air:
                return Color.cyan;
            case Element.NoElement:
                return Color.white;
            default:
                return Color.magenta;
        }
    }
}

class PlayerStats : MonoBehaviour, Damageable
{
    private int maxHealth = 100;
    private float health;
    public int Health { get { return (int) health; } }
    public int MaxHealth { get { return maxHealth; } }
    public int experience;
    public int expToNextLevel = 10;

    public bool weapon;

    public Element elementState;

    public HashSet<Element> defeatedBosses;
    public Dictionary<Element, bool> unlockedElements;
    public Dictionary<Element, float> lastUse;
    public Dictionary<Element, float> cooldowns;

    public void Awake() {
        Player.stats = this;
        Player.gameObject = this.gameObject;
        weapon = true;
    }

    public void Start()
    {
        health = maxHealth;
        experience = 0;

        lastUse = new Dictionary<Element, float>();
        defeatedBosses = new HashSet<Element>();
        unlockedElements = new Dictionary<Element, bool>
        {
            { Element.Fire, false },
            { Element.Earth, false },
            { Element.Water, false },
            { Element.Air, false },
            { Element.NoElement, true }
        };
        cooldowns = new Dictionary<Element, float>
        {
            { Element.Fire, 8f },
            { Element.Earth, 10f },
            { Element.Water, 5f },
            { Element.Air, 7f },
            { Element.NoElement, 1.2f }
        };
    }
    

    public void UseSkill(Element e)
    {
        lastUse[e] = Time.time;
    }

    public bool CanUseSkill(Element e)
    {
        if (!unlockedElements[e])
            return false;

        if (lastUse.ContainsKey(e))
        {
            return (Time.time - lastUse[e]) > cooldowns[e];
        }
        else
        {
            return true;
        }
    }

    public float GetCooldownProportion(Element e)
    {
        if (!unlockedElements[e])
            return -1;
        if (!lastUse.ContainsKey(e))
            return 0;
        
        float cd = (Time.time - lastUse[e])/cooldowns[e];

        if (cd > 0)
            return cd;
        else 
            return 0;
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        if(experience > expToNextLevel)
        {
            expToNextLevel += experience;
        }
    }

    public void UnlockElement(Element e)
    {
        unlockedElements[e] = true;
        ElementUI._instance.AddElement(e);
    }
    
    public void Spawn()
    {
        Player.movement.GotoSpawnpoint();
        Camera.main.transform.forward = Player.gameObject.transform.forward;
        Player.stats.health = Player.stats.maxHealth;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public void TakeDamage(int damage)
    {
        TakeDamage((float) damage);   
    }

    public void TakeDamage(float damage)
    {
        this.health -= damage;
        if (this.health <= 0) 
        {
            OnDeath();
        }        
    }

    public void OnDeath()
    {
        FireBoss.Reset();
        
        Player.gameObject.SetActive(false);

        MenuLogic.Update(MenuState.Dead);
        Cursor.lockState = CursorLockMode.None;
        CameraLogic.instance.animator.SetTrigger("die");
    }
}