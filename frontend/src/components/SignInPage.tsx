import { useAuthContext } from '../auth/Auth';
import { SignInAction } from '../enum/Action';
import { Page } from './Page';
import { StatusText } from '../styles/Style';

interface Prop {
  action: SignInAction;
}

function SignInPage(props: Prop) {
  const { signIn, isAuthenticated } = useAuthContext();
  if (props.action === SignInAction.SIGNIN) signIn();

  return (
    <Page title={'Signin In'}>
      <StatusText>Sign In...</StatusText>
    </Page>
  );
}
export { SignInPage };
