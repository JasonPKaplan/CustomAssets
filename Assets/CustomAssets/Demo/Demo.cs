using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	private float _VSize = 0;
	private float _HSize = 0;

	private float _ZDepth = 0;

	// Use this for initialization
	void Start () {
		_VSize = Camera.main.orthographicSize;
		_HSize = _VSize * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject newObj = ObjectPool.Instance.GetObjectForType ("DemoPrefab");
		if(newObj != null)
		{
			newObj.transform.position = new Vector3 (Random.Range(-_HSize, _HSize), Random.Range(-_VSize, _VSize), _ZDepth);
			_ZDepth -= 0.001f;
			newObj.GetComponent<Renderer>().material.SetColor("_Color", new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f)));
			newObj.GetComponent<Renderer>().material.SetFloat("_Brightness", Random.Range(-0.5f,0.5f));
			newObj.GetComponent<Renderer>().material.SetFloat("_Desaturation", Random.Range(0.0f,1.0f));
			newObj.transform.parent = this.transform;
		}
		else
		{
			ObjectPool.Instance.PoolObject(this.transform.GetChild(0).gameObject);
		}
	}
}
