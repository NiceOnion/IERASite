// HomePage.js
import React from 'react';
import Routingbutton from '../FunctionalComponents/Routingbutton';

function HomePage() {
  return (
    <div>
      <h2>Home Page</h2>
      <p>Welkom op de thuispagina van IERA</p>
      <Routingbutton to="/Aankondigingen">Naar Aankondigingen</Routingbutton>
    </div>
  );
}

export default HomePage;
