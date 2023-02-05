import { Store, createStore, combineReducers } from 'redux';
import { questionsReducer } from './question/QuestionReducers';
import { QuestionsState } from './question/QuestionState';

export interface AppState {
  questions: QuestionsState;
}

const rootReducer = combineReducers<AppState>({ questions: questionsReducer });
export const configureStore = (): Store<AppState> => {
  return createStore(rootReducer, undefined);
};
