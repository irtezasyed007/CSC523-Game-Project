using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planes : MonoBehaviour
{
    
    public Weapon weapon;
    public int movementSpeed;

    private const int MAX_PLANES = 3;
    private static int planeCount = 0;

    private Vector2 direction = Vector2.left;
    private GameObject muzzleFlash;
    private bool duplicateInstance = false;

    // Use this for initialization
    void Awake()
    {
        if(planeCount < MAX_PLANES)
        {
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>(true);
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);

            foreach (SpriteRenderer sr in sprites)
            {
                if (sr.name == "planeMuzzleFlash")
                {
                    muzzleFlash = sr.gameObject;
                    break;
                }
            }

            planeCount++;
        }

        else
        {
            duplicateInstance = true;
            Destroy(gameObject);
        }
        
    }

    private void OnEnable()
    {
        StartCoroutine(FireWeapon());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 val = direction * movementSpeed * Time.deltaTime;
        transform.Translate(val);    
    }

    private IEnumerator FireWeapon()
    {
        while (true)
        {
            StartCoroutine(MuzzleFlash());
            weapon.FireWeapon(muzzleFlash.transform.position, new Vector2(-2500, 0));
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;

        if (go.tag == "OutOfBounds")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!duplicateInstance)
        {
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Remove(gameObject);
            planeCount = 0;
        }
    }
}
