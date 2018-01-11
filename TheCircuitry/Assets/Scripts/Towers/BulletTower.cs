using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletTower : Tower
{

    private GameObject muzzleFlash1;
    private GameObject muzzleFlash2;
    private GameObject turretToRotate;

    // Use this for initialization
    void Start()
    {
        init();
        foreach (Component go in gameObject.GetComponentsInChildren<Component>(true))
        {
            if(go.gameObject.name.Contains("muzzleFlash"))
            {
                if (muzzleFlash1 == null) muzzleFlash1 = go.gameObject;
                else muzzleFlash2 = go.gameObject;
            }

            if(go.gameObject.name == "turretHead")
            {
                this.turretToRotate = go.gameObject;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG FEATURE TO FIX ALL TOWERS IN A LEVEL
        //if (Input.GetMouseButtonDown(1) && base.towerTier == 3)
        //{
        //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 dir = (turretToRotate.transform.position - pos);
        //    this.turretToRotate.transform.right = dir;
        //    this.turretToRotate.transform.eulerAngles = new Vector3(
        //                                this.turretToRotate.transform.eulerAngles.x,
        //                                this.turretToRotate.transform.eulerAngles.y,
        //                                this.turretToRotate.transform.eulerAngles.z + 90
        //                                );

        //    this.dir = dir.normalized;
        //    this.isBroken = !this.isBroken;
        //}

        if (isBroken)
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Play();
        }

        else
        {
            //If it didnt start the countdown yet, start it.
            if (!startCountdown)
            {
                StartCoroutine(TowerBreak());
            }

            brokenTowerParticles.GetComponent<ParticleSystem>().Stop();

            //DEBUG FEATURE FOR SHOOTING
            //if (Input.GetKeyDown(KeyCode.Space) && base.towerTier == 3)
            //{
            //    Vector2 pos1 = (Vector2)turretToRotate.transform.position + offset * turretToRotate.transform.localScale.y;
            //    Vector2 pos2 = (Vector2)turretToRotate.transform.position - offset * turretToRotate.transform.localScale.y;
            //    Vector2 newVelocity = this.dir * velocity;
            //    Vector3 newRotation = new Vector3(
            //                                    this.transform.eulerAngles.x,
            //                                    this.transform.eulerAngles.y,
            //                                    this.transform.eulerAngles.z
            //                                    );

            //    weapon.FireWeapon(newRotation, pos1, newVelocity);
            //    weapon.FireWeapon(newRotation, pos2, newVelocity);
            //    StartCoroutine(MuzzleFlash());
            //}

            List<Enemy> nearEnemies = getNearEntities(60);

            if (nearEnemies.Count != 0)
            {
                if (canFire)
                {
                    int enemyIndex = Random.Range(0, nearEnemies.Count - 1);
                    faceEnemy(nearEnemies[enemyIndex], this.gameObject);

                    Vector2 pos1 = (Vector2)turretToRotate.transform.position + offset * turretToRotate.transform.localScale.y;
                    Vector2 pos2 = (Vector2)turretToRotate.transform.position - offset * turretToRotate.transform.localScale.y;
                    Vector2 newVelocity = this.dir * velocity;
                    Vector3 rotation = new Vector3(
                                                turretToRotate.transform.eulerAngles.x,
                                                turretToRotate.transform.eulerAngles.y,
                                                turretToRotate.transform.eulerAngles.z
                                                );

                    StartCoroutine(MuzzleFlash());
                    weapon.FireWeapon(rotation, pos1, newVelocity);
                    if (base.towerTier >= 2) weapon.FireWeapon(rotation, pos2, newVelocity);
                    StartCoroutine(WeaponCooldown());
                    noWeaponFireCount = 0;
                }
                noWeaponFireCount++;
                noTowerCountdownCount++;

                if (noWeaponFireCount > 75)
                {
                    canFire = true;
                    noWeaponFireCount = 0;
                }

                if (noTowerCountdownCount > 1000)
                {
                    startCountdown = false;
                    noTowerCountdownCount = 0;
                }
            }
        }
    }

    private IEnumerator WeaponCooldown()
    {
        canFire = false;

        yield return new WaitForSeconds(1);

        canFire = true;
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash1.SetActive(true);
        if (muzzleFlash2 != null) muzzleFlash2.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        muzzleFlash1.SetActive(false);
        if (muzzleFlash2 != null) muzzleFlash2.SetActive(false);
    }

}
