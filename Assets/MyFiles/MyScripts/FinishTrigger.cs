using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private RaceManager raceManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            raceManager.FinishRace();
        }
    }
}

