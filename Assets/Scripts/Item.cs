using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum InteractionType
{
    None = 0,
    Pick = 1 << 0,  // 1
    See = 1 << 1,   // 2
    Use = 1 << 2    // 4
}

public abstract class Item : MonoBehaviour, IInteractable
{
    public string itemName;
    public Sprite icon;
    public string seeDescription;
    public InteractionType interactions;

    public virtual void Pick() { }
    public virtual void See() 
    {
        MessageText.instance.ShowText(seeDescription);
    }
    public virtual void Use() { }
}
