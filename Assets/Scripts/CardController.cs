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

#endregion
namespace UnusualCommunication
{
	public class CardController : MonoBehaviour
	{
		[SerializeField] private Sprite frontSprite;
		[SerializeField] private Sprite backSprite;
		[SerializeField] private CardData cardData;
		[SerializeField] private bool shouldFlip = true;
		[SerializeField] private bool bypassRaycast;

		[SerializeField] private float uncoverTime = 12;
		private Camera cam;

		private void Start()
		{
			cam = Camera.main;
			GameObject card = new GameObject("Card"); // parent object
			GameObject cardFront = new GameObject("CardFront");
			GameObject cardBack = new GameObject("CardBack");

			cardFront.transform.parent = card.transform; // make front child of card
			cardBack.transform.parent = card.transform; // make front child of card

			// front (graphic)
			cardFront.AddComponent<SpriteRenderer>();
			SpriteRenderer cardFrontSR = cardFront.GetComponent<SpriteRenderer>();
			cardFrontSR.sprite = frontSprite;
			cardFrontSR.sortingOrder = -1;

			// back
			cardBack.AddComponent<SpriteRenderer>();
			SpriteRenderer cardBackSR = cardBack.GetComponent<SpriteRenderer>();
			cardBackSR.sprite = backSprite;
			cardBackSR.sortingOrder = 1;

			int cardWidth = (int)frontSprite.rect.width;
			int cardHeight = (int)frontSprite.rect.height;

			Debug.Log(cardWidth);
			Debug.Log(cardHeight);

			card.tag = "Card";
			Transform cardTransform = transform;
			card.transform.parent = cardTransform;
			card.transform.position = cardTransform.position;
			card.AddComponent<BoxCollider2D>();
			card.GetComponent<BoxCollider2D>().size = new Vector2(10, 6.4f);

			Debug.Log("Start done");
		}

		private void Update () 
		{
			if(Input.GetMouseButtonDown(0) || Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)) 
			{
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
				
				// We hit a card
				if (shouldFlip)
				{
					if (hit.collider == null) return;
					Debug.Log(hit.transform.parent.name);
					FlipCard(hit.collider.gameObject.transform);
				}
			}
		}

		public CardData GetCardData() => cardData;
		public void SetCardData(CardData data) => cardData = data;

		public void FlipCard(Transform card) => StartCoroutine(UncoverCard(card, true));

		private IEnumerator UncoverCard(Transform card, bool uncover)
		{

			float minAngle = uncover ? 0 : 180;
			float maxAngle = uncover ? 180 : 0; 

			float t = 0;
			bool uncovered = false;

			while(t < 1f) 
			{
				t += Time.deltaTime * uncoverTime;;

				float angle = Mathf.LerpAngle(minAngle, maxAngle, t);
				card.eulerAngles = new Vector3(0, angle, 0);

				if (angle is >= 90 and < 180 or >= 270 and < 360 && !uncovered) 
				{
					uncovered = true;
					for(int i = 0; i < card.childCount; i++) 
					{
						// reverse sorting order to show the otherside of the card
						// otherwise you would still see the same sprite because they are sorted 
						// by order not distance (by default)
						Transform c = card.GetChild(i);
						SpriteRenderer cardNextSprite = c.GetComponent<SpriteRenderer>();
						cardNextSprite.flipX = true;
						cardNextSprite.sortingOrder *= -1;

						yield return null;
					}
				}
				yield return null;
			}
			yield return 0;
		}
	}
}
