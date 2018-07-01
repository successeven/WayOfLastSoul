using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAttackAnimation : MonoBehaviour
{

		Animator _anim;
		public AttackItem _currentAttackItem = null;

		// Use this for initialization
		void Start()
		{
				_anim = GetComponent<Animator>();
		}

		void FixedUpdate()
		{
				//foreach (var item in _attackItems)
				//		if (_anim.GetCurrentAnimatorStateInfo(0).IsTag(item._ID.ToString()))
				//		{
				//				_currentAttackItem = item;
				//				_currentAttackIndex = item._ID;
				//				return;
				//		}
				//_currentAttackItem = null;
				//_currentAttackIndex = 0;
		}
}
