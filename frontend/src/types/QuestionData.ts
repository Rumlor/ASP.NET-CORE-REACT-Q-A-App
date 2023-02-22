import { HttpApiRequest, httpCall } from '../http/Http';
export interface QuestionData {
  questionId: number;
  title: string;
  content: string;
  userName: string;
  created: Date;
  answers: AnswerData[];
}
export interface AnswerData {
  answerId: number;
  content: string;
  userName: string;
  created: Date;
  questionId: number;
}

export const getAllAnsweredQuestions = async (
  token: string
): Promise<QuestionData[]> => {
  let unAnsweredQuestions: QuestionData[] = [];
  const apiRequest: HttpApiRequest = {
    path: 'question/unanswered',
    token: token,
  };
  const response = await httpCall<QuestionData[]>(apiRequest);

  if (response.ok && response.body) {
    unAnsweredQuestions = response.body.map((item) => {
      item.created = new Date(item.created);
      return item;
    });
    unAnsweredQuestions = response.body;
  }

  return unAnsweredQuestions;
};

export const searchQuestions = async (
  criteria: string,
  token: string
): Promise<QuestionData[]> => {
  let questions: QuestionData[] | undefined;
  const response = await httpCall<QuestionData[]>({
    path: `question?search=${criteria}`,
    token: token,
  });
  if (response && response.ok && response.body) questions = response.body;
  if (questions && questions.length > 0)
    questions.map((question) => {
      question.created = new Date(question.created);
      return question;
    });
  return questions || [];
};

export const getQuestionWithId = async (
  id: number,
  token: string
): Promise<QuestionData | null> => {
  let question: QuestionData | undefined;
  const response = await httpCall<QuestionData>({
    path: `question/${id}`,
    token: token,
  });
  if (response && response.ok && response.body) question = response.body;
  if (question) question.created = new Date(question.created);
  question?.answers.forEach(
    (answer) => (answer.created = new Date(answer.created))
  );
  return question || null;
};

export const postAnswer = async (
  data: AnswerData,
  questionId: number | undefined,
  token: string
) => {
  if (questionId === undefined) return false;
  const question = await getQuestionWithId(questionId, token);
  if (!question) return false;

  data.answerId =
    question.answers.length === 0
      ? 1
      : Math.max(...question.answers.map((answer) => answer.answerId)) + 1;
  httpCall<AnswerData, AnswerData>({
    token: token,
    payload: data,
    method: 'post',
    path: 'answer',
  });
  return question;
};
export const postQuestion = async (
  data: QuestionData,
  token: string
): Promise<QuestionData | undefined> => {
  await wait(1000);

  const newQuestion: QuestionData = {
    ...data,
    questionId: 0,
    answers: [],
  };
  httpCall<QuestionData, QuestionData>({
    token: token,
    payload: newQuestion,
    method: 'post',
    path: 'question',
  });
  return newQuestion;
};
export const wait = (ms: number): Promise<void> => {
  return new Promise((resolve, reject) => setTimeout(resolve, ms));
};
