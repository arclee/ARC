using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class arcGameObjectExtension
{

	public static bool IsInLayerMask(this GameObject obj, LayerMask mask){
		return (((1 << obj.layer) & mask.value) > 0);
	}
	
	public static I GetSafeComponent<I>(this GameObject obj) where I : Component
	{
		I comp = obj.GetComponent(typeof(I)) as I;
		
		if(comp == null)
		{
			Debug.Log("Expected to find component of type " 
			               + typeof(I) + " but found none", obj);

			arcErrCollector.Add("Expected to find component", obj, "");
		}
		
		return comp;
	}

	
	//Defined in the common base class for all mono behaviours
	public static I GetInterfaceComponent<I>(this GameObject obj) where I : class
	{
		return obj.GetComponent(typeof(I)) as I;
	}
	
	public static List<I> FindObjectsOfInterface<I>(this GameObject obj) where I : class
	{
		MonoBehaviour[] monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
		List<I> list = new List<I>();
		
		foreach(MonoBehaviour behaviour in monoBehaviours)
		{
			I component = behaviour.GetComponent(typeof(I)) as I;
			
			if(component != null)
			{
				list.Add(component);
			}
		}
		
		return list;
	}
}
