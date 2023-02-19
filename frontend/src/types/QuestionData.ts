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
}

export const questions: QuestionData[] = [
  {
    questionId: 1,
    title: 'Why should I learn TypeScript?',
    content:
      'TypeScript seems to be getting popular so I wondered whether it is worth my time learning it? What benefits does it give over JavaScript?',
    userName: 'Bob',
    created: new Date(),
    answers: [
      {
        answerId: 1,
        content: 'To catch problems earlier speeding up your developments',
        userName: 'Jane',
        created: new Date(),
      },
      {
        answerId: 2,
        content:
          'So, that you can use the JavaScript features of tomorrow, today',
        userName: 'Fred',
        created: new Date(),
      },
    ],
  },
  {
    questionId: 2,
    title: 'Cosmos Changes',
    content: 'Will Cosmos Changes take place in next 2 millenium?',
    userName: 'Bob',
    created: new Date('2022-12-13'),
    answers: [
      {
        answerId: 1,
        content: 'To catch problems earlier speeding up your developments',
        userName: 'Jane',
        created: new Date(),
      },
      {
        answerId: 2,
        content:
          'So, that you can use the JavaScript features of tomorrow, today',
        userName: 'Fred',
        created: new Date(),
      },
    ],
  },
  {
    questionId: 3,
    title: 'Which state management tool should I use?',
    content:
      'There seem to be a fair few state management tools around for React - React, Unstated, ... Which one should I use?',
    userName: 'Bob',
    created: new Date(),
    answers: [],
  },
];

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
  criteria: string
): Promise<QuestionData[]> => {
  let questions: QuestionData[] | undefined;
  const response = await httpCall<QuestionData[]>({
    path: `question?search=${criteria}`,
    token: '',
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
  return question || null;
};

export const postAnswer = async (
  data: AnswerData,
  questionId: number | undefined
) => {
  await wait(1000);
  if (questionId === undefined) return false;
  const question = questions
    .filter((question) => question.questionId === questionId)
    .at(0);
  if (question === undefined) return false;

  data.answerId =
    question.answers.length === 0
      ? 1
      : Math.max(...question.answers.map((answer) => answer.answerId)) + 1;
  question?.answers.push(data);
  return question;
};
export const postQuestion = async (
  data: QuestionData
): Promise<QuestionData | undefined> => {
  await wait(1000);
  const questionId =
    Math.max(...questions.map((question) => question.questionId)) + 1;
  const newQuestion: QuestionData = {
    ...data,
    questionId: questionId,
    answers: [],
  };
  questions.push(newQuestion);
  return newQuestion;
};
export const wait = (ms: number): Promise<void> => {
  return new Promise((resolve, reject) => setTimeout(resolve, ms));
};
