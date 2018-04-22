using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;

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
        
    [SerializeField]
    List<GameObject> _Enemy;


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
