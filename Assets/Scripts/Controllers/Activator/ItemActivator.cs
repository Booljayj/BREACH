//  
//  ItemActivator.cs
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

public class ItemActivator : MonoBehaviour, ActivatorOld {
	public bool Activate(GameObject character, Pointer pointer) {
		if (pointer.stored != null) return false;
		
		StopAllCoroutines();
		StartCoroutine("GrabItem", character);
		pointer.delayed = DelayedActivate;
		return false;
	}
	
	public bool DelayedActivate(GameObject character, Pointer pointer) {
		StopAllCoroutines();
		StartCoroutine("DropItem", character);
		return false;
	}
	
	public bool ActiveUpdate(GameObject c) {return true;}
	public void Deactivate(GameObject c) {}
	
	IEnumerator GrabItem(GameObject character) {
		character.SendMessage("Freeze", true);
		transform.parent = character.transform;
		
		character.SendMessage("Freeze", false);
		yield return null;
	}
	
	IEnumerator DropItem(GameObject character) {
		character.SendMessage("Freeze", true);
		
		character.SendMessage("Freeze", false);
		yield return null;
	}
}

