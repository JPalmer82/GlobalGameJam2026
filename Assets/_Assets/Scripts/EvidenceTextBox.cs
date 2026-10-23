using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class EvidenceTextBox : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public EvidenceManager evidenceManager;
    [SerializeField] public Evidence parentEvidence;
    [SerializeField] public MysteryManager mysteryManager;
    [SerializeField] public Animator playerAnimator;

    private string bodyText = "The weapon found on the body had some colors of ";
    private string materialText = "There appears to be a piece of someone's mask with a material like ";

    private void Start()
    {
        textBox.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerAnimator.SetBool("IsInspecting", true);
            SetKillerInformation();
            textBox.SetActive(true);
            if (!evidenceManager.evidence.Contains(parentEvidence))
            {
                evidenceManager.evidence.Add(parentEvidence);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            textBox.SetActive(false);
            playerAnimator.SetBool("IsInspecting", false);
        }
    }
    private void SetKillerInformation()
    {
        if (parentEvidence.isMaterialEvidence)
        {
            text.text = materialText + "<color=green>" + mysteryManager.killerMaskMaterial + "</color>.";
        }
        if (parentEvidence.isWeaponEvidence)
        {
            text.text = bodyText + $"<color={mysteryManager.killerOutfitColor.ToString()}>" + mysteryManager.killerOutfitColor + "</color>.";
        }
    }
}
