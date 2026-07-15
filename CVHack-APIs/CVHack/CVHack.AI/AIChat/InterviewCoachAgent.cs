using Microsoft.Extensions.AI;

namespace CVHack.AI.AIChat;

// Worker 5 — Interview Coach
// بيدي مقابلة تدريبية شخصية: سؤال واحد في كل مرة، يستنى إجابة، يدي فيدباك صريح ومحدد، وينتقل للي بعده
public class InterviewCoachAgent
{
    private readonly IChatClient _chat;
    private readonly InterviewQuestionBank _questionBank;
    private readonly InterviewSessionStore _sessions;

    private const int DefaultTotalQuestions = 6;

    public InterviewCoachAgent(IChatClient chat, InterviewQuestionBank questionBank, InterviewSessionStore sessions)
    {
        _chat = chat;
        _questionBank = questionBank;
        _sessions = sessions;
    }

    public async Task<InterviewTurnResult> StartAsync(InterviewStartRequest request, CancellationToken ct = default)
    {
        var context = await _questionBank.GetContextAsync(
            request.JobTitle, request.Seniority, "common interview questions for this role", topK: 12, ct: ct);

        var session = new InterviewSession
        {
            JobTitle = request.JobTitle,
            Seniority = request.Seniority,
            CompanyName = request.CompanyName,
            Context = context,
            TotalQuestions = DefaultTotalQuestions
        };

        var prompt = $"""
            You are a professional, friendly interview coach running a mock interview.

            ROLE: {request.JobTitle} (Seniority: {request.Seniority})
            COMPANY: {(string.IsNullOrWhiteSpace(request.CompanyName) ? "(not specified)" : request.CompanyName)}

            Write a short, warm greeting (1-2 sentences) welcoming the candidate to their mock interview for this role,
            then ask the FIRST interview question.

            Rules:
            - Pick the first question ONLY from the REAL QUESTION LIBRARY below — never invent a new one.
            - Prefer a good opening/fundamentals question, not the hardest one.
            - Ask ONE question only. You may lightly rephrase it for natural flow, but keep its original meaning.
            - Never include question numbers or labels (like "Q12." or "Question 5:") from the library in your output — only the natural question text itself.

            REAL QUESTION LIBRARY:
            {context}
            """;

        var ai = (await _chat.GetResponseAsync<InterviewOpeningAi>(prompt, useJsonSchemaResponseFormat: false)).Result;

        session.AskedQuestions.Add(ai.FirstQuestion);
        _sessions.Save(session);

        return new InterviewTurnResult
        {
            SessionId = session.Id,
            Feedback = null,
            Question = $"{ai.Greeting}\n\n{ai.FirstQuestion}",
            QuestionNumber = 1,
            TotalQuestions = session.TotalQuestions,
            IsComplete = false
        };
    }

