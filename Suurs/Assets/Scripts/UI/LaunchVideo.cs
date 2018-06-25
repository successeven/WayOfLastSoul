using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchVideo : MonoBehaviour
{
    void Start()
    {
        Handheld.PlayFullScreenMovie("FinalScreen.avi", Color.black, FullScreenMovieControlMode.CancelOnInput);
    }
}