using System;
using UnityEngine;

public enum AudioPainbuster
{
		None,
		Stalking,
		Explosion,
		Death
}

public class PainbusterManager : AliveObject
{

		[SerializeField]
		float _attack = 10f;

		public bool _canMove = true;

		[NonSerialized]
		public bool _dead = false;

		[NonSerialized]
		public Animator _anima;
		[NonSerialized]
		public AudioManager _audioManager;

		private void Start()
		{
				_audioManager = GetComponent<AudioManager>();
				_anima = GetComponent<Animator>();
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
				if (_dead)
						return;

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
				_audioManager.StopAll();
				if (!_audioManager.IsPlaying(AudioPainbuster.Death.ToString()))
						_audioManager.Play(AudioPainbuster.Death.ToString());
				Invoke("Death", 2f);
		}

		public void Explosion()
		{
				_dead = true;
				_anima.SetBool("Explosion", true);
				_audioManager.StopAll();
				if (!_audioManager.IsPlaying(AudioPainbuster.Explosion.ToString()))
						_audioManager.Play(AudioPainbuster.Explosion.ToString());
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
