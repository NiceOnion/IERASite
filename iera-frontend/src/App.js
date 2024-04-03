// App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './Pages/HomePage';
import OtherPage from './Pages/AnnouncementPage';

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/Aankondigingen" element={<OtherPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
