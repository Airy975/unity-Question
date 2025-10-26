# unity-Question
在unity中实现一个简单的问答游戏

实现这个功能的核心是Question脚本，它实现了文件读取与 UI 交互的问答功能模块。系统通过从本地文本文件中加载问题与答案，动态显示在界面上，玩家（或用户）可以输入回答并实时获得反馈与得分统计。

## 数据结构：QuestionData
```csharp
public class QuestionData
{
    public string question;
    public string answer;
}
```
这是一个简单的容器类，用于存储每道题目的内容与正确答案。

程序运行时，所有题目都会被读取并存入一个 List<QuestionData> 中，方便后续遍历。

## 系统初始化流程
在游戏启动时执行以下代码：
```csharp
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
```
在这串代码中将实现：

1.UI组件检查

确保所有文本对象（问题、答案、提示、得分）均已在 Inspector 中绑定。若有遗漏，会在控制台输出错误提示。

2.文件路径准备
在项目的 Assets/Question 目录下查找题库文件夹，若不存在则自动创建。

3.加载题库
调用 LoadQuestion() 方法从指定路径读取文本题库。

## 题库加载逻辑
```csharp
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
            questionText.text = questions[n].question;
        else
            errorText.text = "没有问题加载";
    }
    else
    {
        errorText.text = "文件不存在";
    }
}
```
在这串代码中必须创建一个名字为test.txt的文件，并且格式必须是

```
问题内容,正确答案
```

例如

```
今有雉（鸡）兔同笼，上有三十五头，下有九十四足。问兔各几何？,23

今有共买物，人出八，盈三；人出七，不足四。问物价各几何？,53
```

在这段代码中将逐行读取文本内容；使用逗号（,）分隔问题与答案；自动创建 QuestionData 实例并添加至列表；若成功加载至少一条题目，则显示第一道题。

## 答案检查与评分机制
```csharp
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
        answerText.text = "";
    }
    else
    {
        questionText.text = "所有问题回答完毕";
        scoreText.text = "你的分数是: " + score + " / " + questions.Count;
        answerText.gameObject.SetActive(false);
    }
}
```
这段代码的逻辑流程为：

对比用户输入的文本与正确答案；

若正确，输出提示“回答正确”，并增加分数；

若错误，显示“回答错误”；

自动切换到下一道题；

当所有题目完成后：

显示总分；

隐藏输入框，防止继续答题。