using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private ShipCard[] playerFleet;
    private ShipCard[] enemyFleet;
    private bool isTurn;
    private List<string> phase = new List<string>();
    int shipNum = 0;
    int phaseNum = 0;
    // Use this for initialization
    void Start()
    {
        playerFleet = GetComponentsInChildren<ShipCard>();
        GameObject enemyPlayer = GameObject.Find("Player2");
        enemyFleet = enemyPlayer.GetComponentsInChildren<ShipCard>();
        isTurn = true;
        phase.Add("movement");
        phase.Add("shooting");
        phase.Add("ordinance");
        shipNum = 0;
        phaseNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (phase[phaseNum] == "movement")
        {
            int turnDistance = 0;

            if (playerFleet[shipNum].type == "Battleship")
            {
                turnDistance = 15;
            }
            else if (playerFleet[shipNum].type == "Cruiser")
            {
                turnDistance = 10;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if ((playerFleet[shipNum].gameObject.transform.position - playerFleet[shipNum].previousPosition).magnitude < (float)playerFleet[shipNum].speed / 10)
                {
                    playerFleet[shipNum].gameObject.transform.position += playerFleet[shipNum].gameObject.transform.right * 0.015f;
                }

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if ((playerFleet[shipNum].gameObject.transform.position - playerFleet[shipNum].previousPosition).magnitude >= (float)turnDistance / 10)
                {
                    if (playerFleet[shipNum].totalRotation < playerFleet[shipNum].turns)
                    {
                        playerFleet[shipNum].totalRotation += 0.375f;
                        playerFleet[shipNum].gameObject.transform.Rotate(new Vector3(0, 0, 0.375f));
                    }
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if ((playerFleet[shipNum].gameObject.transform.position - playerFleet[shipNum].previousPosition).magnitude >= (float)turnDistance / 10)
                {
                    if (playerFleet[shipNum].totalRotation < playerFleet[shipNum].turns)
                    {
                        playerFleet[shipNum].totalRotation += 0.375f;
                        playerFleet[shipNum].gameObject.transform.Rotate(new Vector3(0, 0, -0.375f));
                    }
                }
            }
            else if (Input.GetKey(KeyCode.KeypadEnter))
            {
                print((float)(playerFleet[shipNum].speed / 2) / 10);
                print((playerFleet[shipNum].gameObject.transform.position - playerFleet[shipNum].previousPosition).magnitude);

                if ((playerFleet[shipNum].gameObject.transform.position - playerFleet[shipNum].previousPosition).magnitude >= ((float)playerFleet[shipNum].speed / 2) / 10)
                {
                    print(shipNum < playerFleet.Length - 1);
                    if (shipNum < playerFleet.Length - 1)
                    {
                        shipNum++;
                    }
                    else if (shipNum == playerFleet.Length - 1)
                    {
                        phaseNum++;
                        shipNum = 0;
                    }
                    else if (phaseNum == phase.Count - 1)
                    {
                        //next players turn
                    }
                }
            }
        }
        else if (phase[phaseNum] == "shooting")
        {
            WeaponCard[] shipWeapons = playerFleet[shipNum].gameObject.GetComponentsInChildren<WeaponCard>();
            foreach (WeaponCard shipWeapon in shipWeapons)
            {
                foreach(ShipCard ship in enemyFleet)
                {
                    if (Vector3.Distance(playerFleet[shipNum].transform.position, ship.transform.position) <= shipWeapon.range) {
                        Vector3 targetDir = playerFleet[shipNum].transform.position - ship.transform.position;
                        float angle = Vector3.Angle(targetDir, playerFleet[shipNum].transform.forward);

                        if(shipWeapon.fireArc == "Left")
                        {
                            if (angle >= 45 && angle <=135)
                            {

                            }
                        } else if (shipWeapon.fireArc == "Right")
                        {
                            if ()
                            {

                            }
                        } else if (shipWeapon.fireArc == "Front")
                        {
                            if ()
                            {

                            }
                        } else if (shipWeapon.fireArc == "Left/Front")
                        {
                            if ()
                            {

                            }
                        } else if (shipWeapon.fireArc == "Front/Right")
                        {
                            if ()
                            {

                            }
                        } else if (shipWeapon.fireArc == "Left/Front/Right")
                        {
                            if ()
                            {

                            }
                        }
                    }
                }
            }

        }
        else if (phase[phaseNum] == "ordinance")
        {

        }
    }
}
