﻿using System.Collections;
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
        //phase.Add("ordinance");
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
                if ((playerFleet[shipNum].gameObject.transform.position - playerFleet[shipNum].previousPosition).magnitude >= ((float)playerFleet[shipNum].speed / 2) / 10)
                {   
                    if (shipNum < playerFleet.Length - 1)
                    {
                        shipNum++;
                    }
                    else if (shipNum == playerFleet.Length - 1)
                    {
                        phaseNum++;
                        shipNum = 0;
                    }
                }
            }
        }
        else if (phase[phaseNum] == "shooting")
        {
            int enemyShipNum = 0;
            int weaponNum = 0;
            WeaponCard[] shipWeapons = playerFleet[shipNum].gameObject.GetComponentsInChildren<WeaponCard>();
            bool isInRange = false;
            if (Vector3.Distance(playerFleet[shipNum].transform.position, enemyFleet[enemyShipNum].transform.position) <= shipWeapons[weaponNum].range)
            {
                Vector3 targetDir = playerFleet[shipNum].transform.position - enemyFleet[enemyShipNum].transform.position;
                float angle = Vector3.Angle(targetDir, playerFleet[shipNum].transform.forward);
                bool isInLeftArc = angle >= 45 && angle <= 135;
                bool isInRightArc = angle >= 225 && angle <= 315;
                bool isInFrontArc = angle >= 315 && angle <= 45;

                if (shipWeapons[weaponNum].fireArc == "Left")
                {
                    if (isInLeftArc)
                    {
                        isInRange = true;
                    }
                }
                else if (shipWeapons[weaponNum].fireArc == "Right")
                {
                    if (isInRightArc)
                    {
                        isInRange = true;
                    }
                }
                else if (shipWeapons[weaponNum].fireArc == "Front")
                {
                    if (isInFrontArc)
                    {
                        isInRange = true;
                    }
                }
                else if (shipWeapons[weaponNum].fireArc == "Left/Front")
                {
                    if (isInLeftArc || isInFrontArc)
                    {
                        isInRange = true;
                    }
                }
                else if (shipWeapons[weaponNum].fireArc == "Front/Right")
                {
                    if (isInFrontArc || isInRightArc)
                    {
                        isInRange = true;
                    }
                }
                else if (shipWeapons[weaponNum].fireArc == "Left/Front/Right")
                {
                    if (isInLeftArc || isInFrontArc || isInRightArc)
                    {
                        isInRange = true;
                    }
                }
            }
            if (!isInRange)
            {
                if (enemyShipNum < enemyFleet.Length - 1)
                {
                    enemyShipNum++;
                }
                else if (enemyShipNum == enemyFleet.Length - 1)
                {
                    enemyShipNum = 0;
                    if (weaponNum < shipWeapons.Length - 1)
                    {
                        weaponNum++;
                    }
                    else if (weaponNum == shipWeapons.Length - 1)
                    {
                        if (shipNum < playerFleet.Length - 1)
                        {
                            shipNum++;
                        }
                       else if (shipNum == playerFleet.Length - 1)
                        {
                            shipNum = 0;
                            phaseNum = 0;
                            //other players turn
                        }
                    }
                }
   
            }
            else if (isInRange)
            {
                if (Input.GetKey(KeyCode.KeypadEnter))
                {
                    enemyFleet[enemyShipNum].shields -= shipWeapons[weaponNum].firepower;
                    if(enemyFleet[enemyShipNum].shields <= 0)
                    {
                        enemyFleet[enemyShipNum].hits -= shipWeapons[weaponNum].firepower; // expand upon
                    }
                    print("firing: " + shipWeapons[weaponNum] + " at " + enemyFleet[enemyShipNum]);
                    print("health at " + enemyFleet[enemyShipNum].hits);
                    if (weaponNum < shipWeapons.Length - 1)
                    {
                        weaponNum++;
                    }
                    else if (weaponNum == shipWeapons.Length - 1)
                    {
                        weaponNum = 0;
                        shipNum++;
                    }
                    else if (shipNum == playerFleet.Length - 1)
                    {
                        shipNum = 0;
                        phaseNum++;
                    }

                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (enemyShipNum > 0)
                    {
                        enemyShipNum--;
                    }
                    else if (enemyShipNum == 0)
                    {
                        enemyShipNum = enemyFleet.Length - 1;
                    }
                    print("switching to previous ship");
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (enemyShipNum < enemyFleet.Length - 1)
                    {
                        enemyShipNum++;
                    }
                    else if (enemyShipNum == enemyFleet.Length - 1)
                    {
                        enemyShipNum = 0;
                    }
                    print("switching to next ship");
                }
            }
        }
    }
}
