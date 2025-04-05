using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillSetting
{
    public Image skillGauge;
    public bool isSkillUseable;
    public float delayTime;
    public float curDelayTime;
}

public class PlayerSkillCtrl : MonoBehaviour
{
    //Managers
    GameManager _gameManager;
    GameObject _barrier;
    public SkillSetting[] skillSettings;
    Rigidbody2D _rigid;
    BoxCollider2D _boxCollider;
    SpriteRenderer _spriteRenderer;

	public void Init()
	{
        _gameManager = GameManager.Instance;
        _barrier = transform.Find("Barrier").gameObject;
        _rigid = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

	void Update()
    {
		for (int i = 0; i < skillSettings.Length; i++)
		{
            if (skillSettings[i].isSkillUseable == false)
            {
                skillSettings[i].curDelayTime += Time.deltaTime;
                skillSettings[i].skillGauge.fillAmount = Mathf.Lerp(1, 0, skillSettings[i].curDelayTime/skillSettings[i].delayTime);
                if (skillSettings[i].delayTime < skillSettings[i].curDelayTime)
                {
                    skillSettings[i].isSkillUseable = true;
                    skillSettings[i].curDelayTime = 0;
                }
            }
        }

		if (!_gameManager.IsGameOver)
		{
            if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.J)) && skillSettings[0].isSkillUseable == true)
            {
                StartCoroutine(PlayerSkill_0());
                skillSettings[0].isSkillUseable = false;
            }
            if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.K)) && skillSettings[1].isSkillUseable == true)
            {
                PoolManager.Instance._objectManager.Pop("PlayerPowerBullet", transform.position + new Vector3(1.8f, 0));
                skillSettings[1].isSkillUseable = false;
            }
            if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.L)) && skillSettings[2].isSkillUseable == true)
            {
                StartCoroutine(PlayerSkill_2());
                skillSettings[2].isSkillUseable = false;
            }
            if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Semicolon)) && skillSettings[3].isSkillUseable == true)
            {
                StopCoroutine("PlayerSkill_3");
                StartCoroutine("PlayerSkill_3");
                skillSettings[3].isSkillUseable = false;
            }
        }
    }

    IEnumerator PlayerSkill_0()
	{
        _barrier.SetActive(true);
        _gameManager.islessdamagedMode = true;
        yield return new WaitForSeconds(10);
        _gameManager.islessdamagedMode = false;
        _barrier.SetActive(false);
    }

    IEnumerator PlayerSkill_2()
	{
        _spriteRenderer.color = new Color(1, 1, 1, 0.1f);
        _boxCollider.enabled = false;
        transform.position += new Vector3(4, 0);
        yield return new WaitForSeconds(4);
        _boxCollider.enabled = true;
        _spriteRenderer.color = new Color(1, 1, 1, 1);
        yield return null;
    }

    IEnumerator PlayerSkill_3()
	{
        _gameManager.isPowerUp = true;
        yield return new WaitForSeconds(10);
        _gameManager.isPowerUp = false;
	}
}
