using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERINSULT, PLAYERTURN, ENEMYINSULT, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public Transform AnswersParent;
	public GameObject ButtonAnswerPrefab;
	public GameObject ButtonInsultPrefab;

	public BattleState state;



	string[] Insults = new string[] { "¿Has dejado ya de usar pañales?"," ¡No hay palabras para describir lo asqueroso que eres!"," ¡He hablado con simios más educados que tu!","¡Llevarás mi espada como si fueras un pincho moruno!"
		,"¡Luchas como un ganadero!", "¡No pienso aguantar tu insolencia aquí sentado!", "¡Mi pañuelo limpiará tu sangre!", "¡Ha llegado tu HORA, palurdo de ocho patas!", "¡Una vez tuve un perro más listo que tu!", "¡Nadie me ha sacado sangre jamás, y nadie lo hará!"
		,"¡Me das ganas de vomitar!", "¡Tienes los modales de un mendigo!", "¡He oído que eres un soplón despreciable!", "¡La gente cae a mis pies al verme llegar!", "¡Demasiado bobo para mi nivel de inteligencia!", "Obtuve esta cicatriz en una batalla a muerte!"};
	string[] Answers = new string[] {"¿Por qué? ¿Acaso querías pedir uno prestado?", "Sí que las hay, sólo que nunca las has aprendido."," Me alegra que asistieras a tu reunión familiar diaria.", "Primero deberías dejar de usarla como un plumero."
		,"Qué apropiado, tú peleas como una vaca.","Ya te están fastidiando otra vez las almorranas, ¿Eh?", "Ah, ¿Ya has obtenido ese trabajo de barrendero?", "Y yo tengo un SALUDO para ti, ¿Te enteras?","Te habrá enseñado todo lo que sabes.","¿TAN rápido corres?"
		,"Me haces pensar que alguien ya lo ha hecho.", "Quería asegurarme de que estuvieras a gusto conmigo.","Qué pena me da que nadie haya oído hablar de ti","¿Incluso antes de que huelan tu aliento?","Estaría acabado si la usases alguna vez."
		,"Espero que ya hayas aprendido a no tocarte la nariz."};

	// Start is called before the first frame update
	void Start()
	{
		state = BattleState.START;
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "El combate va a empezar...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		int val = UnityEngine.Random.Range(0, 10);

		if (val <= 5)
        {
			state = BattleState.PLAYERINSULT;
			PlayerInsult();
		}
        else
        {
			state = BattleState.ENEMYINSULT;
			EnemyInsult();
		}


	}

	IEnumerator PlayerAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

		enemyHUD.SetHP(enemyUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			state = BattleState.PLAYERINSULT;
			PlayerInsult();
		}
	}

	void PlayerInsult()
    {
		foreach (Transform child in AnswersParent.transform)
		{
			Destroy(child.gameObject);
		}

		var isLeft = true;
		var height = 50.0f;
		var i = 0;
		foreach (var insult in Insults)
		{
			var buttonInsultCopy = Instantiate(ButtonInsultPrefab, AnswersParent, false);

			var x = buttonInsultCopy.GetComponent<RectTransform>().rect.x * 1.3f;
			buttonInsultCopy.GetComponent<RectTransform>().localPosition = new Vector3(isLeft ? x : -x, height, 0);

			if (!isLeft)
				height += buttonInsultCopy.GetComponent<RectTransform>().rect.y *1.7f;
			isLeft = !isLeft;

			FillListener2(buttonInsultCopy.GetComponent<Button>(), i);

			buttonInsultCopy.GetComponentInChildren<Text>().text = insult;

			i++;
		}

	}

	private void FillListener2(Button button, int i)
	{
		button.onClick.AddListener(() => { InsultSelected(i); });
	}

	private void InsultSelected(int i)
	{
		dialogueText.text = Insults[i];
		StartCoroutine(EnemyTurn(i));
	}

	void PlayerTurn(int index)
	{
		foreach (Transform child in AnswersParent.transform)
		{
			Destroy(child.gameObject);
		}

		var isLeft = true;
		var height = 50.0f;
		var i = 0;
		foreach (var answer in Answers)
		{
			var buttonAnswerCopy = Instantiate(ButtonAnswerPrefab, AnswersParent, false);

			var x = buttonAnswerCopy.GetComponent<RectTransform>().rect.x * 1.3f;
			buttonAnswerCopy.GetComponent<RectTransform>().localPosition = new Vector3(isLeft ? x : -x, height, 0);

			if (!isLeft)
				height += buttonAnswerCopy.GetComponent<RectTransform>().rect.y * 1.7f;
			isLeft = !isLeft;

			FillListener(buttonAnswerCopy.GetComponent<Button>(), i, index);

			buttonAnswerCopy.GetComponentInChildren<Text>().text = answer;

			i++;
		}
	}

	private void FillListener(Button button, int i, int index)
	{
		button.onClick.AddListener(() => { AnswerSelected(i, index); });
	}

	private void AnswerSelected(int i, int index)
	{
		dialogueText.text += "\n" + Answers[i];

		if (i == index)
		{
			StartCoroutine(PlayerAttack());
		}
		else
		{
			StartCoroutine(EnemyAttack());
		}
	}

	void EnemyInsult()
    {
		int index = UnityEngine.Random.Range(0, Insults.Length);
		dialogueText.text = Insults[index];

		PlayerTurn(index);
    }

	IEnumerator EnemyTurn(int i)
	{
		int index = UnityEngine.Random.Range(0, Answers.Length);

		yield return new WaitForSeconds(1f);


		dialogueText.text += "\n" + Answers[index];

		if (i == index)
		{
			StartCoroutine(EnemyAttack());
		}
		else
		{
			StartCoroutine(PlayerAttack());
		}

	}

	IEnumerator EnemyAttack()
    {
		bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if (isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		}
		else
		{
			state = BattleState.ENEMYINSULT;
			EnemyInsult();
		}
	}

	void EndBattle()
	{
		if (state == BattleState.WON)
		{
			SceneManager.LoadScene("Win", LoadSceneMode.Single);
		}
		else if (state == BattleState.LOST)
		{
			SceneManager.LoadScene("Lose", LoadSceneMode.Single);
		}
	}


}