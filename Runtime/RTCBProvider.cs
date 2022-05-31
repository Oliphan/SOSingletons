using UnityEngine;

namespace lmr
{
	/// <summary>
	/// A MonoBehaviour singleton class to provide callbacks for Unity runtime events such as Update, FixedUpdate, etc.
	/// </summary>
	public class RTCBProvider : MonoBehaviour
	{
		static private RTCBProvider mInstance;
		static public RTCBProvider instance
		{
			get
			{
				if (!mInstance)
				{
					new GameObject("Runtime Callback Provider").AddComponent<RTCBProvider>();
				}
				return mInstance;
			}
		}
		public delegate void RTCBDelegate();
		private event RTCBDelegate mUpdate;
		private event RTCBDelegate mFixedUpdate;
		private event RTCBDelegate mLateUpdate;
		private event RTCBDelegate mOnGUI;
		private event RTCBDelegate mOnDrawGizmos;
		public static event RTCBDelegate update
		{
			add { instance.mUpdate += value; }
			remove { instance.mUpdate -= value; }
		}
		public static event RTCBDelegate fixedUpdate
		{
			add { instance.mFixedUpdate += value; }
			remove { instance.mFixedUpdate -= value; }
		}
		public static event RTCBDelegate lateUpdate
		{
			add { instance.mLateUpdate += value; }
			remove { instance.mLateUpdate -= value; }
		}
		public static event RTCBDelegate onGUI
		{
			add { instance.mOnGUI += value; }
			remove { instance.mOnGUI -= value; }
		}
		public static event RTCBDelegate onDrawGizmos
		{
			add { instance.mOnDrawGizmos += value; }
			remove { instance.mOnDrawGizmos -= value; }
		}

		private void Awake()
		{
			if (null == mInstance)
			{
				mInstance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else if (mInstance != this)
			{
				Debug.LogError("RTCBProvider has been instantiated twice! Destroying secondary instance. Please do not instantiate a RTCBProvider manually. One will be instantiated automatically when needed.");
				Destroy(this.gameObject);
			}
		}
		private void Update() { mUpdate?.Invoke(); }
		private void FixedUpdate() { mFixedUpdate?.Invoke(); }
		private void LateUpdate() { mLateUpdate?.Invoke(); }
		private void OnGUI() { mOnGUI?.Invoke(); }
		private void OnDrawGizmos() { mOnDrawGizmos?.Invoke(); }
	}
}
