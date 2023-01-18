#region Copyright Notice

// ******************************************************************************************************************
// 
// UnusualCommunication.CardController.cs © Shadow Wolf Development (SilentWolf6662 & More) - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2023-01-05
// 
// ******************************************************************************************************************

#endregion
#region

using System.Collections;
using UnityEngine;
using static UnityEngine.Debug;
using static UnityEngine.Physics2D;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable HeapView.ObjectAllocation

#endregion
namespace UnusualCommunication
{
	public class CardController : MonoBehaviour
	{
		public Sprite frontSprite;
		public Sprite backSprite;
		public float uncoverTime = 12.0f;
		private Camera camera1;

		// Use this for initialization
		private void Start()
		{
			camera1 = Camera.main;
			GameObject card = new GameObject("Card"); // parent object
			GameObject cardFront = new GameObject("CardFront");
			GameObject cardBack = new GameObject("CardBack");

			cardFront.transform.parent = card.transform; // make front child of card
			cardBack.transform.parent = card.transform; // make back child of card

			// front (motive)
			cardFront.AddComponent<SpriteRenderer>();
			cardFront.GetComponent<SpriteRenderer>().sprite = frontSprite;
			cardFront.GetComponent<SpriteRenderer>().sortingOrder = -1;

			// back
			cardBack.AddComponent<SpriteRenderer>();
			cardBack.GetComponent<SpriteRenderer>().sprite = backSprite;
			cardBack.GetComponent<SpriteRenderer>().sortingOrder = 1;

			card.tag = "CardTag";
			Transform transform1 = transform;
			card.transform.parent = transform1;
			card.transform.position = transform1.position;
			card.AddComponent<BoxCollider2D>();
			card.GetComponent<BoxCollider2D>().size = new Vector2(10f, 6.5f);

			Log("Start done");
		}


		// Update is called once per frame
		private void Update()
		{
			if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
			{
				//Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
				print(Input.mousePosition.ToString());
				RaycastHit2D hit = Raycast(Input.mousePosition, Input.mousePosition);
				// we hit a card
				if (hit.collider != null)
				{
					if (hit.collider.CompareTag("CardTag"))
					{
						Log(hit.collider.transform.parent.name);
						StartCoroutine(uncoverCard(hit.collider.gameObject.transform, true));
					}
				}
			}
		}

		private IEnumerator uncoverCard(Transform card, bool uncover)
		{

			float minAngle = uncover ? 0 : 180;
			float maxAngle = uncover ? 180 : 0;

			float t = 0;
			bool uncovered = false;

			while (t < 1f)
			{
				t += Time.deltaTime * uncoverTime;
				;

				float angle = Mathf.LerpAngle(minAngle, maxAngle, t);
				card.eulerAngles = new Vector3(0, angle, 0);

				if (angle is >= 90 and < 180 or >= 270 and < 360 && !uncovered)
				{
					uncovered = true;
					for (int i = 0; i < card.childCount; i++)
					{
						// reverse sorting order to show the otherside of the card
						// otherwise you would still see the same sprite because they are sorted 
						// by order not distance (by default)
						Transform c = card.GetChild(i);
						c.GetComponent<SpriteRenderer>().sortingOrder *= -1;

						yield return null;
					}
				}

				yield return null;
			}

			yield return 0;
		}
	}
}
