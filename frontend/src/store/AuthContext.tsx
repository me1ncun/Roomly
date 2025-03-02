import React, { useState, useEffect } from 'react';
import { AuthContext } from './AuthContextProvider';

type Props = {
  children: React.ReactNode;
};

export const AuthProvider: React.FC<Props> = ({ children }) => {
  const [authorized, setAuthorized] = useState(() => {
    return localStorage.getItem('authorized') === 'true';
  });

  useEffect(() => {
    localStorage.setItem('authorized', String(authorized));
  }, [authorized]);

  async function login(username: string, password: string) {
    if (username !== 'Artem' || password !== '2004') {
      throw new Error('Username or password is wrong.');
    }
    setAuthorized(true);
  }

  function logout() {
    setAuthorized(false);
    localStorage.removeItem('authorized');
  }

  return (
    <AuthContext.Provider value={{ authorized, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthContext };
