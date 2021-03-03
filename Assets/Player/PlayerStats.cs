using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {NoElement,Fire,Water,Earth,Air};

class PlayerStats : MonoBehaviour
{
    public int health;
    public Element elementState;

    public void Awake() {
        Player.stats = this;
        Player.gameObject = this.gameObject;
    }
}