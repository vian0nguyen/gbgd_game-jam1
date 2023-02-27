using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMultiScript : NPCScript
{
    [Header ("Animations/Tweening")]
    public SpriteRenderer[] sprites;
    public Animation[] spriteAnimations;
    public Color darkenedColor;
    public float spriteShrinkFactor;
    public float animationSpeed;

    //darkens and shrinks sprite that isn't talking
    public IEnumerator DarkenSprite(SpriteRenderer selectedSprite)
    {
        for (float i = 0; i < animationSpeed; i += Time.deltaTime)
        {
            selectedSprite.gameObject.transform.localScale = Vector3.Lerp(selectedSprite.gameObject.transform.localScale, new Vector3(spriteShrinkFactor, spriteShrinkFactor, spriteShrinkFactor), i);
            selectedSprite.color = Color.Lerp(selectedSprite.color, darkenedColor, i);
            selectedSprite.sortingOrder = 0;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    //lightens sprite that is talking
    public IEnumerator GrowSprite(SpriteRenderer selectedSprite)
    {
        for (float i = 0; i < animationSpeed; i += Time.deltaTime)
        {
            selectedSprite.transform.localScale = Vector3.Lerp(selectedSprite.transform.localScale, Vector3.one, i);
            selectedSprite.color = Color.Lerp(selectedSprite.color, Color.white, i);
            selectedSprite.sortingOrder = 1;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    //resets scales and colors of all sprites
    public IEnumerator ResetSprites()
    {
        for (float i = 0; i < animationSpeed; i += Time.deltaTime)
        {
            foreach(SpriteRenderer sr in sprites)
            {
                sr.color = Color.Lerp(sr.color, Color.white, i);
                sr.transform.localScale = Vector3.Lerp(sr.transform.localScale, Vector3.one, i);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
