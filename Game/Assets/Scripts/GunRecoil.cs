using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{

    #region Variables
    // rotations
    private Vector3 currRotate;
    private Vector3 targRotate;

    // recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    // settings
    [SerializeField] private float snapBack;
    [SerializeField] private float retrnSpd;
    #endregion

    void Update()
    {

        targRotate = Vector3.Lerp(targRotate, Vector3.zero, retrnSpd * Time.deltaTime);
        currRotate = Vector3.Slerp(currRotate, targRotate, snapBack * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currRotate);
    }

    public void RecoilFire()
    {

        targRotate += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
