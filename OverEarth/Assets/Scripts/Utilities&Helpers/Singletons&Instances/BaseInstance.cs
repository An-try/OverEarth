using UnityEngine;

namespace Bidon.Utilities
{
	public abstract class BaseInstance<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance)
				{
					return _instance;

				}
				_instance = (T)FindObjectOfType (typeof (T));
				return _instance;
			}
		}
	}
}