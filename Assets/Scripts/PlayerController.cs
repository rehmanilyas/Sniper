using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject sniperCameraObj;
    [SerializeField] Transform mainCameraTr;
    [SerializeField] BulletCameraScript bulletCam;
    [SerializeField] Transform scopeObjTr;
    [SerializeField] GameObject scopeViewImg;

    [SerializeField] GameObject bulletPrefab;

    bool isSniperMode = false;
    bool isChangingMode = false;
    bool isBulletCam = false;

    Vector3 mainCamOffset;
    Vector3 mainCamLocalPos;

    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamOffset = transform.position - mainCameraTr.position;
        mainCamLocalPos = mainCameraTr.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBulletCam)
            return;

        if (Input.GetMouseButtonUp(1) && !isChangingMode)
        {
            if (!isSniperMode)
            {
                ChangeToSniperView();
            }
            else
            {
                ChangeToMainView();
            }
        }

        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x + Input.GetAxis("Vertical"),
            transform.eulerAngles.y + Input.GetAxis("Horizontal"),
            0f
            );

        if (Input.GetMouseButtonUp(0))
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = sniperCameraObj.transform.position - new Vector3(0f, 0.05f, 0f);
        bullet.transform.rotation = sniperCameraObj.transform.rotation;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 5000f);

        if (isSniperMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(bullet.transform.position + bullet.transform.forward, bullet.transform.forward, out hit, 60f))
            {
                if (hit.collider.name.StartsWith("Cube"))
                {
                    isBulletCam = true;
                    bulletCam.FollowBullet(bullet.transform, hit.point);
                    scopeViewImg.SetActive(false);
                }
            }
        }
    }

    void ChangeToSniperView()
    {
        StartCoroutine(GotoSniperView());
    }

    void ChangeToMainView()
    {
        StartCoroutine(GoToMainView());
    }

    IEnumerator GotoSniperView()
    {
        if (isSniperMode || isChangingMode)
            yield break;

        isChangingMode = true;

        while(true)
        {
            mainCameraTr.position = Vector3.MoveTowards(
                mainCameraTr.position,
                scopeObjTr.position,
                Time.deltaTime * 3f
                );

            if (Vector3.Distance(mainCameraTr.position, scopeObjTr.position) < 0.1f)
            {
                break;
            }

            yield return null;
        }

        mainCameraTr.gameObject.SetActive(false);
        sniperCameraObj.SetActive(true);
        scopeViewImg.SetActive(true);

        isChangingMode = false;
        isSniperMode = true;
    }

    IEnumerator GoToMainView()
    {
        if (!isSniperMode || isChangingMode)
            yield break;

        isChangingMode = true;

        mainCameraTr.gameObject.SetActive(true);
        sniperCameraObj.SetActive(false);
        scopeViewImg.SetActive(false);

        //Vector3 targetPos = transform.position - mainCamOffset;
        Vector3 targetPos = mainCamLocalPos;

        while (true)
        {
            mainCameraTr.localPosition = Vector3.MoveTowards(
                mainCameraTr.localPosition,
                targetPos,
                Time.deltaTime * 3f
                );

            if (Vector3.Distance(mainCameraTr.localPosition, targetPos) < 0.1f)
            {
                mainCameraTr.localPosition = targetPos;
                break;
            }

            yield return null;
        }

        
        isChangingMode = false;
        isSniperMode = false;
    }

    public void BulletHitTarget()
    {
        isBulletCam = false;
        scopeViewImg.SetActive(isSniperMode);
    }
}
