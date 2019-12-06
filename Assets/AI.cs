using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AI : MonoBehaviour
{

    string path = @"Assets\MachineLearningTable.txt";
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
                    if (distance < 3)
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
                            }
                            else
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
                if (player.hasCheckedEnemyFleet)
                {
                    if (player.enemyShipsInRange.Count > 0)
                    {
                        player.enemyShipNum = player.enemyShipsInRange[player.enemyShipInRangeNum];
                        string[] learningData = File.ReadAllLines(path);
                        List<Dictionary<string, string>> instances = inputData(learningData);

                        WeaponCard[] shipWeapons = player.playerFleet[player.shipNum].gameObject.GetComponentsInChildren<WeaponCard>();
                        string weaponType = shipWeapons[player.shipNum].type;

                        float distance = (player.playerFleet[player.shipNum].transform.position - player.enemyFleet[player.enemyShipNum].transform.position).magnitude / 10;
                        string shipType = player.enemyFleet[player.enemyShipNum].type;
                        int initialHits = player.enemyFleet[player.enemyShipNum].hits;
                        int initialShields = player.enemyFleet[player.enemyShipNum].shields;
                        int firepower = shipWeapons[player.shipNum].firepower;
                        int armour = player.enemyFleet[player.enemyShipNum].armour;

                        bool fireWeapon;

                        if (weaponType == "WeaponBattery")
                        {
                            Dictionary<string, string> newInstance = new Dictionary<string, string>();

                            string distanceString = "";

                            if (distance <= 1.5)
                            {
                                distanceString = "near";
                            }
                            else if (distance > 1.5 && distance <= 3)
                            {
                                distanceString = "middle";
                            }
                            else if (distance > 3)
                            {
                                distanceString = "far";
                            }

                            string armourString = "";
                            if (armour == 6)
                            {
                                armourString = "heavy";
                            }
                            else if (armour == 5)
                            {
                                armourString = "medium";
                            }
                            else if (armour == 4)
                            {
                                armourString = "light";
                            }

                            newInstance.Add("distance", distanceString);
                            newInstance.Add("shipType", shipType);
                            newInstance.Add("armour", armourString);

                            fireWeapon = naiveBayesClassifier(newInstance, instances);

                            if (fireWeapon || player.enemyShipInRangeNum == player.enemyShipsInRange.Count - 1)
                            {
                                player.fireWeapon(shipWeapons, distance);
                                int finalHits = player.enemyFleet[player.enemyShipNum].hits;
                                int finalShields = player.enemyFleet[player.enemyShipNum].shields;

                                int totalDamage = (initialShields - finalShields) + (initialHits - finalHits);

                                float damagePercentage = totalDamage / firepower;

                                float threshold = 0.25f;

                                if (damagePercentage >= threshold)
                                {
                                    newInstance.Add("goodShot", "yes");
                                }
                                else
                                {
                                    newInstance.Add("goodShot", "no");
                                }
                                outputData(newInstance);
                            }
                            else if (!fireWeapon && player.enemyShipInRangeNum < player.enemyShipsInRange.Count - 1)
                            {
                                player.switchToNextShip();
                            }
                        }
                        else if (weaponType == "LanceBattery")
                        {
                            player.fireWeapon(shipWeapons, distance);
                        }
                    }
                }
            }
        }
    }

    private List<Dictionary<string, string>> inputData(string[] learningData)
    {
        List<Dictionary<string, string>> instances = new List<Dictionary<string, string>>();
        foreach (string line in learningData)
        {
            string[] splitLine = line.Split(',');
            Dictionary<string, string> instance = new Dictionary<string, string>();
            instance.Add("shipType", splitLine[0]);
            instance.Add("distance", splitLine[1]);
            instance.Add("armour", splitLine[2]);
            instance.Add("goodShot", splitLine[3]);
            instances.Add(instance);
        }
        return instances;
    }

    private void outputData(Dictionary<string, string> newInstance)
    {
        string shipType;
        newInstance.TryGetValue("shipType", out shipType);
        string distance;
        newInstance.TryGetValue("distance", out distance);
        string armour;
        newInstance.TryGetValue("armour", out armour);
        string goodShot;
        newInstance.TryGetValue("goodShot", out goodShot);

        string output = shipType + "," + distance + "," + armour + "," + goodShot + "\n";
        File.AppendAllText(path, output);
    }

    bool naiveBayesClassifier(Dictionary<string, string> newInstance, List<Dictionary<string, string>> instances)
    {
        string distance;
        newInstance.TryGetValue("distance", out distance);
        string shipType;
        newInstance.TryGetValue("shipType", out shipType);
        string armour;
        newInstance.TryGetValue("armour", out armour);

        int totalSize = instances.Count;
        int numYes = 0;
        int numNo = 0;
        int numDistYes = 0;
        int numTypeYes = 0;
        int numArmourYes = 0;
        int numDistNo = 0;
        int numTypeNo = 0;
        int numArmourNo = 0;


        if (totalSize == 0)
        {
            return true;
        }
        else
        {
            foreach (Dictionary<string, string> instance in instances)
            {
                string goodShot;
                instance.TryGetValue("goodShot", out goodShot);
                if (goodShot == "yes")
                {
                    numYes++;
                    string whatDistance;
                    instance.TryGetValue("distance", out whatDistance);
                    if (distance == whatDistance)
                    {
                        numDistYes++;
                    }

                    string whatType;
                    instance.TryGetValue("shipType", out whatType);
                    if (shipType == whatType)
                    {
                        numTypeYes++;
                    }

                    string whatArmour;
                    instance.TryGetValue("armour", out whatArmour);
                    if (armour == whatArmour)
                    {
                        numArmourYes++;
                    }
                }
                else if (goodShot == "no")
                {
                    numNo++;
                    string whatDistance;
                    instance.TryGetValue("distance", out whatDistance);
                    if (distance == whatDistance)
                    {
                        numDistNo++;
                    }

                    string whatType;
                    instance.TryGetValue("shipType", out whatType);
                    if (shipType == whatType)
                    {
                        numTypeNo++;
                    }

                    string whatArmour;
                    instance.TryGetValue("armour", out whatArmour);
                    if (armour == whatArmour)
                    {
                        numArmourNo++;
                    }
                }
            }
            float probYes = numYes / totalSize;
            float probNo = numNo / totalSize;
            float probDistYes = numDistYes / totalSize;
            float probDistNo = numDistNo / totalSize;
            float probTypeYes = numTypeYes / totalSize;
            float probTypeNo = numTypeNo / totalSize;
            float probArmourYes = numArmourYes / totalSize;
            float probArmourNo = numArmourNo / totalSize;

            float newInstanceIsGood = probYes * probDistYes * probTypeYes * probArmourYes;
            float newInstanceIsBad = probNo * probDistNo * probTypeNo * probArmourNo;

            return newInstanceIsGood > newInstanceIsBad;
        }
    }
}
