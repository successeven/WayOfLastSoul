using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

		#region Singleton

		public static CameraShake instance;

		void Awake()
		{
				instance = this;
		}

		#endregion

		public float _shakeDuration = 0.3f;          // Time the Camera Shake effect will last
		public float _shakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
		public float _shakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

		private float ShakeElapsedTime = 0f;

		// Cinemachine Shake
		public CinemachineVirtualCamera VirtualCamera;
		private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

		// Use this for initialization
		void Start()
		{
				// Get Virtual Camera Noise Profile
				if (VirtualCamera != null)
						virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
		}

		public void Shake()
		{
				ShakeElapsedTime = _shakeDuration;
		}

		// Update is called once per frame
		void Update()
		{
				// If the Cinemachine componet is not set, avoid update
				if (VirtualCamera != null && virtualCameraNoise != null)
				{
						// If Camera Shake effect is still playing
						if (ShakeElapsedTime > 0)
						{
								// Set Cinemachine Camera Noise parameters
								virtualCameraNoise.m_AmplitudeGain = _shakeAmplitude;
								virtualCameraNoise.m_FrequencyGain = _shakeFrequency;

								// Update Shake Timer
								ShakeElapsedTime -= Time.deltaTime;
						}
						else
						{
								// If Camera Shake effect is over, reset variables
								virtualCameraNoise.m_AmplitudeGain = 0f;
								ShakeElapsedTime = 0f;
						}
				}
		}
}