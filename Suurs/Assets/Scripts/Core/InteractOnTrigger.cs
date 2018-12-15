using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger : MonoBehaviour
{
		public UnityEvent OnEnter, OnStay, OnExit;
		public bool _turnOffAfterExit = false;
		public string _tag = "Player";

		protected Collider2D m_Collider;


		void OnTriggerEnter2D(Collider2D other)
		{
				if (!enabled)
						return;

				if (other.tag == _tag)
						OnEnter.Invoke();
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
				if (!enabled)
						return;

				if (collision.tag == _tag)
						OnStay.Invoke();


		}

		void OnTriggerExit2D(Collider2D other)
		{
				if (!enabled)
						return;

				if (other.tag == _tag)
						OnExit.Invoke();

				if (_turnOffAfterExit)
						this.gameObject.SetActive(false);
		}

}
