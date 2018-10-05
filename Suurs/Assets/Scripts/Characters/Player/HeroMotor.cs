
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class HeroMotor : CharacterMotor
{
		enum StatsEnum
		{
				None = 0,
				Dodge = 1,
				Rapira = 2,
				Jump = 3,
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
		public float _lastAttackTime = 0;

		[NonSerialized]
		public bool _blocking = false;

		public bool _canMove = true;

		
		#region Rapira
		[SerializeField]
		public float _deltaRapiraTime = 1.5f;
		[SerializeField]
		public float _deltaRapiraLength = 1.5f;
		#endregion

		#region Dodge
		[SerializeField]
		float _dodgeLength = 5f;
		[NonSerialized]
		public float _lastDodgeTime = 0;
		#endregion

		StatsEnum currentAttackEnum = StatsEnum.None;

		private void Start()
		{
				_fsm = new MyFSM(Moves);
				//_fsm.AddStates((int)StatsEnum.Dodge, Dodge);
				_fsm.AddStates((int)StatsEnum.Rapira, Rapira);
				_fsm.AddStates((int)StatsEnum.Strike_1, Strike_1);
				_fsm.AddStates((int)StatsEnum.Strike_2, Strike_2);
				_fsm.AddStates((int)StatsEnum.Strike_3, Strike_3);
		}

		private void FixedUpdate()
		{
				_fsm.Invoke();
				CheckGround();
		}

		public void FinishAllAttacks()
		{
				_attacks = false;
				_fsm.FinishAllStates();
		}

		public void Attack()
		{
				if (_deltaRapiraTime > Time.fixedTime - _lastAttackTime)
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
						currentAttackEnum = StatsEnum.Rapira;

				_fsm.RunState((int)currentAttackEnum);
				if (_fsm.GetCurrentState() == Moves)
						_fsm.FinishState();
		}

		private IEnumerator DoStrikeRoll(float time)
		{
				_attacks = true;
				Hero.instance.audioManager.Play(Hero.AudioClips.Strike_1.ToString());
				for (float t = 0; t <= time; t += Time.deltaTime)
						yield return null;

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

		private IEnumerator DoRapira(float time)
		{
				_attacks = true;
				Hero.instance.audioManager.Play(Hero.AudioClips.Rapira.ToString());
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.velocity = new Vector2(_deltaRapiraLength * transform.localScale.x, _rigidbody.velocity.y);
						yield return null;
				}
		}

		private IEnumerator DoDodge(float time)
		{
				_canMove = false;
				_blocking = false;
				_lastDodgeTime = Time.fixedTime;
				_anima.SetTrigger("Dodge");
				Hero.instance.audioManager.Play(Hero.AudioClips.Dodge.ToString());
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.velocity = new Vector2(-_dodgeLength * transform.localScale.x, _rigidbody.velocity.y);
						yield return null;
				}
				_canMove = true;
		}

		protected override bool CanMove()
		{
				return (!_attacks && /*!_rolling &&*/ !_blocking && _canMove);
		}

		public void ResetState()
		{
				//	if (_fsm.isNextState((int)StatsEnum.Strike_2) || _fsm.isNextState((int)StatsEnum.Strike_3))


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

		void Moves()
		{
				_attacks = false;
				_anima.SetInteger("Attack Index", 0);
				_anima.SetBool("Attack", false);

				if (m_Grounded && _jump)
				{
						_jump = false;
						_anima.SetBool("IsFly", true);
						m_Grounded = false;
						_rigidbody.AddForce(new Vector2(0f, _JumpForce));
				}
		}

		public void OnLanding()
		{
				_anima.SetBool("IsFly", m_Grounded);
		}

		void Rapira()
		{
				if (_anima.GetInteger("Attack Index") != (int)StatsEnum.Rapira)
						StartCoroutine(DoRapira(.33f));
				_anima.SetInteger("Attack Index", (int)StatsEnum.Rapira);
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
		
		public void StartDodge()
		{
				_fsm.FinishAllStates();
				StartCoroutine(DoDodge(0.5f));
		}

		public void Jump()
		{
				_jump = true;
				_fsm.FinishOtherStates();
		}

}
