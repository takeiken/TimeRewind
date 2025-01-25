using System.Collections;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public float shakeValue = 0.25f;
    private float currentShakeValue = 0f;

    [SerializeField] private Transform target;

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
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.position += Random.insideUnitSphere * currentShakeValue;
    }
}