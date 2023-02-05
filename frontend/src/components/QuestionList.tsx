/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Component, ReactNode } from 'react';
import { Question } from './Question';
import { QuestionData } from '../types/QuestionData';
import { accent2, gray5 } from '../styles/Style';

export interface Prop {
  data: QuestionData[] | null;
  renderItem?: (item: QuestionData) => JSX.Element;
}

class QuestionList extends Component<Prop, any> {
  render(): ReactNode {
    const { data, renderItem } = this.props;
    return (
      <ul
        css={css`
          list-style: none;
          margin: 10px 0 0 0;
          padding: 0px 20px;
          background-color: #fff;
          border-bottom-left-radius: 4px;
          border-bottom-right-radius: 4px;
          border-top: 3px solid ${accent2};
          box-shadow: 0 3px 5px 0 rgba(0, 0, 0, 0.16);
        `}
      >
        {data ? (
          data.map((item, index) => (
            <li
              key={item.questionId}
              css={css`
                border-top: 1px solid ${gray5};
                :first-of-type {
                  border-top: none;
                }
              `}
            >
              {renderItem ? renderItem(item) : <Question data={item} />}
            </li>
          ))
        ) : (
          <></>
        )}
      </ul>
    );
  }
}
export default QuestionList;
