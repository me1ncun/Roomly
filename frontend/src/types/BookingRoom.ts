export interface BookingRoom {
  bookingId: string;
  roomId: string;
  startTime: string;
  endTime: string;
  status: "Confirmed" | "OnApproval" | "Cancelled";
}
