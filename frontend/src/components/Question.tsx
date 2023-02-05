/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { Component, ReactNode } from 'react';
import { gray1, gray3 } from '../styles/Style';
import { QuestionData } from '../types/QuestionData';
import { Link } from 'react-router-dom';
export interface Prop {
  data: QuestionData;
  showContent?: boolean;
}

class Question extends Component<Prop, any> {
  render(): ReactNode {
    const { showContent = true, data } = this.props;
    return showContent ? (
      <div
        css={css`
          padding: 10px 0px;
          font-size: 19px;
        `}
      >
        <Link
          to={`/questions/${data.questionId}`}
          css={css`
            font-size: 24px;
            font-weight: bold;
            color: ${gray1};
            text-decoration: none;
          `}
        >
          {data.title}
        </Link>
        <div
          css={css`
            font-size: 12px;
            font-style: italic;
            color: ${gray3};
          `}
        >
          {`Asked by ${
            data.userName
          } on ${data.created.toLocaleDateString()} ${data.created.toLocaleTimeString()}`}
          {data.content}
        </div>
      </div>
    ) : (
      <></>
    );
  }
}

export { Question };
