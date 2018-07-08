using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : EnemyController
{

		[SerializeField]
		float _damageLength = 2f;

		public void ResetEnemyAttack()
		{
				if (_attacks)
				{
						_attacks = false;
						_enemyManager.ResetEnemyDealAttack();
				}
		}

		protected override bool CanMove()
		{
				return (!_attacks && !_reciveDamage && _canMove);
		}

		protected override void DoMotion()
		{
		}


		protected override void DoAttack()
		{
				int currentDeltaAttack = (int)Math.Truncate((Time.fixedTime - _lastAttackTime) * 1000);
				if ((_distance <= _deltaDistanceAttack) && !_attacks && currentDeltaAttack > _deltaTimeAttack && !_reciveDamage)
				{
						_attacks = true;
						_anima.SetTrigger("Attack");
						_lastAttackTime = Time.fixedTime;
				}
		}

		public override void TakeHit(float damage)
		{
				_reciveDamage = true;

				_enemyManager._HP -= damage;
				if (_enemyManager._HP <= 0)
						return;
				_anima.SetTrigger("TakeHit");
				_reciveDamage = false;
		}

}
