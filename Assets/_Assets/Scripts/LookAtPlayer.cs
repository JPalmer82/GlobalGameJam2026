using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Update is called once per frame
    void Update()
    {
            transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);
    }
}
