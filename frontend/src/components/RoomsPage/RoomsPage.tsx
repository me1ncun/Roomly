import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { initRooms, sortRooms } from '../../features/roomsSlice';
import { Loader } from '../Loader';
import { Room } from '../../types/Room';

export const RoomsPage = () => {
  const dispatch = useAppDispatch();
  const { rooms, loaded, hasError, sortBy, sortOrder } = useAppSelector(state => state.rooms);

  useEffect(() => {
    dispatch(initRooms());
  }, [dispatch]);

  const handleSort = (column: keyof Room) => {
    if (column !== 'description') {
      dispatch(sortRooms(column));
    }
  };

  const getColumnTitle = (key: string) => {
    return `${key.charAt(0).toUpperCase() + key.slice(1)} ${sortBy === key ? (sortOrder === 'asc' ? '' : '') : ''}`;
  };

  const getRoomType = (type: number) => (type === 0 ? 'Meeting Room' : 'Workplace');

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
              <p data-cy="noRoomsMessage">No available meeting rooms</p>
            )}

            {loaded && rooms.length > 0 && (
              <table className="table is-fullwidth is-striped">
                <thead>
                  <tr>
                    {['name', 'capacity', 'location', 'type'].map((key) => (
                      <th key={key} className="is-clickable" onClick={() => handleSort(key as keyof Room)}>
                        {getColumnTitle(key)}
                      </th>
                    ))}
                    <th>Description</th>
                  </tr>
                </thead>
                <tbody>
                  {rooms.map((room) => (
                    <tr key={room.id}>
                      <td>{room.name}</td>
                      <td>{room.capacity}</td>
                      <td>{room.location}</td>
                      <td>
                        {getRoomType(room.type)}
                      </td>
                      <td>{room.description}</td>
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
