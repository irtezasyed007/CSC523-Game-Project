using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : Tower
{

    private SpriteRenderer[] sprites;
    private GameObject activeGameObject;
    private GameObject loadedTurret;
    private GameObject unloadedTurret;
    private Transform currentTransform;
    private int activeRocketTurretIndex; //0 or 1
    private bool isReloaded = true;

    private new void Start()
    {
        init();
        initComponents();

    }

    private void initComponents()
    {
        foreach (Component comp1 in GetComponentsInChildren<Component>(true))
        {
            GameObject go = comp1.gameObject;

            //Implies more than one head and you must switch between them!
            if (go.name == "turretHeads")
            {
                foreach (Component comp2 in go.GetComponentsInChildren<Component>(true))
                {
                    if (comp2.gameObject.name.Contains("armed") && loadedTurret == null)
                    {
                        loadedTurret = comp2.gameObject;
                        activeGameObject = loadedTurret;
                    }

                    if (comp2.gameObject.name.Contains("unloaded") && unloadedTurret == null)
                    {
                        unloadedTurret = comp2.gameObject;
                    }
                }
            }
        }
    }

    private void initTieredTurrets()
    {
        tier1Tower = Resources.Load<GameObject>("Turrets/tier1RocketTurret");
        tier2Tower = Resources.Load<GameObject>("Turrets/tier2RocketTurret");
        tier3Tower = Resources.Load<GameObject>("Turrets/tier3RocketTurret");
    }

    private void Update()
    {
        //DEBUG FEATURE TO FIX ALL TOWERS IN A LEVEL
        //if (Input.GetMouseButtonDown(1) && base.towerTier == 3)
        //{
        //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 dir = (activeGameObject.transform.position - pos);
        //    activeGameObject.transform.right = dir;
        //    activeGameObject.transform.eulerAngles = new Vector3(
        //                                            activeGameObject.transform.eulerAngles.x,
        //                                            activeGameObject.transform.eulerAngles.y,
        //                                            activeGameObject.transform.eulerAngles.z + 90
        //                                            );

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
            //    Vector2 pos1 = (Vector2)activeGameObject.transform.position + offset * activeGameObject.transform.localScale.y;
            //    Vector2 pos2 = (Vector2)activeGameObject.transform.position - offset * activeGameObject.transform.localScale.y;
            //    Vector2 newVelocity = this.dir * velocity;
            //    Vector3 rotation = new Vector3(
            //                                activeGameObject.transform.eulerAngles.x,
            //                                activeGameObject.transform.eulerAngles.y,
            //                                activeGameObject.transform.eulerAngles.z
            //                                );

            //    weapon.FireWeapon(rotation, pos1, newVelocity);
            //    weapon.FireWeapon(rotation, pos2, newVelocity);

            //    StartCoroutine(WeaponReload());
            //    StartCoroutine(WeaponCooldown());
            //}

            List<Enemy> nearEnemies = getNearEntities(60);

            if (nearEnemies.Count != 0)
            {
                if (canFire && isReloaded)
                {
                    int enemyIndex = Random.Range(0, nearEnemies.Count - 1);
                    faceEnemy(nearEnemies[enemyIndex], this.activeGameObject);

                    Vector2 pos1 = (Vector2)activeGameObject.transform.position + offset * activeGameObject.transform.localScale.y;
                    Vector2 pos2 = (Vector2)activeGameObject.transform.position - offset * activeGameObject.transform.localScale.y;
                    Vector2 newVelocity = this.dir * velocity;
                    Vector3 rotation = new Vector3(
                                                activeGameObject.transform.eulerAngles.x,
                                                activeGameObject.transform.eulerAngles.y,
                                                activeGameObject.transform.eulerAngles.z
                                                );

                    weapon.FireWeapon(rotation, pos1, newVelocity);
                    if(base.towerTier == 2) weapon.FireWeapon(rotation, pos2, newVelocity);                   

                    StartCoroutine(WeaponReload());
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

        yield return new WaitForSeconds(5);
        
        canFire = true;
    }

    private IEnumerator WeaponReload()
    {
        isReloaded = false;
        loadedTurret.SetActive(false);
        updateTransform(unloadedTurret);
        unloadedTurret.SetActive(true);
        activeGameObject = unloadedTurret;

        yield return new WaitForSeconds(3);

        unloadedTurret.SetActive(false);
        updateTransform(loadedTurret);
        loadedTurret.SetActive(true);
        activeGameObject = loadedTurret;
        isReloaded = true;
    }

    private void updateTransform(GameObject go)
    {
        go.transform.eulerAngles = new Vector3(
                                            activeGameObject.transform.eulerAngles.x,
                                            activeGameObject.transform.eulerAngles.y,
                                            activeGameObject.transform.eulerAngles.z
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
            this.towerTier++;

            return true;
        }
    }
}