using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackController : MonoBehaviour
{

		public List<AttackItem> _AttaksList;

		public List<AttackItem> _OrderAttackToPlay;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void AddAttack(bool isCombo)
		{
				if (_OrderAttackToPlay.Count == 0)
				{
						_OrderAttackToPlay.Add(_AttaksList.Where(x => x._isCombo == isCombo).OrderBy(z => z._ID).First());
						return;
				}



		}
}
