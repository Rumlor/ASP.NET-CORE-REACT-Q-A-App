/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';
import { Component, ReactNode } from 'react';
import { AnswerData } from '../types/QuestionData';
import { gray5 } from '../styles/Style';
import { Answer } from './Answer';

interface AnswerListProp {
  data: AnswerData[];
}
interface AnswerListState {}
class AnswerList extends Component<AnswerListProp, AnswerListState> {
  render(): ReactNode {
    const data = this.props.data;
    return (
      <ul
        css={css`
          list-style: none;
          margin: 10px 0 0 0;
          padding: 0;
        `}
      >
        {data.map((item) => (
          <li
            css={css`
              border-top: 1px solid ${gray5};
            `}
            key={item.answerId}
          >
            <Answer data={item} />
          </li>
        ))}
      </ul>
    );
  }
}
export { AnswerList };
