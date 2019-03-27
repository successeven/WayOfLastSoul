using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Attacks/Item")]
[System.Serializable]
public class AttackItem : ScriptableObject
{
    new public string name = "New Item";  // Name of the item
    public int _ID;
    public float _damage;
    public GameObject _EffectRightSide;
    public GameObject _EffectLeftSide;
}
