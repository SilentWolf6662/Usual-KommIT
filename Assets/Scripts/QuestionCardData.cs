using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace UnusualCommunication
{
	[CreateAssetMenu(fileName = "Question Card Data"), Serializable]
	public class QuestionCardData : ScriptableObject
	{
		[FormerlySerializedAs("cardData")] public CardMaterialData cardMaterialData;
		public string question;
	}
}