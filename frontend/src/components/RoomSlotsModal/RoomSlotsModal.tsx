import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { clearSlots, fetchRoomSlots } from "../../features/roomSlotsSlice";
import { RoomSlot } from "../../types/RoomSlot";
import { Loader } from "../Loader";

type Props = {
  roomId: string | null;
  roomName: string | null;
  onClose: () => void;
};

export const RoomSlotsModal: React.FC<Props> = ({ roomId, roomName, onClose }) => {
  const dispatch = useAppDispatch();
  const { slots, loaded, hasError } = useAppSelector(
    (state) => state.roomSlots
  );

  useEffect(() => {
    if (roomId) {
      dispatch(fetchRoomSlots(roomId));
    }
    return () => {
      dispatch(clearSlots());
    };
  }, [roomId, dispatch]);

  if (!roomId) return null;

  return (
    <div className="modal is-active">
      <div className="modal-background" onClick={onClose}></div>
      <div className="modal-content">
        <div className="box">
          <h2 className="is-size-3 has-text-weight-bold">Available Slots for {roomName}</h2>
          {!loaded && <Loader />}

          {hasError && (
            <p data-cy="roomsLoadingError" className="has-text-danger">
              Something went wrong: {hasError}
            </p>
          )}

          {loaded && slots.length === 0 && !hasError && (
            <p data-cy="noRoomsMessage">No available slots</p>
          )}

          {loaded && slots.length > 0 && (
            <ul>
              {slots.map((slot: RoomSlot) => (
                <li key={slot.startTime}>
                  {new Date(slot.startTime).toLocaleTimeString()} -{" "}
                  {new Date(slot.endTime).toLocaleTimeString()}
                  {slot.isAvailable ? (
                    <span className="tag is-success">Available</span>
                  ) : (
                    <span className="tag is-danger">Booked</span>
                  )}
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
      <button
        className="modal-close is-large"
        aria-label="close"
        onClick={onClose}
      ></button>
    </div>
  );
};
