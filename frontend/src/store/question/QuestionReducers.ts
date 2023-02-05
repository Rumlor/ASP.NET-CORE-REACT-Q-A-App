import * as questionActions from './QuestionActions';
import { QuestionActions } from './QuestionActions';
import { initialState, QuestionsState } from './QuestionState';

export const questionsReducer = (
  state = initialState,
  action: QuestionActions
): QuestionsState => {
  switch (action.type) {
    case questionActions.GETTINGUNANSWEREDQUESTIONS: {
      return { ...state, loading: true };
    }
    case questionActions.GOTUNANSWEREDQUESTIONS: {
      return { ...state, unAnswered: action.questions, loading: false };
    }
    case questionActions.GETTINGQUESTION: {
      return { ...state, viewing: null, loading: true };
    }
    case questionActions.GOTQUESTION: {
      return { ...state, viewing: action.question, loading: false };
    }
    case questionActions.SEARCHINGQUESTIONS: {
      return { ...state, searched: [], loading: true };
    }
    case questionActions.SEARCHEDQUESTIONS: {
      return { ...state, searched: action.questions, loading: false };
    }
    case questionActions.ASKINGINGQUESTION: {
      return { ...state, loading: true, posted: false };
    }
    case questionActions.ASKEDQUESTION: {
      return { ...state, loading: false, posted: true };
    }
    default:
      return state;
  }
};
