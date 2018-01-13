using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : Tower
{

    private TowerManager towerManager;
    private GameObject turretHead; //Basically what we are rotating (the turret-head)
    private Transform currentTransform; //The transform of where the turretHead should be facing
    private GameObject loadedTurretHead; //GameObject that contains the SpriteRenderer for a loaded rocket turret
    private GameObject unloadedTurretHead; //GameObject that contains the SpriteRenderer for an unloaded rocket turret
    private GameObject turretBase;
    private bool isReloaded = true;

    private new void Start()
    {
        init();
        initComponents();
        this.towerManager = this.GetComponentInParent<TowerManager>();
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

            Debug.Log("Not Broken!");
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
                    faceEnemy(nearEnemies[enemyIndex], this.turretHead);

                    Vector2 pos1 = (Vector2)turretHead.transform.position + offset * turretHead.transform.localScale.y;
                    Vector2 pos2 = (Vector2)turretHead.transform.position - offset * turretHead.transform.localScale.y;
                    Vector2 newVelocity = this.dir * velocity;
                    Vector3 rotation = new Vector3(
                                                turretHead.transform.eulerAngles.x,
                                                turretHead.transform.eulerAngles.y,
                                                turretHead.transform.eulerAngles.z
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
        loadedTurretHead.SetActive(false);
        updateTransform(unloadedTurretHead);
        unloadedTurretHead.SetActive(true);
        turretHead = unloadedTurretHead;

        yield return new WaitForSeconds(3);

        unloadedTurretHead.SetActive(false);
        updateTransform(loadedTurretHead);
        loadedTurretHead.SetActive(true);
        turretHead = loadedTurretHead;
        isReloaded = true;
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