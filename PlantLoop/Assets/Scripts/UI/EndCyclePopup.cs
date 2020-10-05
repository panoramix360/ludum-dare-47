using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCyclePopup : MonoBehaviour
{
    private Animator animator;
    private PlayerLevelUp.LevelType levelType;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetLevelType(PlayerLevelUp.LevelType levelType)
    {
        this.levelType = levelType;
    }

    public void OnFinishFadeIn()
    {
        switch(levelType)
        {
            case PlayerLevelUp.LevelType.WATER:
                ShowWaterUpgrade();
                break;
            case PlayerLevelUp.LevelType.ENERGY:
                ShowEnergyUpgrade();
                break;
            case PlayerLevelUp.LevelType.HP:
                ShowHpUpgrade();
                break;
            case PlayerLevelUp.LevelType.DRAW:
                ShowAllUpgrade();
                break;
            default:
                Debug.LogError("Atributo não encontrado");
                break;
        }
    }

    public void ShowAllUpgrade()
    {
        animator.SetTrigger("PlayAllUpgrade");
    }

    public void ShowWaterUpgrade()
    {
        animator.SetTrigger("PlayWaterUpgrade");
    }

    public void ShowEnergyUpgrade()
    {
        animator.SetTrigger("PlayEnergyUpgrade");
    }

    public void ShowHpUpgrade()
    {
        animator.SetTrigger("PlayHpUpgrade");
    }
}
