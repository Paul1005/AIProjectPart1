using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCard : MonoBehaviour
{
    public int pointValue;
    public string type;
    public int hits;
    public float speed;
    public int turns;
    public int shields;
    private int maxShields;
    public int armour;
    public int turrets;
    public ArrayList weapons;
    public Vector3 previousPosition;
    public float totalRotation;
    public bool specialOrderChosen;
    public int extraMovement;
    public int turnMultiplier;
    public int minMoveMultiplier;
    public float maxMoveMultiplier;

    // Use this for initialization
    void Start()
    {
        previousPosition = gameObject.transform.position;
        totalRotation = 0;
        maxShields = shields;
        specialOrderChosen = false;
        extraMovement = 0;
        turnMultiplier = 1;
        minMoveMultiplier = 1;
        maxMoveMultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (shields < 0)
        {
            shields = 0;
        }
        else if (shields > maxShields)
        {
            shields = maxShields;
        }

        if (hits <= 0)
        {
            Destroy(gameObject);
        }
    }
}
