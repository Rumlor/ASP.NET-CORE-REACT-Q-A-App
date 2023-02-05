import { QuestionData } from '../../types/QuestionData';

export interface QuestionsState {
  readonly loading: boolean; // => A flag that show current state if a server request is being made
  readonly posted: boolean; //=> A flag that shows currenct state if a server request is succesfully made and questions has been posted
  readonly unAnswered: QuestionData[]; //=> An array containing unanswered questions
  readonly viewing: QuestionData | null; //=> The question the user is viewing
  readonly searched: QuestionData[]; //=>An array containing questions matched in the search
}
export const initialState: QuestionsState = {
  loading: false,
  posted: false,
  unAnswered: [],
  viewing: null,
  searched: [],
};
