using UnityEngine;

public class EnemyTurns : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.forward = mainCamera.transform.forward;
    }
}