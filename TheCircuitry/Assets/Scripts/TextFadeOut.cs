using System.Collections;
using UnityEngine;
using UnityEngine.UI;
class TextFadeOut : MonoBehaviour
{
 
    //Fade time in seconds
    public float fadeOutTime;

    public void FadeOut()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeOutRoutine());
    }

    public void FadeOut(Vector3 pos)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        Text text = GetComponent<Text>();
        Color originalColor = text.color;

        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + t, gameObject.transform.position.z);
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }

        Destroy(gameObject);
    }

    private void updateTextPosition(Text text, Transform newPos)
    {
        text.transform.position = new Vector3(newPos.position.x, newPos.position.y, newPos.position.z);
    }

}