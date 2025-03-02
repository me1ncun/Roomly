import { createSlice } from "@reduxjs/toolkit";

type AuthState = {
  authorized: boolean;
};

const initialState: AuthState = {
  authorized: localStorage.getItem("authorized") === "true",
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    login: (state) => {
      state.authorized = true;
      localStorage.setItem("authorized", "true");
    },
    logout: (state) => {
      state.authorized = false;
      localStorage.removeItem("authorized");
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;
