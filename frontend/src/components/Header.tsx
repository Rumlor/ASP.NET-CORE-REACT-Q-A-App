/** @jsxImportSource @emotion/react */
import React from 'react';
import { css } from '@emotion/react';
import { fontFamily, fontSize, gray1, gray2, gray5 } from '../styles/Style';
import { Icon } from './Icon';
import { Link, NavigateFunction, useNavigate } from 'react-router-dom';
import { IAuth0Context, useAuthContext } from '../auth/Auth';
interface HeaderState {
  criteria: string;
}
interface HeaderProp {
  navigate: NavigateFunction;
  authContext: IAuth0Context;
}
class Header extends React.Component<HeaderProp, HeaderState> {
  state: HeaderState = {
    criteria: '',
  };

  handleSearchEvent(event: React.KeyboardEvent<HTMLInputElement>): void {
    if (
      event.key === 'Enter' &&
      this.state.criteria != null &&
      this.state.criteria !== ''
    )
      this.props.navigate(`/search?criteria=${this.state.criteria}`);
  }
  render(): React.ReactNode {
    const { loading, user, isAuthenticated } = this.props.authContext;
    console.log(this.props.authContext);
    return (
      <div
        css={css`
          position: fixed;
          box-sizing: border-box;
          top: 0;
          width: 100%;
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 10px 20px;
          background-color: #fff;
          border-bottom: 1px solid ${gray5};
          box-shadow: 0 3px 7px 0 rgba(110, 112, 114, 0.21);
        `}
      >
        <Link
          to={'/'}
          css={css`
            font-size: 24px;
            font-weight: bold;
            color: ${gray1};
            text-decoration: none;
          `}
        >
          Q & A
        </Link>
        <input
          type="text"
          placeholder="Search..."
          onChange={(e) => this.setState({ criteria: e.target.value })}
          onKeyDown={(e) => this.handleSearchEvent(e)}
          css={css`
            box-sizing: border-box;
            font-family: ${fontFamily};
            font-size: ${fontSize};
            padding: 8px 10px;
            border: 1px solid ${gray5};
            border-radius: 3px;
            color: ${gray2};
            background-color: white;
            width: 200px;
            height: 30px;
            &:focus {
              outline-color: ${gray5};
            }
          `}
        />
        <div>
          {!loading &&
            (isAuthenticated ? (
              <div>
                <span>{user?.name}</span>
                <Link
                  to={'signout'}
                  css={css`
                    font-family: ${fontFamily};
                    font-size: ${fontSize};
                    padding: 5px 10px;
                    background-color: transparent;
                    color: ${gray2};
                    text-decoration: none;
                    cursor: pointer;
                    span {
                      margin-left: 7px;
                    }
                    :focus {
                      outline-color: ${gray5};
                    }
                  `}
                >
                  <Icon />
                  <span>Sign Out</span>
                </Link>
              </div>
            ) : (
              <Link
                to={'signin'}
                css={css`
                  font-family: ${fontFamily};
                  font-size: ${fontSize};
                  padding: 5px 10px;
                  background-color: transparent;
                  color: ${gray2};
                  text-decoration: none;
                  cursor: pointer;
                  span {
                    margin-left: 7px;
                  }
                  :focus {
                    outline-color: ${gray5};
                  }
                `}
              >
                <Icon />
                <span>Sign In</span>
              </Link>
            ))}
        </div>
      </div>
    );
  }
}

function HeaderFunc(prop: any) {
  const navigate = useNavigate();
  const authContext = useAuthContext();
  return <Header {...prop} navigate={navigate} authContext={authContext} />;
}
export { HeaderFunc };
