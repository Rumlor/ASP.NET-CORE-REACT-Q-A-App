/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { useNavigate } from 'react-router-dom';
import { Component, Dispatch, ReactNode } from 'react';
import { PrimaryButton } from '../styles/Style';
import {
  gettingUnAnsweredQuestions,
  gotUnAnsweredQuestions,
} from '../store/question/QuestionActions';
import { Page } from './Page';
import { PageTitle } from './PageTitle';
import QuestionList from './QuestionList';
import { useSelector, useDispatch } from 'react-redux';
import { getAllAnsweredQuestions } from '../types/QuestionData';
import { AppState } from '../store/Store';
import { AnyAction } from 'redux';
import { QuestionsState } from '../store/question/QuestionState';

interface HomePageProp {
  navigation: any;
  dispatch: Dispatch<AnyAction>;
  selector: QuestionsState;
}

class HomePage extends Component<HomePageProp, any> {
  constructor(props: HomePageProp) {
    super(props);
    this.goAskPage.bind(this);
  }

  componentDidMount(): void {
    this.populateQuestionData();
  }

  async populateQuestionData(): Promise<void> {
    const { dispatch } = this.props;
    dispatch(gettingUnAnsweredQuestions());
    const result = await getAllAnsweredQuestions();
    dispatch(gotUnAnsweredQuestions(result));
  }

  goAskPage(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
    this.props.navigation('ask');
  }
  render(): ReactNode {
    const { selector } = this.props;
    return (
      <Page>
        <div
          css={css`
            display: flex;
            align-items: center;
            justify-content: space-between;
          `}
        >
          <PageTitle>Unanswered Questions</PageTitle>
          <PrimaryButton onClick={(e) => this.goAskPage(e)}>
            Ask a question
          </PrimaryButton>
        </div>
        {selector.loading ? (
          <div>Questions Are Loading..</div>
        ) : (
          <QuestionList data={selector.unAnswered} />
        )}
      </Page>
    );
  }
}
function HomePageFunc(props: any) {
  const nav = useNavigate();
  const dispatch = useDispatch();
  const selector = useSelector((state: AppState) => state.questions);
  return (
    <HomePage
      {...props}
      navigation={nav}
      dispatch={dispatch}
      selector={selector}
    ></HomePage>
  );
}
export { HomePageFunc };