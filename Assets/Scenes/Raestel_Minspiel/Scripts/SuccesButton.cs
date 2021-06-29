using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccesButton : AbstractButton
{
    public GameObject Light;
    public GameObject Text;

    bool trigger = false;

    [SerializeField]
    private Animator controller;

    [Header("Key Management")]
    public GameObject Key;

    [SerializeField]
    public Vector3 KeySpawnPosition;

    public Mesh KeyMesh;

    public override void PushButton()
    {
        //Light.SetActive(true);
        //Text.SetActive(true);
        trigger = true;
        controller.SetBool("click", true);
        //RaetselMinigameController.Instance.ChallengeCompleted();
        Instantiate(Key, KeySpawnPosition, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Light.SetActive(false);
        //Text.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireMesh(KeyMesh, KeySpawnPosition);
    }
}
