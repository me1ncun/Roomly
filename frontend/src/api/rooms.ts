import { Room } from '../types/Room';
import { client } from '../utils/fetchClient';

export const createRoom = (data: Omit<Comment, 'id'>) => {
  return client.post<Room>('/api/rooms', data);
};

export const getSelectedRoom = (roomId: number) => {
  return client.get<Room>(`/api/rooms/${roomId}/slots`);
};

export const getRooms = () => {
  return client.get<Room[]>('/api/rooms');
};
