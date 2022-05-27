using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace lmr
{
	public class SOSingletonAutoCreator : AssetPostprocessor
	{
		public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var singletonClasses = new List<System.Type>();
			foreach (var assembl in assemblies)
			{
				singletonClasses.AddRange(assembl.GetTypes().Where(t => t.IsSubclassOf(typeof(SOSingletonBase))));
			}
			foreach (var sc in singletonClasses)
			{
				if (sc.IsGenericType) { continue; }
				string location = "Assets/Resources/Singletons/" + sc.Name + ".asset";
				bool exists = assetExists(location);
				bool deleted = false;
				if (null != deletedAssets)
				{
					foreach (string item in deletedAssets)
					{
						var deletedName = item.Substring(item.LastIndexOf('/') + 1);
						if (string.Compare(sc.Name + ".cs", deletedName) == 0)
						{
							deleted = true;
							if (SOSingletonSettingsProvider.autoDelete.toggle)
							{
								Debug.Log("Auto-Deleting: " + location);
								AssetDatabase.DeleteAsset(location);
							}
							continue;
						}
					}
				}
				if (deleted) { continue; }
				if (!exists && SOSingletonSettingsProvider.autoCreate.toggle)
				{
					Debug.Log("Auto-Creating: " + location);
					sc.GetMethod("CreateSingleton", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Invoke(null, null);
				}
			}
		}
		static bool assetExists(string assetPath)
		{
			bool result = !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(assetPath));
			result &= (null != AssetDatabase.GetMainAssetTypeAtPath(assetPath));
			return result;
		}
	}

	static class SOSingletonSettingsProvider
	{
		public class ToggleOption
		{
			GUIContent _content;
			readonly string _key;
			bool _toggle;
			public ToggleOption(GUIContent content, string key, bool defaultValue)
			{
				_content = content;
				_key = key;
				_toggle = EditorPrefs.GetBool(_key, defaultValue);
			}
			public bool toggle
			{
				get
				{
					return _toggle;
				}
				set
				{
					if (value != _toggle)
					{
						_toggle = value;
						EditorPrefs.SetBool(_key, value);
					}
				}
			}
			public GUIContent content
			{
				get
				{
					return _content;
				}
			}
		}

		private static ToggleOption _autoCreate =
			new ToggleOption(
				new GUIContent("Singleton Auto-Ensure Existence", "Enables automatic creation of singleton instance assets when their class exists and one is not present in the Resources/Singletons folder."),
				"lmr.SOSingleton.autoCreate",
				true
			);
		private static ToggleOption _autoDelete =
			new ToggleOption(
				new GUIContent("Singleton Auto-Delete", "Enables automatic deletion of singleton instance assets in the Resources/Singletons folder when their class is deleted."),
				"lmr.SOSingleton.autoDelete",
				false
			);

		public static ToggleOption autoCreate
		{
			get
			{
				return _autoCreate;
			}
		}
		public static ToggleOption autoDelete
		{
			get
			{
				return _autoDelete;
			}
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			var provider = new SettingsProvider("Preferences/SOSingleton Settings", SettingsScope.User)
			{
				label = "SOSingleton Settings",

				guiHandler = (searchContext) =>
				{
					EditorGUI.indentLevel += 1;
					bool prevAC = autoCreate.toggle;
					autoCreate.toggle = EditorGUILayout.ToggleLeft(_autoCreate.content, autoCreate.toggle);
					if (prevAC != autoCreate.toggle && autoCreate.toggle)
					{
						SOSingletonAutoCreator.OnPostprocessAllAssets(null, null, null, null);
					}
					autoDelete.toggle = EditorGUILayout.ToggleLeft(_autoDelete.content, autoDelete.toggle);
					EditorGUI.indentLevel -= 1;
				},

				// Unity Preferences search keywords
				keywords = new HashSet<string>(new[] { "Singleton", "SO", "SOSingleton", "Scriptable" })
			};

			return provider;
		}
	}
}
