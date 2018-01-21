using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class JITManager : MonoBehaviour
{

    private GameObject startWaveJIT;
    private GameObject fixTurretJIT;

    // Use this for initialization
    void Awake()
    {   
        foreach (RectTransform rt in GetComponentsInChildren<RectTransform>(true))
        {
            if (rt.gameObject.name == "ClickToFix_JIT") fixTurretJIT = rt.gameObject;
            else if (rt.gameObject.name == "ClickToStart_JIT") startWaveJIT = rt.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wave.wave.isStarted() && GameManager.Manager.tipShown 
            && SceneManager.GetActiveScene().name == "level1")
        {
            GameObject startTurret = GameObject.Find("bulletTurrets").GetComponentInChildren<Tower>().gameObject;

            if (startTurret.GetComponent<Tower>().isBroken)
            {
                fixTurretJIT.SetActive(true);
            }

            else
            {
                fixTurretJIT.SetActive(false);
                startWaveJIT.SetActive(true);
            }
        }

        else
        {
            fixTurretJIT.SetActive(false);
            startWaveJIT.SetActive(false);
        }
    }
}
