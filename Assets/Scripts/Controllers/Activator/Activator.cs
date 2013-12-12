//  
//  Activator.cs
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

public interface Activator {
	//the action upon activating. Return true if this will continue to the ActiveUpdate
	bool Activate(GameObject character, Pointer pointer);
	//the action which checks if we are still activating
	bool ActiveUpdate(GameObject character);
	//the action when we stop activating
	void Deactivate(GameObject character);
}

//a delayed activate happens after an activator has been clicked.
//It can also start the ActiveUpdate if it returns true;
public delegate bool DelayedActivate(GameObject character, Pointer pointer);