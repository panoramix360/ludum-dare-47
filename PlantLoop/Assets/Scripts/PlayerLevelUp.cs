using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    [SerializeField] private int upgradesToLevelUp;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public int GetUpgradesToLevelUp()
    {
        return upgradesToLevelUp;
    }

    public void NextLevel(LevelType type)
    {
        currentLevel++;
        List<AttributeIncrementor> incrementors = new List<AttributeIncrementor>();

        if (currentLevel % 2 == 0)
        {
            switch(type)
            {
                case LevelType.ENERGY:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.ENERGY, 0, 0.3f));
                    break;
                case LevelType.WATER:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.WATER, 0, 0.3f));
                    break;
                case LevelType.HP:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.HP, 0, 0.3f));
                    break;
                case LevelType.DRAW:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.ENERGY, 0, 0.1f));
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.WATER, 0, 0.1f));
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.HP, 0, 0.1f));
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case LevelType.ENERGY:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.ENERGY, 30, 0));
                    break;
                case LevelType.WATER:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.WATER, 30, 0));
                    break;
                case LevelType.HP:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.HP, 30, 0));
                    break;
                case LevelType.DRAW:
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.ENERGY, 10, 0));
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.WATER, 10, 0));
                    incrementors.Add(new AttributeIncrementor(AttributeEnum.HP, 10, 0));
                    break;
            }
        }

        IncrementAttributes(incrementors);
    }

    private void IncrementAttributes(List<AttributeIncrementor> incrementors)
    {
        foreach(AttributeIncrementor incrementor in incrementors)
        {
            switch(incrementor.type)
            {
                case AttributeEnum.ENERGY:
                    player.energy.baseValue += incrementor.baseIncrementor;
                    player.energy.unitPerTime += incrementor.perTimeIncrementor;
                    break;
                case AttributeEnum.WATER:
                    player.water.baseValue += incrementor.baseIncrementor;
                    player.water.unitPerTime += incrementor.perTimeIncrementor;
                    break;
                case AttributeEnum.HP:
                    player.hp.baseValue += incrementor.baseIncrementor;
                    player.hp.unitPerTime += incrementor.perTimeIncrementor;
                    break;
            }
        }
    }

    public enum LevelType
    {
        WATER, ENERGY, HP, DRAW
    }
}
