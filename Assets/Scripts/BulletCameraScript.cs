using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FollowBullet(Transform bulletTr, Vector3 targetPos)
    {
        transform.position = bulletTr.position - (bulletTr.forward * 2f);
        transform.LookAt(targetPos);

        gameObject.SetActive(true);

        StartCoroutine(FollowRoutine(bulletTr));
    }

    IEnumerator FollowRoutine(Transform bulletTr)
    {
        float waitingTime = 1f;

        float targetTimeScale = 0.1f;
        
        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        Vector3 target = bulletTr.position + (bulletTr.forward * 2f) + new Vector3(-2f, 3f, 0f);
        while (true)
        {
            if (bulletTr != null)
            {
                //transform.LookAt(bulletTr.position);
                target = bulletTr.position - (bulletTr.forward * 3f) + new Vector3(-1f, 1f, 0f);
                transform.position = target;
                //transform.position = Vector3.MoveTowards(transform.position, target, Time.unscaledDeltaTime * 6f);
            }else
            {
                waitingTime -= Time.unscaledDeltaTime;
            }

            if (waitingTime <= 0)
            {
                break;
            }

            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        gameObject.SetActive(false);
        PlayerController.Instance.BulletHitTarget();
    }
}
