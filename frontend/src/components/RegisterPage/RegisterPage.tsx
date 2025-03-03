import { useState } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { registerUser } from "../../features/authSlice";

export const RegisterPage = () => {
  const dispatch = useAppDispatch();
  const { loading, error } = useAppSelector((state) => state.auth);

  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    dispatch(registerUser({ name, email, password }));
  };

  return (
    <div className="container">
      <h1 className="title">Register</h1>
      <form onSubmit={handleSubmit}>
        <div className="field">
          <label className="label">Name</label>
          <input className="input" type="text" value={name} onChange={(e) => setName(e.target.value)} required />
        </div>
        <div className="field">
          <label className="label">Email</label>
          <input className="input" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        </div>
        <div className="field">
          <label className="label">Password</label>
          <input className="input" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <button className="button is-primary" type="submit" disabled={loading}>Register</button>
        {error && <p className="has-text-danger">{error}</p>}
      </form>
    </div>
  );
};
