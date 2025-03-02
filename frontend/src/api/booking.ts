import { BookingRoom } from "../types/BookingRoom";
import { client } from "../utils/fetchClient";

export const createBooking = (newBooking: BookingRoom) => {
  return client.post<BookingRoom>('/bookings', newBooking);
};

export const getBookings = () => {
  return client.get<BookingRoom[]>(`/bookings`);
};

export const deleteBooking = (bookingId: string) => {
  return client.delete(`/bookings/${bookingId}/cancel`);
};
