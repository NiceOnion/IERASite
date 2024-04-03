import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import HomePage from './Pages/HomePage';
import OtherPage from './Pages/OtherPage';

function App() {
  return (
    <Router>
      <div>
        <nav>
          <ul>
            <li>
              <Link to="/">Home</Link>
            </li>
            <li>
              <Link to="/other">Other Page</Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path="/other">
            <OtherPage />
          </Route>
          <Route path="/">
            <HomePage />
          </Route>
        </Routes>
      </div>
    </Router>
  );
}

export default App;
