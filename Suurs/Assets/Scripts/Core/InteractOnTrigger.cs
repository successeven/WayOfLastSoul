using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger : MonoBehaviour
{
		public UnityEvent OnEnter, OnExit;
		public bool _turnOffAfterExit = false;

		protected Collider2D m_Collider;


		void OnTriggerEnter2D(Collider2D other)
		{
				if (!enabled)
						return;

				OnEnter.Invoke();
		}

		void OnTriggerExit2D(Collider2D other)
		{
				if (!enabled)
						return;

				OnExit.Invoke();
				if (_turnOffAfterExit)
						this.gameObject.SetActive(false);
		}
		
}
