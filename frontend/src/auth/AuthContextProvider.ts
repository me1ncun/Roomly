/* eslint-disable @typescript-eslint/no-unused-vars */
import React from "react";

export const AuthContext = React.createContext({
  authorized: false,
  login: (username: string, password: string) => Promise.resolve(),
  logout: () => {},
});
