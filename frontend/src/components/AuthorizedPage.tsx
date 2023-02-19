import { useAuthContext } from '../auth/Auth';
import { Page } from './Page';

export const AuthorizedPage = (props: { children: any }) => {
  const { isAuthenticated } = useAuthContext();
  return isAuthenticated ? (
    <>{props.children}</>
  ) : (
    <Page title={'You Dont Have Access To The Page.'}>{null}</Page>
  );
};
