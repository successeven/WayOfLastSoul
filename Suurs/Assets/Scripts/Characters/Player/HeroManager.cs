using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour {


    [Header("Уровень")]
    public int _Level = 1; //уровень

    [Header("Максимальная жизнь")]
    public int _Health = 100; //Максимальная жизнь
    [Header("Текущая жизнь")]
    public int _HP = 100; //Текущая жизнь

    [Header("Максимальная глобальная жизнь")]
    public int _GlobalHealth = 100; //Максимальная глобальная жизнь

    [Header("Текущая глобальная жизнь")]
    public int _GlobalHP = 100; //Текущая глобальная жизнь

    [Header("Атака")]
    public int _Attack = 25; //атака

    [Header("Щит")]
    public float _Shield = 50f; //Щит   

    [Header("Защита")]
    public float _Protaction = 0f; //Защита    

    [Header("Скорость атаки")]
    public float _SpeedAttack = 100f; //Скорость атаки

    [Header("Ловкость")]
    public  float _Agility = 0; //Ловкость

    [Header("Сила")]
    public float _Power = 0; //Сила


    [Header("Жизнеспособность")]
    public float _Vitality = 0; //Жизнеспособность


    [Header("Интервал кувырков (в милисекундах) ")]
    public int _DeltaRoll = 2000; //Интервал кувырков (в милисекундах) 
    
    private void OnGUI()
    {
        string boxText =
            "Level = " + _Level + "\n" +
            "HP = " + _HP + "(" + _Health + ")\n" +
            "GlobalHP = " + _GlobalHP + "(" + _GlobalHealth + ")\n" +
            "Attack = " + _Attack + "\n" +
            "Agility = " + _Agility + "\n";
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

        _Attack += 2;
        _Health += 2;
        _Power++;
        if ((_Power != 0) && (_Power % 10 == 0))
        {
            _Attack += 10 + (int)(_Power / 10) * 10;
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
}
