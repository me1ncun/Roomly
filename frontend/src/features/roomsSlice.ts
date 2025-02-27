import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Room } from "../types/Room";
import { getRooms } from "../api/rooms";

export type RoomsState = {
  rooms: Room[];
  loaded: boolean;
  hasError: string;
}

const initialState: RoomsState = {
  rooms: [],
  loaded: false,
  hasError: '',
};

export const initRooms = createAsyncThunk('rooms/fetch', async () => {
  return getRooms();
});

export const roomsSlice = createSlice({
  name: "rooms",
  initialState,
  reducers: {
    clearRooms: state => {
      state.rooms = [];
    },
  },
  extraReducers: builder => {
    builder.addCase(initRooms.pending, state => {
      state.loaded = false;
      state.hasError = '';
    });
    builder.addCase(initRooms.fulfilled, (state, action: PayloadAction<Room[]>) => {
      state.rooms = action.payload;
      state.loaded = true;
    });
    builder.addCase(initRooms.rejected, (state, action) => {
      state.hasError = action.error.message || '';
      state.loaded = true;
    });
  },
});

export const { clearRooms } = roomsSlice.actions;
export default roomsSlice.reducer;
