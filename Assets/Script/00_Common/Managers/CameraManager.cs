using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : GameObjectSingleton<CameraManager>
{
    public void Shake(float amount = 0.5f, float duration = 0.25f)
    {
        if (this.cameraTransform == null) return;
        Run.Coroutine(this.ShakeCoroutine(amount * 1.5f, duration));
    }

    public void SetCameraObj(Transform cameraTransform)
    {
        this.cameraTransform = cameraTransform;
    }

    private IEnumerator ShakeCoroutine(float amount, float duration)
    {
        this.originPos = this.cameraTransform.localPosition;
        float timer = 0;
        while (timer <= duration)
        {
            this.cameraTransform.localPosition = (Vector3)Random.insideUnitCircle * amount + this.originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        this.cameraTransform.localPosition = this.originPos;
    }

    private Transform cameraTransform;
    private Vector3 originPos;
}
