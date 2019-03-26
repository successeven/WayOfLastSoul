using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public enum GameTimeLines
{
    StartScene = 0,
    ChangeScene = 1,
    ResetUI = 2
}

public class GameManager : MonoBehaviour
{

    #region Singleton

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayFromTimelines(GameTimeLines.StartScene);
    }

    #endregion

    public List<PlayableDirector> playableDirectors;
    public List<TimelineAsset> timelines;

    public void PlayFromTimelines(int index)
    {
        TimelineAsset selectedAsset;

        if (timelines.Count <= index)
        {
            selectedAsset = timelines[timelines.Count - 1];
        }
        else
        {
            selectedAsset = timelines[index];
        }

        playableDirectors[0].Play(selectedAsset);
    }

    public void PlayFromTimelines(GameTimeLines gameTimeLine)
    {
        TimelineAsset selectedAsset;

        if (timelines.Count <= (int)gameTimeLine)
        {
            selectedAsset = timelines[timelines.Count - 1];
        }
        else
        {
            selectedAsset = timelines[(int)gameTimeLine];
        }

        playableDirectors[0].Play(selectedAsset);
    }
}
