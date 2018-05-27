using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

public class GlobalHealthController : MonoBehaviour
{
	public Image _image;
	public int _CountPosition;
	public float phis;
	public float k;
	public float p;

	public float _ShowTime;

	[NonSerialized]
	public bool _ShowHealth = true;
	bool _CurrentState = false;

	HeroManager _heroManager;
	UILineRenderer _renderer;
	// Use this for initialization
	void Start()
	{
		//	_heroManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>();
		_renderer = GetComponent<UILineRenderer>();
	}

	void FixedUpdate()
	{
		if (_renderer.Points.Length == _CountPosition && _ShowHealth || _renderer.Points.Length == 0 && !_ShowHealth)
			return;

		if (_ShowHealth)
		{
			_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
			int _Speed = (int)Mathf.Abs((float)_CountPosition / (_ShowTime / Time.deltaTime));
			DrawGlobalHealth(_Speed);
		}
		else
		{
			_renderer.Points = new Vector2[0];
			_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 255);
		}
	}

	public void VisibleHealth(bool inState)
	{
		_ShowHealth = inState;
	}


	public void DrawGlobalHealth(int inAddCountPosition)
	{
		int currentCount = _renderer.Points.Length;
		if (currentCount + inAddCountPosition > _CountPosition)
			inAddCountPosition = _CountPosition - currentCount;

		_renderer.Points = new Vector2[currentCount + inAddCountPosition];
		_renderer.Points[0].x = 0;
		_renderer.Points[0].y = 0;
		for (int i = 1; i < _renderer.Points.Length; i++)
		{
			float phi = (i + p) * Mathf.PI / phis;
			float r = k * phi;
			_renderer.Points[i].x = r * Mathf.Cos(phi);
			_renderer.Points[i].y = r * Mathf.Sin(phi);
		}
	}
}
