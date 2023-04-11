using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [Header("Change the side padding until the right and left most objects are pulled exactly to the center")]
    [SerializeField] private GameObject scrollBar;
    //[SerializeField] private float temp = 0;
    [SerializeField] private float fadeInSpeed = .1f;
    private float scroll_pos = 0;
    private float[] pos;

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        //get the screen position of all the items
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance* i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.GetComponent<Scrollbar>().value;

        }
        //if there is no input, lerp the scrollBar to the nearest button
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        //constantly check that all the items are the right size, enlarging the one closest to scroll_pos
        for (int i = 0; i < pos.Length; i++)
        {
            //if the target item
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i]-(distance / 2))
            {
                //enlarge item
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.1f, 1.1f), fadeInSpeed);
                //notify this item was selected and check if it was new
                CheckOnSelect(i);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.9f, 0.9f), fadeInSpeed);
                    }
                }
            }
        }
    }

    int currentlySelectedItem = -1;
    private void CheckOnSelect(int _newSelected)
    {
        if(currentlySelectedItem != _newSelected)
        {
            Debug.Log("Selected: " + transform.GetChild(_newSelected).name);
            SoundPool.instance.PlaySound("item_focus");
        }
        currentlySelectedItem = _newSelected;
    }


}

