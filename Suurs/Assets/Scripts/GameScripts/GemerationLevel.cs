using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;


[Flags]
public enum InclineLandType
{
    InclineStones = 1,
    Stairs = 2,
};

[Flags]
public enum LandType
{
    GrassLow = 1,
    Grass = 2,
    Stones = 4,
    Tiles = 8
};

public enum GroundName
{
    Horizontal,
    Incline,
    Finish
};

[Flags]
public enum Objects
{
    None = 0,
    Shield = 1,
    Pikes = 2,
    Knifes = 4,
    KnifesLow = 8,
    KnifeHigh = 16,
    ColumnBig = 32,
    ColumnLow = 64,
    ColumnHigh = 128,
    ColumnMedium = 256,
    Vase = 512,
    Vase_2 = 1024,
    Corpse_1 = 2048,
    Corpse_2 = 4096,
    Corpse_3 = 8192
}

public class GemerationLevel : MonoBehaviour
{
    [SerializeField]
    public int _Сomplexity; //Сложность уровня
    [SerializeField]
    [Range(0, 200)]
    int _countEnemy; //кол-во всего врагов на сцене
    [SerializeField]
    [Range(0, 10)]
    int _countEnemyByPoint; //кол-во всего врагов в 1 месте

    [SerializeField] [Range(0, 50)] [Tooltip("Частота появления врага (платформ) (0 - все в 1 месте)")]
    int _frequencySpawn; //через сколько платформ появится враг


    [Space(10)]
    [SerializeField]
    [Range(0, 500)]
    int _Lengthmap; //длина карты

    [SerializeField]
    [Range(0, 100)]
    int _PercentLevel; //Процент "изгибаемости" локации

    [SerializeField]
    GameObject _СurrentObject;//Текущая позиция

    [Space( 10)]
    [SerializeField]
    [EnumFlag]
    Objects _Objects;
    List<Objects> _selectObjects;

    [Space( 10)]
    [SerializeField]
    [EnumFlag]
    LandType _Lands;
    List<LandType> _selectLands;

    [Space (10)]
    [SerializeField]
    [EnumFlag]
    InclineLandType _InclineLands;
    List<InclineLandType> _selectInclineLands;

    int[] _map;

    object[] _landsOnMap;



    [SerializeField]
    SpriteAtlas _LandsAtlas;

    [SerializeField]
    SpriteAtlas _ObjectsAtlas;

    [SerializeField]
    GameObject _DrawObject;

    [Space(10)]

    [SerializeField]
    List<GameObject> _Grounds;

    [SerializeField]
    List<GameObject> _Enemys;


    Dictionary<string, GameObject> _GroundsbyName;

    public GameObject GetGroundbyName(GroundName a_Name)
    {
        string name = a_Name.ToString();
        if (!_GroundsbyName.ContainsKey(name))
        {
            Debug.LogError(string.Format("ERROR! Ground \"{0}\" not found!", name));
            return null;
        }
        else
            return _GroundsbyName[name];
    }

    private void Awake()
    {
        _GroundsbyName = new Dictionary<string, GameObject>();
        foreach (var item in _Grounds)
            _GroundsbyName.Add(item.name.Split('_')[0] , item);

				_map = new int[_Lengthmap];

        _selectObjects = new List<Objects>();
        foreach (Objects i in Enum.GetValues(typeof(Objects)))
            if ((_Objects & i) != 0)
                _selectObjects.Add(i);

        _landsOnMap = new object[_Lengthmap];

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


        _selectLands = new List<LandType>();
        foreach (LandType i in Enum.GetValues(typeof(LandType)))
            if ((_Lands & i) != 0)
                _selectLands.Add(i);

        _selectInclineLands = new List<InclineLandType>();
        foreach (InclineLandType i in Enum.GetValues(typeof(InclineLandType)))
            if ((_InclineLands & i) != 0)
                _selectInclineLands.Add(i);

        // Пробегаем по карте и заполняем наклонные
        for (int i = 0; i < _Lengthmap; i++)
        {
            if (_map[i] != 0)
            {
                currentLandType = UnityEngine.Random.Range(0, _selectInclineLands.Count);
                _landsOnMap[i] = _selectInclineLands[currentLandType];
                if (_selectInclineLands[currentLandType] == InclineLandType.Stairs)
                {
                    _landsOnMap[i - 1] = LandType.Tiles;
                    _landsOnMap[i + 1] = LandType.Tiles;
                }
            }
        }

        #region Start
        // СТАРТ
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        int lenghtGrass = UnityEngine.Random.Range(1, 3);
        while (currentIndex < lenghtGrass)
        {
            if (_landsOnMap[currentIndex + 1] != null)
            {
                _landsOnMap[currentIndex++] = LandType.GrassLow;
                break;
            }

            if (currentIndex == lenghtGrass - 1)
                _landsOnMap[currentIndex++] = LandType.GrassLow;
            else
                _landsOnMap[currentIndex++] = LandType.Grass;
        }

   //     _landsOnMap[currentIndex] = LandType.Stones;
        #endregion

        #region Finish
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        lenghtGrass = UnityEngine.Random.Range(1, 3);

        for (int i = _landsOnMap.Length - 1; i > (_landsOnMap.Length - 1 - lenghtGrass); i--)
        {
            if (_landsOnMap[i - 1] != null)
            {
                _landsOnMap[i] = LandType.GrassLow;
                break;
            }

            if (i == _Lengthmap - lenghtGrass)
                _landsOnMap[i] = LandType.GrassLow;
            else
                _landsOnMap[i] = LandType.Grass;
        }
        //_landsOnMap[_Lengthmap - 1 - lenghtGrass] = LandType.Stones;
        #endregion

        #region OtherFill
        //Промежуток        
        for (int i = currentIndex; i < _Lengthmap; i++)
        {
            if (_landsOnMap[i] != null)
                continue;

            currentLandType = UnityEngine.Random.Range(0, _selectLands.Count);

            if (_selectLands[currentLandType] == LandType.Grass)
            {

                if (_landsOnMap[i + 1] != null) // следующая позиция свободна?
                {
                    _landsOnMap[i] = LandType.Stones;
                    continue;
                }

                UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
                lenghtGrass = UnityEngine.Random.Range(2, 6);

                int firstIndex = i;

                for (int j = firstIndex; j < firstIndex + lenghtGrass; j++)
                {
                    if (j == firstIndex)
                    {
                        _landsOnMap[j] = LandType.GrassLow;
                        continue;
                    }

                    if (_landsOnMap[j + 1] != null)
                    {
                        _landsOnMap[j] = LandType.GrassLow;
                        break;
                    }

                    if (j == firstIndex + lenghtGrass - 1)
                        _landsOnMap[j] = LandType.GrassLow;
                    else
                        _landsOnMap[j] = LandType.Grass;
                    i++;
                }
            }
            else
                _landsOnMap[i] = _selectLands[currentLandType];
        }
        #endregion
      
    }

