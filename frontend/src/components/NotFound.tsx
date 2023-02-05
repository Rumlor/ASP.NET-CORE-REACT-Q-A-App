import { Component, ReactNode } from 'react';
import { Page } from './Page';

class NotFound extends Component {
  render(): ReactNode {
    return <Page title="Not Found"> {null} </Page>;
  }
}
export { NotFound };
