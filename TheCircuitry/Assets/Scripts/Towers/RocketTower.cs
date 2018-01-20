using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : Tower
{
    public static int price = 250;

    private TowerManager towerManager;
    private GameObject turretHead; //Basically what we are rotating (the turret-head)
    private Transform currentTransform; //The transform of where the turretHead should be facing
    private GameObject loadedTurretHead; //GameObject that contains the SpriteRenderer for a loaded rocket turret
    private GameObject unloadedTurretHead; //GameObject that contains the SpriteRenderer for an unloaded rocket turret
    private GameObject turretBase;
    private bool isReloaded = true;

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
        foreach (Component comp1 in GetComponentsInChildren<Component>(true))
        {
            GameObject go = comp1.gameObject;

            //Implies more than one head and you must switch between them!
            if (go.name == "turretHead")
            {
                foreach (Component comp2 in go.GetComponentsInChildren<Component>(true))
                {
                    if (comp2.gameObject.name.Contains("armed") && loadedTurretHead == null)
                    {
                        loadedTurretHead = comp2.gameObject;
                        turretHead = loadedTurretHead;
                    }

                    if (comp2.gameObject.name.Contains("unloaded") && unloadedTurretHead == null)
                    {
                        unloadedTurretHead = comp2.gameObject;
                    }
                }
            }

            else if(go.name == "turretBase")
            {
                this.turretBase = go;
            }
        }
    }

    private void Update()
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

                ttlCnt++; //Added this in so towers break faster if they are shooting
                StartCoroutine(WeaponReload());
                weapon.FireWeapon(rotation, pos1, newVelocity);
                if (base.towerTier >= 2) weapon.FireWeapon(rotation, pos2, newVelocity);

                yield return new WaitForSeconds(weaponFireRate);
            }

            else yield return null;
        }

    }

    private IEnumerator WeaponReload()
    {
        loadedTurretHead.SetActive(false);
        updateTransform(unloadedTurretHead);
        unloadedTurretHead.SetActive(true);
        turretHead = unloadedTurretHead;

        yield return new WaitForSeconds(weaponFireRate/2);

        unloadedTurretHead.SetActive(false);
        updateTransform(loadedTurretHead);
        loadedTurretHead.SetActive(true);
        turretHead = loadedTurretHead;
    }

    private void updateTransform(GameObject go)
    {
        go.transform.eulerAngles = new Vector3(
                                            turretHead.transform.eulerAngles.x,
                                            turretHead.transform.eulerAngles.y,
                                            turretHead.transform.eulerAngles.z
                                            );
    }

    public override string getTowerType()
    {
        return "Rocket";
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