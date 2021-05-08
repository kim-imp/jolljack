using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Hero Player;

    private float PHP;
    private float MaxHp = 100;

    [SerializeField]
    private Image HPImage;



    // Start is called before the first frame update
    void Start()
    {
        PHP = Player.HP;
        HPImage.fillAmount = (float)PHP / (float)MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHp();
    }

    private void HandleHp()
    {
        PHP = Player.HP;
        HPImage.fillAmount = Mathf.Lerp(HPImage.fillAmount, (float)PHP / (float)MaxHp, Time.deltaTime * 10);
    }
}
