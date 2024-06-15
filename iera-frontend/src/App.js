import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './Pages/HomePage';
import AANKONDIGINGEN from './Data/Hardcoded';
import AnnouncementPage from './Pages/AnnouncementPage';
import CreateAnnouncement from './Pages/CreateAnnouncementPage';

function App() {

  console.log(AANKONDIGINGEN)
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage announcements={AANKONDIGINGEN} />} />
        <Route path="/announcement/:id" Component={AnnouncementPage} />
        <Route path='/announcement/create' Component={CreateAnnouncement} />
      </Routes>
    </Router>
  );
}

export default App;
