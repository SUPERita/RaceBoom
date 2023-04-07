using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance = null;
    [SerializeField] private GameObject messagePrefab = null;
    [SerializeField] private Transform spawnPoint = null;
     
    [SerializeField] private float fadeSpeed = .5f;

    private List<RectTransform> messages = new List<RectTransform>();

    private void Awake()
    {
        if (instance) { Destroy(gameObject); return; }
        instance = this;
    }
    
    public void SendMessage(string _arg)
    {
        GameObject g = Instantiate(messagePrefab, spawnPoint);

        //set msg text
        g.GetComponentInChildren<TextMeshProUGUI>().SetText(_arg);

        //fade msg in
        g.GetComponent<RectTransform>().localPosition = Vector3.right * -g.GetComponent<RectTransform>().sizeDelta.x;
        g.GetComponent<RectTransform>().DOLocalMoveX(0, fadeSpeed);

        //fade other msgs in list
        for (int i = 0; i < messages.Count; i++)
        {
            messages[i].DOAnchorPosY(-messages[i].sizeDelta.y * (messages.Count-i), fadeSpeed/2f);
        }


        messages.Add(g.GetComponent<RectTransform>());

        //set destroy
        StartCoroutine(RemoveFromList(g, 2f));

    }
    private IEnumerator RemoveFromList(GameObject _g, float _delay)
    {
        yield return new WaitForSeconds(_delay);

        messages.Remove(_g.GetComponent<RectTransform>());
        _g.GetComponent<RectTransform>().DOLocalMoveX(-_g.GetComponent<RectTransform>().sizeDelta.x, fadeSpeed)
            .OnComplete(()=>Destroy(_g));

        
    }

    [Button]
    private void SampleMessage(string _arg)
    {
        SendMessage(_arg);
    }
}
