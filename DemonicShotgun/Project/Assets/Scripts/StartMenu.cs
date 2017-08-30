using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour 
{

	void Start () 
	{
		GameManager.Instance.SetStartMenuState();
	}
}
