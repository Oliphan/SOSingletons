using UnityEngine;

namespace lmr
{
	/// <summary>
	/// A MonoBehaviour singleton class to provide callbacks for Unity runtime events such as Update, FixedUpdate, etc.
	/// Set to execute before the default execution order of 0, so that "manager" classes can subscribe to be updated before other scripts.
	/// </summary>
	[DefaultExecutionOrder(-100)]
	public class RTCBProvider : MonoBehaviour
	{
		static private RTCBProvider m_Instance;
		static public RTCBProvider instance
		{
			get
			{
				if (!m_Instance)
				{
					new GameObject("Runtime Callback Provider").AddComponent<RTCBProvider>();
				}
				return m_Instance;
			}
		}
		public delegate void RTCBDelegate();
		private event RTCBDelegate m_Update;
		private event RTCBDelegate m_FixedUpdate;
		private event RTCBDelegate m_LateUpdate;
		private event RTCBDelegate m_OnGUI;
		private event RTCBDelegate m_OnDrawGizmos;
		public static event RTCBDelegate update
		{
			add { instance.m_Update += value; }
			remove { instance.m_Update -= value; }
		}
		public static event RTCBDelegate fixedUpdate
		{
			add { instance.m_FixedUpdate += value; }
			remove { instance.m_FixedUpdate -= value; }
		}
		public static event RTCBDelegate lateUpdate
		{
			add { instance.m_LateUpdate += value; }
			remove { instance.m_LateUpdate -= value; }
		}
		public static event RTCBDelegate onGUI
		{
			add { instance.m_OnGUI += value; }
			remove { instance.m_OnGUI -= value; }
		}
		public static event RTCBDelegate onDrawGizmos
		{
			add { instance.m_OnDrawGizmos += value; }
			remove { instance.m_OnDrawGizmos -= value; }
		}

		private void Awake()
		{
			if (null == m_Instance)
			{
				m_Instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else if (m_Instance != this)
			{
				Debug.LogError("RTCBProvider has been instantiated twice! Destroying secondary instance. Please do not instantiate a RTCBProvider manually. One will be instantiated automatically when needed.");
				Destroy(this.gameObject);
			}
		}
		private void Update() { m_Update?.Invoke(); }
		private void FixedUpdate() { m_FixedUpdate?.Invoke(); }
		private void LateUpdate() { m_LateUpdate?.Invoke(); }
		private void OnGUI() { m_OnGUI?.Invoke(); }
		private void OnDrawGizmos() { m_OnDrawGizmos?.Invoke(); }
	}
}
