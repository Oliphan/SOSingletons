using UnityEngine;

namespace lmr
{
	/// <summary>
	/// A MonoBehaviour singleton class to provide callbacks for Unity runtime events such as Update, FixedUpdate, etc.
	/// </summary>
	public class RTCBProvider : MonoBehaviour
	{
		static private RTCBProvider _instance;
		static public RTCBProvider inst
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
		public event RTCBDelegate update;
		public event RTCBDelegate fixedUpdate;
		public event RTCBDelegate lateUpdate;
		public event RTCBDelegate onGUI;
		public event RTCBDelegate onDrawGizmos;

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
