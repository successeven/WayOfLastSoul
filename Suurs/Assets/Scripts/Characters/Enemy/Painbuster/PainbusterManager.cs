using System;
using UnityEngine;

public class PainbusterManager : AliveObject
{

		[SerializeField]
		float _attack = 10f;

		public bool _canMove = true;

		[NonSerialized]
		public bool _dead = false;

		[NonSerialized]
		public Animator _anima;

		private void Start()
		{
				_anima = GetComponent<Animator>();
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
				if (collision.tag == "Player")
				{
						Explosion();
						Hero.instance.Manager.TakeDamage(_attack);
				}
		}

		public void FallToPieces()
		{
				_dead = true;
				_anima.SetBool("FallToPieces", true);
				Invoke("Death", 2f);
		}

		public void Explosion()
		{
				_dead = true;
				_anima.SetBool("Explosion", true);
				Invoke("Death", 2f);
		}

		public override void TakeHit(float damage, int attackID)
		{
				FallToPieces();
		}

		protected override void Death()
		{
				Die();
		}
}
