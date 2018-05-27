using System.Collections;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Hero
{
	public int _Level; //уровень
	public int _Health; //Максимальная жизнь
	public int _HP; //Текущая жизнь
	public int _GlobalHealth; //Максимальная глобальная жизнь
	public int _GlobalHP; //Текущая глобальная жизнь
	public int _attack; //атака
	public float _Shield; //Щит   
	public float _Protaction; //Защита 
	public float _SpeedAttack; //Скорость атаки
	public float _Agility; //Ловкость
	public float _Power; //Сила
	public float _Vitality; //Жизнеспособность
	public int _DeltaRoll; //Интервал кувырков (в милисекундах) 
}

public class HeroManager : MonoBehaviour
{

	public int _Level = 1; ///уровень
	public int _Health = 100; ///Максимальная жизнь	
	public int _HP = 100; ///Текущая жизнь
	public int _MaxEnergy = 100; ///Максимальная жизнь	
	public int _Energy = 100; ///Текущая жизнь
	public int _GlobalHealth = 100; ///Максимальная глобальная жизнь
	public int _GlobalHP = 100; ///Текущая глобальная жизнь
	public int _attack = 25; ///атака
	public float _Shield = 50f; ///Щит   
	public float _Protaction = 0f; ///Защита 
	public float _SpeedAttack = 100f; ///Скорость атаки
	public float _Agility = 0; ///Ловкость
	public float _Power = 0; ///Сила
	public float _Vitality = 0; ///Жизнеспособность
	public int _DeltaRoll = 2000; ///Интервал кувырков (в милисекундах) 


	public Hero heroStat;
	string path;

	Animator _anima;
	HeroController _controller;
	bool _death = false;

	bool _DealDamage = false;

	private void Start()
	{
		_anima = GetComponent<Animator>();
		_controller = GetComponent<HeroController>();
		/*
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Hero.json");
#else
        path = Path.Combine(Application.dataPath, "Hero.json");
#endif
        if (File.Exists(path))
            heroStat = JsonUtility.FromJson<Hero>(File.ReadAllText(path));
            */
	}

	private void Update()
	{
		if (_HP <= 0 && !_death)
		{
			_death = true;
			_anima.SetTrigger("Death");
			_controller.Die();
		}
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy" && _anima.GetFloat("Attack") != 0)
		{
			if (!_DealDamage)
			{
				_DealDamage = true;

				GameObject enemy = collision.transform.root.gameObject;
				EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
				EnemyController enemyController = enemy.GetComponent<EnemyController>();

				if (enemyManager._HP <= 0)
					return;

				enemyManager._HP -= _attack;
				enemyController.TakeHit();
				_controller._comboAttack++;
			}
		}
	}

	public void ResetHeroDealAttack()
	{
		_DealDamage = false;
	}

	private void OnGUI()
	{
		string boxText =
			"Level = " + _Level + "\n" +
			"Attack = " + _attack + "\n" +
			"Agility = " + _Agility + "\n" +
			"ComboAttack = " + _controller._comboAttack + "\n";
		GUI.Box(new Rect(0, 0, 150, 100), boxText);
	}

	void AddAgility()
	{
		if (_Agility == 100)
			return;

		_SpeedAttack += _SpeedAttack * 0.002f;
		_DeltaRoll -= 100;
		_Agility++;
		if ((_Agility != 0) && (_Agility % 7 == 0))
			_SpeedAttack += _SpeedAttack * 0.02f;
		if ((_Agility != 0) && (_Agility % 100 == 0))
			_SpeedAttack += _SpeedAttack * 0.02f;

	}
	void AddPower()
	{
		if (_Power == 100)
			return;

		_attack += 2;
		_Health += 2;
		_Power++;
		if ((_Power != 0) && (_Power % 10 == 0))
		{
			_attack += 10 + (int)(_Power / 10) * 10;
			_Health += 10;
		}
		if ((_Agility != 0) && (_Agility % 100 == 0))
			_SpeedAttack += _SpeedAttack * 0.02f;

	}
	void AddVitality()
	{
		if (_Vitality == 100)
			return;

		_Health += 10;
		_Shield += 2f;
		_Vitality++;
		if ((_Vitality != 0) && (_Vitality % 10 == 0))
		{
			_Health += 10 + (int)(_Vitality / 10) * 10;
			_Protaction += 5f;
		}
		/*
        if ((_Agility != 0) && (_Agility % 100 == 0))
            _SpeedAttack += _SpeedAttack * 0.02f;*/
	}

	void Load()
	{
		//PlayerPrefs.
	}
}
