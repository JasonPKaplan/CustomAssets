using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	/// <summary>
	/// The instance for global access.
	/// </summary>
    public static ObjectPool Instance;
   
    /// <summary>
    /// The object prefabs which the pool can handle.
    /// </summary>
    public GameObject[] ObjectPrefabs;
   
    /// <summary>
    /// The pooled objects currently available.
    /// </summary>
    public List<GameObject>[] PooledObjects;

	/// <summary>
	/// The amount of objects of each type to buffer.
	/// </summary>
	public int[] AmountToBuffer;

	public int DefaultBufferAmount = 3;

	/// <summary>
	/// The container object that we will keep unused pooled objects in so we dont clog up the editor with objects.
	/// </summary>
	protected GameObject _ContainerObject;

	/// <summary>
    /// The unpooled objects for repooling everything.
    /// </summary>
    private List<GameObject> _UnPooledObjects;
   
    void Awake ()
    {
        Instance = this;
		// Caution: Script execution order.
		Init ();
    }
   
	/// <summary>
	/// Init this instance.
	/// </summary>
	public void Init ()
	{
		_ContainerObject = new GameObject("ObjectPool");

        // Loop through the object prefabs and make a new list for each one.
        // We do this because the pool can only support prefabs set to it in the editor,
        // so we can assume the lists of pooled objects are in the same order as object prefabs in the array
        PooledObjects = new List<GameObject>[ObjectPrefabs.Length];
		_UnPooledObjects = new List<GameObject>();
        int i = 0;
        foreach (GameObject objectPrefab in ObjectPrefabs )
        {
            PooledObjects[i] = new List<GameObject>(); 
            int bufferAmount;
            if (i < AmountToBuffer.Length)
			{
				bufferAmount = AmountToBuffer[i];
			}
            else
			{
                bufferAmount = DefaultBufferAmount;
			}
            for (int n = 0; n < bufferAmount; n++)
            {
                GameObject newObj = Instantiate(objectPrefab) as GameObject;
                newObj.name = objectPrefab.name;
                PoolObject(newObj);
            }
            i++;
        }
	}
   
    /// <summary>
    /// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
    /// then null will be returned.
    /// </summary>
    /// <returns>
    /// The object for type.
    /// </returns>
    /// <param name='objectType'>
    /// Object type.
    /// </param>
    /// <param name='onlyPooled'>
    /// If true, it will only return an object if there is one currently pooled.
    /// </param>
    public GameObject GetObjectForType ( string objectType , bool onlyPooled = true)
    {
        for(int i = 0; i < ObjectPrefabs.Length; i++)
        {
			if (ObjectPrefabs[i].name == objectType)
            {
                if (PooledObjects[i].Count > 0)
                {
                    GameObject pooledObject = PooledObjects[i][0];
                    PooledObjects[i].RemoveAt(0);
                    pooledObject.transform.parent = null;
                    pooledObject.SetActive(true);
					_UnPooledObjects.Add(pooledObject);
                    return pooledObject;
                   
                }
				else if (!onlyPooled)
				{
					GameObject obj = Instantiate(ObjectPrefabs[i]) as GameObject;
					_UnPooledObjects.Add(obj);
                    return obj;
                }
                break;
            }
        }
           
        //If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
        return null;
    }
   
    /// <summary>
    /// Pools the object specified.  Will not be pooled if there is no prefab of that type.
    /// </summary>
    /// <param name='obj'>
    /// Object to be pooled.
    /// </param>
    public bool PoolObject ( GameObject obj )
    {
        for (int i = 0; i < ObjectPrefabs.Length; i++)
        {
            if (ObjectPrefabs[i].name == obj.name)
            {
                obj.SetActive(false);
                obj.transform.parent = _ContainerObject.transform;
                PooledObjects[i].Add(obj);
                return true;
            }
        }
		return false;
    }
	
	/// <summary>
	/// Pools all objects.
	/// </summary>
	/// <param name='onlyPooled'>
	/// If true, it will only pool objects from the initial buffer.
	/// </param>
	public void PoolAllObjects ( bool onlyPooled = true )
	{
		foreach (GameObject obj in _UnPooledObjects)
		{
			if (!onlyPooled && obj.name.Contains("(Clone)"))
			{
				obj.name = obj.name.Substring(0, obj.name.Length - 7);
			}
			if (!PoolObject(obj))
			{
				obj.SetActive(false);
				Object.Destroy(obj);
			}
		}
	}
}