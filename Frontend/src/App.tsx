import React from 'react';
import './CSS/LayoutBase.css'
import LayoutBase from './Components/Layouts/LayoutBase';
import Home from './Components/Home'
import Gallery from './Components/Gallery'
import Shows from './Components/Shows';
import Contacts from './Components/Contacts';
import About from './Components/About';
import LeftSidebarLayout from './Components/Layouts/LeftSidebarLayout';
import { BrowserRouter, Routes, Route } from 'react-router-dom';


function App() {
  return (
    <div className="App">
      <LeftSidebarLayout/>
      <BrowserRouter>
      <Routes>
        <Route path="*" element={<LayoutBase renderBody={<Home/>} isAuthorize={false}/>}/>
        <Route path="/Gallery" element={<LayoutBase renderBody={<Gallery/>} isAuthorize={false}/>}/>
        <Route path="/Shows" element={<LayoutBase renderBody={<Shows/>} isAuthorize={false}/>}/>
        <Route path="/Contacts" element={<LayoutBase renderBody={<Contacts/>} isAuthorize={false}/>}/>
        <Route path="/About" element={<LayoutBase renderBody={<About/>} isAuthorize={false}/>}/>
      </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
