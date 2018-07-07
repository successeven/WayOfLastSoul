using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
		enum StateScene
		{
				None = 0,
				Start = 1,
				Game = 2,
				Pause = 3,
				Exit = 4
		}
		Animator _anima;

		bool needChange;

		bool gameLoad = false;
		 StateScene _state = StateScene.None;
		
		void Start()
		{
				_anima = GetComponent<Animator>();
		}

		public void ShowUI()
		{
				_anima.enabled = true;
				gameLoad = true;
				_state = StateScene.Start;
				needChange = true;
		}

		public void HideUI()
		{
				_state = StateScene.Exit;
				needChange = true;
		}

		public void PauseGame()
		{
				_state = StateScene.Pause;
				needChange = true;
				Time.timeScale = 0.1f;
		}

		public void DisableTime()
		{
				Time.timeScale = 0;
		}

		public void NormalizeTime()
		{
				Time.timeScale = 1;

		}

		public void ContinueGame()
		{
				Debug.Log("Continue");
				_state = StateScene.Game;
				needChange = true;
				Time.timeScale = 0.1f;
		}

		public void ExitGame()
		{
				Time.timeScale = 1;
				SceneManager.LoadScene("Menu");						
		}

		// Update is called once per frame
		void Update()
		{
				if (!gameLoad)
						return;

				if (!needChange)
						return;

				switch (_state)
				{
						case StateScene.Start:
								_anima.SetInteger("State", 1);
								break;

						case StateScene.Pause:
								_anima.SetInteger("State", 3);
								break;

						case StateScene.Game:
								_anima.SetInteger("State", 4);
								break;

						case StateScene.Exit:
								_anima.SetInteger("State", 2);
								break;
				}

				_anima.SetTrigger("Change");
				needChange = false;
		}
}
