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

interface SearchPageProp {
  searchParams: URLSearchParams;
  dispatcher: Dispatch<AnyAction>;
  selector: QuestionsState;
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
    const results = await searchQuestions(searchParamValue);
    this.props.dispatcher(searchedQuestions(results));
  }
}
function SearchPageFunc(prop: any) {
  const [params] = useSearchParams();
  const dispatcher = useDispatch();
  const selector = useSelector((state: AppState) => state.questions);
  return (
    <SearchPage
      {...prop}
      searchParams={params}
      dispatcher={dispatcher}
      selector={selector}
    />
  );
}
export { SearchPageFunc };
