using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Button : Item
{
    [SerializeField] private Door door;
    [SerializeField] private bool pressed;
    public override void Use()
    {
        if (!pressed)
        {
            MessageText.instance.ShowText("Apertei o botão");
            door.isButtonPressed = true;
            pressed = true;
            if(door.haveDisplay) door.turnOnDisplay();
        }
        else
        {
            MessageText.instance.ShowText("Esse botão já foi apertado");
        }
    }
}
