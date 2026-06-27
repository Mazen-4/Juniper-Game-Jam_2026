using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class lootItem 
{
    public GameObject itemPrefab;
    [Range(0, 100)] public float dropChance;
}
