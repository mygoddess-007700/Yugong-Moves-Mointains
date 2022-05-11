using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int number = 1;
    public int score = 100;
    
    [Header("Reference")]
    public TextAsset Questions;
    public TextAsset Options;

    public TMP_Text numT;
    public TMP_Text scoreT;
    public TMP_Text questionT;
    
    public Image rightI1;
    public Image errorI1;
    public Image rightI2;
    public Image errorI2;
    public Image rightI3;
    public Image errorI3;

    public Image answer1;
    public Image answer2;
    public Image answer3;

    public AudioSource rightA;
    public AudioSource errorA;

    [Header("问题")]
    public string [] questions;
    [Header("选项答案")]
    public string [] options;
    [Header("答案")]
    public string [] answers;
    [Header("题目数量")]
    public int maxNum = 10;
    [Header("题目分数")]
    public int shootRightScore = 10;
    [Header("是否处于回答结束状态")]
    public bool isAnswerDone = false;
    
    void Awake()
    {
        instance = this;

        questions = Questions.text.Split('\n');
        options = Options.text.Split(' ');
        maxNum = questions.Length;
    }

    void Start()
    {
        Color tColor = rightI1.color;
        tColor.a = 0;
        rightI1.color = tColor;
        errorI1.color = tColor;

        tColor = rightI2.color;
        tColor.a = 0;
        rightI2.color = tColor;
        errorI2.color = tColor;

        tColor = rightI3.color;
        tColor.a = 0;
        rightI3.color = tColor;
        errorI3.color = tColor;
        
        questionT.text = questions[0];
    }

    void Update()
    {
        numT.text = "number: " + number.ToString();
        scoreT.text = "score: " + score.ToString();    
    }

    public void ShootRight(int index)
    {
        if (isAnswerDone)
        {
            return;
        }
        GameController.instance.isAnswerDone = true;
        switch (index)
        {
            case 1:
                StartCoroutine(FadeIn(rightI1));
                break;
            case 2:
                StartCoroutine(FadeIn(rightI2));
                break;
            case 3:
                StartCoroutine(FadeIn(rightI3));
                break;
        }

        StartCoroutine(ShowAnswer());

        //播放音效
        rightA.Play();

        score += shootRightScore;
        scoreT.text = "score: " + score.ToString();
        StaticData.correct++;

        StartCoroutine(NextQuestion(number));
        StartCoroutine(NextAnswers(number));

        if (number < maxNum)
        {
            StartCoroutine(AddNumber());
        }
    }

    public void ShootError(int rightIndex, int errorIndex)
    {
        if (isAnswerDone)
        {
            return;
        }
        GameController.instance.isAnswerDone = true;
        switch (rightIndex)
        {
            case 1:
                StartCoroutine(FadeIn(rightI1));
                if (errorIndex == 1)
                {
                    StartCoroutine(FadeIn(errorI1));
                }
                else if (errorIndex == 2)
                {
                    StartCoroutine(FadeIn(errorI2));
                }
                else if (errorIndex == 3)
                {
                    StartCoroutine(FadeIn(errorI3));
                }
                break;
            case 2:
                StartCoroutine(FadeIn(rightI2));
                if (errorIndex == 1)
                {
                    StartCoroutine(FadeIn(errorI1));
                }
                else if (errorIndex == 2)
                {
                    StartCoroutine(FadeIn(errorI2));
                }
                else if (errorIndex == 3)
                {
                    StartCoroutine(FadeIn(errorI3));
                }
                break;
            case 3:
                StartCoroutine(FadeIn(rightI3));
                if (errorIndex == 1)
                {
                    StartCoroutine(FadeIn(errorI1));
                }
                else if (errorIndex == 2)
                {
                    StartCoroutine(FadeIn(errorI2));
                }
                else if (errorIndex == 3)
                {
                    StartCoroutine(FadeIn(errorI3));
                }
                break;
        }

        StartCoroutine(ShowAnswer());

        //播放音效
        errorA.Play();

        scoreT.text = "score: " + score.ToString();
        StaticData.error++;

        StartCoroutine(NextQuestion(number));
        StartCoroutine(NextAnswers(number));

        if (number < maxNum)
        {
            StartCoroutine(AddNumber());
        }
    }

    public IEnumerator NextQuestion(int num)
    {
        if (num >= maxNum)
        {
            FadeIntoNextScene.instance.LoadNextScene();
        }
        yield return new WaitForSeconds(1f);

        questionT.text = questions[num];
    }

    public IEnumerator NextAnswers(int num)
    {
        if (num >= maxNum)
        {
            FadeIntoNextScene.instance.LoadNextScene();
        }

        float fadeDuration = 0.7f;
        float fadeDone = Time.time + fadeDuration;
        Color tColor = answer1.color;
        while (Time.time < fadeDone)
        {
            tColor.a = (fadeDone - Time.time) / fadeDuration;
            answer1.color = tColor;
            answer2.color = tColor;
            answer3.color = tColor;

            yield return null;
        }

        tColor.a = 0;
        answer1.color = tColor;
        answer2.color = tColor;
        answer3.color = tColor;

        yield return new WaitForSeconds(0.4f);

        float fadeOutDuration = 0.8f;
        float fadeOutDone = Time.time + fadeOutDuration;
        while (Time.time < fadeOutDone)
        {
            tColor.a = 1 - (fadeOutDone - Time.time) / fadeOutDone;
            answer1.color = tColor;
            answer2.color = tColor;
            answer3.color = tColor;
            yield return null;
        }
        tColor.a = 1;
        answer1.color = tColor;
        answer2.color = tColor;
        answer3.color = tColor;
    }

    public IEnumerator FadeIn(Image i)
    {
        Color tColor = i.color;
        float fadeOutDuration = 0.8f;
        float fadeOutDone = Time.time + fadeOutDuration;
        while (Time.time < fadeOutDone)
        {
            tColor.a = 1 - (fadeOutDone - Time.time) / fadeOutDone;
            i.color = tColor;
            yield return null;
        }
        tColor.a = 1;
        i.color = tColor;

        StartCoroutine(FadeOut(i));
    }

    public IEnumerator FadeOut(Image i)
    {
        Color tColor = i.color;
        float fadeOutDuration = 1.2f;
        float fadeOutDone = Time.time + fadeOutDuration;
        while (Time.time < fadeOutDone)
        {
            tColor.a = (fadeOutDone - Time.time) / fadeOutDone;
            i.color = tColor;
            yield return null;
        }
        tColor.a = 0;
        i.color = tColor;
    }

    public IEnumerator ShowAnswer()
    {
        float fadeDuration = 0.7f;
        float fadeDone = Time.time + fadeDuration;
        yield return null;
    }

    public IEnumerator AddNumber()
    {
        yield return new WaitForSeconds(1f);

        number++;
        isAnswerDone = false;
    }
}