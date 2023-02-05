/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
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
const AskPage = React.lazy(() => import('./components/AskPage'));
function App() {
  const store = configureStore();
  return (
    <Provider store={store}>
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
                  <AskPage />
                </React.Suspense>
              }
            />
            <Route path="signin" element={<SignInPage />} />
            <Route path="questions/:questionId" element={<QuestionPage />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </div>
      </BrowserRouter>
    </Provider>
  );
}

export default App;
