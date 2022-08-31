import React, { FunctionComponent, useEffect, useState } from 'react'
import Home from '..';
import Card from '../components/Card';
import PocketBase from "pocketbase";

interface IndexProps {
    children?: React.ReactNode
}

type Room = {
    name?: string;
    maxCapacity?: number;
    currentCapacity?: number;
};

const client: PocketBase = new PocketBase(
    "https://picpresence.ncastillo.xyz"
);

const Index: FunctionComponent<IndexProps> = ({ children }) => {

    const [rooms, setRooms] = useState<Room[]>([]);

    useEffect(() => {

        const fetchRooms = async () => {
            const response = await fetch('/api/rooms');
            const rooms = await response.json();
            setRooms(rooms);
        }

        fetchRooms();

        // (Optionally) authenticate
        //client.users.authViaEmail('arellano@pic.com', 'arellano123');

        // Subscribe to changes in any record from the collection
        client.realtime.subscribe('rooms', e => {
            const index: number = rooms.findIndex(room => room.name === e.record['name']);
            if (index !== -1) {
                rooms[index] = {
                    name: e.record['name'],
                    maxCapacity: e.record['maxCapacity'],
                    currentCapacity: e.record['currentCapacity']
                };
                setRooms(rooms);
            }
        });

        return () => {
            // cleanup
        }
    }, [])



    return (
        <Home>


            <div className='grid grid-cols-3 gap-6'>
                {rooms.map((room, key) => (
                        
                    <Card key={key} title={room.name}>

                        <div className="flex space-x-2 justify-between">
                            <span className="text-xs font-normal leading-tight text-gray-500 dark:text-gray-400">
                                Max Capacity
                            </span>
                            <span className="bg-gray-100 text-gray-800 text-xs font-semibold mr-2 px-2.5 py-0.5 rounded dark:bg-zinc-700 dark:text-gray-300">{room.maxCapacity}</span>
                        </div>
                        <div className="flex space-x-2 justify-between mt-3">


                            <span className="text-xs font-normal leading-tight text-gray-500 dark:text-gray-400">
                                Current Capacity
                            </span>
                            <span className="bg-gray-100 text-gray-800 text-xs font-semibold mr-2 px-2.5 py-0.5 rounded dark:bg-green-700 dark:text-gray-300">{room.currentCapacity}</span>
                        </div>
                        <button type="button" className="mt-5 shadow-lg text-white bg-opacity-50 border border-zinc-700 font-medium rounded-lg text-sm px-5 py-1.5 text-center mr-2 mb-2 bg-transparent">See</button>

                    </Card>
                ))}
            </div>


        </Home>
    )
}

export default Index;
