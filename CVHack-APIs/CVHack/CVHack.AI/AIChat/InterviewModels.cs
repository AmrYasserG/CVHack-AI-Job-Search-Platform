namespace CVHack.AI.AIChat;

// =====================================================================
// شكل الطلب والرد اللي بيتبادلهم الـ Frontend مع الـ Agent
// =====================================================================

public class InterviewStartRequest
{
    public string JobTitle { get; set; } = string.Empty;
    public string Seniority { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}

public class InterviewAnswerRequest
{
    public Guid SessionId { get; set; }
    public string Answer { get; set; } = string.Empty;
}

public class InterviewTurnResult
{
    public Guid SessionId { get; set; }
    public string? Feedback { get; set; }       // فيدباك على إجابة المستخدم (null لأول سؤال بس)
    public string? Question { get; set; }       // السؤال الجاي (null لو المقابلة خلصت)
    public int QuestionNumber { get; set; }
    public int TotalQuestions { get; set; }
    public bool IsComplete { get; set; }
    public string? ClosingMessage { get; set; } // بيتملي لما المقابلة تخلص
}

// =====================================================================
// حالة المقابلة — متخزنة في الذاكرة طول ما الجلسة شغالة (مفيش داتابيز)
// =====================================================================

public class InterviewSession
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string JobTitle { get; init; } = string.Empty;
    public string Seniority { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string Context { get; init; } = string.Empty;   // النصوص اللي جابها الـ RAG من بنك الأسئلة

    public List<string> AskedQuestions { get; init; } = new();
    public List<string> Answers { get; init; } = new();

    public int TotalQuestions { get; init; } = 6;
    public int CurrentQuestionNumber => AskedQuestions.Count;
}

// =====================================================================
// الشكل اللي الـ LLM بيرجعه (داخلي بس — مش بيوصل للفرونت إند زي ما هو)
// =====================================================================

internal class InterviewOpeningAi
{
    public string Greeting { get; set; } = string.Empty;
    public string FirstQuestion { get; set; } = string.Empty;
}

internal class InterviewTurnAi
{
    public string Feedback { get; set; } = string.Empty;
    public string NextQuestion { get; set; } = string.Empty;
}

internal class InterviewClosingAi
{
    public string Feedback { get; set; } = string.Empty;
    public string ClosingMessage { get; set; } = string.Empty;
}
