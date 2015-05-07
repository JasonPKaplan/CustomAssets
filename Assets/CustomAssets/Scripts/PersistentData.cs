using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PersistentData
{
	/// <summary>
	/// The Instance.
	/// </summary>
	private static PersistentData _Instance = null;
	public static PersistentData Instance
	{
		get
		{
			if(_Instance == null)
			{
				_Instance = new PersistentData();
				Load();
			}
			return _Instance;
		}
	}
	
	private Dictionary<string, object> _persistentData;

	private PersistentData()
	{
		_persistentData = new Dictionary<string, object>();
	}

	public static void Set(string key, object val)
	{
		if(Instance._persistentData.ContainsKey(key))
		{
			Instance._persistentData[key] = val;
		}
		else
		{
			Instance._persistentData.Add(key, val);	
		}
	}
	
	public static object Get(string key)
	{
		if(Instance._persistentData.ContainsKey(key))
		{
			return Instance._persistentData[key];
		}
		return null;
	}
	
	public static void Load()
	{
		string filename = Application.dataPath + "/StreamingAssets" + "/persdata";	
		if (System.IO.File.Exists (filename))
		{
			using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ();
				Instance._persistentData = (Dictionary<string, object>)formatter.Deserialize (stream);
			}
		}
	}
	
	public static void Save()
	{
		string directory = Application.dataPath + "/StreamingAssets";
		string filename =  directory + "/persdata";
		if (!System.IO.Directory.Exists (directory))
		{
			System.IO.Directory.CreateDirectory(directory);
		}
		using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))																												//No compression
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			formatter.Serialize(stream, Instance._persistentData);
		}
	}
}
