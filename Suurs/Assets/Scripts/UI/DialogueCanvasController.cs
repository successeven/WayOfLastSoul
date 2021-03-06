﻿using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueCanvasController : MonoBehaviour
{
		public Animator animator;
		public TextMeshProUGUI textMeshProUGUI;

		protected Coroutine m_DeactivationCoroutine;

		protected readonly int m_HashActivePara = Animator.StringToHash("Active");

		IEnumerator SetAnimatorParameterWithDelay(float delay)
		{
				yield return new WaitForSeconds(delay);
				if (transform.gameObject.activeInHierarchy)
						animator.SetBool(m_HashActivePara, false);
		}

		public void ActivateCanvasWithText(string text)
		{
				if (m_DeactivationCoroutine != null)
				{
						StopCoroutine(m_DeactivationCoroutine);
						m_DeactivationCoroutine = null;
				}

				gameObject.SetActive(true);
				animator.SetBool(m_HashActivePara, true);
				textMeshProUGUI.text = text;
		}

		public void ActivateCanvasWithTranslatedText(string phraseKey)
		{
				if (m_DeactivationCoroutine != null)
				{
						StopCoroutine(m_DeactivationCoroutine);
						m_DeactivationCoroutine = null;
				}

				gameObject.SetActive(true);
				animator.SetBool(m_HashActivePara, true);

				string tempText = Translator.Instance[phraseKey].ToString().Replace("{n}", "\r\n");
				textMeshProUGUI.text = tempText;
				textMeshProUGUI.font = Translator.Instance.GetFont;
		}

		public void DeactivateCanvasWithDelay(float delay)
		{
				if (transform.gameObject.activeInHierarchy)
						m_DeactivationCoroutine = StartCoroutine(SetAnimatorParameterWithDelay(delay));
		}

		public void ActivateAndDeactivateCanvasWithTranslatedText(string phraseKey, float delay)
		{
				ActivateCanvasWithTranslatedText(phraseKey);
				DeactivateCanvasWithDelay(delay);
		}
}