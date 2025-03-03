import React, { useState, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { loginUser } from "../../features/authSlice";
import { Link, useNavigate } from "react-router-dom";

export const LoginPage = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { loading, error, token } = useAppSelector((state) => state.auth);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    dispatch(loginUser({ email, password }));
  };

  useEffect(() => {
    if (token) {
      navigate("/");
    }
  }, [token, navigate]);

  return (
    <div className="container">
      <h1 className="title">Login</h1>
      <form onSubmit={handleSubmit}>
        <div className="field">
          <label className="label">Email</label>
          <input
            className="input"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="field">
          <label className="label">Password</label>
          <input
            className="input"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button className="button is-primary" type="submit" disabled={loading}>
          Login
        </button>
        {error && <p className="has-text-danger">{error}</p>}
      </form>

      <p className="mt-4">
        Have no acc?{" "}
        <Link to="/register" className="has-text-link">
          Reg
        </Link>
      </p>
    </div>
  );
};
