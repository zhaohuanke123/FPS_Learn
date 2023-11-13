using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vanks.FPS
{
    public class WeaponController : MonoBehaviour
    {
        public Transform bulletStartPoint;
        public GameObject bulletPrefab;
        public float fireInterval = 0.5f;
        public bool isFire;

        public Transform defaultPoint;
        public Transform backPoint;
        public float lerpRatio = 0.2f;

        private AudioSource shotAudio;

        // ViewControl
        public Camera mainCamera;
        public Camera weaponCamera;
        public Vector3 weaponCameraDefaultPoint;
        public Vector3 weaponCameraCenterPoint;
        public float defaultView = 60;
        public float aimView = 30;
        public float viewLerpRatio = 0.2f;

        private void Start()
        {
            shotAudio = GetComponent<AudioSource>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            weaponCamera = GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<Camera>();
        }

        private void Update()
        {
            OpenFire();
            ViewChange();

            Ray ray = new Ray(bulletStartPoint.position, bulletStartPoint.forward);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        }

        public void OpenFire()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isFire = true;
                StartCoroutine("Fire");
            }

            if (Input.GetMouseButton(0))
            {
            }

            if (Input.GetMouseButtonUp(0))
            {
                isFire = false;
                StopCoroutine("Fire");
            }
        }

        IEnumerator Fire()
        {
            while (isFire)
            {
                if (bulletStartPoint && bulletPrefab)
                {
                    GameObject newBullet = BulletPool.Instance.Get();
                    // Instantiate(bulletPrefab, bulletStartPoint.position, bulletStartPoint.rotation);
                    newBullet.transform.position = bulletStartPoint.position;
                    newBullet.transform.rotation = bulletStartPoint.rotation;
                    BulletController bulletController = newBullet.GetComponent<BulletController>();
                    bulletController._rigidbody.velocity = bulletStartPoint.forward * bulletController.startSpeed;
                    bulletController.bulletType = BulletType.Player;
                    PlayShotAudio();

                    StopCoroutine("WeaponBackAnimation");
                    StartCoroutine("WeaponBackAnimation");
                }


                yield return new WaitForSeconds(fireInterval);
            }
        }

        IEnumerator WeaponBackAnimation()
        {
            if (defaultPoint && backPoint)
            {
                while (transform.localPosition != backPoint.localPosition)
                {
                    transform.localPosition =
                        Vector3.Lerp(transform.localPosition, backPoint.localPosition, lerpRatio * 4);
                    yield return null;
                }

                while (transform.localPosition != defaultPoint.localPosition)
                {
                    transform.localPosition =
                        Vector3.Lerp(transform.localPosition, defaultPoint.localPosition, lerpRatio);
                    yield return null;
                }
            }
        }

        private void PlayShotAudio()
        {
            if (shotAudio)
            {
                shotAudio.Play();
            }
        }

        public void ViewChange()
        {
            if (Input.GetMouseButtonDown(1))
            {
                StopCoroutine("ViewToDefault");
                StartCoroutine("ViewToCenter");
            }

            if (Input.GetMouseButtonUp(1))
            {
                StopCoroutine("ViewToCenter");
                StartCoroutine("ViewToDefault");
            }
        }

        IEnumerator ViewToCenter()
        {
            while (weaponCamera.transform.localPosition != weaponCameraCenterPoint)
            {
                weaponCamera.transform.localPosition =
                    Vector3.Lerp(weaponCamera.transform.localPosition, weaponCameraCenterPoint, viewLerpRatio);
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, aimView, viewLerpRatio);
                weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView, aimView, viewLerpRatio);

                yield return null;
            }

            yield return null; // 等待一帧
        }

        IEnumerator ViewToDefault()
        {
            while (weaponCamera.transform.localPosition != weaponCameraDefaultPoint)
            {
                weaponCamera.transform.localPosition =
                    Vector3.Lerp(weaponCamera.transform.localPosition, weaponCameraDefaultPoint, viewLerpRatio);
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultView, viewLerpRatio);
                weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView, defaultView, viewLerpRatio);

                yield return null;
            }

            yield return null; // 等待一帧
        }

        private void OnDrawGizmos()
        {
            if (bulletStartPoint)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(bulletStartPoint.position, bulletStartPoint.forward * 100);
            }
        }
    }
}