using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace UnusualCommunication
{
	[CreateAssetMenu(fileName = "Answer Card Data"), Serializable]
	public class AnswerCardData : ScriptableObject
	{
		[FormerlySerializedAs("cardData")] public CardMaterialData cardMaterialData;
		[Space(20)] public QAData[] data;
	}
}