    private void Generation()
    {
        GenerationLevels();
        GenerationLands();

        GameObject ground;
        bool drawingGrass = true; //true т к самая первая локация с травой
        for (int i = 0; i < _Lengthmap; i++)
        {
            switch (_map[i])
            {
                case 1:
                case -1:
                    ground = GetGroundbyName(GroundName.Incline);
                    IncertPrefab(ground, _map[i]);
                    break;
                case 0:
                    ground = GetGroundbyName(GroundName.Horizontal);
                    IncertPrefab(ground, _map[i]);
                    DrawObjects();
                    if (i >= 5)
                        IncertEnemy(i);
                    break;
            }
            DrawLand(i, ref drawingGrass);
        }
        ground = GetGroundbyName(GroundName.Finish);
        IncertPrefab(ground, 0);
    }

    private void IncertEnemy(int inIndexMap)
    {
        if (_countEnemy == 0)
            return;

        if (inIndexMap % _frequencySpawn == 0)
        {
            GameObject currentPosPuzzle = _СurrentObject.transform.Find("ConnectionPuzzle").gameObject;
            UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());

            GameObject enemy = _Enemys[UnityEngine.Random.Range(0, _Enemys.Count )];
            int countEnemy = UnityEngine.Random.Range(1, _countEnemyByPoint + 1);
            int deltaX = 0;
            for (int i = 0; i < countEnemy; i++)
            {
                if (_countEnemy == 0)
                    break;

                Vector3 position = new Vector3(
                currentPosPuzzle.transform.position.x - deltaX,
                currentPosPuzzle.transform.position.y + 20,
                currentPosPuzzle.transform.position.z);

                Instantiate(enemy, position, Quaternion.identity);
                deltaX += 3;
                _countEnemy--;
            }
        }
    }

    void DrawObjects()
    {
        if (_selectObjects.Count == 0)
            return;


        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());

        GameObject currentPosPuzzle = _СurrentObject.transform.Find("ConnectionPuzzle").gameObject;

        int countObjects = UnityEngine.Random.Range(0, 2);
        for (int i = 0; i < countObjects; i++)
        {
            Objects drawObject = _selectObjects[UnityEngine.Random.Range(0, _selectObjects.Count)];
            Sprite sprite = _ObjectsAtlas.GetSprite(drawObject.ToString());
            if (sprite != null)
            {
                Vector3 position = new Vector3(
                UnityEngine.Random.Range(_СurrentObject.transform.position.x, currentPosPuzzle.transform.position.x),
                _СurrentObject.transform.position.y,
                _СurrentObject.transform.position.z);

                GameObject drawingObject = Instantiate(_DrawObject, position, Quaternion.identity, _СurrentObject.transform);
                drawingObject.GetComponent<SpriteRenderer>().sprite = _ObjectsAtlas.GetSprite(drawObject.ToString());

                if (drawObject.ToString().Contains("Column"))
                {
                    Vector3 theScale = drawingObject.transform.localScale;
                    float scailPoint = UnityEngine.Random.Range(0.5f,1.2f);
                    theScale.x *= scailPoint;
                    theScale.y *= scailPoint;
                    drawingObject.transform.localScale = theScale;

                    Quaternion target = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-20, 20));
                    drawingObject.transform.rotation = target; 
                }
            }
        }
    }

    void DrawLand(int inIndex, ref bool isDrawing)
    {
        Sprite land;

        string landName;

        if (_map[inIndex] == 0)
            landName = ((LandType)_landsOnMap[inIndex]).ToString();
        else
            landName = ((InclineLandType)_landsOnMap[inIndex]).ToString();

        land = _LandsAtlas.GetSprite(landName);
        GameObject landComponent = _СurrentObject.transform.Find("Land").gameObject;
        landComponent.GetComponent<SpriteRenderer>().sprite = land;
        if (landName == LandType.GrassLow.ToString())
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
