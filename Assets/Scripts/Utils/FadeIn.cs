using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class FadeIn
{
    /// <summary>
    /// Fades the alpha of the provided image with a linear speed and fires an action afterwards
    /// </summary>
    /// <param name="image"> The UI Image the fade will be applied to</param>
    /// <param name="fadeSpeed"> The speed in which the fade happens</param>
    /// <param name="action"> the action that will fire once the fading courotine reachs its end</param>
    /// <returns></returns>
    public static IEnumerator FadeUIImage(Image image, float fadeSpeed, Action action)
    {
        if(!image.gameObject.activeInHierarchy)
        {
            image.gameObject.SetActive(true);
        }

        while(image.color.a < 1f)
        {
            
            float newAlpha = image.color.a + fadeSpeed;
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);

            yield return null;
        }
        
        action?.Invoke();
    }
}
