using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public static CameraController Instance;
	public Transform player;
	public Vector3 offset;
  
    // Use this for initialization
    public Coroutine my_co;

    void Start()
    {
     
    }


    void Update()
    {
		transform.position = new Vector3 (player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }



}
