// شكل الطلب اللي بيتبعت لما نبدأ مقابلة جديدة
export interface InterviewStartRequest {
  jobTitle: string;
  seniority: string;
  companyName: string;
}

// شكل الطلب اللي بيتبعت مع كل إجابة
export interface InterviewAnswerRequest {
  sessionId: string;
  answer: string;
}

// شكل الرد اللي بيرجع من الباك في كل دور (بداية أو إجابة)
export interface InterviewTurnResult {
  sessionId: string;
  feedback: string | null;
  question: string | null;
  questionNumber: number;
  totalQuestions: number;
  isComplete: boolean;
  closingMessage: string | null;
}