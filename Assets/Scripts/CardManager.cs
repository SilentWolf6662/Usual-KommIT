#region Copyright Notice
// ******************************************************************************************************************
// 
// UnusualCommunication.CardManager.cs © Shadow Wolf Development (SilentWolf6662 & Bambinidk) - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2023-04-20
// 
// ******************************************************************************************************************
#endregion
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
using Random = UnityEngine.Random;
namespace UnusualCommunication
{
	public class CardManager : MonoBehaviour
	{
		[SerializeField] private Card[] cards;
		private readonly List<Card> availableCards = new List<Card>();
		[SerializeField] private Card questionCard;
		[SerializeField] private QuestionCardData[] questions;
		private readonly List<QuestionCardData> availableQuestions = new List<QuestionCardData>();
		private bool runCode = true;

		private void Start()
		{
			availableCards.Clear();
			availableQuestions.Clear();
			foreach (Card card in cards) availableCards.Add(card);
			foreach (QuestionCardData question in questions) availableQuestions.Add(question);
			GiveRandomQuestion();
		}

		public void StartGame()
		{
			LoadScene(GetActiveScene().buildIndex);
		}

		private void GiveRandomQuestion()
		{
			int questionIndex = Random.Range(0, availableQuestions.Count);
			if (!(questionIndex + 1 <= availableQuestions.Count))
			{
				Debug.Log("Out of questions");
				ValidateCards();
				return;
			}
			questionCard.questionText.text = availableQuestions.ToArray()[questionIndex].question;
			availableQuestions.Remove(availableQuestions[questionIndex]);
		}
		
		public void ValidateCards()
		{
			if (!runCode) return;
			bool nextQuest = false;
			if (availableCards.Count == 1)
				if (availableCards[0].facedUp)
					LoadScene(1);
			for (int i = 0; i < availableCards.Count; i++)
			{
				Card card = availableCards[i];
				if (!card.facedUp) card.locked = true;
				if (!card.locked) continue;
				for (int j = 0; j < card.cardAnswerData.data.Length; j++)
					if (card.cardAnswerData.data[j].question == questionCard.questionText.text)
						if (!card.cardAnswerData.data[j].answer)
						{
							availableCards.Remove(availableCards[i]);
							nextQuest = true;
						}
						else if (card.cardAnswerData.data[j].answer)
						{
							Debug.Log("Lost");
							nextQuest = false;
							runCode = false;
						}
			}
			if (nextQuest) GiveRandomQuestion();
		}
	}
}
