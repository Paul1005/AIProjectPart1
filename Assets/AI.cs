using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AI : MonoBehaviour
{
    private Player player;
    bool isEnemyShipMarked;
    int enemyShip;
    bool firstMoveComplete;
    bool turnComplete;
    bool secondMoveComplete;
    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponent<Player>();
        isEnemyShipMarked = false;
        enemyShip = 0;
        firstMoveComplete = false;
        turnComplete = false;
        secondMoveComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isTurn)
        {
            if (!player.playerFleet[player.shipNum].specialOrderChosen)
            {
                /*player.allAheadFull();

                player.comeToNewHeading();

                player.burnRetros();

                player.lockOn();*/

                player.noSpecialOrder();
            }
            else if (player.phase[player.phaseNum] == "movement")
            {
                float distance = (player.playerFleet[player.shipNum].transform.position - player.enemyFleet[enemyShip].transform.position).magnitude;

                if (!isEnemyShipMarked)
                {
                    for (int i = 1; i < player.enemyFleet.Length; i++)
                    {
                        if ((player.playerFleet[player.shipNum].transform.position - player.enemyFleet[0].transform.position).magnitude < distance)
                        {
                            enemyShip = i;
                        }
                    }
                    isEnemyShipMarked = true;
                }
                else if (isEnemyShipMarked)
                {
                    Vector3 targetDir = player.enemyFleet[enemyShip].transform.position - player.playerFleet[player.shipNum].transform.position;
                    float angle = Vector3.SignedAngle(targetDir, player.playerFleet[player.shipNum].transform.up, Vector3.forward);

                    // Decision tree for movement
                    if (distance < 6)
                    {
                        if (!firstMoveComplete)
                        {
                            if ((player.playerFleet[player.shipNum].transform.position - player.playerFleet[player.shipNum].previousPosition).magnitude < player.playerFleet[player.shipNum].turnDistance / 10)
                            {
                                player.moveForward();
                            }
                            else
                            {
                                firstMoveComplete = true;
                            }
                        }
                        else if (!turnComplete)
                        {
                            if (player.playerFleet[player.shipNum].totalRotation < player.playerFleet[player.shipNum].turns && Mathf.Abs(angle) < 90)
                            {
                                if (angle > 0)
                                {
                                    player.turnLeft();
                                }
                                else if (angle < 0)
                                {
                                    player.turnRight();
                                }
                            } else
                            {
                                turnComplete = true;
                            }
                        }
                        else
                        {
                            player.finishMovingShip();
                            isEnemyShipMarked = false;
                            firstMoveComplete = false;
                            turnComplete = false;
                        }
                    }
                    else if (distance >= 6)
                    {
                        if (!firstMoveComplete)
                        {
                            if ((player.playerFleet[player.shipNum].transform.position - player.playerFleet[player.shipNum].previousPosition).magnitude < player.playerFleet[player.shipNum].turnDistance / 10)
                            {
                                player.moveForward();
                            }
                            else
                            {
                                firstMoveComplete = true;
                            }
                        }
                        else if (!turnComplete)
                        {
                            if (Mathf.Abs(angle) > 11.25 && player.playerFleet[player.shipNum].totalRotation < player.playerFleet[player.shipNum].turns)
                            {
                                if (angle < -11.25)
                                {
                                    player.turnLeft();
                                }
                                else if (angle > 11.25)
                                {
                                    player.turnRight();
                                }
                            }
                            else
                            {
                                turnComplete = true;
                            }
                        }
                        else if (!secondMoveComplete)
                        {
                            if ((player.playerFleet[player.shipNum].transform.position - player.playerFleet[player.shipNum].previousPosition).magnitude < player.playerFleet[player.shipNum].speed / 10)
                            {
                                player.moveForward();
                            }
                            else
                            {
                                secondMoveComplete = true;
                            }
                        }
                        else
                        {
                            player.finishMovingShip();
                            isEnemyShipMarked = false;
                            firstMoveComplete = false;
                            turnComplete = false;
                            secondMoveComplete = false;
                        }
                    }
                }
            }
            else if (player.phase[player.phaseNum] == "shooting")
            {
                WeaponCard[] shipWeapons = player.playerFleet[player.shipNum].gameObject.GetComponentsInChildren<WeaponCard>();
                float distance = (player.playerFleet[player.shipNum].transform.position - player.enemyFleet[player.enemyShipNum].transform.position).magnitude;
                string strFilePath = @"D:\Documents\Comp8901AIProject\MachineLearningTable.txt"; //change to system path at some point
                //string strFilePath = "laptoppath";
                string output = "test\ntest";
                File.AppendAllText(strFilePath, output);
                //print("Current Weapon is: " + shipWeapons[weaponNum]);
                bool fireWeapon = false;
                //player.enemyFleet[player.enemyShipNum].hits;
                for (int i = 0; i < player.enemyFleet.Length; i++)
                {

                    player.enemyShipNum = i;
                    if (player.isInRange)
                    {

                    }
                }
                if (fireWeapon)
                {
                    player.fireWeapon(shipWeapons, distance);
                }
                else if (fireWeapon) { 
                
                    if (player.enemyShipNum < player.enemyFleet.Length - 1)
                    {
                        player.enemyShipNum++;
                    }
                    else if (player.enemyShipNum == player.enemyFleet.Length - 1)
                    {
                        player.enemyShipNum = 0;
                    }
                    print("switching to next ship");
                }
            }
        }
    }
}
