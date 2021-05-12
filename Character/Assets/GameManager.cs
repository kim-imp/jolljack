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
    private float QCT;
    private float WCT;
    private float ECT;
    private float MQCT;
    private float MWCT;
    private float MECT;

    [SerializeField]
    private Image HPImage;

    [SerializeField]
    private Image FuryImage;

    [SerializeField]
    private Image QCoolImage;
    [SerializeField]
    private Image WCoolImage;
    [SerializeField]
    private Image ECoolImage;





    // Start is called before the first frame update
    void Start()
    {
        PHP = Player.HP;
        HPImage.fillAmount = (float)PHP / (float)MaxHp;
        PFury = Player.Fury;
        FuryImage.fillAmount = (float)PFury / (float)MaxFury;
        QCT = Player.NQCTime;
        WCT = Player.NWCTime;
        ECT = Player.NECTime;
        MQCT = Player.QCTime;
        MWCT = Player.WCTime;
        MECT = Player.ECTime;
    }

    // Update is called once per frame
    void Update()
    {
        QCT = Player.NQCTime;
        WCT = Player.NWCTime;
        ECT = Player.NECTime;
        HandleFury();
        HandleHp();
        HandleQCool();
        HandleWCool();
        HandleECool();
        print(QCT);
    }

    // HP 게이지 관리
    private void HandleHp()
    {
        PHP = Player.HP;
        HPImage.fillAmount = Mathf.Lerp(HPImage.fillAmount, (float)PHP / (float)MaxHp, Time.deltaTime * 10);
    }

    // 분노 게이지 관리
    private void HandleFury()
    {
        PFury = Player.Fury;
        FuryImage.fillAmount = Mathf.Lerp(FuryImage.fillAmount, (float)PFury / (float)MaxFury, Time.deltaTime * 10);
    }


    // 쿨타임 관리
    private void HandleQCool()
    {
        QCoolImage.fillAmount = 1 - ((float)QCT / (float)MQCT);
        //QCoolImage.fillAmount = Mathf.Lerp(QCoolImage.fillAmount, 1 - ((float)QCT / (float)MQCT), Time.deltaTime);
    }
    private void HandleWCool()
    {
        WCoolImage.fillAmount = 1 - ((float)WCT / (float)MWCT);
        //WCoolImage.fillAmount = Mathf.Lerp(WCoolImage.fillAmount, 1 - ((float)WCT / (float)MWCT), Time.deltaTime);
    }
    private void HandleECool()
    {
        ECoolImage.fillAmount = 1 - ((float)ECT / (float)MECT);
        //ECoolImage.fillAmount = Mathf.Lerp(ECoolImage.fillAmount, 1 - ((float)ECT / (float)MECT), Time.deltaTime);
    }
}
