using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionLimitScript : MonoBehaviour
{
    public enum endType { None, Top, Bottom };
    [System.Serializable]
    public struct path
    {
        public endType pathEnd;
    }
    public path[] paths;
    public SpriteRenderer sr;
    public Sprite topEnd;
    public Sprite bottomEnd;
    public Sprite defaultEnd;

    //checks what kind of road transition this is before changing the sprite
    public void ChangeSprite(int currentAreaNumber)
    {
        switch (paths[currentAreaNumber].pathEnd)
        {
            case TransitionLimitScript.endType.Top:
                ValidateSpriteChange(topEnd, TransitionLimitScript.endType.Top);
                break;
            case TransitionLimitScript.endType.Bottom:
                ValidateSpriteChange(bottomEnd, TransitionLimitScript.endType.Bottom);
                break;
            default:
                ValidateSpriteChange(defaultEnd, TransitionLimitScript.endType.None);
                break;
        }
    }

    //checks if there is a valid sprite to use (otherwise prints out changes)
    public void ValidateSpriteChange(Sprite newSprite, TransitionLimitScript.endType pathType)
    {
        if (newSprite != null)
            sr.sprite = newSprite;
        else
            Debug.Log(this.name + " has been changed to " + pathType.ToString());
    }
}
