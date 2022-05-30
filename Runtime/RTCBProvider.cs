using UnityEngine;

namespace lmr
{
	/// <summary>
	/// A MonoBehaviour singleton class to provide callbacks for Unity runtime events such as Update, FixedUpdate, etc.
	/// </summary>
	public class RTCBProvider : MonoBehaviour
	{
		static private RTCBProvider _instance;
		static public RTCBProvider instance
		{
			get
			{
				if (!_instance)
				{
					new GameObject("Runtime Callback Provider").AddComponent<RTCBProvider>();
				}
				return _instance;
			}
		}
		public delegate void RTCBDelegate();
		private event RTCBDelegate _update;
		private event RTCBDelegate _fixedUpdate;
		private event RTCBDelegate _lateUpdate;
		private event RTCBDelegate _onGUI;
		private event RTCBDelegate _onDrawGizmos;
		public static event RTCBDelegate update
		{
			add { instance._update += value; }
			remove { instance._update -= value; }
		}
		public static event RTCBDelegate fixedUpdate
		{
			add { instance._fixedUpdate += value; }
			remove { instance._fixedUpdate -= value; }
		}
		public static event RTCBDelegate lateUpdate
		{
			add { instance._lateUpdate += value; }
			remove { instance._lateUpdate -= value; }
		}
		public static event RTCBDelegate onGUI
		{
			add { instance._onGUI += value; }
			remove { instance._onGUI -= value; }
		}
		public static event RTCBDelegate onDrawGizmos
		{
			add { instance._onDrawGizmos += value; }
			remove { instance._onDrawGizmos -= value; }
		}

		private void Awake()
		{
			if (null == _instance)
			{
				_instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else if (_instance != this)
			{
				Debug.LogError("RTCBProvider has been instantiated twice! Destroying secondary instance. Please do not instantiate a RTCBProvider manually. One will be instantiated automatically when needed.");
				Destroy(this.gameObject);
			}
		}
		private void Update() { update?.Invoke(); }
		private void FixedUpdate() { fixedUpdate?.Invoke(); }
		private void LateUpdate() { lateUpdate?.Invoke(); }
		private void OnGUI() { onGUI?.Invoke(); }
		private void OnDrawGizmos() { onDrawGizmos?.Invoke(); }
	}
}
