//  
//  GasDictionary.cs
//  
//  Author:
//       Justin Bool <booljayj@gmail.com>
// 
//  Copyright (c) 2013 
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GasDictionary : ScriptableObject {
	[SerializeField] public List<GasPair> list;
	public Dictionary<string,float> dictionary;
	
	public GasDictionary() {
		dictionary = new Dictionary<string, float>();
		if (list == null) {
			Debug.Log("List was recreated");
			list = new List<GasPair>();
		}
	}
	
	public float this[string k] {
		get {
			if (dictionary.ContainsKey(k)) return dictionary[k];
			else return 0f;
		}
		set {
			if (dictionary.ContainsKey(k))
				dictionary[k] = value;
			else
				dictionary.Add(k, value);
		}
	}
	
	#region Serialization
	void OnEnable() {
		Debug.Log("Dictionary Enabled");
		dictionary.Clear();
		foreach (GasPair pair in list) {
			dictionary.Add(pair.type, pair.val);
		}
	}
	
	void OnDisable() {
		Debug.Log("Dictionary Disabled");
		list.Clear();
		foreach (KeyValuePair<string,float> pair in dictionary) {
			list.Add(new GasPair(pair.Key, pair.Value));
		}
	}
	#endregion
	
	#region Dictionary Access
	public void Clear() {
		dictionary.Clear();
	}
	
	public void Add(string k, float v) {
		dictionary.Add(k,v);
	}
	
	public void Remove(string k) {
		dictionary.Remove(k);
	}
	
	public Dictionary<string,float>.Enumerator GetEnumerator() {
		return dictionary.GetEnumerator();
	}
	
	public Dictionary<string,float>.KeyCollection Keys {
		get {return dictionary.Keys;}
	}
	
	public Dictionary<string,float>.ValueCollection Values {
		get {return dictionary.Values;}
	}
	
	public int Count {
		get {return dictionary.Count;}
	}
	#endregion
}

[System.Serializable]
public class GasPair {
	public string type;
	public float val;
	
	public GasPair(string type, float val) {
		this.type = type;
		this.val = val;
	}
	
	public override bool Equals(object obj) {
		GasPair P = obj as GasPair;
		if (P == null || P.type != type) return false;
		else return true;
	}
	public override int GetHashCode() {
		return type.GetHashCode();
	}
}