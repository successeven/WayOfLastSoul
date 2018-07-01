using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Attacks/Item")]
public class AttackItem : ScriptableObject
{
		new public string name = "New Item";  // Name of the item
		public int _ID;
		public AnimationClip _clip;
		public bool _isCombo;
		public int? _previousAtackID;
		public float _damage;

}
