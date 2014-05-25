/*
 * Inspired by the Messenger class by Udell Games
 * http://www.udellgames.com/posts/messenger/
 */

using UnityEngine;
using System.Collections.Generic;
using System;

public class Messenger : MonoBehaviour {
	private Dictionary<Type, Delegate> actions = new Dictionary<Type, Delegate>();
	private Dictionary<Type, Delegate> functions = new Dictionary<Type, Delegate>();

	public bool debug;

	/// <summary>
	/// Sends the specified message.
	/// </summary>
	/// <typeparam name="T">The type of message.</typeparam>
	/// <param name="message">The message.</param>
	public void Send<T>(T message) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Send ({0}):{1}", typeof(T).ToString(), message.ToString()));
		#endif

		if (actions.ContainsKey(typeof(T))) {
			((Action<T>)actions[typeof(T)])(message);
		}
	}

	/// <summary>
	/// Sends an asynchronous message.
	/// </summary>
	/// <typeparam name="T">The type of message.</typeparam>
	/// <param name="message">The message.</param>
	/// <param name="callback">The callback to be executed when the message receive function has completed</param>
	public IAsyncResult SendAsync<T>(T message, AsyncCallback callback = null) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Send Async ({0}):{1}", typeof(T).ToString(), message.ToString()));
		#endif

		if (actions.ContainsKey(typeof(T))) {
			return ((Action<T>)actions[typeof(T)]).BeginInvoke(message, callback, null);
		}

		return null;
	}

	/// <summary>
	/// Send a request.
	/// </summary>
	/// <typeparam name="T">The type of the parameter of the request.</typeparam>
	/// <typeparam name="R">The return type of the request.</typeparam>
	/// <returns>The result of the request.</returns>
	public R Request<T, R>(T parameter) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Request ({0}):{2} -> ({1})", typeof(T).ToString(), typeof(R).ToString(), parameter.ToString()));
		#endif

		if (functions.ContainsKey(typeof(T))) {
			var function = functions[typeof(T)] as Func<T, R>;

			if (function != null) {
				return function(parameter);
			}
		}

		return default(R);
	}

	/// <summary>
	/// Sends an asynchronous request.
	/// </summary>
	/// <typeparam name="T">The type of message being sent</typeparam>
	/// <param name="callback">The callback to be executed when the request finishes.</param>
	/// <returns>An IAsyncResult for the asynchronous operation.</returns>
	public IAsyncResult RequestAsync<T, R>(T parameter, AsyncCallback callback = null) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Request Async ({0}):{2} -> ({1})", typeof(T).ToString(), typeof(R).ToString(), parameter.ToString()));
		#endif

		if (functions.ContainsKey(typeof(T))) {
			var function = functions[typeof(T)] as Func<T, R>;

			if (function != null) {
				return function.BeginInvoke(parameter, callback, null);
			}
		}

		return null;
	}

	/// <summary>
	/// Register a function for a request message
	/// </summary>
	/// <typeparam name="T">Type of message to receive</typeparam>
	/// <param name="request">The function that fills the request</param>
	public void Register<T, R>(Func<T, R> request) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Registered Request ({0}) -> ({1})", typeof(T).ToString(), typeof(R).ToString()));
		#endif

		if (functions.ContainsKey(typeof(T))) {
			functions[typeof(T)] = Delegate.Combine(functions[typeof(T)], request);
		}
		else {
			functions.Add(typeof(T), request);
		}
	}

	/// <summary>
	/// Register an action for a message.
	/// </summary>
	/// <typeparam name="T">Type of message to receive</typeparam>
	/// <param name="action">The action that happens when the message is received.</param>
	public void Register<T>(Action<T> action) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Registered ({0})", typeof(T).ToString()));
		#endif

		if (actions.ContainsKey(typeof(T))) {
			actions[typeof(T)] = (Action<T>)Delegate.Combine(actions[typeof(T)], action);
		}
		else {
			actions.Add(typeof(T), action);
		}
	}

	/// <summary>
	/// Unregister a request
	/// </summary>
	/// <typeparam name="T">The type of request to unregister.</typeparam>
	/// <typeparam name="R">The return type of the request.</typeparam>
	/// <param name="request">The request to unregister.</param>
	public void Unregister<T,R>(Func<T,R> request) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Unregistered Request ({0}) -> ({1})", typeof(T).ToString(), typeof(R).ToString()));
		#endif

		if(functions.ContainsKey(typeof(T))) {
			functions[typeof(T)] = Delegate.Remove(functions[typeof(T)], request);
		}
	}

	/// <summary>
	/// Unregister an action.
	/// </summary>
	/// <typeparam name="T">The type of message.</typeparam>
	/// <param name="action">The action to unregister.</param>
	public void Unregister<T>(Action<T> action) {
		#if DEBUG
		if (debug) Debug.Log(string.Format("Unregistered ({0})", typeof(T).ToString()));
		#endif

		if(actions.ContainsKey(typeof(T))) {
			actions[typeof(T)] = (Action<T>)Delegate.Remove(actions[typeof(T)], action);
		}
	}
}
