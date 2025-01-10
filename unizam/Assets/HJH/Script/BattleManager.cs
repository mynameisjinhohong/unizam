using UnityEngine;

public enum BattleState
{
    StartDice,
    EndDice,
    ChooseEnemy,
    UseSkill,
    EnemyTurn,
    EnemyTurnEnd,
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    BattleState state;

    public BattleState State
    {
        get { return state; }
        set
        {
            switch (value)
            {
                case BattleState.StartDice:
                    break;

                case BattleState.EndDice:
                    break;

                case BattleState.ChooseEnemy:
                    break;

                case BattleState.UseSkill:
                    break;

                case BattleState.EnemyTurn:
                    break;
                case BattleState.EnemyTurnEnd:
                    break;

            }

        }
    }

    public GameObject enemyPrefab;
    public BattlePlayer player;

    private void Start()
    {
        state = BattleState.StartDice;
        player.player = GameManager.Instance.player;
        for(int i = 0; i<GameManager.Instance.enemies.Count; i++)
        {

        }

    }


}
