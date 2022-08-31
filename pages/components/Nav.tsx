import Link from 'next/link'
import React, { FunctionComponent } from 'react'

interface NavProps {
    children?: React.ReactNode
    items: Array<Item>,
    className?: string
}

export interface Item {
    name: string
    url: string
}

const Nav: FunctionComponent<NavProps> = ({ children, items, className }) => {
    return (
        <ul className={`flex flex-wrap text-sm font-medium text-center text-gray-500 dark:text-gray-400  ${className}`}>
            {
                items.map((item, index) => (
                    <li className="mr-2" key={index}>
                        <Link href={item.url} >

                            <button className="text-white bg-gradient-to-r from-orange-400 via-orange-500 to-orange-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-red-300 dark:focus:ring-red-800 shadow-lg shadow-orange-500/50 dark:shadow-lg dark:shadow-orange-800/80 font-medium rounded-lg text-sm px-5 py-1.5 text-center mr-2 mb-2">

                                {item.name}
                            </button>

                        </Link>
                    </li>))
            }
        </ul>
    )
}

export default Nav;
