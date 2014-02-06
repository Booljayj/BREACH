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
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GasDictionary : IEnumerable {
	[SerializeField] private List<int> indexes = new List<int>();
	[SerializeField] private List<string> keys = new List<string>();
	[SerializeField] private List<float> values = new List<float>();
	
	public List<string> Keys {get{return keys;}}
	public List<float> Values {get{return values;}}
	
	public float this[string k] {
		get {
			int index = indexes.IndexOf(k.GetHashCode());
			if (index == -1)
				return 0;
			else
				return values[index];
		}
		set {
			int index = indexes.IndexOf(k.GetHashCode());
			if (index == -1) {
				indexes.Add(k.GetHashCode());
				keys.Add(k);
				values.Add(value);
			} else {
				values[index] = value;
			}
		}
	}
	
	public void Clear() {
		indexes.Clear(); keys.Clear(); values.Clear();
	}
	
	public void Add(string k, float v) {
		indexes.Add(k.GetHashCode()); keys.Add(k); values.Add(v);
	}
	
	public void Remove(string k) {
		int index = indexes.IndexOf(k.GetHashCode());
		if (index < -1) {
			indexes.RemoveAt(index);
			keys.RemoveAt(index);
			values.RemoveAt(index);
		} else {
			Debug.LogWarning("Attempted to remove a nonexistent gas");
		}
	}

	public int IndexOf(string k) {
		return indexes.IndexOf(k.GetHashCode());
	}
	
	public IEnumerator GetEnumerator() {
		return new GasDictionaryIterator(keys, values);
	}

	public List<KeyValuePair<string, float>> ToList() {
		List<KeyValuePair<string, float>> list = new List<KeyValuePair<string, float>>();
		for (int i = 0; i < keys.Count; i++) {
			list.Add(new KeyValuePair<string, float>(keys[i], values[i]));
		}
		return list;
	}
}

public class GasDictionaryIterator : IEnumerator {
	int index = -1;
	
	List<string> keys;
	List<float> values;
	
	public GasDictionaryIterator(List<string> keylist, List<float> valuelist) {
		keys = keylist;
		values = valuelist;
	}
	
	public bool MoveNext() {
		index++;
		return (index < keys.Count);
	}
	
	public void Reset() {
		index = -1;
	}
	
	object IEnumerator.Current {
		get {return Current;}
	}
	
	KeyValuePair<string, float> Current {
		get {return new KeyValuePair<string, float>(keys[index], values[index]);}
	}
}