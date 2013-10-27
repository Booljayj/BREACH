using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicDictionary<T,K> : IEnumerable where K: new () {
	Dictionary<T,K> dict = new Dictionary<T, K>();
	
	//Constructors
	public DynamicDictionary() {
		dict = new Dictionary<T, K>();
	}
	public DynamicDictionary(IDictionary<T,K> d) {
		dict = new Dictionary<T, K>(d);
	}
	
	//Enumerable Interface
	public IEnumerator GetEnumerator() {
		return dict.GetEnumerator();
	}
	public void Add(T key, K val) {
		dict.Add(key, val);
	}
	
	//The Magic
	public K this[T key] {
		get {
			if (!dict.ContainsKey(key)) return new K();
			return dict[key];
		}
		set {
			if (!dict.ContainsKey(key)) dict.Add(key, value);
			dict[key] = value;
		}
	}
}

