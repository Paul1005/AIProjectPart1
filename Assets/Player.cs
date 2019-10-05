using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private ShipCard[] fleet;
    private bool isTurn;
    private List<string> phase = new List<string>();
    // Use this for initialization
    void Start()
    {
        fleet = GetComponentsInChildren<ShipCard>();
        isTurn = true;
        phase.Add("movement");
        phase.Add("shooting");
        phase.Add("ordinance");
    }

    // Update is called once per frame
    void Update()
    {
        /*while (isTurn)
        {
            isTurn = false;
        }*/

        int shipNum = 0;
        int phaseNum = 0;
        if (phase[phaseNum] == "movement")
        {
            int turnDistance = 0;

            if (fleet[shipNum].type == "Battleship")
            {
                turnDistance = 15;
            }
            else if (fleet[shipNum].type == "Cruiser")
            {
                turnDistance = 10;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if ((fleet[shipNum].gameObject.transform.position - fleet[shipNum].previousPosition).magnitude < (float)fleet[shipNum].speed / 10)
                {
                    fleet[shipNum].gameObject.transform.position += fleet[shipNum].gameObject.transform.right * 0.015f;
                }

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if ((fleet[shipNum].gameObject.transform.position - fleet[shipNum].previousPosition).magnitude >= (float)turnDistance / 10)
                {
                    if (fleet[shipNum].totalRotation < fleet[shipNum].turns)
                    {
                        fleet[shipNum].totalRotation += 0.375f;
                        fleet[shipNum].gameObject.transform.Rotate(new Vector3(0, 0, 0.375f));
                    }
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if ((fleet[shipNum].gameObject.transform.position - fleet[shipNum].previousPosition).magnitude >= (float)turnDistance / 10)
                {
                    if (fleet[shipNum].totalRotation < fleet[shipNum].turns)
                    {
                        fleet[shipNum].totalRotation += 0.375f;
                        fleet[shipNum].gameObject.transform.Rotate(new Vector3(0, 0, -0.375f));
                    }
                }
            }
        }
        else if (phase[phaseNum] == "shooting")
        {
            WeaponCard[] weapons = fleet[shipNum].gameObject.GetComponentsInChildren<WeaponCard>();

        }
        else if (phase[phaseNum] == "ordinance")
        {

        }

        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            phaseNum++;
        }

        //shipNum++;
    }
}
