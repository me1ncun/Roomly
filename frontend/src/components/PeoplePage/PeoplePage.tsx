import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { initRooms } from '../../features/roomsSlice';
import { Loader } from '../Loader';
import { Room } from '../../types/Room';

export const PeoplePage = () => {
  const dispatch = useAppDispatch();
  const { rooms, loaded, hasError } = useAppSelector(state => state.rooms);

  useEffect(() => {
    dispatch(initRooms());
  }, [dispatch]);

  return (
    <main className="section">
      <div className="container">
        <br />
        <br />
        <h1 className="title">Meeting Rooms</h1>
        <div className="block">
          <div className="box table-container">
            {!loaded && <Loader />}

            {hasError && (
              <p data-cy="roomsLoadingError" className="has-text-danger">
                Something went wrong: {hasError}
              </p>
            )}

            {loaded && rooms.length === 0 && !hasError && (
              <p data-cy="noRoomsMessage">
                No available meeting rooms
              </p>
            )}

            {loaded && rooms.length > 0 && (
              <table className="table is-fullwidth is-striped">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Capacity</th>
                    <th>Location</th>
                  </tr>
                </thead>
                <tbody>
                  {rooms.map((room: Room) => (
                    <tr key={room.id}>
                      <td>{room.name}</td>
                      <td>{room.capacity}</td>
                      <td>{room.location}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        </div>
      </div>
    </main>
  );
};
