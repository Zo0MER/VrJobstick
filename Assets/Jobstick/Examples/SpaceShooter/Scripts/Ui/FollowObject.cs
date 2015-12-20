using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine.UI;

public class FollowObject : MonoBehaviour
{
    public Transform fallowObject;

    public Camera cameraGame;
    public Camera cameraGui;

    public Vector3 pointToScreen;

    public void SetPosition(Transform fallow)
    {
        pointToScreen = cameraGame.WorldToScreenPoint(fallow.position);
        //transform.localPosition = pointToScreen;
    }

	void Start () 
	{
	
	}
	void Update ()
	{
        SetPosition(fallowObject);
	}
}