    public async Task<InterviewTurnResult> AnswerAsync(InterviewAnswerRequest request, CancellationToken ct = default)
    {
        var session = _sessions.Get(request.SessionId)
            ?? throw new InvalidOperationException("Interview session not found or has expired.");

        session.Answers.Add(request.Answer);

        var isLastQuestion = session.AskedQuestions.Count >= session.TotalQuestions;
        var lastQuestion = session.AskedQuestions[^1];
        var askedSoFar = string.Join("\n", session.AskedQuestions.Select((q, i) => $"{i + 1}. {q}"));

        if (isLastQuestion)
        {
            var closingPrompt = $"""
                You are a professional, honest interview coach. This is the LAST question of the mock interview.

                ROLE: {session.JobTitle} (Seniority: {session.Seniority})
                QUESTION ASKED: {lastQuestion}
                CANDIDATE'S ANSWER: {request.Answer}

                CRITICAL RULES FOR FEEDBACK:
                - If the candidate's answer is empty, irrelevant, or clearly wrong (e.g. "hello", "I don't know", random text),
                  you MUST say so directly. Do NOT praise or call it a good answer.
                - If the answer is partially correct, point out exactly what is right and what is missing.
                - If the answer is strong and accurate, then you may praise it — but be specific about why.
                - NEVER give generic positive feedback unless the answer is genuinely correct and detailed.
                - Always verify the answer against your knowledge of {session.JobTitle} best practices.

                1. Give honest, specific feedback on this answer (2-4 sentences).
                   If the answer is empty, irrelevant, or wrong, say so clearly and explain what a correct answer should include.
                2. Write a short closing message (2-3 sentences) wrapping up the whole mock interview, encouraging,
                   and wishing them luck.
                """;

            var closingAi = (await _chat.GetResponseAsync<InterviewClosingAi>(closingPrompt, useJsonSchemaResponseFormat: false)).Result;

            return new InterviewTurnResult
            {
                SessionId = session.Id,
                Feedback = closingAi.Feedback,
                Question = null,
                QuestionNumber = session.CurrentQuestionNumber,
                TotalQuestions = session.TotalQuestions,
                IsComplete = true,
                ClosingMessage = closingAi.ClosingMessage
            };
        }

        var prompt = $"""
            You are a professional, friendly and honest interview coach running a one-on-one mock interview.

            ROLE: {session.JobTitle} (Seniority: {session.Seniority})

            QUESTION ASKED: {lastQuestion}
            CANDIDATE'S ANSWER: {request.Answer}

            QUESTIONS ALREADY ASKED (never repeat any of these):
            {askedSoFar}

            REAL QUESTION LIBRARY (pick the next question ONLY from here):
            {session.Context}

            STEP 1 — CLASSIFY THE ANSWER:
            First, decide what type of answer this is:
            - GREETING: if the candidate said something like "hello", "hi", "ok", "ready", "let's go" — not a technical answer
            - IRRELEVANT: if the answer has nothing to do with the question
            - WRONG: if the answer is technically incorrect based on your knowledge and the question context
            - PARTIAL: if the answer is on the right track but missing key details
            - CORRECT: if the answer is accurate and sufficiently detailed

            STEP 2 — RESPOND BASED ON TYPE:
            - If GREETING: respond naturally and warmly (e.g. "Great, let's get started!"), then repeat the SAME question. Do NOT move to next question.
            - If IRRELEVANT: politely note it's not relevant and ask them to try answering the question again. Do NOT move to next question.
            - If WRONG: clearly explain what's incorrect and what the right answer should cover. Then move to next question.
            - If PARTIAL: acknowledge what's right, explain what's missing, suggest improvement. Then move to next question.
            - If CORRECT: give specific praise explaining why it's good. Then move to next question.

            IMPORTANT:
            - For GREETING and IRRELEVANT cases, set NextQuestion to the SAME current question (do not pick a new one).
            - For WRONG, PARTIAL, CORRECT cases, pick the NEXT question from the REAL QUESTION LIBRARY.
            - Never repeat a question already asked.
            - Always verify technical answers against your knowledge of {session.JobTitle} best practices.
            - Never include question numbers or labels (like "Q12." or "Question 5:") from the library in NextQuestion — only the natural question text itself.
            """;

        var ai = (await _chat.GetResponseAsync<InterviewTurnAi>(prompt, useJsonSchemaResponseFormat: false)).Result;

        // لو الإجابة كانت greeting أو irrelevant، نكرر نفس السؤال ومنضيفش سؤال جديد
        var isNonAnswer = string.Equals(ai.NextQuestion?.Trim(), lastQuestion?.Trim(), StringComparison.OrdinalIgnoreCase)
                          || ai.NextQuestion == lastQuestion;
        if (!isNonAnswer)
        {
            session.AskedQuestions.Add(ai.NextQuestion);
        }
        _sessions.Save(session);

        return new InterviewTurnResult
        {
            SessionId = session.Id,
            Feedback = ai.Feedback,
            Question = isNonAnswer ? lastQuestion : ai.NextQuestion,
            QuestionNumber = session.CurrentQuestionNumber,
            TotalQuestions = session.TotalQuestions,
            IsComplete = false
        };
    }
}