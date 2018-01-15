using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletTower : Tower
{

    private TowerManager towerManager;
    private GameObject turretHead;
    private GameObject muzzleFlash1;
    private GameObject muzzleFlash2;
    private GameObject turretBase;

    // Use this for initialization
    void Start()
    {
        init();
        initComponents();
        this.towerManager = this.GetComponentInParent<TowerManager>();
    }

    private void initComponents()
    {
        foreach (Component go in GetComponentsInChildren<Component>(true))
        {
            if (go.gameObject.name.Contains("muzzleFlash"))
            {
                if (muzzleFlash1 == null) muzzleFlash1 = go.gameObject;
                else
                {
                    muzzleFlash2 = go.gameObject;
                    if (towerTier == 1) muzzleFlash2.SetActive(false);
                }
            }

            else if (go.gameObject.name == "turretHead")
            {
                this.turretHead = go.gameObject;
            }

            else if (go.gameObject.name == "turretBase")
            {
                this.turretBase = go.gameObject;
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

        if (Input.GetMouseButtonDown(1))
        {
            foreach(Enemy e in Enemy.instantiedEnemies)
            {
                Vector2 tower = (Vector2)transform.position;
                Vector2 enemy = (Vector2)e.gameObject.transform.position;
                Debug.Log("Diff: " + Vector2.Distance(tower, enemy));
            }
        }

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

            List<Enemy> nearEnemies = getNearEntities(175);

            if (nearEnemies.Count != 0)
            {
                if (canFire)
                {
                    int enemyIndex = Random.Range(0, nearEnemies.Count - 1);
                    faceEnemy(nearEnemies[enemyIndex], this.turretHead);

                    Vector2 pos1 = (Vector2)turretHead.transform.position + offset * turretHead.transform.localScale.y;
                    Vector2 pos2 = (Vector2)turretHead.transform.position - offset * turretHead.transform.localScale.y;
                    Vector2 newVelocity = this.dir * velocity;
                    Vector3 rotation = new Vector3(
                                                turretHead.transform.eulerAngles.x,
                                                turretHead.transform.eulerAngles.y,
                                                turretHead.transform.eulerAngles.z
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

        yield return new WaitForSeconds(weaponFireRate);

        canFire = true;
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash1.SetActive(true);
        if (towerTier > 1) muzzleFlash2.SetActive(true);

        yield return new WaitForSeconds(0.05f);

        muzzleFlash1.SetActive(false);
        if (towerTier > 1) muzzleFlash2.SetActive(false);
    }

    public override string getTowerType()
    {
        return "Bullet";
    }

    public override bool upgradeTower()
    {
        if (this.towerTier == 3) return false;

        else
        {
            if (towerTier == 1)
            {
                towerManager.activateNextTower(1);
            }

            else if (towerTier == 2)
            {
                towerManager.activateNextTower(2);
            }

            return true;
        }
    }


}
