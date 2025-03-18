using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class CameraSmoothFollow : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private Camera m_Camera;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public float shakeValue = 0.25f;
    private float currentShakeValue = 0f;
    public float maxCameraSize = 12f;
    public float minCameraSize = 7f;
    public float zoomSpeed = 2f;

    [SerializeField] private Transform target;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
    }

    public void ShakeCamera()
    {
        StartCoroutine(StartShakeCamera());
    }

    private IEnumerator StartShakeCamera()
    {
        currentShakeValue = shakeValue;

        yield return new WaitForSeconds(0.25f);
        currentShakeValue = 0f;
    }

    private void FixedUpdate()
    {
        if (characterMovement == null)
        {
            characterMovement = CharacterMovement.Instance;
            return;
        }

        Vector3 targetPosition = target.position + offset;
        float targetCameraSize = minCameraSize;

        if (characterMovement.currentBodyType == CharacterBodyType.BodyType.Soul)
        {
            if (CharacterPuppet.Instance == null) return;
            Vector2 centerPoint = (CharacterPuppet.Instance.gameObject.transform.position + characterMovement.gameObject.transform.position) / 2f;
            targetPosition = (Vector3)centerPoint + offset;
            float t = Mathf.InverseLerp(0f, characterMovement.maxSeperateDistance, Vector3.Distance(CharacterPuppet.Instance.gameObject.transform.position, characterMovement.gameObject.transform.position));
            targetCameraSize = Mathf.Lerp(minCameraSize, maxCameraSize, t);
        }

        // Smoothly interpolate the camera size
        float currentCameraSize = Mathf.Lerp(m_Camera.orthographicSize, targetCameraSize, zoomSpeed * Time.deltaTime);

        // Smoothly move the camera
        float currentSmoothTime = characterMovement.isDashing ? smoothTime/2f : smoothTime;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, currentSmoothTime);
        transform.position += Random.insideUnitSphere * currentShakeValue;

        // Apply the new camera size
        m_Camera.orthographicSize = currentCameraSize;
    }
}