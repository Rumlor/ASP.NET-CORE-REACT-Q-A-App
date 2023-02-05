import { QuestionData } from '../../types/QuestionData';

export type QuestionActions =
  | ReturnType<typeof gettingUnAnsweredQuestions>
  | ReturnType<typeof gotUnAnsweredQuestions>
  | ReturnType<typeof gettingQuestion>
  | ReturnType<typeof gotQuestion>
  | ReturnType<typeof searchingQuestions>
  | ReturnType<typeof searchedQuestions>
  | ReturnType<typeof askingQuestion>
  | ReturnType<typeof askedQuestion>;

//ACTIONS
export const GETTINGUNANSWEREDQUESTIONS = 'GettingUnAnsweredQuestions';
export const GOTUNANSWEREDQUESTIONS = 'GotUnAnsweredQuestions';

export const GETTINGQUESTION = 'GettingQuestion';
export const GOTQUESTION = 'GotQuestion';

export const SEARCHINGQUESTIONS = 'SearchingQuestions';
export const SEARCHEDQUESTIONS = 'SearchedQuestions';

export const ASKINGINGQUESTION = 'AskingQuestion';
export const ASKEDQUESTION = 'AskedQuestion';

export const gettingUnAnsweredQuestions = () =>
  ({ type: GETTINGUNANSWEREDQUESTIONS } as const);

export const gotUnAnsweredQuestions = (questions: QuestionData[]) =>
  ({ type: GOTUNANSWEREDQUESTIONS, questions: questions } as const);

export const gettingQuestion = () => ({ type: GETTINGQUESTION } as const);

export const gotQuestion = (question: QuestionData | null) =>
  ({ type: GOTQUESTION, question: question } as const);

export const searchingQuestions = () => ({ type: SEARCHINGQUESTIONS } as const);

export const searchedQuestions = (questions: QuestionData[]) =>
  ({ type: SEARCHEDQUESTIONS, questions: questions } as const);

export const askingQuestion = () => ({ type: ASKINGINGQUESTION } as const);
export const askedQuestion = () => ({ type: ASKEDQUESTION } as const);
