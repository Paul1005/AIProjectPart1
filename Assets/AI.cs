using System.Collections;
using System.Collections.Generic;
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
            if (player.phase[player.phaseNum] == "movement")
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
                    if (distance < 3)
                    {
                        if ((angle > 45 && angle < 135) || (angle < 325 && angle > 225))
                        {
                            if ((player.playerFleet[player.shipNum].transform.position - player.playerFleet[player.shipNum].previousPosition).magnitude < player.playerFleet[player.shipNum].speed / 2 / 10)
                            {
                                player.moveForward();
                            }
                            else
                            {
                                player.finishMovingShip();
                                isEnemyShipMarked = false;
                            }
                        }
                        else
                        {
                            if (Mathf.Abs(angle - Vector3.Angle(targetDir, player.playerFleet[player.shipNum].transform.up)) < player.playerFleet[player.shipNum].turns)
                            {
                                player.turnLeft();
                            }
                            else
                            {
                                if ((player.playerFleet[player.shipNum].transform.position - player.playerFleet[player.shipNum].previousPosition).magnitude < player.playerFleet[player.shipNum].speed / 2 / 10)
                                {
                                    player.moveForward();
                                }
                                else
                                {
                                    player.finishMovingShip();
                                    isEnemyShipMarked = false;
                                }
                            }
                        }
                    }
                    else if (distance >= 3)
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
                            if (angle > 0 && player.playerFleet[player.shipNum].totalRotation < player.playerFleet[player.shipNum].turns)
                            {
                                if (angle < 0)
                                {
                                    player.turnLeft();
                                }
                                else if (angle > 0)
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
        }
    }
}
