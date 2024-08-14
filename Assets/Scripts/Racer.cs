using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Racer : MonoBehaviour
{
    public Text positionText;
    public Text lapsText;
    public Text finalText;
    public GameObject readyForm;
    public GameObject notReadyForm;

    public Button readyButton;

    PhotonView pv;
    public GameObject CP;
    public int lap;

    public RaceManager raceManager;
    public CarCPManager cpManager;

    public bool isReady;

    public bool isAi = false;

    private void Awake()
    {
        raceManager = FindObjectOfType<RaceManager>();
        CP = raceManager.SetCp();
        lap = 0;

        cpManager = gameObject.GetComponent<CarCPManager>();

        isReady = isAi = gameObject.tag == "Enemy";

        positionText = raceManager.positionText;
        lapsText = raceManager.lapsText;
        finalText = raceManager.finalText;
        readyForm = raceManager.ready;
        notReadyForm = raceManager.notReady;
        readyButton = raceManager.readyButton;

        this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;

        if (!isAi)
        {
            pv = GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                pv.RPC("RPC_AddPlayer", RpcTarget.AllBuffered);
                readyButton.onClick.AddListener(this.SetReady);
            }
        }
        else
        {
            raceManager.SetRacerPosition(this);
        }
    }

    [PunRPC]
    public void RPC_AddPlayer()
    {
        RaceManager.raceManager.SetRacerPosition(this);
    }

    [PunRPC]
    public void RPC_UpdateReady()
    {
        isReady = !isReady;
        RaceManager.raceManager.SetReady(isReady);
    }

    public void SetReady()
    {
        if (!isAi && pv.IsMine)
        {
            if (readyForm.activeSelf == true)
            {
                notReadyForm.SetActive(true);
                readyForm.SetActive(false);
            }
            else
            {
                notReadyForm.SetActive(false);
                readyForm.SetActive(true);
            }

            // Debug.Log(isReady);


            pv.RPC("RPC_UpdateReady", RpcTarget.AllBuffered);
        }
    }

    public void StartRacer()
    {
        StartCoroutine(StartRace());
    }

    private IEnumerator StartRace()
    {
        if (!isAi)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = true;
            finalText.text = "3";
            yield return new WaitForSeconds(1);
            finalText.text = "2";
            yield return new WaitForSeconds(1);
            finalText.text = "1";
            yield return new WaitForSeconds(1);
            finalText.text = "ПОГНАЛИ!";
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(2);
            finalText.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(3);
            this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = true;
        }
        
        //cam.cameraMode = RCC_Camera.CameraMode.TPS; 
    }

    private void Update()
    {
        if (!isAi && pv.IsMine)
        {
            positionText.text = cpManager.carPosition.ToString() + "/" + raceManager.GetTotalCars().ToString() + "/" + raceManager.readyPlayers.ToString() + "/" + isReady;
            lapsText.text = raceManager.GetLap(cpManager.carNumber).ToString() + "/" + raceManager.GetTotalLaps().ToString();

            if (raceManager.FinishTrace(cpManager.carNumber))
            {
                finalText.gameObject.SetActive(true);
                this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
                if (cpManager.carPosition == 1)
                {
                    finalText.text = "WIN! Вы заняли 1 место!";
                }
                else
                {
                    finalText.text = "LOOSE! Вы заняли " + cpManager.carPosition.ToString() + " место.";
                }
                this.enabled = false;
            }
        }
    }

}
