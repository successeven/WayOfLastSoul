using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SnapScrolling : MonoBehaviour
{

		public GameObject _panPrefab;
		public ScrollRect _scrollRect;
		public float _snapSpeed;
		public Animator _anima;

		RectTransform _contentRect;

		GameObject[] _instItems;
		Vector2[] _posItems;
		float[] _alfaItems;
		Text[] _ItemMenuText;
		Vector2[] _scaleItems;
		int _CurrrentPosID;
		bool _isScrolling = false;
		Vector2 _contentVector;

		public float _panOffset = 30f; //растояние м/у пунктами меню.
		public float _scaleOffSet = 2f;
		public float _scaleSpeed = 10f;

		int _countItems = 3;


		void InstPanel(int inIndex, string inText)
		{
				_instItems[inIndex] = Instantiate(_panPrefab, transform, false);
				_ItemMenuText[inIndex] = _instItems[inIndex].GetComponentInChildren<Text>();
				_ItemMenuText[inIndex].text = inText;

				if (inIndex == 0)
						return;
				_instItems[inIndex].transform.localPosition = new Vector2(_instItems[inIndex].transform.localPosition.x,
								_instItems[inIndex - 1].transform.localPosition.y - _panPrefab.GetComponent<RectTransform>().sizeDelta.y - _panOffset);
				_posItems[inIndex] = -_instItems[inIndex].transform.localPosition;
		}

		void Start()
		{
				_contentRect = GetComponent<RectTransform>();
				_instItems = new GameObject[_countItems];
				_posItems = new Vector2[_countItems];
				_scaleItems = new Vector2[_countItems];
				_ItemMenuText = new Text[_countItems];
				_alfaItems = new float[_countItems];
				int i = 0;
				InstPanel(i, "NEW GAME");
				_instItems[i++].GetComponent<Button>().onClick.AddListener(() => OnButtonNewGameClick());
				InstPanel(i, "OPTIONS");
				_instItems[i++].GetComponent<Button>().onClick.AddListener(() => OnButtonOptionsClick());
				InstPanel(i, "EXIT");
				_instItems[i++].GetComponent<Button>().onClick.AddListener(() => OnButtonExitClick());
		}

		void FixedUpdate()
		{
				if (_contentRect.anchoredPosition.y <= _posItems[0].y && !_isScrolling || _contentRect.anchoredPosition.y >= _posItems[_posItems.Length - 1].y && !_isScrolling)
						_scrollRect.inertia = false;

				float nearestPos = float.MaxValue;
				for (int i = 0; i < _countItems; i++)
				{
						float distance = Mathf.Abs(_contentRect.anchoredPosition.y - _posItems[i].y);
						if (distance < nearestPos)
						{
								nearestPos = distance;
								_CurrrentPosID = i;
						}
						float scale = Mathf.Clamp(1 / (distance / _panOffset) * _scaleOffSet, 0.5f, 1.4f);
						_scaleItems[i].x = Mathf.SmoothStep(_instItems[i].transform.localScale.x, scale, _scaleSpeed * Time.fixedDeltaTime);
						_scaleItems[i].y = Mathf.SmoothStep(_instItems[i].transform.localScale.y, scale, _scaleSpeed * Time.fixedDeltaTime);
						_instItems[i].transform.localScale = _scaleItems[i];


						for (int j = 0; j < _countItems; j++)
						{
								if (Mathf.Abs(j - _CurrrentPosID) >= 2)
										_alfaItems[j] = 0.2f;
								else if (Mathf.Abs(j - _CurrrentPosID) == 1)
										_alfaItems[j] = 0.6f;
								else
										_alfaItems[j] = 1;

								_ItemMenuText[j].color = new Color(_ItemMenuText[j].color.r, _ItemMenuText[j].color.g, _ItemMenuText[j].color.b, _alfaItems[j]);
						}
				}

				float scrollVelocity = Mathf.Abs(_scrollRect.velocity.y);
				if (scrollVelocity < 150 && !_isScrolling)
						_scrollRect.inertia = false;

				if (_isScrolling || scrollVelocity > 150)
						return;
				_contentVector.y = Mathf.SmoothStep(_contentRect.anchoredPosition.y, _posItems[_CurrrentPosID].y, _snapSpeed * Time.fixedDeltaTime);
				_contentRect.anchoredPosition = _contentVector;
		}

		public void Scrolling(bool inScroll)
		{
				_isScrolling = inScroll;
				if (inScroll)
						_scrollRect.inertia = true;
		}

		public void OnButtonNewGameClick()
		{
				PlayerPrefs.SetInt("NextLVL", 1);
				PlayerPrefs.SetInt("CompletedLVL", 0);
				SceneManager.LoadScene("Loading");
		}

		public void OnButtonOptionsClick()
		{
				_anima.SetBool("Options", true);
		}

		public void OnButtonExitClick()
		{
				Debug.Log("Exit");
		}
}
