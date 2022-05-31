using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace lmr
{
	public class SOSingleton<T> : SOSingletonBase where T : SOSingleton<T>
	{
		protected static T m_Instance;
		public static T instance
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = Resources.Load<T>("Singletons/" + typeof(T).Name);
					mInstance.init();
				}
				return mInstance;
			}
		}
		protected virtual void init() { }
#if UNITY_EDITOR
		public static void CreateSingleton()
		{
			var created = ScriptableObject.CreateInstance<T>();
			void ensureFolder(string path, string folderName)
			{
				if (!AssetDatabase.IsValidFolder(path + "/" + folderName))
				{
					AssetDatabase.CreateFolder(path, folderName);
				}
			}
			ensureFolder("Assets", "Resources");
			ensureFolder("Assets/Resources", "Singletons");
			AssetDatabase.CreateAsset(created, "Assets/Resources/Singletons/" + typeof(T).Name + ".asset");
		}
#endif
	}

	public class SOSingletonBase : ScriptableObject { }
}
