using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Witness : MonoBehaviour
{
    public bool isKiller;

    [SerializeField] public string bodyType;
    [SerializeField] public string maskMaterial;
    [SerializeField] public string outfitColor;
    [SerializeField] private MysteryManager mysteryManager;
    [SerializeField] private Transform maskLocation;

    [SerializeField] List<GameObject> masks = new List<GameObject>();
    [SerializeField] List<string> maskType = new List<string>();

    [SerializeField] public TextBoxPopUp killerLie;

    private int? assignedMaskIndex;

    private void Awake()
    {
    }

    private void Start()
    {
        if (isKiller)
        {
            Debug.Log($"Not Killer Body Type is {mysteryManager.notKillerBodyType}");
            killerLie.possibleTexts[0] = killerLie.possibleTexts[0] + "<color=yellow>" + mysteryManager.notKillerBodyType + "</color>.";
        }

        if (!assignedMaskIndex.HasValue)
        {
            assignedMaskIndex = Mathf.Clamp(Random.Range(0, masks.Count), 0, Mathf.Max(0, masks.Count -1));
        }

        int randomMask = Mathf.Clamp(assignedMaskIndex.Value, 0, masks.Count - 1);
        if (string.IsNullOrEmpty(maskMaterial) && maskType != null && maskType.Count > randomMask)
        {
            maskMaterial = maskType[randomMask];
        }

        GameObject mask = masks[randomMask];
        Instantiate(mask, maskLocation.position, maskLocation.rotation, maskLocation);
    }

    public void SetMaskByIndex(int index)
    {
        int clamped = Mathf.Clamp(index, 0, masks.Count - 1);
        assignedMaskIndex = index;
        if (maskType != null && maskType.Count > clamped)
        {
            maskMaterial = maskType[clamped];
        }
    }
}
