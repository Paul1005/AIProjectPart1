using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private ShipCard[] playerFleet;
    private ShipCard[] enemyFleet;
    GameObject enemyPlayer;
    public bool isTurn;
    private List<string> phase = new List<string>();
    int shipNum = 0;
    int phaseNum = 0;
    int enemyShipNum = 0;
    int weaponNum = 0;

    // Use this for initialization
    void Start()
    {
        if (gameObject.name == "Player1")
        {
            enemyPlayer = GameObject.Find("Player2");
        }
        else if (gameObject.name == "Player2")
        {
            enemyPlayer = GameObject.Find("Player1");
        }

        phase.Add("movement");
        phase.Add("shooting");
        //phase.Add("ordinance");
        shipNum = 0;
        phaseNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerFleet = GetComponentsInChildren<ShipCard>();
        enemyFleet = enemyPlayer.GetComponentsInChildren<ShipCard>();
        if (isTurn)
        {
            foreach (ShipCard ship in playerFleet)
            {
                ship.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            playerFleet[shipNum].gameObject.GetComponent<MeshRenderer>().material.color = Color.green;

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
                        playerFleet[shipNum].gameObject.transform.position += playerFleet[shipNum].gameObject.transform.up * 0.015f;
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
                else if (Input.GetKeyUp(KeyCode.KeypadEnter))
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
                        playerFleet[shipNum].previousPosition = playerFleet[shipNum].gameObject.transform.position;
                        playerFleet[shipNum].totalRotation = 0;
                    }
                }
            }
            else if (phase[phaseNum] == "shooting")
            {
                WeaponCard[] shipWeapons = playerFleet[shipNum].gameObject.GetComponentsInChildren<WeaponCard>();
                bool isInRange = false;
                foreach (ShipCard ship in enemyFleet)
                {
                    ship.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                }
                enemyFleet[enemyShipNum].gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                print(enemyFleet[enemyShipNum]);
                float distance = Vector3.Distance(playerFleet[shipNum].transform.position, enemyFleet[enemyShipNum].transform.position);
                if (distance <= shipWeapons[weaponNum].range / 10)
                {
                    Vector3 targetDir = playerFleet[shipNum].transform.position - enemyFleet[enemyShipNum].transform.position;
                    float angle = Vector3.Angle(targetDir, playerFleet[shipNum].transform.up);
                    //print(angle);
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
                            weaponNum = 0;
                            if (shipNum < playerFleet.Length - 1)
                            {
                                shipNum++;
                            }
                            else if (shipNum == playerFleet.Length - 1)
                            {
                                shipNum = 0;
                                phaseNum = 0;
                                isTurn = false;
                                enemyPlayer.GetComponent<Player>().isTurn = true;
                                foreach (ShipCard ship in playerFleet)
                                {
                                    ship.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                                }
                                //other players turn
                            }
                        }
                    }

                }
                else if (isInRange)
                {
                    print("Current Weapon is: " + shipWeapons[weaponNum]);
                    if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
                    {
                        float firepower = shipWeapons[weaponNum].firepower;
                        int damage = 0;
                        if (shipWeapons[weaponNum].type == "WeaponBattery")
                        {
                            if (distance <= 1.5)
                            {
                                if (enemyFleet[enemyShipNum].type == "Battleship")
                                {
                                    firepower *= 0.8f;
                                }
                                else if (enemyFleet[enemyShipNum].type == "Cruiser")
                                {
                                    firepower *= 0.75f;
                                }
                                else if (enemyFleet[enemyShipNum].type == "Escort")
                                {
                                    firepower *= 0.67f;
                                }
                            }
                            else if (distance > 1.5 && distance <= 3)
                            {
                                if (enemyFleet[enemyShipNum].type == "Battleship")
                                {
                                    firepower *= 0.75f;
                                }
                                else if (enemyFleet[enemyShipNum].type == "Cruiser")
                                {
                                    firepower *= 0.67f;
                                }
                                else if (enemyFleet[enemyShipNum].type == "Escort")
                                {
                                    firepower *= 0.5f;
                                }
                            }
                            else if (distance > 3)
                            {
                                if (enemyFleet[enemyShipNum].type == "Battleship")
                                {
                                    firepower *= 0.67f;
                                }
                                else if (enemyFleet[enemyShipNum].type == "Cruiser")
                                {
                                    firepower *= 0.5f;
                                }
                                else if (enemyFleet[enemyShipNum].type == "Escort")
                                {
                                    firepower *= 0.33f;
                                }
                            }
                            firepower = Mathf.CeilToInt(firepower);
                            for (int i = 0; i < firepower; i++)
                            {
                                if (Random.Range(1, 6) >= enemyFleet[enemyShipNum].armour)
                                {
                                    damage++;
                                }
                            }
                        }
                        else if (shipWeapons[weaponNum].type == "LanceBattery")
                        {
                            for (int i = 0; i < firepower; i++)
                            {
                                if (Random.Range(1, 6) >= 4)
                                {
                                    damage++;
                                }
                            }
                        }


                        enemyFleet[enemyShipNum].shields -= damage;
                        if (enemyFleet[enemyShipNum].shields <= 0)
                        {
                            enemyFleet[enemyShipNum].hits -= damage; // expand upon
                        }

                        print("firing: " + shipWeapons[weaponNum] + " at " + enemyFleet[enemyShipNum]);
                        print("shields at:" + enemyFleet[enemyShipNum].shields);
                        print("health at " + enemyFleet[enemyShipNum].hits);
                        if (weaponNum < shipWeapons.Length - 1)
                        {
                            weaponNum++;
                        }
                        else if (weaponNum == shipWeapons.Length - 1)
                        {
                            weaponNum = 0;

                            if (shipNum <= playerFleet.Length - 1)
                            {
                                shipNum++;
                            }
                            else if (shipNum == playerFleet.Length - 1)
                            {
                                shipNum = 0;
                                phaseNum = 0;
                                isTurn = false;
                                enemyPlayer.GetComponent<Player>().isTurn = true;
                                foreach (ShipCard ship in playerFleet)
                                {
                                    ship.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                                }
                                //other players turn
                            }
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.LeftArrow))
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
                    else if (Input.GetKeyUp(KeyCode.RightArrow))
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
}
