using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCard : MonoBehaviour {
    public int pointValue;
	public string type;
	public int hits;
	public int speed;
	public int turns;
	public int shields;
	public int armour;
	public int frontArmour;
	public int turrets;
    public ArrayList weapons;
    public Vector3 previousPosition;
    public float totalRotation;

	// Use this for initialization
	void Start () {
        previousPosition = gameObject.transform.position;
        totalRotation = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
