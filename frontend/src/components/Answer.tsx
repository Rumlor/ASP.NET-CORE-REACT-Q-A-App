/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Component, ReactNode } from 'react';
import { AnswerData } from '../types/QuestionData';
import { gray3 } from '../styles/Style';
import { Page } from './Page';

interface AnswerProp {
  data: AnswerData;
}
interface AnswerState {}
class Answer extends Component<AnswerProp, AnswerState> {
  render(): ReactNode {
    const data = this.props.data;
    return (
      <div
        css={css`
          padding: 10px 0px;
        `}
      >
        <div
          css={css`
            padding: 10px 0px;
            font-size: 13px;
          `}
        >
          {data.content}
        </div>
        <div
          css={css`
            font-size: 12px;
            font-style: italic;
            color: ${gray3};
          `}
        >
          {`Answered By ${
            data.userName
          } at ${data.created.toLocaleDateString()} ${data.created.toLocaleTimeString()}`}
        </div>
      </div>
    );
  }
}
export { Answer };
