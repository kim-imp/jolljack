using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Hero Player;

    private float PHP;
    private float MaxHp = 100;
    private float PFury;
    private float MaxFury = 100;

    [SerializeField]
    private Image HPImage;

    [SerializeField]
    private Image FuryImage;



    // Start is called before the first frame update
    void Start()
    {
        PHP = Player.HP;
        HPImage.fillAmount = (float)PHP / (float)MaxHp;
        PFury = Player.Fury;
        FuryImage.fillAmount = (float)PFury / (float)MaxFury;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFury();
        HandleHp();
    }

    private void HandleHp()
    {
        PHP = Player.HP;
        HPImage.fillAmount = Mathf.Lerp(HPImage.fillAmount, (float)PHP / (float)MaxHp, Time.deltaTime * 10);
    }
    private void HandleFury()
    {
        PFury = Player.Fury;
        FuryImage.fillAmount = Mathf.Lerp(FuryImage.fillAmount, (float)PFury / (float)MaxFury, Time.deltaTime * 10);
    }
}
