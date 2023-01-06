#region Copyright Notice

// ******************************************************************************************************************
// 
// UnusualCommunication.DebugUtils.cs © Shadow Wolf Development (SilentWolf6662 & Bambinidk) - All Rights Reserved
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
using System.Collections.Generic;
using System.Linq;
namespace UnusualCommunication
{
	public static class DebugUtils
	{
		public static string ToString(Array array) => array == null ? "null" : $"{{{string.Join(", ", array.Cast<object>().Select(o => o.ToString()).ToArray())}}}";

		public static string ToString<TKey, TValue>(Dictionary<TKey, TValue> dict) => dict == null ? "null" : $"{{{string.Join(", ", dict.Select(kvp => $"{kvp.Key}:{kvp.Value}").ToArray())}}}";
	}
}
