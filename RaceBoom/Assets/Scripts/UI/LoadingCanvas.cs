using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider = null;
    [SerializeField] private TextMeshProUGUI loadingText = null;
    private CancellationTokenSource cancellationTokenSource;
    void Start()
    {
        
        StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(LerpToValue(.15f, .5f));
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(LerpToValue(.20f, .25f));
        yield return new WaitForSeconds(.25f);
        yield return StartCoroutine(LerpToValue(.30f, 1f));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(LerpToValue(.35f, .5f));
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(LerpToValue(.70f, 1f));
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(LerpToValue(1f, .5f));
        DOTween.Clear(true);
        SceneManager.LoadScene("GameLoop");
    }

    private IEnumerator LerpToValue(float endValue, float duration)
    {
        // Store the current value of the slider as the starting value
        float startValue = loadingSlider.value;

        // Lerp the slider value and the text value to match the end value
        DOTween.To(() => startValue, x => {
            loadingSlider.value = x;
            //float _f = (((int)(10 * x * 100)) / 10f);
            float _f = (int)(x * 100);
            loadingText.text = _f.ToString() + "%";
        }, endValue, duration).SetEase(Ease.OutQuad);

        // Wait for the lerp to finish
        yield return new WaitForSeconds(duration);
    }

    private void OnDisable()
    {
        StopCoroutine(StartLoading());
    }

}
