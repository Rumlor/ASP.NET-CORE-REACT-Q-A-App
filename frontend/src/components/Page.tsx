/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Component } from 'react';
import { PageTitle } from './PageTitle';

interface Prop {
  title?: string;
  children: React.ReactNode;
}

class Page extends Component<Prop, any> {
  render(): React.ReactNode {
    const title = this.props.title && <PageTitle>{this.props.title}</PageTitle>;
    return (
      <div
        css={css`
          margin: 50px auto 20px auto;
          padding: 30px 20px;
          max-width: 600px;
        `}
      >
        {title}
        {this.props.children}
      </div>
    );
  }
}
export { Page };
