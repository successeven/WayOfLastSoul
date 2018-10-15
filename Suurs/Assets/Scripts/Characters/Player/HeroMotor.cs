
using System;
using System.Collections;
using UnityEngine;

public class HeroMotor : CharacterMotor
{
		enum StatsEnum
		{
				None = 0,
				Uppercut = 1,
				Shield_Attack = 2,
				Strike_1 = 4,
				Strike_2 = 5,
				Strike_3 = 6
		}

		MyFSM _fsm;

		public int AttackIndex
		{
				get
				{
						return _anima.GetInteger("Attack Index");
				}
		}

		[SerializeField]
		float _attacksLength = 2f;

		[NonSerialized]
		public bool _attacks = false;

		[NonSerialized]
		public bool _jump = false;

		[NonSerialized]
		public bool _dodge = false;

		[NonSerialized]
		public float _lastAttackTime = 0;

		[NonSerialized]
		public bool _blocking = false;

		public bool _canMove = true;


		#region Shield_Attack
		[SerializeField]
		public float _deltaShield_AttackTime = 1.5f;
		[SerializeField]
		float _deltaShield_AttackLength = 1.5f;
		[SerializeField]
		float _deltaShield_AttackHeight = 1.5f;
		[SerializeField]
		float _deltaShield_AttackForceX = 1.5f;
		#endregion

		[Space(10)]
		#region Dodge
		bool _isDodging = false;
		[SerializeField]
		float _dodgeLength = 5f;
		[SerializeField]
		float _dodgeHeight = 5f;
		[NonSerialized]
		public float _lastDodgeTime = 0;
		#endregion

		StatsEnum currentAttackEnum = StatsEnum.None;

		private void Start()
		{
				_fsm = new MyFSM(Moves);
				_fsm.AddStates((int)StatsEnum.Shield_Attack, Shield_Attack);
				_fsm.AddStates((int)StatsEnum.Strike_1, Strike_1);
				_fsm.AddStates((int)StatsEnum.Strike_2, Strike_2);
				_fsm.AddStates((int)StatsEnum.Strike_3, Strike_3);
				_fsm.AddStates((int)StatsEnum.Uppercut, DoUppercut);
		}

		private void FixedUpdate()
		{
				if (Hero.instance.Manager._Health > 0)
				{
						_fsm.Invoke();
						CheckGround();
				}				
		}

		public void FinishAllAttacks()
		{
				_attacks = false;
				_fsm.FinishAllStates();
		}

		protected override bool CanMove()
		{
				return (!_attacks && !_isDodging && !_blocking && _canMove);
		}

		public void Attack()
		{
				if (_deltaShield_AttackTime > Time.fixedTime - _lastAttackTime)
				{
						_lastAttackTime = Time.fixedTime;
						if (_fsm.GetCurrentState() == Moves)
						{
								currentAttackEnum = StatsEnum.Strike_1;
						}
						else if (_fsm.GetCurrentState() != Strike_3)
								currentAttackEnum++;
						else
								return;
				}
				else
						currentAttackEnum = StatsEnum.Shield_Attack;

				_fsm.RunState((int)currentAttackEnum);
				if (_fsm.GetCurrentState() == Moves)
						_fsm.FinishState();
		}

