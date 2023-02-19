/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { fontFamily, fontSize, gray2 } from './styles/Style';
import { HeaderFunc as Header } from './components/Header';
import { HomePageFunc as HomePage } from './components/HomePage';
import { SearchPageFunc as SearchPage } from './components/SearchPage';
import { SignInPage } from './components/SignInPage';
import { NotFound } from './components/NotFound';
import { QuestionPageFunc as QuestionPage } from './components/QuestionPage';
import React from 'react';
import { Provider } from 'react-redux';
import { configureStore } from './store/Store';
import { SignOutPage } from './components/SignOutPage';
import { AuthProvider } from './auth/Auth';
import { SignInAction, SignOutAction } from './enum/Action';
import { AuthorizedPage } from './components/AuthorizedPage';

const AskPage = React.lazy(() => import('./components/AskPage'));
function App() {
  const store = configureStore();

  return (
    <Provider store={store}>
      <AuthProvider>
        <BrowserRouter>
          <div
            css={css`
              font-family: ${fontFamily};
              font-size: ${fontSize};
              color: ${gray2};
            `}
          >
            <Header />
            <Routes>
              <Route path="" element={<HomePage />} />
              <Route path="search" element={<SearchPage />} />
              <Route
                path="ask"
                element={
                  <React.Suspense
                    fallback={
                      <div
                        css={css`
                          margin-top: 100px;
                          text-align: center;
                        `}
                      >
                        Page is Loading..
                      </div>
                    }
                  >
                    <AuthorizedPage>
                      <AskPage />
                    </AuthorizedPage>
                  </React.Suspense>
                }
              />
              <Route
                path="signin"
                element={<SignInPage action={SignInAction.SIGNIN} />}
              />
              <Route
                path="signin-callback"
                element={<SignInPage action={SignInAction.SIGNIN_CALLBACK} />}
              />
              <Route
                path="signout"
                element={<SignOutPage action={SignOutAction.SIGNOUT} />}
              />
              <Route
                path="signout-callback"
                element={
                  <SignOutPage action={SignOutAction.SIGNOUT_CALLBACK} />
                }
              />
              <Route path="questions/:questionId" element={<QuestionPage />} />
              <Route path="*" element={<NotFound />} />
            </Routes>
          </div>
        </BrowserRouter>
      </AuthProvider>
    </Provider>
  );
}

export default App;
