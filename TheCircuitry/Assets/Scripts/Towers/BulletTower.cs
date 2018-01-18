using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletTower : Tower
{
    public static int price = 150;

    private TowerManager towerManager;
    private GameObject turretHead;
    private GameObject muzzleFlash1;
    private GameObject muzzleFlash2;
    private GameObject turretBase;

    private void OnEnable()
    {
        if (!initialized)
        {
            init();
            initComponents();
            this.towerManager = this.GetComponentInParent<TowerManager>();
            initialized = true;
        }

        StartCoroutine(FireWeapon());
        StartCoroutine(TowerBreak());
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
        if (isBroken)
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Play();
        }

        else
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Stop();
        }
    }

    private IEnumerator FireWeapon()
    {
        while (true)
        {
            List<Enemy> nearEnemies = getNearEntities(175);

            if (nearEnemies.Count > 0 && !isBroken)
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

                yield return new WaitForSeconds(weaponFireRate);
            }

            else yield return null;
        }
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