		void Moves()
		{
				_attacks = false;
				_anima.SetInteger("Attack Index", 0);
				_anima.SetBool("Attack", false);

				if (m_Grounded && _jump)
				{
						_jump = false;
						m_Grounded = false;
						_rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, _JumpForce), ForceMode2D.Impulse);
						_anima.SetTrigger("Jump");
				}

				if (m_Grounded && _dodge)
				{
						_dodge = false;
						if (!_isDodging)
						{
								StartCoroutine(DoDodge(.3f));
								_anima.SetTrigger("Dodge");
						}
				}
		}

		public void ResetState()
		{
				_fsm.FinishState();
		}

		public void SetBlock()
		{
				_fsm.FinishAllStates();
				_blocking = true;
				_anima.SetBool("Blocking", _blocking);
		}

		public void UnSetBlock()
		{
				_blocking = false;
				_anima.SetBool("Blocking", _blocking);
		}

		public void OnLanding()
		{
				_anima.SetBool("IsFly", !m_Grounded);
		}

		Vector2 _startPos;
		void Shield_Attack()
		{
				if (_anima.GetInteger("Attack Index") != (int)StatsEnum.Shield_Attack)
				{
						_startPos = transform.position;
						StartCoroutine(DoShield_Attack(2.6f, _startPos));
				}
				_anima.SetInteger("Attack Index", (int)StatsEnum.Shield_Attack);
				_anima.SetBool("Attack", true);
		}
		
		Coroutine AttackCoroutine;
		void Strike_1()
		{
				if (_anima.GetInteger("Attack Index") != (int)StatsEnum.Strike_1)
				{
						AttackCoroutine = StartCoroutine(DoAttack(Hero.AudioClips.Strike_1.ToString(), 0.6f));
				}
				_anima.SetInteger("Attack Index", (int)StatsEnum.Strike_1);
				_anima.SetBool("Attack", true);
		}

		void Strike_2()
		{
				if (_anima.GetInteger("Attack Index") != (int)StatsEnum.Strike_2)
				{
						StopCoroutine(AttackCoroutine);
						AttackCoroutine = StartCoroutine(DoAttack(Hero.AudioClips.Strike_2.ToString(), .03f));
				}
				_anima.SetInteger("Attack Index", (int)StatsEnum.Strike_2);
				_anima.SetBool("Attack", true);
		}

		void Strike_3()
		{
				if (_anima.GetInteger("Attack Index") != (int)StatsEnum.Strike_3)
				{
						StopCoroutine(AttackCoroutine);
						AttackCoroutine = StartCoroutine(DoAttack(Hero.AudioClips.Strike_3.ToString(), .4f));
				}
				_anima.SetInteger("Attack Index", (int)StatsEnum.Strike_3);
				_anima.SetBool("Attack", true);
		}
		
		public void Jump()
		{
				_jump = true;
				_fsm.FinishOtherStates();
		}

		public void Uppercut()
		{
				_fsm.RunState((int)StatsEnum.Uppercut);
				if (_fsm.GetCurrentState() == Moves)
						_fsm.FinishState();
		}

		void DoUppercut()
		{
				Debug.Log("Апперкот!");
				if (_anima.GetInteger("Attack Index") != (int)StatsEnum.Uppercut)
				{
						Hero.instance.audioManager.Play(Hero.AudioClips.Uppercut.ToString());
				}
				_anima.SetInteger("Attack Index", (int)StatsEnum.Uppercut);
				_anima.SetBool("Attack", true);
		}


		public void Dodge()
		{
				_dodge = true;
				_fsm.FinishAllStates();
		}
		
		private IEnumerator DoDodge(float time)
		{
				_isDodging = true;
				Hero.instance.audioManager.Play(Hero.AudioClips.Dodge.ToString());
				yield return new WaitForSeconds(.04f);
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.AddForce(new Vector2(-_dodgeLength * transform.localScale.x, _dodgeHeight), ForceMode2D.Impulse);
						yield return null;
				}
				_isDodging = false;
		}

		private IEnumerator DoAttack(string AudioClipName, float time)
		{
				_attacks = true;
				Hero.instance.audioManager.Play(AudioClipName);
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.velocity = new Vector2(_attacksLength * transform.localScale.x, _rigidbody.velocity.y);
						yield return null;
				}

		}

		private IEnumerator DoShield_Attack(float time, Vector2 startPos)
		{
				_attacks = true;
				Hero.instance.audioManager.Play(Hero.AudioClips.Shield_Attack.ToString());
				yield return new WaitForSeconds(1f);
				Vector2 EndPos = startPos;
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						if (transform.position.x > startPos.x + _deltaShield_AttackLength)
						{
								EndPos.x = startPos.x + (_deltaShield_AttackLength * transform.localScale.x);
								EndPos.y = transform.position.y;
								transform.position = EndPos;
								_rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 2, 0);
								yield break;
						}
						else
						{
								_rigidbody.AddForce(new Vector2(_deltaShield_AttackForceX * transform.localScale.x, _deltaShield_AttackHeight), ForceMode2D.Impulse);
								yield return null;
						}
				}
		}

}
