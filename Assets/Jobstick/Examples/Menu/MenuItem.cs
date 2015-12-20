using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public string loadSceneName;

    public void OnPressItem()
    {
        Application.LoadLevel(loadSceneName);
    }

    public void Back()
    {
        Application.Quit();
    }

	void Start () 
	{

	}
	void Update () 
	{
	
	}
}
