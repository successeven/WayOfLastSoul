using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(GhostMotor))]
public class GhostManager : EnemyManager
{
    [NonSerialized]
		public bool _isAgressive = false;
		AudioManager _audioManager;
		GhostMotor _motor;

		protected Animator _anima;

		protected override void SetStartSkills()
		{
				_anima = GetComponent<Animator>();
				_audioManager = GetComponent<AudioManager>();
				_motor = GetComponent<GhostMotor>();
		}

		private void Update()
		{
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ghost"), !IsAttack() && !_isAgressive);
		}

		protected override bool IsAttack()
		{
					return _motor._attacks;
		}

		protected override void Death()
		{
				_audioManager.Play(GhostSounds.Death.ToString());
				_anima.SetTrigger("Death");
		}

		public override void TakeHit(float damage, int attackID)
		{
				if (attackID != _dealAttackID)
				{
                    _motor.ResetAllState();
						_dealAttackID = attackID;
						_reciveDamage = true;
						_isAgressive = true;
						_audioManager.Play(GhostSounds.When_Hit.ToString());
						_HP -= damage;
						if (_HP <= 0)
								return;
						_anima.SetTrigger("TakeHit");
				}
		}
}
