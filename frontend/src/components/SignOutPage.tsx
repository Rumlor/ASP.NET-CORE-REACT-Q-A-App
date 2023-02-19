import { SignOutAction } from '../enum/Action';
import { useAuthContext } from '../auth/Auth';
import { Page } from './Page';
import { StatusText } from '../styles/Style';

interface Prop {
  action: SignOutAction;
}
const SignOutPage = (props: Prop) => {
  const { signOut } = useAuthContext();
  let message = 'Signing Out..';
  switch (props.action) {
    case SignOutAction.SIGNOUT:
      signOut();
      break;
    case SignOutAction.SIGNOUT_CALLBACK:
      message = 'Successfully Signed Out..';
      break;
  }
  return (
    <Page title={'Sign Out'}>
      <StatusText>{message}</StatusText>
    </Page>
  );
};
export { SignOutPage };
