using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private ShipCard[] fleet;
    private bool isTurn;
	// Use this for initialization
	void Start () {
        fleet = GetComponentsInChildren<ShipCard>();
        isTurn = true;
	}
	
	// Update is called once per frame
	void Update () {
        while (isTurn)
        {
            isTurn = false;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {

        }
	}
}
