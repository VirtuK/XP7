using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCircle : MonoBehaviour
{
    public void callCircle()
    {
        if (InteractionManagar.instance.isNear && !InteractionManagar.instance.haveItemSelected)
        {
            InteractionManagar.instance.CheckInteractions();
        }
        gameObject.SetActive(false);
    }
}
