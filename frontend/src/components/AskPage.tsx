import { Component, Dispatch, ReactNode } from 'react';
import { Page } from './Page';
import {
  FieldSet,
  FieldContainer,
  FieldLabel,
  FieldInput,
  FieldTextArea,
  FormButtonContainer,
  FieldError,
  SubmissionSuccess,
  PrimaryButton,
} from '../styles/Style';
import { useForm, UseFormHandleSubmit, UseFormRegister } from 'react-hook-form';
import { FieldValues, FormState } from 'react-hook-form/dist/types';
import { postQuestion, QuestionData } from '../types/QuestionData';
import { useDispatch } from 'react-redux';
import { useSelector } from 'react-redux';
import { AppState } from '../store/Store';
import { AnyAction } from 'redux';
import { QuestionsState } from '../store/question/QuestionState';
import {
  askedQuestion,
  askingQuestion,
} from '../store/question/QuestionActions';

type AskPageContent = {
  title: string;
  content: string;
};

interface AskPageProp {
  handleSubmit: UseFormHandleSubmit<AskPageContent>;
  register: UseFormRegister<AskPageContent>;
  dispatch: Dispatch<AnyAction>;
  selector: QuestionsState;
  formState: FormState<AskPageContent>;
}

class AskPage extends Component<AskPageProp, any> {
  postQuestionData(data: QuestionData) {
    this.props.dispatch(askingQuestion());
    postQuestion(data).then((res) => {
      this.props.dispatch(askedQuestion());
    });
  }

  onValidSubmit = (data: FieldValues, event?: React.BaseSyntheticEvent) => {
    const questionData: QuestionData = {
      userName: 'Fred',
      created: new Date(),
      questionId: 0,
      answers: [],
      title: data.title,
      content: data.content,
    };
    this.postQuestionData(questionData);
  };
  onInValidSubmit = (data: FieldValues, event?: React.BaseSyntheticEvent) => {
    console.log('invalid');
  };

  render(): ReactNode {
    const { loading, posted } = this.props.selector;
    return (
      <Page title="Ask a question">
        <form
          onSubmit={this.props.handleSubmit(
            this.onValidSubmit,
            this.onInValidSubmit
          )}
        >
          <FieldSet disabled={loading}>
            {posted &&
              !this.props.formState.errors.content?.message &&
              !this.props.formState.errors.title?.message && (
                <SubmissionSuccess>Successfully submitted!</SubmissionSuccess>
              )}
            <FieldContainer>
              <FieldLabel htmlFor="title">Title</FieldLabel>
              <FieldInput
                {...this.props.register('title', {
                  required: 'This field is required',
                  minLength: {
                    message: 'This field must contain 10 characters at least',
                    value: 10,
                  },
                })}
                placeholder="Title of your question.."
                id="title"
                name="title"
                type="text"
              />
              {this.props.formState.errors?.title?.message && (
                <FieldError>
                  {this.props.formState.errors?.title?.message}
                </FieldError>
              )}
            </FieldContainer>
            <FieldContainer>
              <FieldLabel htmlFor="content">Content</FieldLabel>
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
              />
              {this.props.formState.errors?.content?.message && (
                <FieldError>
                  {this.props.formState.errors?.content?.message}
                </FieldError>
              )}
            </FieldContainer>
            <FormButtonContainer>
              <PrimaryButton type="submit">Submit Your Question</PrimaryButton>
            </FormButtonContainer>
          </FieldSet>
        </form>
      </Page>
    );
  }
}

function AskPageFunc(prop: any) {
  const { register, handleSubmit, formState } = useForm<AskPageContent>({
    defaultValues: { title: '', content: '' },
  });
  const dispatch = useDispatch();
  const selector = useSelector((state: AppState) => state.questions);
  return (
    <AskPage
      {...prop}
      register={register}
      handleSubmit={handleSubmit}
      formState={formState}
      dispatch={dispatch}
      selector={selector}
    />
  );
}
export default AskPageFunc;
