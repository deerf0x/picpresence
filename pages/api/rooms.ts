import type { NextApiRequest, NextApiResponse } from "next";
import PocketBase from "pocketbase";

type Room = {
  name?: string;
  maxCapacity?: number;
  currentCapacity?: number;
};

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Array<Room>>
) {
  const client: PocketBase = new PocketBase(
    "https://picpresence.ncastillo.xyz"
  );
  const rooms: Array<Room> = [];

  //client.users.authViaEmail('arellano@pic.com', 'arellano123');

  const records = await client.records.getFullList("rooms", 200, {
    sort: "-created",
  });

  records.forEach((record) => {
    rooms.push({
      name: record.name,
      maxCapacity: record.maxCapacity,
      currentCapacity: record.currentCapacity,
    });
  });

  if (records && records.length > 0) {
    res.status(200).json(rooms);
  }
}
