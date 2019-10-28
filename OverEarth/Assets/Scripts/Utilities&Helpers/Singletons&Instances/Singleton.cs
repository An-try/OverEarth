using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	private static bool _isDestroy;

	private protected virtual void Awake ()
	{
		if (!_instance)
			_instance = this as T;
		else if (!_instance.Equals (this))
			Destroy (gameObject);
	}

    private protected virtual void OnDestroy ()
	{
		_isDestroy = true;
	}

	public static T Instance
	{
		get
		{
			if (_instance)
				return _instance;

			_instance = (T) FindObjectOfType (typeof(T));
			if (FindObjectsOfType (typeof(T)).Length > 1)
			{
				Debug.LogError ("[Singleton] Something went really wrong " +
				                " - there should never be more than 1 singleton!" +
				                " Reopening the scene might fix it.");
				return _instance;
			}

			if (!_isDestroy)
			{
				if (!_instance)
				{
					var singleton = new GameObject ();
					_instance = singleton.AddComponent<T> ();
					singleton.name = "(singleton) " + typeof(T);
					Debug.Log ("[Singleton] An instance of " + typeof(T) +
					           " is needed in the scene, so '" + singleton +
					           "' was created with DontDestroyOnLoad.");
				}
			}

			return _instance;
		}
	}
}