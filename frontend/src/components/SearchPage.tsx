/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { Component, Dispatch, ReactNode } from 'react';
import { useSelector } from 'react-redux';
import { useDispatch } from 'react-redux';
import { useSearchParams } from 'react-router-dom';
import { AnyAction } from 'redux';
import { URLSearchParams } from 'url';
import {
  searchedQuestions,
  searchingQuestions,
} from '../store/question/QuestionActions';
import { QuestionsState } from '../store/question/QuestionState';
import { AppState } from '../store/Store';
import { searchQuestions } from '../types/QuestionData';
import { Page } from './Page';
import QuestionList from './QuestionList';
import { useAuthContext } from '../auth/Auth';

interface SearchPageProp {
  searchParams: URLSearchParams;
  dispatcher: Dispatch<AnyAction>;
  selector: QuestionsState;
  getToken: () => Promise<string>;
}
class SearchPage extends Component<SearchPageProp, any> {
  render(): ReactNode {
    const searchParamValue = this.props.searchParams.get('criteria') || '';
    const { searched: questions } = this.props.selector;
    return (
      <Page title="Search Results">
        {this.props.searchParams && (
          <p
            css={css`
              font-size: 16px;
              font-style: italic;
              margin-top: 0px;
            `}
          >
            for "{searchParamValue}"
          </p>
        )}
        {questions?.length !== 0 ? (
          <QuestionList data={questions} />
        ) : (
          <p
            css={css`
              font-size: 16px;
              font-style: italic;
              margin-top: 0px;
            `}
          >
            No Result
          </p>
        )}
      </Page>
    );
  }
  componentDidMount(): void {
    this.populateQuestionsData();
  }
  componentDidUpdate(
    prevProps: Readonly<SearchPageProp>,
    prevState: Readonly<any>,
    snapshot?: any
  ): void {
    const searchParamValue = this.props.searchParams.get('criteria') || '';
    const prevSearchParamValue = prevProps.searchParams.get('criteria') || '';
    if (searchParamValue !== prevSearchParamValue) this.populateQuestionsData();
  }
  async populateQuestionsData(): Promise<void> {
    const searchParamValue = this.props.searchParams.get('criteria') || '';
    this.props.dispatcher(searchingQuestions());
    let token;
    try {
      token = await this.props.getToken();
    } catch (e) {
      console.error(e);
    }
    const results = await searchQuestions(searchParamValue, token || '');
    this.props.dispatcher(searchedQuestions(results));
  }
}
function SearchPageFunc(prop: any) {
  const [params] = useSearchParams();
  const dispatcher = useDispatch();
  const { getToken } = useAuthContext();
  const selector = useSelector((state: AppState) => state.questions);
  return (
    <SearchPage
      {...prop}
      searchParams={params}
      dispatcher={dispatcher}
      selector={selector}
      getToken={getToken}
    />
  );
}
export { SearchPageFunc };
