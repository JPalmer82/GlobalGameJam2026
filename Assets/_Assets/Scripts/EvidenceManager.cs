using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    [SerializeField] public List<Evidence> evidence = new List<Evidence>();
    [SerializeField] private FistPersonController player;

    // Update is called once per frame
    void Update()
    {
        if (evidence.Count == 3)
        {
            ConcludeInvestigation();
        }
    }

    private void ConcludeInvestigation()
    {
        player.canAccuse = true;
    }
}
