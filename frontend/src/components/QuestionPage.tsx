/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import {
  gray3,
  gray6,
  FieldSet,
  FieldContainer,
  FieldLabel,
  FieldTextArea,
  FormButtonContainer,
  FieldError,
  SubmissionSuccess,
  PrimaryButton,
} from '../styles/Style';
import { Component, ReactNode } from 'react';
import { useParams } from 'react-router-dom';
import { Page } from './Page';
import {
  AnswerData,
  getQuestionWithId,
  postAnswer,
} from '../types/QuestionData';
import React from 'react';
import { AnswerList } from './AnswerList';
import { FieldValues, useForm, UseFormRegister } from 'react-hook-form';
import { FormState, UseFormHandleSubmit } from 'react-hook-form/dist/types';
import { useSelector } from 'react-redux';
import { useDispatch } from 'react-redux';
import { AppState } from '../store/Store';
import {
  gettingQuestion,
  gotQuestion,
} from '../store/question/QuestionActions';
import { Dispatch } from 'redux';
import { QuestionsState } from '../store/question/QuestionState';

interface QuestionPageProp {
  pageParam: any;
  register: UseFormRegister<QuestionPageContent>;
  handleSubmit: UseFormHandleSubmit<QuestionPageContent>;
  formState: FormState<QuestionPageContent>;
  dispatcher: Dispatch;
  selector: QuestionsState;
}

type QuestionPageContent = {
  content: string;
};
class QuestionPage extends Component<QuestionPageProp, any> {
  componentDidMount(): void {
    this.populateQuestionData();
  }
  async populateQuestionData(): Promise<void> {
    const result = await getQuestionWithId(
      Number(this.props.pageParam.questionId)
    );
    this.props.dispatcher(gotQuestion(result));
  }
  onValidSubmit = (data: FieldValues, event?: React.BaseSyntheticEvent) => {
    const newAnser: AnswerData = {
      answerId: 0,
      content: data.content,
      userName: 'fred',
      created: new Date(),
    };
    event?.preventDefault();
    this.props.dispatcher(gettingQuestion());
    postAnswer(newAnser, this.props.selector.viewing?.questionId).then(
      (res) => res && this.props.dispatcher(gotQuestion(res))
    );
  };
  onInValidSubmit = (data: FieldValues, event?: React.BaseSyntheticEvent) => {
    event?.preventDefault();
  };
  render(): ReactNode {
    const { viewing: question, loading } = this.props.selector;
    console.log(question);
    return (
      <Page>
        <div
          css={css`
            background-color: white;
            padding: 15px 20px 20px 20px;
            border-radius: 4px;
            border: 1px solid ${gray6};
            box-shadow: 0 3px 5px 0 rgba(0, 0, 0, 0.16);
          `}
        >
          <div
            css={css`
              font-size: 19px;
              font-weight: bold;
              margin: 10px 0px 5px;
            `}
          >
            {question === null ? 'Question Is Loading..' : question.title}
          </div>
          {question !== null && (
            <React.Fragment>
              <p
                css={css`
                  margin-top: 0px;
                  background-color: white;
                `}
              >
                {question.content}
              </p>
              <div
                css={css`
                  font-size: 12px;
                  font-style: italic;
                  color: ${gray3};
                `}
              >
                {`Askey by ${
                  question.userName
                } at ${question.created.toLocaleDateString()} ${question.created.toLocaleTimeString()}`}
              </div>
              <AnswerList data={question.answers} />
            </React.Fragment>
          )}
        </div>
        <form
          onSubmit={this.props.handleSubmit(
            this.onValidSubmit,
            this.onInValidSubmit
          )}
        >
          <FieldSet disabled={loading && !this.props.formState.errors.content}>
            {!this.props.formState.errors.content?.message &&
              this.props.formState.isSubmitSuccessful && (
                <SubmissionSuccess>Answer Submitted!</SubmissionSuccess>
              )}
            <FieldContainer>
              <FieldLabel htmlFor="content">Your Answer</FieldLabel>
              <FieldTextArea
                id="content"
                placeholder="Description of your question.."
                {...this.props.register('content', {
                  required: 'This field is required',
                  minLength: {
                    message: 'This field must contain 10 characters at least',
                    value: 10,
                  },
                })}
              ></FieldTextArea>
            </FieldContainer>
            <FormButtonContainer>
              <PrimaryButton type="submit">Submit Your Answer</PrimaryButton>
              {this.props.formState.errors.content?.message && (
                <FieldError>
                  {this.props.formState.errors.content.message}
                </FieldError>
              )}
            </FormButtonContainer>
          </FieldSet>
        </form>
      </Page>
    );
  }
}

function QuestionPageFunc(props: any) {
  const param = useParams();
  const dispatcher = useDispatch();
  const selector = useSelector((state: AppState) => state.questions);
  const { register, handleSubmit, formState } = useForm<QuestionPageContent>({
    defaultValues: { content: '' },
  });
  return (
    <QuestionPage
      {...props}
      pageParam={param}
      register={register}
      handleSubmit={handleSubmit}
      formState={formState}
      dispatcher={dispatcher}
      selector={selector}
    />
  );
}
export { QuestionPageFunc };
