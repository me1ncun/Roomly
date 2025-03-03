/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { login, register, logout } from "../api/auth";
import { Login, Register, User } from "../types/Auth";

interface AuthState {
  token: string | null;
  user: User | null;
  loading: boolean;
  error: string | null;
}

const storedToken = localStorage.getItem("token") || null;
const storedUser = localStorage.getItem("user");

const initialState: AuthState = {
  token: storedToken,
  user: storedUser ? JSON.parse(storedUser) : null,
  loading: false,
  error: null,
};

export const loginUser = createAsyncThunk(
  "auth/loginUser",
  async (credentials: Login, { rejectWithValue }) => {
    try {
      const response = await login(credentials);

      console.log("сыроед:", response);

      if (typeof response === "string") {
        return { token: response };
      } else if (response.token) {
        return response;
      } else {
        return rejectWithValue("неправильный формат от сервера");
      }
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);


export const registerUser = createAsyncThunk(
  "auth/registerUser",
  async (userData: Register, { rejectWithValue }) => {
    try {
      const response = await register(userData);
      // localStorage.setItem("token", response.token);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);

export const logoutUser = createAsyncThunk("auth/logoutUser", async (_, { rejectWithValue }) => {
  try {
    await logout();
    return null;
  } catch (error: any) {
    return rejectWithValue(error.message);
  }
});

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(loginUser.fulfilled, (state, action) => {
        state.loading = false;
        state.token = action.payload.token;
    
        if (action.payload.user) {
          state.user = action.payload.user;
          localStorage.setItem("user", JSON.stringify(action.payload.user));
        }
        // console.log("response", state.token);
        localStorage.setItem("token", action.payload.token);
      }) 
      .addCase(loginUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(registerUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(registerUser.fulfilled, (state, action) => {
        state.loading = false;
        state.token = action.payload.token;
        if (action.payload.user) {
          state.user = action.payload.user;
        }
      })
      .addCase(registerUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(logoutUser.fulfilled, (state) => {
        state.loading = false;
        state.token = null;
        state.user = null;
        
        localStorage.removeItem("token");
        localStorage.removeItem("user");
    })          
      .addCase(logoutUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export default authSlice.reducer;
