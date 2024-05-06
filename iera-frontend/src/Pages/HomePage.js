import React from 'react';
import { Link } from 'react-router-dom'; // Import Link from react-router-dom
import Routingbutton from '../FunctionalComponents/Routingbutton';
import GetAllAnnouncements from '../Data/AnnouncementDAL';


// Dashboard component
function Dashboard({ announcements }) {
  GetAllAnnouncements()

  return (
    <div>
      <h3>thuispagina</h3>
      <div>
        <p>
          Actuele ontwikkelingen:
        </p>
        <AnnouncementArray announcements={announcements} />
      </div>
    </div>
  );
}

// AnnouncementArray component
function AnnouncementArray({ announcements }) {
  console.log(announcements)
  const array = () => {
    return announcements.map(announcement => (
      <AnnouncementCard
        announcement={announcement}
        key={announcement.id}
      />
    ));
  }

  return (
    <div className='announcement-container'>
      {array()}
      <footer>
        <Routingbutton to="/Aankondigingen">Naar Aankondigingen</Routingbutton>
      </footer>
    </div>
  );
}

// AnnouncementCard component
function AnnouncementCard({ announcement }) {
  const { id, student, naam, aangekodigdOp: aangekondigdOp, body } = announcement;
  return (
    <div key={id} className='announcement-card'>
      <Link to={`/announcement/${id}`}>
        <strong>{student}</strong>
        <h2>{naam}</h2>
        <p>{aangekondigdOp}</p>
        <p>{body}</p>
      </Link>
    </div>
  );
}

// HomePage component
export default Dashboard;
