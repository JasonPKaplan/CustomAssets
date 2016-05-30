using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIImageAnimation : MonoBehaviour {

	public Sprite[] SpriteAnimation;
	public int FPS = 24;
	private Image _Image;
	private int _Index = 0;
	private float _ElapsedTime;

	void Start ()
	{
		_Image = this.GetComponent<Image> ();
		if (SpriteAnimation.Length > 0 && SpriteAnimation [0] != null)
		{
			_Image.sprite = SpriteAnimation [0];
		}
	}
	
	void Update ()
	{
		_ElapsedTime += Time.deltaTime;
		if (_ElapsedTime > 1.0f / FPS)
		{
			_ElapsedTime -= 1.0f / FPS;
			_Index++;
			if (_Index >= SpriteAnimation.Length)
			{
				_Index = 0;
			}
			_Image.sprite = SpriteAnimation [_Index];
		}
	}
}
