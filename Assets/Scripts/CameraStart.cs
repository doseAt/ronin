using UnityEngine;
using System.Collections;

public class CameraStart : MonoBehaviour {


    private float size;
    private float ratio;

	void Start () 
    {
        ratio = GetComponent<Camera>().aspect;
        size = (-2.4f) * ratio + 7.0f;
        Camera.main.orthographicSize = size;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}
}
