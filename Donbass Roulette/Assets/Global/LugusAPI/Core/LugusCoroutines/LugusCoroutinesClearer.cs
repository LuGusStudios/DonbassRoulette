using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The reason for the existence of this class is kinda convoluted:

// PROBLEM 1:
// LugusCoroutinesDefault is not a Monobehaviour and does not inherit from one of the LugusSingletons, contrary to most other singletons.
// This means its static singleton instance does not get nulled in the OnDestroy() method of the LugusSingletons.
// Implementing OnDestroy() on it directly also doesn't work, since, not being a Monobehaviour, it doesn't receive those events.
// If the static instance isn't nulled, it persists across scene loads.
// If it persists across scene loads, it can retain references to long destroyed coroutine handles, which will result in errors.

// PROBLEM 2:
// We could check if a handle == null in the LugusCoroutinesDefault's GetHandle() method and skip it if that's the case.
// However, LugusCoroutinesDefault's list of handles is not a list of LugusCoroutineHandleDefault, but of ILugusCoroutineHandle (which is an interface).
// When Unity Destroy()s an object, it does not actually (immediately) null the object. Instead, it replaces it with some sort of dummy object.
// To compensate for this, Unity overloads the == operator so that comparing this dummy object against null DOES return true.
// Unfortunately, this overload only holds for traditional Unity engine objects and NOT for interfaces, which means that 
// in the situation described above, myInterface == null will return false because of the dummy object.
// If we, therefore, want to check for destroyed coroutine handles, we'd first have to cast the interface to a Monobehaviour, 
// but that would beat the whole point of having an interface.

// For more info:
// http://answers.unity3d.com/questions/586144/destroyed-monobehaviour-not-comparing-to-null.html

// SO: This solution is to attach a Monobehaviour to the parent of the coroutine handle transforms. This Monobehavior CAN receive the OnDestroy() event, 
// and will will then set the singleton instance to null, solving the first problem and avoiding the second.

public class LugusCoroutinesClearer : MonoBehaviour 
{
	protected void OnDestroy()
	{
		LugusCoroutines.Change(null);
	}
}
