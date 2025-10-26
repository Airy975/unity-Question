using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class QuestionData
{
    public string question;
    public string answer;
}

public class Question : MonoBehaviour
{
    public List<QuestionData> questions = new List<QuestionData>();

    string filePath;

    public Text answerText; // 用于显示用户输入答案的UI文本
    public Text questionText; // 用于显示问题的UI文本
    public Text errorText; // 用于显示错误信息的UI文本
    public Text scoreText; // 用于显示分数的UI文本

    int n = 0; // 当前问题的索引
    int score = 0; // 用户的分数

    // 在游戏开始时调用
    void Start()
    {
        // 检查UI组件是否已关联
        if (answerText == null || questionText == null || errorText == null || scoreText == null)
        {
            Debug.LogError("请确保所有UI组件都已在Inspector中正确关联。");
            return;
        }

        filePath = Path.Combine(Application.dataPath, "Question");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        LoadQuestion(filePath);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // 从文件中加载问题
    void LoadQuestion(string filePath)
    {
        filePath = Path.Combine(filePath, "test.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length != 2) continue;

                QuestionData q = new QuestionData
                {
                    question = parts[0].Trim(),
                    answer = parts[1].Trim()
                };

                questions.Add(q);
            }

            if (questions.Count > 0)
            {
                questionText.text = questions[n].question;
            }
            else
            {
                errorText.text = "没有问题加载";
            }
        }
        else
        {
            errorText.text = "文件不存在";
        }
    }

    // 检查答案是否正确
    public void Check()
    {
        if (answerText.text == questions[n].answer)
        {
            errorText.text = "回答正确";
            score++;
        }
        else
        {
            errorText.text = "回答错误";
        }

        n++;

        if (n < questions.Count)
        {
            questionText.text = questions[n].question;
            answerText.text = ""; // 清空答案输入框
        }
        else
        {
            questionText.text = "所有问题回答完毕";
            scoreText.text = "你的分数是: " + score + " / " + questions.Count;
            answerText.gameObject.SetActive(false); // 隐藏答案输入框
        }
    }

    // 加载指定场景
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
