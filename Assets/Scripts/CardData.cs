#region Copyright Notice

// ******************************************************************************************************************
// 
// UnusualCommunication.CardData.cs © Shadow Wolf Development (SilentWolf6662 & Bambinidk) - All Rights Reserved
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
using System;
using UnityEngine;
namespace UnusualCommunication
{
	[CreateAssetMenu(fileName = "Card Data"), Serializable]
	public class CardData : ScriptableObject
	{
		[Header("Graphic Setup"), Space] public Sprite frontSprite;
		public Sprite backSprite;
		[Space(20)] public QAData[] data;

	}
}
