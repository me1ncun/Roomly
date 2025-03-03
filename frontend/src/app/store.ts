import { configureStore, ThunkAction, Action } from "@reduxjs/toolkit";
import roomsReducer from "../features/roomsSlice";
import roomSlotsReducer from "../features/roomSlotsSlice";
import authReducer from "../features/authSlice";

export const store = configureStore({
  reducer: {
    auth: authReducer,
    rooms: roomsReducer,
    roomSlots: roomSlotsReducer,
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;

export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;