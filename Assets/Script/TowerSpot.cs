using UnityEngine;
using UnityEngine.InputSystem;
public class TowerSpot : MonoBehaviour
{
    public GameObject selectMenu;
    public GameObject[] towerPrefabs;

    private GameObject currentTower;
    private SpriteRenderer circleRenderer;
    public Vector3 towerOffset = Vector3.zero;

    private void Start()
    {
        circleRenderer = GetComponent<SpriteRenderer>();
        if (selectMenu != null) selectMenu.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null)
            {
                TowerOptionButton optionBtn = hit.GetComponent<TowerOptionButton>();
                if (optionBtn != null && optionBtn.parentSpot == this)
                {
                    optionBtn.SelectThisTower();
                    return;
                }

                if (hit.gameObject == gameObject && currentTower == null)
                {
                    selectMenu.SetActive(!selectMenu.activeSelf);
                    return;
                }
            }

            if (selectMenu.activeSelf)
            {
                selectMenu.SetActive(false);
            }
        }
    }

    public void BuildTower(int index)
    {
        if (currentTower != null || index < 0 || index >= towerPrefabs.Length) return;

        Tower tower = towerPrefabs[index].GetComponent<Tower>();
        if (tower != null && GameManager.instance!=null) 
        { 
            if(!GameManager.instance.spendMoney(tower.cost))
            {
                return;
            }
        }
        Vector3 spawnPosition = transform.position+ towerOffset;
        currentTower = Instantiate(towerPrefabs[index],spawnPosition, Quaternion.identity);
        currentTower.transform.localScale= 0.7f * Vector3.one;

        Tower towerComp = currentTower.GetComponent<Tower>();
        if (towerComp != null)
        {
            towerComp.isBuilt = true;
        }

        gameObject.SetActive(false);
        selectMenu.SetActive(false);
    }
}
