import React from 'react';
import './CSS/LayoutBase.css'
import LayoutBase from './Components/Layouts/LayoutBase';
import Home from './Components/Home'
import LeftSidebarLayout from './Components/Layouts/LeftSidebarLayout';

function App() {
  return (
    <div className="App">
      <LeftSidebarLayout/>
      <LayoutBase renderBody={<Home/>} isAuthorize={false}/>
    </div>
  );
}

export default App;
