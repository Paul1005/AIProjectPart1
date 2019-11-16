using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCard : MonoBehaviour
{
    public int pointValue;
    public string type;
    public int hits;
    public int speed;
    public int turns;
    public int shields;
    private int maxShields;
    public int armour;
    public int turrets;
    public ArrayList weapons;
    public Vector3 previousPosition;
    public float totalRotation;

    // Use this for initialization
    void Start()
    {
        previousPosition = gameObject.transform.position;
        totalRotation = 0;
        maxShields = shields;
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
