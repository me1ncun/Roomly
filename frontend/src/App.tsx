import { NavLink, Outlet } from "react-router-dom";
import classNames from "classnames";
import { useAppDispatch, useAppSelector } from "./app/hooks";
import { logout } from "./features/authSlice";

const getLinkActiveClass = ({ isActive }: { isActive: boolean }) =>
  classNames("navbar-item", {
    "has-background-grey-lighter": isActive,
  });

export const App = () => {
  const authorized = useAppSelector((state) => state.auth.authorized);
  const dispatch = useAppDispatch();

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
              <button className="button is-danger ml-2" onClick={() => dispatch(logout())}>
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
