using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papel : Item
{
    [SerializeField] GameObject paper;
    public override void Use()
    {
        paper.SetActive(true);
    }
}
