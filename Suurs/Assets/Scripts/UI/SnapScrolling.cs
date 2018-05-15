using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{

		public GameObject _panPrefab;
		public ScrollRect _scrollRect;
		public float _snapSpeed;

		RectTransform _contentRect;

		GameObject[] _instPans;
		Vector2[] _panPos;
		float[] _panAlfa;
		Text[] _panText;
		Vector2[] _pansScale;
		int _CurrrentPosID;
		bool _isScrolling = false;
		Vector2 _contentVector;

		public float _panOffset = 30f; //растояние м/у пунктами меню.
		public float _scaleOffSet = 2f;
		public float _scaleSpeed = 10f;

		int _countItems = 3;


		void InstPanel(int inIndex, string inText)
		{
				_instPans[inIndex] = Instantiate(_panPrefab, transform, false);
				_panText[inIndex] = _instPans[inIndex].GetComponent<Text>();
				_panText[inIndex].text = inText;
				if (inIndex == 0)
						return;
				_instPans[inIndex].transform.localPosition = new Vector2(_instPans[inIndex].transform.localPosition.x, 
						_instPans[inIndex - 1].transform.localPosition.y - _panPrefab.GetComponent<RectTransform>().sizeDelta.y - _panOffset);
				_panPos[inIndex] = -_instPans[inIndex].transform.localPosition;
		}

		void Start()
		{
				_contentRect = GetComponent<RectTransform>();
				_instPans = new GameObject[_countItems];
				_panPos = new Vector2[_countItems];
				_pansScale = new Vector2[_countItems];
				_panText = new Text[_countItems];
				_panAlfa = new float[_countItems];
				int i = 0;
				InstPanel(i++, "START");
				InstPanel(i++, "OPTIONS");
				InstPanel(i++, "EXIT");
		}

		void FixedUpdate()
		{
				if (_contentRect.anchoredPosition.y <= _panPos[0].y && !_isScrolling || _contentRect.anchoredPosition.y >= _panPos[_panPos.Length - 1].y && !_isScrolling)
						_scrollRect.inertia = false;

				float nearestPos = float.MaxValue;
				for (int i = 0; i < _countItems; i++)
				{
						float distance = Mathf.Abs(_contentRect.anchoredPosition.y - _panPos[i].y);
						if (distance < nearestPos)
						{
								nearestPos = distance;
								_CurrrentPosID = i;
						}
						float scale = Mathf.Clamp(1 / (distance / _panOffset) * _scaleOffSet, 0.5f, 1.4f);
						_pansScale[i].x = Mathf.SmoothStep(_instPans[i].transform.localScale.x, scale, _scaleSpeed * Time.fixedDeltaTime);
						_pansScale[i].y = Mathf.SmoothStep(_instPans[i].transform.localScale.y, scale, _scaleSpeed * Time.fixedDeltaTime);
						_instPans[i].transform.localScale = _pansScale[i];


						for (int j = 0; j < _countItems; j++)
						{
								if (Mathf.Abs(j - _CurrrentPosID) >= 2)
										_panAlfa[j] = 0.2f;
								else if (Mathf.Abs(j - _CurrrentPosID) == 1)
										_panAlfa[j] = 0.6f;
								else
										_panAlfa[j] = 1;

								_panText[j].color = new Color(_panText[j].color.r, _panText[j].color.g, _panText[j].color.b, _panAlfa[j]);
						}
				}

				float scrollVelocity = Mathf.Abs(_scrollRect.velocity.y);
				Debug.Log(scrollVelocity);
				if (scrollVelocity < 150 && !_isScrolling)
						_scrollRect.inertia = false;

				if (_isScrolling || scrollVelocity > 150)
						return;
				_contentVector.y = Mathf.SmoothStep(_contentRect.anchoredPosition.y, _panPos[_CurrrentPosID].y, _snapSpeed * Time.fixedDeltaTime);
				_contentRect.anchoredPosition = _contentVector;
		}
		public void Scrolling(bool inScroll)
		{
				_isScrolling = inScroll;
				if (inScroll)
						_scrollRect.inertia = true;
		}
}
