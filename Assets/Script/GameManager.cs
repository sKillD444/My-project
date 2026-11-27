using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public int money = 100;
    public int currentMoney;
    public TextMeshProUGUI moneyText;
    public GameObject btnStart;
    public EnemySpawn enemySpawner;

    public int HP = 3;
    public int currentHP;
    public TextMeshProUGUI hpText;
    public GameObject gameOverUI;

    private bool isOver = false;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        currentMoney = money;
        currentHP = HP;

        UpdateHPUI();
        UpdateMoneyUI();

        if(gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (enemySpawner != null)
        {
            enemySpawner.StartWaves();
        }
        if(btnStart != null)
        {
            btnStart.SetActive(false);
        }
    }

    public bool spendMoney(int amount)
    {
        if(currentMoney < amount)
        {
           return false;
        }
        currentMoney -= amount;
        UpdateMoneyUI();
        return true;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public void TakeDamage(int amount)
    {
        if (isOver) return;

        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        UpdateHPUI();

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    private void UpdateMoneyUI()
    {
       if(moneyText != null)
        {
            moneyText.text = currentMoney.ToString();
        }
    }

    private void GameOver()
    {
        isOver = true;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
        {
            hpText.text = currentHP.ToString();
        }
    }
}
