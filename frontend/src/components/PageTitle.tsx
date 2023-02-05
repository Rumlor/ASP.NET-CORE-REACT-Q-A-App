/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Component } from 'react';

interface Prop {
  children: React.ReactNode;
}
class PageTitle extends Component<Prop, any> {
  renderTitle(strings: TemplateStringsArray, exp: string): string {
    //just for demo purpose for tagged templates.
    return exp;
  }
  render(): React.ReactNode {
    return (
      <h2
        css={css`
          font-size: 15px;
          font-weight: bold;
          margin: 10px 0px 5px;
          text-align: center;
          text-transform: uppercase;
        `}
      >
        {this.renderTitle`${this.props.children as string}`}
      </h2>
    );
  }
}
export { PageTitle };
