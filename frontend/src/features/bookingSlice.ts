import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { BookingRoom } from "../types/BookingRoom";
import { getBookings, createBooking, deleteBooking } from "../api/booking";

export type BookingRoomState = {
  bookings: BookingRoom[];
  loaded: boolean;
  hasError: string;
};

const initialState: BookingRoomState = {
  bookings: [],
  loaded: false,
  hasError: '',
};

export const initBookings = createAsyncThunk('bookings/fetch', async () => {
  return getBookings();
});

export const addBooking = createAsyncThunk(
  'bookings/add',
  async (newBooking: BookingRoom) => {
    return createBooking(newBooking);
  }
);

export const removeBooking = createAsyncThunk(
  'bookings/remove',
  async (roomId: string) => {
    await deleteBooking(roomId);
    return roomId;
  }
);

export const bookingSlice = createSlice({
  name: "bookings",
  initialState,
  reducers: {},
  extraReducers: builder => {
    builder
      // getBookings
      .addCase(initBookings.pending, state => {
        state.loaded = false;
        state.hasError = '';
      })
      .addCase(initBookings.fulfilled, (state, action: PayloadAction<BookingRoom[]>) => {
        state.bookings = action.payload;
        state.loaded = true;
      })
      .addCase(initBookings.rejected, (state, action) => {
        state.hasError = action.error.message || 'Failed to load bookings';
        state.loaded = true;
      })

      // createBooking
      .addCase(addBooking.fulfilled, (state, action: PayloadAction<BookingRoom>) => {
        state.bookings.push(action.payload);
      })
      .addCase(addBooking.rejected, (state, action) => {
        state.hasError = action.error.message || 'Failed to create booking';
      })

      // deleteBooking
      .addCase(removeBooking.fulfilled, (state, action: PayloadAction<string>) => {
        state.bookings = state.bookings.filter(booking => booking.roomId !== action.payload);
      })
      .addCase(removeBooking.rejected, (state, action) => {
        state.hasError = action.error.message || 'Failed to delete booking';
      });
  },
});

export default bookingSlice.reducer;
