using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    [SerializeField] private int upgradesToLevelUp;

    private PlayerAttributes playerAttributes;

    private void Start()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
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

        upgradesToLevelUp++;
    }

    private void IncrementAttributes(List<AttributeIncrementor> incrementors)
    {
        foreach(AttributeIncrementor incrementor in incrementors)
        {
            switch(incrementor.type)
            {
                case AttributeEnum.ENERGY:
                    playerAttributes.energy.IncrementBaseValue(incrementor.baseIncrementor);
                    playerAttributes.energy.IncrementUnitPerTime(incrementor.perTimeIncrementor);
                    break;
                case AttributeEnum.WATER:
                    playerAttributes.water.IncrementBaseValue(incrementor.baseIncrementor);
                    playerAttributes.water.IncrementUnitPerTime(incrementor.perTimeIncrementor);
                    break;
                case AttributeEnum.HP:
                    playerAttributes.hp.IncrementBaseValue(incrementor.baseIncrementor);
                    playerAttributes.hp.IncrementUnitPerTime(incrementor.perTimeIncrementor);
                    break;
            }
        }
    }

    public enum LevelType
    {
        WATER, ENERGY, HP, DRAW
    }
}
