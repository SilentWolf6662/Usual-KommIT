#region Copyright Notice
// ******************************************************************************************************************
// 
// UnusualCommunication.Card.cs © Shadow Wolf Development (SilentWolf6662 & Bambinidk) - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2023-01-20
// 
// ******************************************************************************************************************
#endregion
using System.Collections;
using TMPro;
using UnityEngine;
namespace UnusualCommunication
{
	public class Card : MonoBehaviour
	{
		[SerializeField] private float backSpeedMultiplier = 3, frontSpeedMultiplier = 3;
		public AnswerCardData cardAnswerData;
		public QuestionCardData cardQuestionData;
		private CardMaterialData cardMaterialData;
		public TMP_Text questionText;
		[SerializeField] private GameObject front, back;
		public bool shouldLockAtStart;
		public bool locked;
		private bool coroutineAllowed;
		public bool facedUp;

		private void Start()
		{
			facedUp = false;
			coroutineAllowed = true;

			if (cardAnswerData != null)
				cardMaterialData = cardAnswerData.cardMaterialData;
			else if (cardQuestionData != null) 
				cardMaterialData = cardQuestionData.cardMaterialData;

			if (cardMaterialData.frontMaterial != null) 
				front.GetComponent<MeshRenderer>().materials[0] = cardMaterialData.frontMaterial;
			if (cardMaterialData.backMaterial != null)
				back.GetComponent<MeshRenderer>().materials[0] = cardMaterialData.backMaterial;
			RotateCard();
			locked = shouldLockAtStart;
		}

		private void OnMouseDown()
		{
			if (!locked && coroutineAllowed) 
				RotateCard();
		}

		public void RotateCard()
		{
			if (!locked && coroutineAllowed)
			{
				Task rotate = new Task(!facedUp ? RotateBack() : RotateFront());
				rotate.Finished += delegate(bool manual)
				{
					if (manual) Debug.Log("Rotate task was stopped unexpected manually.");
					else facedUp = !facedUp;
				};
			}
		}

		public IEnumerator RotateBack()
		{
			coroutineAllowed = false;
			for (float i = 0f; i <= 180f; i += 1 * backSpeedMultiplier)
			{
				transform.rotation = Quaternion.Euler(i*2, i, 0f);
				yield return new WaitForSeconds(0);
				if (i is >= -180f or <= -180f) transform.rotation = Quaternion.Euler(0f, -180f, 0f);
			}
			yield return new WaitForSeconds(0);
			coroutineAllowed = true;
		}

		public IEnumerator RotateFront()
		{
			coroutineAllowed = false;
			for (float i = 180f; i <= 360f; i += 1 * frontSpeedMultiplier)
			{
				transform.rotation = Quaternion.Euler(i, i, 0f);
				yield return new WaitForSeconds(0);
				if (i >= 360f) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
			yield return new WaitForSeconds(0);
			coroutineAllowed = true;
		}

		
	}
}
