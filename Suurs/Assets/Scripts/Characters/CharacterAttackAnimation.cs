﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackAnimation : MonoBehaviour
{

		Animator _anim;
		public List<AttackItem> _attackItems;
		int _currentAttackIndex = 0;

		// Use this for initialization
		void Start()
		{
				_anim = GetComponent<Animator>();
		}

		// Update is called once per frame
		void FixedUpdate()
		{
				foreach (var item in _attackItems)
						if (_anim.GetCurrentAnimatorStateInfo(0).IsTag(item._ID.ToString()))
						{
								_currentAttackIndex = item._ID;
								return;
						}
				_currentAttackIndex = 0;
		}
}
