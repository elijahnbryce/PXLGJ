using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerBar : MonoBehaviour
{
    private GameObject powerBar;
    [SerializeField] private float barHoldTime = 1f;

    private void Start()
    {
        powerBar = GetComponentInChildren<GameObject>();
        HideBar();
    }

    public void ShowBar()
    {
        powerBar.SetActive(true);
    }

    public void HideBar()
    {
        StartCoroutine(GreenFN());
    }

    private IEnumerator GreenFN()
    {
        yield return new WaitForSeconds(barHoldTime);
        powerBar.SetActive(false);
    }

    public void UpdateBar(float fill)
    {
        powerBar.GetComponent<Image>().fillAmount = fill;
    }
}
