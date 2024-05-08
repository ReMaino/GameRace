using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CarCPManager : MonoBehaviour
{

    public Text PositionText;
    public Text LapsText;
    public Text PosText;

    public int CarNumber;
    public int cpCrossed = 0;
    public int CarPosition;

    public RaceManage raceManager;
    PhotonView pv;

    private void Awake()
    {
        raceManager = FindObjectOfType<RaceManage>();
        PositionText = raceManager.PositionText;
        LapsText = raceManager.LapsText;
        PosText = raceManager.PosText;
        raceManager.InitializationCar(gameObject);
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            PositionText.enabled = true;
            LapsText.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CP"))
        {
            cpCrossed += 1;
            raceManager.CarCollectedCP(CarNumber, cpCrossed);
        }
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            PositionText.text = CarPosition.ToString() + "/" + raceManager.GetTotalCars().ToString();
            LapsText.text = raceManager.GetLap(CarNumber).ToString() + "/" + raceManager.GetTotalLaps().ToString();

            if (raceManager.FinishTrace(CarNumber))
            {
                RCC.SetControl(gameObject.GetComponent<RCC_CarControllerV3>(), false);
                if (CarPosition == 1)
                {
                    PosText.text = "WIN! Вы заняли 1 место!";
                }
                else
                {
                    PosText.text = "LOOSE! Вы заняли " + CarPosition.ToString() + " место.";
                }
            }
        }
    }
}
