using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public enum LandType
{
    None = 0,
    Grass = 1,
    InclineStones = 2,
    Stairs = 3,
    Stones = 4,
    Tiles = 5,
    GrassLow = 6
};

public enum GroundName
{
    Horizontal,
    Incline,
    Finish
};

[System.Flags]
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
    ColumnMedium = 256
}

public class PrefabsScripts : MonoBehaviour
{
    [SerializeField]
    SpriteAtlas _Lands;    
    public SpriteAtlas Lands
    {
        get
        {
            return _Lands;
        }
    }


    [SerializeField]
    SpriteAtlas _ObjectsAtlas;
    public SpriteAtlas Objects
    {
        get
        {
            return _ObjectsAtlas;
        }
    }

    [SerializeField]
    GameObject _DrawObject;
    public GameObject DrawObject
    {
        get
        {
            return _DrawObject;
        }
    }

    [Space(10)]

    [SerializeField]
    List<GameObject> _Grounds;




    Dictionary<string, GameObject> _GroundsbyName;
    
    private void Awake()
    {
        _GroundsbyName = new Dictionary<string, GameObject>();
        foreach(var item in _Grounds)
            _GroundsbyName.Add(item.name, item);
    }

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

}
