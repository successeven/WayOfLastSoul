using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{

		#region Singleton

		public static GameManager instance;

		void Awake()
		{
				instance = this;
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
}
