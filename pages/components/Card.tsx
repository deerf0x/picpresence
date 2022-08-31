import React, { FunctionComponent } from 'react'

interface CardProps {
    children?: React.ReactNode,
    title?: string,
    className?: string,
}

const Card: FunctionComponent<CardProps> = ({ children, title, className }) => {
    return (

        <div className={`block p-6 max-w-sm bg-white rounded-lg border border-gray-200 shadow-md hover:bg-gray-100 dark:bg-zinc-800 dark:bg-opacity-50 dark:border-gray-700 dark:hover:bg-zinc-800 ${className}`}>
            <h5 className="mb-2 text-1xl font-bold tracking-tight text-gray-900 dark:text-white">{title}</h5>
            {children}
        </div>

    )
}

export default Card;
