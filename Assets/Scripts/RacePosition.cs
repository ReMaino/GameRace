using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacePosition : MonoBehaviour
{
    public Text positionText;
    public Transform playerTransform;
    public Transform[] checkpoints; // ������ ���� ���������� �� ������
    public int totalPlayers = 4; // ����� ���������� �������

    void Update()
    {
        int playerPosition = CalculatePlayerPosition();
        positionText.text = playerPosition + "/" + totalPlayers;
    }

    private int CalculatePlayerPosition()
    {
        float playerDistanceToFinish = GetDistanceToFinish(); // �������� ���������� ������ �� ������

        int playerPosition = 1; // �� ��������� ������� ������ 1

        for (int i = 0; i < totalPlayers; i++)
        {
            float distanceToFinish = Vector3.Distance(playerTransform.position, checkpoints[checkpoints.Length - 1].position); // ���������� ������� ������ �� ������

            if (distanceToFinish < playerDistanceToFinish) // ���� ������ ����� ����� � ������
            {
                playerPosition++;
            }
        }

        return playerPosition;
    }

    private float GetDistanceToFinish()
    {
        float distance = Vector3.Distance(playerTransform.position, checkpoints[checkpoints.Length - 1].position); // ���������� �� ������ �� ���������� ���������
        return distance;
    }
}