using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextBoxPopUp : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public List<string> possibleTexts = new List<string>();
    [SerializeField] public EvidenceManager evidenceManager;
    [SerializeField] public Evidence witnessEvidence;
    [SerializeField] public MysteryManager mysteryManager;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Animator ownerAnimator;

    private Witness parentWitness;

    private void Start()
    {
        textBox.SetActive(false);
        parentWitness = gameObject.GetComponentInParent<Witness>();
    }

    private void RandomizeText()
    {
        int randomDialogue = Random.Range(0, possibleTexts.Count);
        text.text = possibleTexts[randomDialogue];

        if (text.text == possibleTexts[0] && !parentWitness.isKiller)
        {
            text.text = text.text + "<color=yellow>" + mysteryManager.killerBodyType + "</color>.";
            if (!evidenceManager.evidence.Contains(witnessEvidence))
            {
                evidenceManager.evidence.Add(witnessEvidence);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerAnimator.SetBool("IsInteracting", true);
            ownerAnimator.SetBool("IsTalking", true);
            textBox.SetActive(true);
            RandomizeText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            textBox.SetActive(false);
            playerAnimator.SetBool("IsInteracting", false);
            ownerAnimator.SetBool("IsTalking", false);
        }
    }
}
