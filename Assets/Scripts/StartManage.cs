using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StartManage : MonoBehaviour
{
    //public RCC_Camera cam;
    public Text countText;
    public GameObject[] Enemies;
    public Transform[] Spawners;
    public int maxCars = 8;

    public void GoRace(List<Racer> racers)
    {

        foreach (GameObject obj in Enemies)
        {
            obj.GetComponent<Rigidbody>().isKinematic = false;
        }

       // cam.cameraMode = RCC_Camera.CameraMode.CINEMATIC;
        StartCoroutine(StartRace(racers));
    }

    private void SpawnAI (int pos)
    {
        PhotonNetwork.Instantiate("ToyotaAI", Spawners[pos].position, Quaternion.identity, 0);
    }

    public void SpawnAIs(int countRacers)
    {
        for (int i = countRacers; i < maxCars; i++)
        {
            SpawnAI(i);
        }
    }

    private IEnumerator StartRace(List<Racer> racers)
    {
        countText.text = "3";
        yield return new WaitForSeconds(1);
        countText.text = "2";
        yield return new WaitForSeconds(1);
        countText.text = "1";
        yield return new WaitForSeconds(1);
        countText.text = "œŒ√Õ¿À»!";
        yield return new WaitForSeconds(0.7f);
        countText.gameObject.SetActive(false);
        //cam.cameraMode = RCC_Camera.CameraMode.TPS; 

        foreach (var racer in racers)
        {
            racer.GetComponent<RCC_CarControllerV3>().enabled = true;
        }
    }
}
