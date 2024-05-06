// HomePage.js
import React from 'react';
import Routingbutton from '../FunctionalComponents/Routingbutton';

function Test() {
  return (
    <div>
      <h3>Home Page</h3>
      <body>
      <p>Welkom op de thuispagina van IERA</p>
      <b/>
      < p>
      Hier staat hele informatieve tekst
      </p>
      <Routingbutton to="/Aankondigingen">Naar Aankondigingen</Routingbutton>
      </body>
    </div>
  );
}

  // meat of the page -> holds elements like the article array and cool stuff in the future (hopefully)
function Dashboard({announcementArray})
{
 return (
  <div>
    <h3>thuispagina</h3>
    <body>
      <p>
        Actuele ontwikkelingen:
        <AnnouncementArray announcementArray={announcementArray}/>
      </p>
    </body>
  </div>
  );
}

  //Get all Article cards and put them in a simple array
function AnnouncementArray( { announcementArray })
{
  //the array containing the announcements to be shown
  const array = []

  //for each announcement, send it to the announcement card function to create a card for in the array
  announcementArray.forEach((announcement) => {
    array.push(
      <AnnouncementCard announcement={announcement} />
    )
  });

  return (
    <div> 
      <body>
        {array}
      </body>
      <footer>
      <Routingbutton to="/Aankondigingen">Naar Aankondigingen</Routingbutton>
      </footer>
     </div>
  )
}


//Get information of an article, and create a card according to it
function AnnouncementCard(announcement)
{
    return (
      <div>
        <h4>{announcement.student}</h4>
        <h5>{announcement.naam}</h5>
        <h6>{announcement.aangekodigdOp}</h6>
        <p>{announcement.body}</p>
      </div>
    )
}

const AANKONDIGINGEN = [
  {naam: "kamer gezocht", aangekodigdOp: "12-5-2023", student: "Richard", body: "Ik ben een hele leuke jongen uit het noorden van het oosten van het land"},
  {naam: "Pils gezocht", aangekodigdOp: "9-6-2023", student: "Frederik", body: "Help! pils is op. kom brengen op Antoniuslaan 15. Ik geef sloppy toppie"},
  {naam: "Project Semester 6", aangekodigdOp: "4-4-2023", student: "Janne", body:"Hi! Ik wil flexen met mijn super mooie project. Check vooral mn github en geef me voldoening :)"}
]


export default function HomePage() {
  return <Dashboard announcementArray={AANKONDIGINGEN}/>
}
