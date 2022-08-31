import type { NextPage } from 'next'
import Nav from './components/Nav';

interface HomeProps {
  children?: React.ReactNode
}

const Home: NextPage<HomeProps> = ({ children }) => {
  return (
    <div className='grid grid-rows-2 m-5'>
      <div className='flex flex-col mx-auto'>
        <h1 className='text-zinc-300 font-bold text-6xl'>
          PICPRESENCE
        </h1>
        <p className='text-zinc-300 text-1xl'>
          A PIC microcontroller presence detection system
        </p>
      </div>
      <Nav items={[{ name: "Rooms", url: "rooms" }, { name: "History", url: "history" }]} className="md:mx-56 mt-5" />

      <div className=' text-zinc-200 flex mt-6 md:mx-56'>
        {children}
      </div>
    </div>
  )
}

export default Home
