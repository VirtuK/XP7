using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
[System.Serializable]
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
    [SerializeField] public string itemName;
    [SerializeField] public Sprite icon;
    [SerializeField, TextArea(3, 6)] public string seeDescription;
    [SerializeField] public InteractionType interactions;
    [SerializeField] public string itemID;

    [SerializeField] public bool IgnoreOnSave; 

    private void Awake()
    {
        itemID = gameObject.GetInstanceID().ToString();
    }
    public virtual void Pick() 
    {
        ItemData item = new ItemData(itemName, itemID, icon, this);
        InventoryManager.instance.AddItem(item);
        ListDestruction();
        gameObject.SetActive(false);
    }
    public virtual void See() 
    {
        MessageText.instance.ShowText(seeDescription);
    }
    public virtual void Use() { }

    public void ListDestruction()
    {
        // Notify SceneSerializationManager when this object is destroyed
        if (SceneSerializationManager.instance != null)
        {
            SceneSerializationManager.instance.RegisterDestroyedObject(this.gameObject);
        }
    }
}
