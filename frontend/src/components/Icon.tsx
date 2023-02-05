/** @jsxImportSource @emotion/react */
import { Component, ReactNode } from 'react';
import { css } from '@emotion/react';
import user from '../user.svg';
interface IconProp {}
interface IconState {}
export class Icon extends Component<IconProp, IconState> {
  render(): ReactNode {
    return (
      <img
        src={user}
        alt="User"
        width="12px"
        css={css`
          width: 12px;
          opacity: 0.6;
        `}
      />
    );
  }
}
