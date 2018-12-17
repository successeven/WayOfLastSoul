using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(.1f, 3f)]
	public float pitch = 1f;

	public bool loop = false;
    public bool Surround = false;
    public AudioRolloffMode RolloffMode = AudioRolloffMode.Linear;

    [Range(1f, 100f)]
    public float MinDistance= 1f;

		[Range(1f, 100f)]
		public float MaxDistance = 100f;


		[HideInInspector]
	public AudioSource source;

}
