import React, { useState } from 'react';
import './CSS/LayoutBase.css'
import LayoutBase from './Components/Layouts/LayoutBase';
import Home from './Components/Home'
import Gallery from './Components/Gallery'
import Shows from './Components/Shows';
import Contacts from './Components/Contacts';
import About from './Components/About';
import LeftSidebarLayout from './Components/Layouts/LeftSidebarLayout';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Show from './Components/Show';
import { Provider } from 'mobx-react';
import { appStore } from './AppStore';


function App() {
  const [showPopup, setShowPopup] = useState(false);
  return (
    <div className="App">
      <Provider appStore={appStore}>
        <LeftSidebarLayout/>
        <BrowserRouter>
        <Routes>
          <Route path="*" element={<LayoutBase renderBody={<Home/>}/>}/>
          <Route path="/Gallery" element={<LayoutBase renderBody={<Gallery/>}/>}/>
          <Route path="/Shows" element={<LayoutBase renderBody={<Shows/>}/>}/>
          <Route path="/Contacts" element={<LayoutBase renderBody={<Contacts/>}/>}/>
          <Route path="/About" element={<LayoutBase renderBody={<About/>}/>}/>
          <Route path="/Show/:id" element={<LayoutBase renderBody={<Show/>}/>}/>
          <Route path="/About" element={<LayoutBase renderBody={<About/>}/>}/>
        </Routes>
        </BrowserRouter>
      </Provider>
    </div>
  );
}

export default App;
