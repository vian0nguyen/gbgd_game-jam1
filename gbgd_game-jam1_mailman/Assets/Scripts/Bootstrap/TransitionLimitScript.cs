using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionLimitScript : MonoBehaviour
{
    public enum connectionType { None, Both, Top, Bottom };
    [System.Serializable]
    public struct path
    {
        public connectionType pathConnect;
    }
    public path[] paths;
    public SpriteRenderer sr;
    public Sprite topEnd;
    public Sprite bottomEnd;
    public Sprite defaultEnd;

    //checks what kind of road transition this is before changing the sprite
    public void ChangeSprite(int currentAreaNumber)
    {
        switch (paths[currentAreaNumber].pathConnect)
        {
            case TransitionLimitScript.connectionType.Top:
                ValidateSpriteChange(topEnd, TransitionLimitScript.connectionType.Top);
                break;
            case TransitionLimitScript.connectionType.Bottom:
                ValidateSpriteChange(bottomEnd, TransitionLimitScript.connectionType.Bottom);
                break;
            case TransitionLimitScript.connectionType.None:
                sr.sprite = null;
                break;
            default:
                ValidateSpriteChange(defaultEnd, TransitionLimitScript.connectionType.Both);
                break;
        }
    }

    //checks if there is a valid sprite to use (otherwise prints out changes)
    public void ValidateSpriteChange(Sprite newSprite, TransitionLimitScript.connectionType pathType)
    {
        if (newSprite != null)
            sr.sprite = newSprite;
        else
            Debug.Log(this.name + " has been changed to " + pathType.ToString());
    }
}
