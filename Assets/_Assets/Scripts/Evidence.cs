using UnityEngine;

public class Evidence : MonoBehaviour
{
    [SerializeField] public bool isWitnessEvidence;
    [SerializeField] public bool isWeaponEvidence;
    [SerializeField] public bool isMaterialEvidence;

    [SerializeField] public GameObject fur;
    [SerializeField] public GameObject scales;
    [SerializeField] public GameObject feather;

    [SerializeField] public MysteryManager mysteryManager;

    private string materialText;
    private string weaponText;

    private void Start()
    {
        if (fur != null && scales != null && feather != null)
        {
            fur.SetActive(false);
            scales.SetActive(false);
            feather.SetActive(false);
            if (mysteryManager.killerMaskMaterial == "Feathers")
            {
                feather.SetActive(true);
            }
            if (mysteryManager.killerMaskMaterial == "Scales")
            {
                scales.SetActive(true);
            }
            if (mysteryManager.killerMaskMaterial == "Fur")
            {
                fur.SetActive(true);
            }
        }

    }
}
