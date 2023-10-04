import React from 'react';
import './CSS/LayoutBase.css'
import LayoutBase from './Components/Layouts/LayoutBase';
import Home from './Components/Home'

function App() {
  return (
    <div className="App">
      <LayoutBase renderBody={<Home/>} isAuthorize={false}/>
    </div>
  );
}

export default App;
