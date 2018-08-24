using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Attacks/Item")]
[System.Serializable]
public class AttackItem : ScriptableObject
{
    new public string name = "New Item";  // Name of the item
    public int _ID;
    public AnimationClip _clip;
    public bool _isCombo;
    public string _previousState;
    public string _nextState;
    public float _damage;

}
