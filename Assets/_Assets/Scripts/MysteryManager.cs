using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MysteryManager : MonoBehaviour
{
    [SerializeField] List<GameObject> witnesses = new List<GameObject>();
    [SerializeField] List<GameObject> innocents = new List<GameObject>();
    [SerializeField] public string killerBodyType;
    [SerializeField] public string notKillerBodyType;
    [SerializeField] public string killerMaskMaterial;
    [SerializeField] public string killerOutfitColor;

    void Awake()
    {
        int randomKiller = Random.Range(0, witnesses.Count);
        int randomWitness = 0;
        Debug.Log($"The Killer is: {witnesses[randomKiller]}");
        PopulateInnocentsList(witnesses.Count, randomKiller);
        AssignMasksEvenly(witnesses, maskTypeCount : 3);
        KillerEvidence(randomKiller);
        DeterminePatsy(randomWitness, randomKiller);
        notKillerBodyType = innocents[randomWitness].GetComponent<Witness>().bodyType;
        Debug.Log($"The Killer's patsy is: {innocents[randomWitness]}");
    }

    private void PopulateInnocentsList(int witnessCount, int theKiller)
    {
        for (int i = 0; i < witnessCount; i++)
        {
            innocents.Add(witnesses[i]);
        }
        innocents.Remove(witnesses[theKiller]);
        if (theKiller != 0)
        {
            innocents.Remove(witnesses[theKiller - 1]);
        }
        if (theKiller != 5)
        {
            innocents.Remove(witnesses[theKiller + 1]);
        }
    }

    private void KillerEvidence(int randomKiller)
    {
        Witness killer = witnesses[randomKiller].GetComponent<Witness>();
        killerBodyType = killer.bodyType;
        killerMaskMaterial = killer.maskMaterial;
        killerOutfitColor = killer.outfitColor;
        killer.isKiller = true;
    }

    private int DeterminePatsy(int randomWitness, int randomKiller)
    {
        while (randomWitness == randomKiller)
        {
            randomWitness = Random.Range(0, innocents.Count);
        }

        return randomWitness;
    }

    private void AssignMasksEvenly(List<GameObject> targets, int maskTypeCount)
    {
        List<int> maskIndices = new List<int>();
        int total = targets.Count;

        if (total == 6 && maskTypeCount == 3)
        {
            maskIndices.AddRange(new[] { 0, 0, 1, 1, 2, 2 });
        }
        else
        {
            int perType = total / maskTypeCount;
            int remainder = total % maskTypeCount;

            for (int i = 0; i < maskTypeCount; i++)
            {
                int j = perType + (i < remainder ? 1 : 0);
                for (int k = 0; k < j; k++)
                {
                    maskIndices.Add(i);
                }
            }

            while (maskIndices.Count < total)
            {
                maskIndices.Add(Random.Range(0, maskTypeCount));
            }
            while (maskIndices.Count > total)
            {
                maskIndices.RemoveAt(maskIndices.Count - 1);
            }
        }

        for (int i = 0; i < maskIndices.Count; i++)
        {
            int j = Random.Range(i, maskIndices.Count);
            (maskIndices[i], maskIndices[j]) = (maskIndices[j], maskIndices[i]);
        }

        for (int i = 0; i < targets.Count; i++)
        {
            GameObject go = targets[i];
            Witness witness = go.GetComponent<Witness>();
            witness.SetMaskByIndex(maskIndices[i]);
        }
    }
}
