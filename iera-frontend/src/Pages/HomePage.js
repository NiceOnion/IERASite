// HomePage.js
import React from 'react';
import { useNavigate } from 'react-router-dom';

function HomePage() {
  const history = useNavigate();

  const handleClick = () => {
    history.push('/other');
  };

  return (
    <div>
      <h2>Home Page</h2>
      <button onClick={handleClick}>Go to Other Page</button>
    </div>
  );
}

export default HomePage;
