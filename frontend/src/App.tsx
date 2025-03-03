import { NavLink, Outlet } from "react-router-dom";
import classNames from "classnames";
import { useAppDispatch, useAppSelector } from "./app/hooks";
import { logoutUser } from "./features/authSlice";

const getLinkActiveClass = ({ isActive }: { isActive: boolean }) =>
  classNames("navbar-item", {
    "has-background-grey-lighter": isActive,
  });

export const App = () => {
  const dispatch = useAppDispatch();
  const { token } = useAppSelector((state) => state.auth);

  const handleLogout = () => {
    dispatch(logoutUser());
  };

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

            {token ? (
              <button className="button is-danger" onClick={handleLogout}>
                Logout
              </button>
            ) : (
              <>
                <NavLink className={getLinkActiveClass} to="/login">Login</NavLink>
                <NavLink className={getLinkActiveClass} to="/register">Register</NavLink>
              </>
            )}
          </div>
        </div>
      </nav>

      <Outlet />
    </div>
  );
};
