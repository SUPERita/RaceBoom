using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string downSound = "btn_down";
    [SerializeField] private string upSound = "btn_up";
    public void OnPointerDown(PointerEventData eventData)
    {
        SoundPool.instance.PlaySound(downSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //SoundPool.instance.PlaySound(upSound);
    }
}
