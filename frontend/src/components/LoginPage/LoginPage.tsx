import { useNavigate, useLocation } from "react-router-dom";
import React, { useState } from "react";
import { login } from "../../features/authSlice";
import { useAppDispatch } from "../../app/hooks";

export const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { state } = useLocation();
  const path = state?.pathname || "/";

  function handleSubmit(event: React.FormEvent) {
    event.preventDefault();
    setErrorMessage("");

    if (username !== "Artem" || password !== "2004") {
      setErrorMessage("Username or password is wrong.");
      return;
    }

    dispatch(login());
    navigate(path, { replace: true });
  }

  return (
    <section className="section">
      <div className="container">
        <div className="column is-half is-offset-one-quarter">
          <div className="box has-shadow">
            <h1 className="title has-text-centered has-text-primary">Sign In</h1>

            <form onSubmit={handleSubmit}>
              <div className="field">
                <label className="label">Username</label>
                <div className="control has-icons-left">
                  <input
                    className="input is-medium"
                    type="text"
                    placeholder="Enter your username"
                    value={username}
                    onChange={(event) => {
                      setUsername(event.target.value);
                      setErrorMessage("");
                    }}
                  />
                  <span className="icon is-small is-left">
                    <i className="fas fa-user"></i>
                  </span>
                </div>
              </div>

              <div className="field">
                <label className="label">Password</label>
                <div className="control has-icons-left">
                  <input
                    className="input is-medium"
                    type="password"
                    placeholder="Enter your password"
                    value={password}
                    onChange={(event) => {
                      setPassword(event.target.value);
                      setErrorMessage("");
                    }}
                  />
                  <span className="icon is-small is-left">
                    <i className="fas fa-lock"></i>
                  </span>
                </div>
              </div>

              {errorMessage && (
                <div className="notification is-danger is-light animate__animated animate__fadeIn">
                  <button className="delete" onClick={() => setErrorMessage("")}></button>
                  {errorMessage}
                </div>
              )}

              <div className="field">
                <div className="control">
                  <button type="submit" className="button is-primary is-fullwidth is-medium">
                    Sign In
                  </button>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </section>
  );
};
