// OtherPage.js
import React from 'react';
import Routingbutton from '../FunctionalComponents/Routingbutton';

function AnnouncementPage() {
  return (
    <div>
      <h2>Aankondigingen</h2>
      <p>Welkom bij de Aankondigingen pagina van IERA</p>
      <Routingbutton to="/">Go to Home Page</Routingbutton>
    </div>
  );
}

export default AnnouncementPage;
