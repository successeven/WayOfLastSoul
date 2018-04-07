using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GemerationMap : MonoBehaviour
{
    [SerializeField]
    int _Lengthmap; //длина карты

    [SerializeField]
    [Range(0, 100)]
    int _PercentLevel; //Процент "изгибаемости" локации

    [SerializeField]
    GameObject _СurrentObject;//Текущая позиция


    [SerializeField]
    List<GroundName> _List;

    PrefabsScripts _prefabsScripts;
    int[] _map;
    LandType[] _ladsOnMap;

    void Start()
    {
        _prefabsScripts = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<PrefabsScripts>();

        if (_Lengthmap < 5)
            _Lengthmap = 5;
        _map = new int[_Lengthmap];
        _ladsOnMap = new LandType[_Lengthmap];
        for (int i = 0; i < _Lengthmap; i++)
            _ladsOnMap[i] = LandType.None;

        Generation();
    }

    int GetLevel()
    {
        double RangePoint = (100 - _PercentLevel) / 2; //вероятнось одного из изгибов
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        int percentPoint = UnityEngine.Random.Range(0, 101);
        if (percentPoint > 0 && percentPoint <= RangePoint) // от 0 до "вероятности изгиба"
            return -1;
        else if (percentPoint > (RangePoint + _PercentLevel) && percentPoint <= 100)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// генерация уровней(перепадов) карты
    /// </summary>
    void GenerationLevels()
    {
        _map[0] = 0;// Первая и вторая локация обязательно горизонтальная . т к стартовая позиция с травой 
        _map[1] = 0;// Первая и вторая локация обязательно горизонтальная . т к стартовая позиция с травой 
        for (int i = 2; i < _Lengthmap - 2; i++)
        {
            int level = GetLevel();
            if (i != 0)
            {
                if (level + _map[i - 1] == 0) //Проверяем на излом /\
                    level = 0;

                if (level + _map[i - 1] == -2 || level + _map[i - 1] == 2) //Проверка на 2 подряд идущих склона
                    level = 0;
            }
            _map[i] = level;
        }
        _map[_Lengthmap - 2] = 0; // Последняя локация обязательно горизонтальная. 
        _map[_Lengthmap - 1] = 0; // Последняя локация обязательно горизонтальная. 
    }

    void GenerationLands()
    {
        int currentIndex = 0;
        int currentLandType;
        LandType[] inclineLands = { LandType.InclineStones, LandType.Stairs };
        LandType[] horizontalLands = { LandType.Grass, LandType.Stones};

        // Пробегаем по карте и заполняем наклонные
        for (int i = 0; i < _Lengthmap; i++)
        {
            if (_map[i] != 0)
            {
                currentLandType = UnityEngine.Random.Range(0, 2);
                _ladsOnMap[i] = inclineLands[currentLandType];
                if (inclineLands[currentLandType] == LandType.Stairs)
                {
                    _ladsOnMap[i - 1] = LandType.Tiles;
                    _ladsOnMap[i + 1] = LandType.Tiles;
                }
            }
        }

        #region Start
        // СТАРТ
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        int lenghtGrass = UnityEngine.Random.Range(1, 3);
        while (currentIndex < lenghtGrass)
        {
            if (_ladsOnMap[currentIndex + 1] != LandType.None)
            {
                _ladsOnMap[currentIndex++] = LandType.GrassLow;
                break;
            }

            if (currentIndex == lenghtGrass - 1)
                _ladsOnMap[currentIndex++] = LandType.GrassLow;
            else
                _ladsOnMap[currentIndex++] = LandType.Grass;
        }

        _ladsOnMap[currentIndex] = LandType.Stones;
        #endregion

        #region Finish
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        lenghtGrass = UnityEngine.Random.Range(1, 3);

        for (int i = _ladsOnMap.Length - 1; i > (_ladsOnMap.Length - 1 - lenghtGrass); i--)
        {
            if (_ladsOnMap[i - 1] != LandType.None)
            {
                _ladsOnMap[i] = LandType.GrassLow;
                break;
            }

            if (i == _Lengthmap - lenghtGrass)
                _ladsOnMap[i] = LandType.GrassLow;
            else
                _ladsOnMap[i] = LandType.Grass;
        }
        _ladsOnMap[_Lengthmap - 1 - lenghtGrass] = LandType.Stones;
        #endregion

        #region OtherFill
        //Промежуток        
        for (int i = currentIndex; i < _Lengthmap; i++)
        {
            if (_ladsOnMap[i] != LandType.None)
                continue;

            currentLandType = UnityEngine.Random.Range(0, 2);

            if (horizontalLands[currentLandType] == LandType.Grass)
            {
                if (_ladsOnMap[i - 1] == LandType.GrassLow) // предыдущая не трава?
                {
                    _ladsOnMap[i] = LandType.Stones;
                    continue;
                }

                if (_ladsOnMap[i + 1] != LandType.None) // следующая позиция свободна?
                {
                    _ladsOnMap[i] = LandType.Stones;
                    continue;
                }

                UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
                lenghtGrass = UnityEngine.Random.Range(2, 6);
                int indexI = 0;
                for (int j = i; j < i + lenghtGrass; j++)
                {
                    if (j == i)
                    {
                        _ladsOnMap[j] = LandType.GrassLow;
                        continue;
                    }

                    if (_ladsOnMap[j + 1] != LandType.None)
                    {
                        _ladsOnMap[j] = LandType.GrassLow;
                        break;
                    }

                    if (j == i + lenghtGrass - 1)
                        _ladsOnMap[j] = LandType.GrassLow;
                    else
                        _ladsOnMap[j] = LandType.Grass;

                    indexI++;
                }
                i += indexI;
            }
            else
                _ladsOnMap[i] = horizontalLands[currentLandType];
        }
        #endregion

    }

    private void Generation()
    {
        GenerationLevels();
        GenerationLands();
        /*
        string test = string.Empty;
        for (int i = 0; i < _Lengthmap; i++)
            test += " " + _map[i];
        Debug.Log(test);

        test = string.Empty;
        for (int i = 0; i < _Lengthmap; i++)
            test += " " + (int)_ladsOnMap[i];
        Debug.Log(test);*/

        GameObject ground;
        bool drawingGrass = true; //true т к самая первая локация с травой
        for (int i = 0; i < _Lengthmap; i++)
        {
            switch (_map[i])
            {
                case 1:
                case -1:
                    ground = _prefabsScripts.GetGroundbyName(GroundName.Incline);
                    IncertPrefab(ground, _map[i]);
                    break;
                case 0:
                    ground = _prefabsScripts.GetGroundbyName(GroundName.Horizontal);
                    IncertPrefab(ground, _map[i]);
                    DrawObjects();
                    break;
            }
            DrawLand(_ladsOnMap[i], ref drawingGrass);
        }
        ground = _prefabsScripts.GetGroundbyName(GroundName.Finish);
        IncertPrefab(ground, 0);
    }

    void DrawObjects()
    {
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        Array values = Enum.GetValues(typeof(Objects));
        GameObject _currentPosPuzzle = _СurrentObject.transform.Find("ConnectionPuzzle").gameObject;

        int countObjects = UnityEngine.Random.Range(0, 2);
        for (int i = 0; i < countObjects; i++)
        {
            Objects drawObject = (Objects)values.GetValue(UnityEngine.Random.Range(1, values.Length));
            Sprite sprite = _prefabsScripts.Objects.GetSprite(drawObject.ToString());
            if (sprite != null)
            {
                Vector3 position = new Vector3(
                UnityEngine.Random.Range(_СurrentObject.transform.position.x, _currentPosPuzzle.transform.position.x),
                _СurrentObject.transform.position.y - 0.5f,
                _СurrentObject.transform.position.z);

                Quaternion target = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-20, 20));
                GameObject drawingObject = Instantiate(_prefabsScripts.DrawObject, position, target, _СurrentObject.transform);
                drawingObject.GetComponent<SpriteRenderer>().sprite = _prefabsScripts.Objects.GetSprite(drawObject.ToString());

                if (drawObject.ToString().Contains("Column"))
                {
                    Vector3 theScale = drawingObject.transform.localScale;
                    float scailPoint = UnityEngine.Random.Range(0.5f, 0.8f);
                    theScale.x *= scailPoint;
                    theScale.y *= scailPoint;
                    drawingObject.transform.localScale = theScale;
                }
            }
        }
    }

    void DrawLand(LandType inLandType, ref bool isDrawing)
    {
        Sprite land = _prefabsScripts.Lands.GetSprite(inLandType.ToString());
        GameObject landComponent = _СurrentObject.transform.Find("Land").gameObject;
        landComponent.GetComponent<SpriteRenderer>().sprite = land;
        if (inLandType == LandType.GrassLow)
        {
            if (isDrawing) //если true значит начали же рисовать траву и вот впроцесе ее риисования. значит надо развернуть
            {
                GameObject _currentPosPuzzle = _СurrentObject.transform.Find("ConnectionPuzzle").gameObject;

                Vector3 position = new Vector3(
                    _currentPosPuzzle.transform.position.x,
                    _currentPosPuzzle.transform.position.y,
                    _currentPosPuzzle.transform.position.z);

                Vector3 theScale = landComponent.transform.localScale;
                theScale.x *= -1;
                landComponent.transform.localScale = theScale;
                landComponent.transform.position = position;
            }
            isDrawing = !isDrawing;
        }
    }

    void IncertPrefab(GameObject inGround, int inLevel)
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
