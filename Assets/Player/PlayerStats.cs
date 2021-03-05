using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {NoElement,Fire,Water,Earth,Air};

class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;

    public int health;

    public int experience;
    public int expToNextLevel = 10;
    public Element elementState;

    public void Awake() {
        Player.stats = this;
        Player.gameObject = this.gameObject;
    }

    public void Start()
    {
        health = maxHealth;
        experience = 2;
    }
}