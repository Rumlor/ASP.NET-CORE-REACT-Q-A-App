import React from 'react';
import { Auth0Client, createAuth0Client } from '@auth0/auth0-spa-js';
import { authSettings } from '../AppSettings';
interface Auth0User {
  name: string;
  email: string;
}
export interface IAuth0Context {
  isAuthenticated: boolean;
  user?: Auth0User;
  signIn: () => void;
  signOut: () => void;
  loading: boolean;
}

export const AuthContext = React.createContext<IAuth0Context>({
  isAuthenticated: false,
  loading: true,
  signIn: () => {},
  signOut: () => {},
  user: undefined,
});

export const useAuthContext = () => React.useContext(AuthContext);

export const getAccessToken = async () => {
  const authClient = await createAuth0Client({
    ...authSettings,
    authorizationParams: { redirect_uri: authSettings.redirect_uri },
  });
  const token = await authClient.getTokenSilently();
  return token;
};
export const AuthProvider = (props: { children: any }) => {
  const [isAuthenticated, setAuthenticated] = React.useState<boolean>(false);
  const [user, setUser] = React.useState<Auth0User | undefined>(undefined);
  const [auth0Client, setAuth0Client] = React.useState<Auth0Client>();
  const [loading, setLoading] = React.useState<boolean>(false);

  const getAuth0Client = () => {
    if (!auth0Client) throw new Error('Auth Client not set!');
    return auth0Client;
  };
  React.useEffect(() => {
    const initAuth = async () => {
      setLoading(true);
      const auth0ClientCreated = await createAuth0Client({
        domain: authSettings.domain,
        clientId: authSettings.clientId,
        authorizationParams: {
          audience: authSettings.audience,
          redirect_uri: window.location.origin + authSettings.redirect_uri,
        },
      });
      setAuth0Client(auth0ClientCreated);
      if (
        window.location.pathname === '/signin-callback' &&
        window.location.search.indexOf('code=') > -1
      ) {
        await auth0ClientCreated.handleRedirectCallback();
      }
      const isAuthenticated = await auth0ClientCreated.isAuthenticated();
      if (isAuthenticated) {
        setUser(await auth0ClientCreated.getUser());
      }
      setAuthenticated(isAuthenticated);
      setLoading(false);
    };
    initAuth();
  }, []);
  return (
    <AuthContext.Provider
      value={{
        isAuthenticated,
        loading,
        user,
        signIn: () => getAuth0Client().loginWithRedirect(),
        signOut: () =>
          getAuth0Client().logout({
            clientId: authSettings.clientId,
            logoutParams: {
              returnTo: window.location.origin + '/signout-callback',
            },
          }),
      }}
    >
      {props.children}
    </AuthContext.Provider>
  );
};
