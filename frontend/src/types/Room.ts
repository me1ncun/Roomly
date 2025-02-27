import { RoomType } from "./RoomType";

export interface Room {
  id: string;
  name: string;
  location: string;
  capacity: number;
  type: RoomType;
  description: string;
}
