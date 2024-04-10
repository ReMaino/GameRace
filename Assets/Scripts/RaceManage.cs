using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManage : MonoBehaviour
{
    public GameObject RaceUI;

    public GameObject CP;
    public GameObject CheckpointHolder;

    public Transform[] CheckpointsPosition;
    public GameObject[] Cars;
    public GameObject[] CheckpointsForeachCar;

    private int totalcars;
    private int totalcheckpoints;

    public int[] lapsForeachCars;
    public int totalLaps;

    public Text PositionText;
    public Text PosText;
    public Text LapsText;


    private void Awake()
    {
        RCC_CarControllerV3[] carControllerser = FindObjectsOfType<RCC_CarControllerV3>();
        Cars = new GameObject[carControllerser.Length];

        for (int i = 0; i < carControllerser.Length; i++)
        {
            Cars[i] = carControllerser[i].gameObject;
        }
    }

    void Start()
    {
        totalcars = Cars.Length;
        totalcheckpoints = CheckpointHolder.transform.childCount;

        RaceUI.SetActive(true);

        SetCheckpoints();
        SetCarPosition();
        SetLaps();
    }

    void SetCheckpoints()
    {
        CheckpointsPosition = new Transform[totalcheckpoints];

        for (int i = 0; i < totalcheckpoints; i++)
        {
            CheckpointsPosition[i] = CheckpointHolder.transform.GetChild(i).transform;
        }

        CheckpointsForeachCar = new GameObject[totalcars];

        for (int i = 0; i < totalcars; i++)
        {
            CheckpointsForeachCar[i] = Instantiate(CP, CheckpointsPosition[0].position, CheckpointsPosition[0].rotation);
            CheckpointsForeachCar[i].name = "CP" + i;
            CheckpointsForeachCar[i].layer = 15 + i;
        }
    }

    void SetLaps()
    {
        lapsForeachCars = new int[totalcars];
    }

    void SetCarPosition()
    {
        for (int i = 0; i < totalcars; i++)
        {
            Cars[i].GetComponent<CarCPManager>().CarPosition = i + 1;
            Cars[i].GetComponent<CarCPManager>().CarNumber = i;
        }
    }

    public void CarCollectedCP(int carNumber, int cpNumber)
    {
        cpNumber = cpNumber % CheckpointsPosition.Length;

        CheckpointsForeachCar[carNumber].transform.position = CheckpointsPosition[cpNumber].transform.position;
        CheckpointsForeachCar[carNumber].transform.rotation = CheckpointsPosition[cpNumber].transform.rotation;

        comparePositions(carNumber);

        if (cpNumber + 1 == totalcheckpoints)
        {
            lapsForeachCars[carNumber]++;

            if (lapsForeachCars[carNumber] == totalLaps && carNumber == 1)
            {
                if (Cars[1].GetComponent<CarCPManager>().CarPosition == 1)
                {
                    PosText.text = "WIN! Вы заняли 1 место!";
                }
                else
                {
                    PosText.text = "LOOSE! Вы заняли " + Cars[0].GetComponent<CarCPManager>().CarPosition.ToString() + " место.";
                }
            }
        }
    }

    void comparePositions(int carNumber)
    {
        if (Cars[carNumber].GetComponent<CarCPManager>().CarPosition > 1)
        {
            GameObject currentCar = Cars[carNumber];
            int currentCarPosition = currentCar.GetComponent<CarCPManager>().CarPosition;
            int currentCarCp = currentCar.GetComponent<CarCPManager>().cpCrossed;

            GameObject carInFront = null;
            int carInFrontPos = 0;
            int carInFrontCp = 0;

            for (int i = 0; i < totalcars; i++)
            {
                if (Cars[i].GetComponent<CarCPManager>().CarPosition == currentCarPosition - 1)
                {
                    carInFront = Cars[i];
                    carInFrontCp = carInFront.GetComponent<CarCPManager>().cpCrossed;
                    carInFrontPos = carInFront.GetComponent<CarCPManager>().CarPosition;
                    break;
                }
            }

            // this car has crossed ладнео не англичанин я
            if (currentCarCp > carInFrontCp)
            {
                currentCar.GetComponent<CarCPManager>().CarPosition = currentCarPosition - 1;
                carInFront.GetComponent<CarCPManager>().CarPosition = carInFrontPos + 1;

                Debug.Log("Car " + carNumber + " has over taken " + carInFront.GetComponent<CarCPManager>().CarNumber);
            }
        }
    }


    private void Update()
    {
        PositionText.text = Cars[1].GetComponent<CarCPManager>().CarPosition.ToString() + "/" + totalcars.ToString();
        LapsText.text = lapsForeachCars[1].ToString() + "/" + totalLaps.ToString();
    }
}
