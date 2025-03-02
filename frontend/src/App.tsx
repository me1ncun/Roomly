import { NavLink, Outlet } from 'react-router-dom';
import { useContext } from 'react';
import { AuthContext } from './store/AuthContext';
import classNames from 'classnames';

const getLinkActiveClass = ({ isActive }: { isActive: boolean }) =>
  classNames('navbar-item', {
    'has-background-grey-lighter': isActive,
  });

export const App = () => {
  const { authorized, logout } = useContext(AuthContext);

  return (
    <div data-cy="app">
      <nav
        data-cy="nav"
        className="navbar is-fixed-top has-shadow"
        role="navigation"
        aria-label="main navigation"
      >
        <div className="container">
          <div className="navbar-brand">
            <NavLink className={getLinkActiveClass} to="/">Home</NavLink>
            
            <NavLink className={getLinkActiveClass} to="/rooms">Rooms</NavLink>

            {authorized ? (
              <button className="button is-danger ml-2" onClick={logout}>
                Logout
              </button>
            ) : (
              <NavLink className={getLinkActiveClass} to="/login">Login</NavLink>
            )}
          </div>
        </div>
      </nav>

      <Outlet />
    </div>
  );
};
