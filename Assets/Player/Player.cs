using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player 
{
    public static GameObject ActiveGameObject { get { return (elementGameObject) ? elementGameObject : gameObject; } }
    public static GameObject elementGameObject;
    public static GameObject gameObject;
    public static PlayerMovement movement;
    public static PlayerStats stats;
} 
