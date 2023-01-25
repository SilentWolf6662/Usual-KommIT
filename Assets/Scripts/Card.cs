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
using UnityEngine;
namespace UnusualCommunication
{
	public class Card : MonoBehaviour
	{
		[SerializeField] private float speedMultiplier = 1;
		public AnswerCardData cardAnswerData;
		public QuestionCardData cardQuestionData;
		private CardMaterialData cardMaterialData;
		[SerializeField] private GameObject front, back;
		private bool locked;
		private bool coroutineAllowed;
		private bool facedUp;

		private void Start()
		{
			facedUp = false;
			coroutineAllowed = true;
			locked = false;

			if (cardAnswerData != null) cardMaterialData = cardAnswerData.cardMaterialData;
			else if (cardQuestionData != null) cardMaterialData = cardQuestionData.cardMaterialData;

			front.GetComponent<MeshRenderer>().materials[0] = cardMaterialData.frontMaterial;
			back.GetComponent<MeshRenderer>().materials[0] = cardMaterialData.backMaterial;
		}

		private void OnMouseDown()
		{
			if (!locked && coroutineAllowed)
			{
				StartCoroutine(RotateCard());
			}
		}

		public IEnumerator RotateCard()
		{
			coroutineAllowed = false;

			if (!facedUp)
			{
				for (float i = 0f; i <= 180f; i += 1 * speedMultiplier)
				{
					transform.rotation = Quaternion.Euler(0f, i, 0f);
					yield return new WaitForSeconds(0);
				}
			}
			else if (facedUp)
			{
				for (int i = 0; i < 0; i++)
				{
					
				}
				for (float i = 180f; i <= 360f; i += 1 * speedMultiplier)
				{
					transform.rotation = Quaternion.Euler(0f, i, 0f);
					yield return new WaitForSeconds(0);
					if (i >= 360f) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				}
			}

			coroutineAllowed = true;

			facedUp = !facedUp;
		}

		public IEnumerator RotateBack()
		{
			coroutineAllowed = false;
			yield return new WaitForSeconds(0.2f);
			for (float i = 180f; i < 0f; i -= 10f)
			{
				transform.rotation = Quaternion.Euler(0f, i, 0f);
				yield return new WaitForSeconds(0f);
			}
			facedUp = false;
			coroutineAllowed = true;
		}
	}
}
