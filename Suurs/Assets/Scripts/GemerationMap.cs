using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GemerationMap : MonoBehaviour
{
    [SerializeField]
    int _Lenghtmap; //длина карты

    [SerializeField]
    GameObject _СurrentObject;//Текущая позиция

    PrefabsScripts _prefabsScripts;
    int[] _map;
    LandType[] _ladsOnMap;



    void Start()
    {
        _prefabsScripts = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<PrefabsScripts>();

        _map = new int[_Lenghtmap];
        _ladsOnMap = new LandType[_Lenghtmap];
        for (int i = 0; i < _Lenghtmap; i++)
            _ladsOnMap[i] = LandType.None;

        Generation();
    }
    /// <summary>
    /// генерация уровней(перепадов) карты
    /// </summary>
    void GenerationLevels()
    {
        _map[0] = 0;// Первая локация обязательно горизонтальная. 
        for (int i = 1; i < _Lenghtmap - 1; i++)
        {
            int level = UnityEngine.Random.Range(-1, 1);
            if (i != 0)
            {
                if (level + _map[i - 1] == 0) //Проверяем на излом /\
                    level = 0;

                if (level + _map[i - 1] == -2 || level + _map[i - 1] == 2) //Проверка на 2 подряд идущих склона
                    level = 0;
            }
            _map[i] = level;
        }
        _map[_Lenghtmap - 1] = 0; // Последняя локация обязательно горизонтальная. 
    }

    void GenerationLands()
    {
        int currentLandType;
        LandType[] inclineLands = { LandType.InclineStones, LandType.Stairs };
        LandType[] horizontalLands = { LandType.Grass, LandType.Stones, LandType.Tiles };
        for (int i = 0; i < _Lenghtmap; i++)
        {
            if (_ladsOnMap[i] != LandType.None)
                continue;

            if (_map[i] == -1 || _map[i] == 1)
            {
                currentLandType = UnityEngine.Random.Range(0, 1);
                _ladsOnMap[i] = inclineLands[currentLandType];
                if (inclineLands[currentLandType] == LandType.Stairs)
                {
                    _ladsOnMap[i - 1] = LandType.Tiles;
                    _ladsOnMap[i + 1] = LandType.Tiles;
                }
                continue;
            }

            currentLandType = UnityEngine.Random.Range(0, 2);
            _ladsOnMap[i] = horizontalLands[currentLandType];
        }
    }

    private void Generation()
    {
        GenerationLevels();
        GenerationLands();

        GameObject ground;
        for (int i = 0; i < _Lenghtmap; i++)
        {
            Sprite land = _prefabsScripts.Lands.GetSprite(_ladsOnMap[i].ToString());
            string test = land.name + " " + i.ToString();
            switch (_map[i])
            {
                case 1:
                case -1:
                    ground = _prefabsScripts.GetGroundbyName(GroundName.Incline);
                    IncertPrefab(ground, _map[i], land);
                    break;
                case 0:
                    ground = _prefabsScripts.GetGroundbyName(GroundName.Horizontal);
                    IncertPrefab(ground, _map[i], land);
                    break;
            }

            GameObject landComponent = _СurrentObject.transform.Find("Land").gameObject;
            landComponent.GetComponent<SpriteRenderer>().sprite = land;
            //landComponent.GetComponent<LoadSprite>()._currentSprite = land;
        }
    }

    void IncertPrefab(GameObject inGround, int inLevel, Sprite inLand)
    {
        GameObject _currentPosPuzzle = _СurrentObject.transform.Find("ConnectionPuzzle").gameObject; //Соеденительная точка у префаба которй уже на сцене

        Vector3 position = new Vector3(
            _currentPosPuzzle.transform.position.x,
            _currentPosPuzzle.transform.position.y,
            _currentPosPuzzle.transform.position.z);

        _СurrentObject = Instantiate(inGround, position, _currentPosPuzzle.transform.rotation);
        if (inLevel == -1)
        {
            GameObject _NewPuzzle = _СurrentObject.transform.Find("ConnectionPuzzle").gameObject;
            position = new Vector3(_NewPuzzle.transform.position.x, _NewPuzzle.transform.position.y, _NewPuzzle.transform.position.z);

            position.x = _NewPuzzle.transform.position.x;
            position.y = _currentPosPuzzle.transform.position.y - (position.y - _currentPosPuzzle.transform.position.y);

            Vector3 theScale = _СurrentObject.transform.localScale;
            theScale.x *= -1;
            _СurrentObject.transform.localScale = theScale;

            _СurrentObject.transform.position = position;

            GameObject puzzleIn = _СurrentObject.transform.Find("ConnectionPuzzle").gameObject;
            puzzleIn.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
