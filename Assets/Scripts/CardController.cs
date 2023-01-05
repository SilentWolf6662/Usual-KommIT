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

using System;
using UnityEngine;

#endregion
namespace UnusualCommunication
{
	public class CardController : MonoBehaviour
	{
		[SerializeField] private Sprite frontSprite;
		[SerializeField] private Sprite backSprite;

		[SerializeField] private float uncoverTime = 12;

		private void Start()
		{
			GameObject card = new GameObject("Card"); // parent object
			GameObject cardFront = new GameObject("CardFront");
			GameObject cardBack = new GameObject("CardBack");

			cardFront.transform.parent = card.transform; // make front child of card
			cardBack.transform.parent = card.transform; // make front child of card

			// front (graphic)
			cardFront.AddComponent<SpriteRenderer>();
			cardFront.GetComponent<SpriteRenderer>().sprite = frontSprite;
			cardFront.GetComponent<SpriteRenderer>().sortingOrder = -1;

			// back
			cardBack.AddComponent<SpriteRenderer>();
			cardBack.GetComponent<SpriteRenderer>().sprite = backSprite;
			cardBack.GetComponent<SpriteRenderer>().sortingOrder = 1;

			int cardWidth = (int)frontSprite.rect.width;
			int cardHeight = (int)frontSprite.rect.height;

			Debug.Log(cardWidth);
			Debug.Log(cardHeight);

			card.tag = "Card";
			Transform cardTransform = transform;
			card.transform.parent = cardTransform;
			card.transform.position = cardTransform.position;
			card.AddComponent<BoxCollider2D>();
			card.GetComponent<BoxCollider2D>().size = new Vector2(cardWidth, cardHeight);

			Debug.Log("Start done");
		}
	}
}
