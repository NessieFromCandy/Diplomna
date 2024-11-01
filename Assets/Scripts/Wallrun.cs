using UnityEngine;

public class WallRun : MonoBehaviour
{
    public float wallRunSpeed = 10f;
    public float wallRunDuration = 2f;
    public LayerMask wallLayer;
    public float wallDistance = 1f;

    private CharacterController controller;
    private bool isWallRunning = false;
    private float wallRunTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (IsAgainstWall() && Input.GetKeyDown(KeyCode.Space))
        {
            StartWallRun();
        }

        if (isWallRunning)
        {
            UpdateWallRun();
        }
    }

    private bool IsAgainstWall()
    {
        return Physics.Raycast(transform.position, transform.right, wallDistance, wallLayer) ||
               Physics.Raycast(transform.position, -transform.right, wallDistance, wallLayer);
    }

    private void StartWallRun()
    {
        isWallRunning = true;
        wallRunTimer = wallRunDuration;
    }

    private void UpdateWallRun()
    {
        wallRunTimer -= Time.deltaTime;

        if (wallRunTimer <= 0)
        {
            EndWallRun();
            return;
        }

        Vector3 wallRunDirection = transform.right * wallRunSpeed;
        controller.Move(wallRunDirection * Time.deltaTime);
    }

    private void EndWallRun()
    {
        isWallRunning = false;
    }
}