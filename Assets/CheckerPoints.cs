using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerPoints : MonoBehaviour
{
    [SerializeField] private Dictionary<GameObject, int> Cars;
    [SerializeField] private List<GameObject> WayPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
        }
    }
}
