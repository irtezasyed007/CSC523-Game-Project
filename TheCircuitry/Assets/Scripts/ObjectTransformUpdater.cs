using UnityEngine;
using System.Collections;

public class ObjectTransformUpdater : MonoBehaviour
{
    public float minScale;
    public float maxScale;
    public float scaleRate = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    private void applyScaleRate()
    {
        transform.localScale += Vector3.one * scaleRate;
    }

    // Update is called once per frame
    void Update()
    {
        //if we exceed the defined range then correct the sign of scaleRate.
        if (transform.localScale.x < minScale)
        {
            scaleRate = Mathf.Abs(scaleRate);
        }
        else if (transform.localScale.x > maxScale)
        {
            scaleRate = -Mathf.Abs(scaleRate);
        }

        applyScaleRate();
    }
}
